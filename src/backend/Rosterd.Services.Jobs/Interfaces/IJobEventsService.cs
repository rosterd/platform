using System.Collections.Generic;
using System.Threading.Tasks;
using Rosterd.Domain.Enums;
using Rosterd.Domain.Messaging;

namespace Rosterd.Services.Jobs.Interfaces
{
    public interface IJobEventsService
    {
        /// <summary>
        /// Generates a new job created event and sends the event to event grid
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        Task GenerateNewJobCreatedEvent(long jobId);

        /// <summary>
        /// Generates a job status change event
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="newJobsStatus"></param>
        /// <returns></returns>
        Task GenerateJobStatusChangedEvent(long jobId,
            JobStatus newJobsStatus);

        /// <summary>
        /// Generates a job status change event for each given job in the list of jobs
        /// </summary>
        /// <param name="jobIds"></param>
        /// <param name="newJobsStatus"></param>
        /// <returns></returns>
        Task GenerateJobStatusChangedEvent(List<long> jobIds, JobStatus newJobsStatus);

        /// <summary>
        /// Generates a new job cancelled event and send the event to the event grid
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        Task GenerateJobCancelledEvent( long jobId);

        Task HandleNewJobCreatedEvent(NewJobCreatedMessage jobCreatedMessage);

        Task HandleJobCancelledEvent(JobCancelledMessage jobCancelledMessage);

        Task HandleJobStatusChangedEvent(JobStatusChangedMessage jobStatusChangedMessage);

        /// <summary>
        /// Removes all finished jobs from Azure Search
        /// </summary>
        /// <param name="jobsIdsToRemoveFromSearch"></param>
        /// <returns></returns>
        Task RemoveFinishedJobsFromSearch(List<long> jobsIdsToRemoveFromSearch);
    }
}
