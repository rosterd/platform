using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rosterd.Data.SqlServer.Context;
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
        public async Task GenerateNewJobCreatedEvent(long jobId)
        {
            //Send to storage queue
            var newJobCreatedMessage = new NewJobCreatedMessage(jobId.ToString());
            await _jobsQueueClient.QueueClient.SendMessageAsync(BinaryData.FromObjectAsJson(newJobCreatedMessage));
        }

        ///<inheritdoc/>
        public async Task GenerateJobStatusChangedEvent(long jobId, JobStatus newJobsStatus)
        {
            var jobStatusChangeEvent = new JobStatusChangedMessage(jobId, newJobsStatus);

            //Send to storage queue
            await _jobsQueueClient.QueueClient.SendMessageAsync(BinaryData.FromObjectAsJson(jobStatusChangeEvent));
        }

        ///<inheritdoc/>
        public async Task GenerateJobStatusChangedEvent(List<long> jobIds, JobStatus newJobsStatus)
        {
            if (jobIds.IsNullOrEmpty())
                return;

            foreach (var jobId in jobIds)
            {
                await GenerateJobStatusChangedEvent(jobId, newJobsStatus);
            }
        }

        ///<inheritdoc/>
        public async Task GenerateJobCancelledEvent(long jobId)
        {
            var jobCancelledEvent = new JobCancelledMessage(jobId);

            //Send to storage queue
            await _jobsQueueClient.QueueClient.SendMessageAsync(BinaryData.FromObjectAsJson(jobCancelledEvent));
        }

        ///<inheritdoc/>
        public async Task HandleNewJobCreatedEvent(NewJobCreatedMessage jobCreatedMessage)
        {
            //Get the latest job info
            var job = await _context.Jobs.Include(s => s.JobSkills).FirstAsync(s => s.JobId == jobCreatedMessage.ToLong());

            //Translate to domain model to search model and save to search
            var jobModel = job.ToSearchModel();
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
            var jobId = jobStatusChangedMessage.JobId;

            //Get the existing job from db (source of truth)
            var currentJob = await _context.Jobs.FindAsync(jobId.ToLong());
            var searchModelToUpdate = currentJob.ToSearchModel();

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
    }
}
