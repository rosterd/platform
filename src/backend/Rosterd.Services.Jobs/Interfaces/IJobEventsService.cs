using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Rosterd.Domain.Enums;

namespace Rosterd.Services.Jobs.Interfaces
{
    public interface IJobEventsService
    {
        /// <summary>
        /// Generates a new job created event and sends the event to event grid
        /// </summary>
        /// <param name="eventGridClient"></param>
        /// <param name="topicHostName"></param>
        /// <param name="environmentThisEventIsBeingGenerateFrom"></param>
        /// <param name="jobId"></param>
        /// <returns></returns>
        Task GenerateNewJobCreatedEvent(IEventGridClient eventGridClient, string topicHostName, string environmentThisEventIsBeingGenerateFrom, long jobId);

        /// <summary>
        /// Generates a job status change event
        /// </summary>
        /// <param name="eventGridClient"></param>
        /// <param name="topicHostName"></param>
        /// <param name="environmentThisEventIsBeingGenerateFrom"></param>
        /// <param name="jobId"></param>
        /// <param name="newJobsStatus"></param>
        /// <returns></returns>
        Task GenerateJobStatusChangedEvent(IEventGridClient eventGridClient, string topicHostName, string environmentThisEventIsBeingGenerateFrom, long jobId,
            JobStatus newJobsStatus);

        /// <summary>
        /// Generates a job status change event for each given job in the list of jobs
        /// </summary>
        /// <param name="eventGridClient"></param>
        /// <param name="topicHostName"></param>
        /// <param name="environmentThisEventIsBeingGenerateFrom"></param>
        /// <param name="jobIds"></param>
        /// <param name="newJobsStatus"></param>
        /// <returns></returns>
        Task GenerateJobStatusChangedEvent(IEventGridClient eventGridClient, string topicHostName, string environmentThisEventIsBeingGenerateFrom,
            List<long> jobIds, JobStatus newJobsStatus);

        /// <summary>
        /// Generates a new job cancelled event and send the event to the event grid
        /// </summary>
        /// <param name="eventGridClient"></param>
        /// <param name="topicHostName"></param>
        /// <param name="environmentThisEventIsBeingGenerateFrom"></param>
        /// <param name="jobId"></param>
        /// <returns></returns>
        Task GenerateJobCancelledEvent(IEventGridClient eventGridClient, string topicHostName, string environmentThisEventIsBeingGenerateFrom,  long jobId);

        Task HandleNewJobCreatedEvent(EventGridEvent jobCreatedEvent);

        Task HandleJobCancelledEvent(EventGridEvent jobCancelledEvent);

        Task HandleJobStatusChangedEvent(EventGridEvent jobStatusChangedEvent);

        /// <summary>
        /// Removes all finished jobs from Azure Search
        /// </summary>
        /// <param name="jobsIdsToRemoveFromSearch"></param>
        /// <returns></returns>
        Task RemoveFinishedJobsFromSearch(List<long> jobsIdsToRemoveFromSearch);
    }
}
