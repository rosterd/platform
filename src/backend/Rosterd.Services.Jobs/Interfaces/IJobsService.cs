using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rosterd.Domain.Enums;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.JobModels;

namespace Rosterd.Services.Jobs.Interfaces
{
    public interface IJobsService
    {
        /// <summary>
        /// Gets all the jobs
        /// </summary>
        /// <param name="pagingParameters"></param>
        /// <returns></returns>
        Task<PagedList<JobModel>> GetAllJobs(PagingQueryStringParameters pagingParameters);

        /// <summary>
        /// Gets a specific job
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        Task<JobModel> GetJob(long jobId);

        /// <summary>
        /// Adds a new job
        /// </summary>
        /// <param name="jobModel"></param>
        /// <returns></returns>
        Task<JobModel> CreateJob(JobModel jobModel);

        /// <summary>
        /// Deletes job
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="jobCancellationReason"></param>
        /// <returns></returns>
        Task RemoveJob(long jobId, string jobCancellationReason);

        /// <summary>
        /// Gets all the jobs that are relevant for a given staff
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="pagingParameters"></param>
        /// <returns></returns>
        Task<PagedList<JobModel>> GetRelevantJobsForStaff(long staffId, PagingQueryStringParameters pagingParameters);

        /// <summary>
        /// Gets all the currently active jobs for a given staff
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="pagingParameters"></param>
        /// <returns></returns>
        Task<PagedList<JobModel>> GetCurrentJobsForStaff(long staffId, PagingQueryStringParameters pagingParameters);

        /// <summary>
        /// Gets a list of all the jobs that are matching a given set of statuses
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="jobsStatusesToQueryFor"></param>
        /// <param name="pagingParameters"></param>
        /// <returns></returns>
        Task<PagedList<JobModel>> GetJobsForStaff(long staffId, List<JobStatus> jobsStatusesToQueryFor, PagingQueryStringParameters pagingParameters);

        /// <summary>
        /// Gets a list of all the jobs that are matching a given status
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="jobsStatusToQueryFor"></param>
        /// <param name="pagingParameters"></param>
        /// <returns></returns>
        Task<PagedList<JobModel>> GetJobsForStaff(long staffId, JobStatus jobsStatusToQueryFor, PagingQueryStringParameters pagingParameters);

        /// <summary>
        /// Marks the jobs as accepted for the given staff
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="staffId"></param>
        /// <returns></returns>
        Task<bool> AcceptJobForStaff(long jobId, long staffId);

        /// <summary>
        /// Removes the staff member from the job
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="staffId"></param>
        /// <returns></returns>
        Task<bool> CancelJobForStaff(long jobId, long staffId);

        /// <summary>
        /// Adds a jobs status change record
        /// NB: Just adds the record to the context does not save changes to DB need to call .SavesChangesAsync() as part of the caller
        /// method if this needs to be saved to the db
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="jobStatusChangedTo"></param>
        /// <param name="statusChangeReason"></param>
        /// <param name="eventOccurredDateTime"></param>
        /// <returns></returns>
        Task CreateJobsStatusChangeRecord(long jobId, JobStatus jobStatusChangedTo, string statusChangeReason, DateTime? eventOccurredDateTime = null);

        /// <summary>
        /// Gets all the jobs that have past their end datetime and needs to be expired
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<long>> GetAllJobsThatAreExpiredButStatusStillNotSetToExpired();

        /// <summary>
        /// Gets all the jobs that are finished and the status needs to be set to feedback
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<long>> GetAllJobsThatAreFinishedButStatusStillNotSetToFeedback();
    }
}
