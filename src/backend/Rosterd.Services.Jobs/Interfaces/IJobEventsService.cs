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
        /// <param name="auth0OrganizationId"></param>
        /// <returns></returns>
        Task GenerateNewJobCreatedEvent(long jobId, string auth0OrganizationId);

        /// <summary>
        /// Generates a job status change event
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="newJobsStatus"></param>
        /// <param name="auth0OrganizationId"></param>
        /// <returns></returns>
        Task GenerateJobStatusChangedEvent(long jobId,
            JobStatus newJobsStatus, string auth0OrganizationId);

        /// <summary>
        /// Generates a new job cancelled event and send the event to the event grid
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="auth0OrganizationId"></param>
        /// <returns></returns>
        Task GenerateJobCancelledEvent(long jobId, string auth0OrganizationId);

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
