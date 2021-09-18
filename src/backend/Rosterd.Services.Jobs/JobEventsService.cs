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
using Rosterd.Infrastructure.Search.Interfaces;
using Rosterd.Services.Jobs.Interfaces;
using Rosterd.Services.Mappers;

namespace Rosterd.Services.Jobs
{
    public class JobEventsService : IJobEventsService
    {
        private readonly IRosterdDbContext _context;
        private readonly ISearchIndexProvider _searchIndexProvider;

        public JobEventsService(IRosterdDbContext context, ISearchIndexProvider searchIndexProvider)
        {
            _context = context;
            _searchIndexProvider = searchIndexProvider;
        }

        ///<inheritdoc/>
        public async Task GenerateNewJobCreatedEvent(long jobId)
        {
            //Get the latest job info
            var job = await _context.Jobs.Include(s => s.JobSkills).FirstAsync(s => s.JobId == jobId);

            //Translate to domain model and create event
            var jobModel = job.ToSearchModel();
            var jobCreatedEvent = new NewJobCreatedMessage(jobModel);

            //Sent the event to event grid
            //await eventGridClient.PublishEventsAsync(topicHostName, new List<EventGridEvent> { jobCreatedEvent });
        }

        ///<inheritdoc/>
        public async Task GenerateJobStatusChangedEvent(long jobId, JobStatus newJobsStatus)
        {
            var jobStatusChangeEvent = new JobStatusChangedMessage(jobId, newJobsStatus);

            //Sent the event to event grid
            //await eventGridClient.PublishEventsAsync(topicHostName, new List<EventGridEvent> { jobStatusChangeEvent });
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

            //Sent the event to event grid
            //await eventGridClient.PublishEventsAsync(topicHostName, new List<EventGridEvent> { jobCancelledEvent });
        }

        ///<inheritdoc/>
        public async Task HandleNewJobCreatedEvent(NewJobCreatedMessage jobCreatedMessage)
        {
            //var jobModel = jobCreatedEvent.Data as JobSearchModel;
            //await _searchIndexProvider.AddOrUpdateDocumentsToIndex(RosterdConstants.Search.JobsIndex, new List<JobSearchModel> {jobModel});
        }

        public Task HandleJobCancelledEvent(JobCancelledMessage jobCancelledMessage) => throw new System.NotImplementedException();

        ///<inheritdoc/>
        public async Task HandleJobStatusChangedEvent(JobStatusChangedMessage jobStatusChangedMessage)
        {
            //var jobId = jobCancelledEvent.Data as string;
            //await _searchIndexProvider.DeleteDocumentsFromIndex(RosterdConstants.Search.JobsIndex, JobSearchModel.Key(), new List<string>() {jobId});
        }

        ///<inheritdoc/>
        //public async Task HandleJobStatusChangedEvent(EventGridEvent jobStatusChangedEvent)
        //{
        //    var jobId = (jobStatusChangedEvent.Data as JobStatusChangedMessage)?.JobId;

        //    //Get the existing job from db (source of truth)
        //    var currentJob = await _context.Jobs.FindAsync(jobId.ToInt64());
        //    var searchModelToUpdate = currentJob.ToSearchModel();

        //    //Update the existing job document with the new status changes
        //    await _searchIndexProvider.AddOrUpdateDocumentsToIndex(RosterdConstants.Search.JobsIndex, new List<JobSearchModel>{searchModelToUpdate});
        //}

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
