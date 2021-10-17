using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rosterd.Data.SqlServer.Context;
using Rosterd.Data.SqlServer.Models;
using Rosterd.Domain;
using Rosterd.Domain.Enums;
using Rosterd.Domain.Messaging;
using Rosterd.Domain.Search;
using Rosterd.Infrastructure.Extensions;
using Rosterd.Infrastructure.Messaging;
using Rosterd.Infrastructure.Search.Interfaces;
using Rosterd.Services.Jobs.Interfaces;
using Rosterd.Services.Mappers;

namespace Rosterd.Services.Jobs
{
    public class JobEventsService : IJobEventsService
    {
        private readonly IRosterdDbContext _context;
        private readonly ISearchIndexProvider _searchIndexProvider;
        private readonly IQueueClient<JobsQueueClient> _jobsQueueClient;

        public JobEventsService(IRosterdDbContext context, ISearchIndexProvider searchIndexProvider, IQueueClient<JobsQueueClient> jobsQueueClient)
        {
            _context = context;
            _searchIndexProvider = searchIndexProvider;
            _jobsQueueClient = jobsQueueClient;
        }

        ///<inheritdoc/>
        public async Task GenerateNewJobCreatedEvent(long jobId, string auth0OrganizationId)
        {
            //Send to storage queue
            var newJobCreatedMessage = new NewJobCreatedMessage(jobId.ToString(), auth0OrganizationId);
            await _jobsQueueClient.SendMessageWithNoExpiry(BinaryData.FromObjectAsJson(newJobCreatedMessage));
        }

        ///<inheritdoc/>
        public async Task GenerateJobStatusChangedEvent(long jobId, JobStatus newJobsStatus, string auth0OrganizationId)
        {
            var jobStatusChangeEvent = new JobStatusChangedMessage(jobId, newJobsStatus, auth0OrganizationId);

            //Send to storage queue
            await _jobsQueueClient.SendMessageWithNoExpiry(BinaryData.FromObjectAsJson(jobStatusChangeEvent));
        }

        ///<inheritdoc/>
        public async Task GenerateJobCancelledEvent(long jobId, string auth0OrganizationId)
        {
            var jobCancelledEvent = new JobCancelledMessage(jobId, auth0OrganizationId);

            //Send to storage queue
            await _jobsQueueClient.SendMessageWithNoExpiry(BinaryData.FromObjectAsJson(jobCancelledEvent));
        }

        ///<inheritdoc/>
        public async Task HandleNewJobCreatedEvent(NewJobCreatedMessage jobCreatedMessage)
        {
            //Get the latest job info
            var jobId = jobCreatedMessage.JobId.ToLong();
            var job = await _context.Jobs
                .Include(s => s.JobSkills)
                .Include(y => y.Facility)
                .FirstAsync(s => s.JobId == jobId);

            //Translate to domain model to search model and save to search
            var jobsSkills = await GetSkills(job.JobSkills.AlwaysList());
            var jobModel = job.ToSearchModel(jobsSkills, jobCreatedMessage.Auth0OrganizationId);
            await _searchIndexProvider.AddOrUpdateDocumentsToIndex(RosterdConstants.Search.JobsIndex, new List<JobSearchModel> { jobModel });
        }

        public async Task HandleJobCancelledEvent(JobCancelledMessage jobCancelledMessage)
        {
            var jobId = jobCancelledMessage.JobId;
            await _searchIndexProvider.DeleteDocumentsFromIndex(RosterdConstants.Search.JobsIndex, JobSearchModel.Key(), new List<string>() { jobId });
        }

        ///<inheritdoc/>
        public async Task HandleJobStatusChangedEvent(JobStatusChangedMessage jobStatusChangedMessage)
        {
            var jobId = jobStatusChangedMessage.JobId.ToLong();

            //Get the existing job from db (source of truth)
            var currentJob = await _context.Jobs.Include(s => s.JobSkills).FirstAsync(s => s.JobId == jobId);

            var jobsSkills = await GetSkills(currentJob.JobSkills.AlwaysList());
            var searchModelToUpdate = currentJob.ToSearchModel(jobsSkills, jobStatusChangedMessage.Auth0OrganizationId);

            //Update the existing job document with the new status changes
            await _searchIndexProvider.AddOrUpdateDocumentsToIndex(RosterdConstants.Search.JobsIndex, new List<JobSearchModel> { searchModelToUpdate });
        }

        ///<inheritdoc/>
        public async Task RemoveFinishedJobsFromSearch(List<long> jobsIdsToRemoveFromSearch)
        {
            if (jobsIdsToRemoveFromSearch.IsNullOrEmpty())
                return;

            var jobsToRemove = jobsIdsToRemoveFromSearch.Select(s => s.ToString()).AlwaysList();
            await _searchIndexProvider.DeleteDocumentsFromIndex(RosterdConstants.Search.JobsIndex, JobSearchModel.Key(), jobsToRemove);
        }

        public async Task UpdateStatusOfJobInSearch(long jobId, JobStatus jobStatus)
        {
            var jobDocument = await _searchIndexProvider.GetSearchClient(RosterdConstants.Search.JobsIndex).GetDocumentAsync<JobSearchModel>(jobId.ToString());
            if (jobDocument?.Value != null)
            {
                var jobSearchModel = jobDocument.Value;
                jobSearchModel.JobStatusName = jobStatus.ToString();

                await _searchIndexProvider.AddOrUpdateDocumentToIndex(RosterdConstants.Search.JobsIndex, jobSearchModel);
            }
        }

        private async Task<List<Skill>> GetSkills(List<JobSkill> jobSkills)
        {
            if (jobSkills.IsNullOrEmpty())
                return new List<Skill>();

            var staffSkillIds = jobSkills.Select(s => s.SkillId).ToList();
            var skills = await _context.Skills.Where(s => staffSkillIds.Contains(s.SkillId)).ToListAsync();

            return skills;
        }
    }
}
