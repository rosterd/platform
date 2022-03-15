using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rosterd.Domain.Enums;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.JobModels;
using Rosterd.Domain.Search;

namespace Rosterd.Services.Jobs.Interfaces
{
    public interface IJobsService
    {
        /// <summary>
        /// Gets all the jobs
        /// </summary>
        /// <param name="pagingParameters"></param>
        /// <param name="userContextUsersAuth0OrganizationId"></param>
        /// <param name="jobStatus"></param>
        /// <returns></returns>
        Task<PagedList<JobModel>> GetAllJobs(PagingQueryStringParameters pagingParameters, string userContextUsersAuth0OrganizationId, JobStatus? jobStatus = null);

        /// <summary>
        /// Gets a specific job
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="auth0OrganizationId"></param>
        /// <returns></returns>
        Task<JobModel> GetJob(long jobId, string auth0OrganizationId);

        /// <summary>
        /// Adds a new job
        /// </summary>
        /// <param name="jobModel"></param>
        /// <param name="auth0OrganizationId"></param>
        /// <returns></returns>
        Task<JobModel> CreateJob(JobModel jobModel, string auth0OrganizationId);

        /// <summary>
        /// Deletes job
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="cancellationReason"></param>
        /// <param name="auth0OrganizationId"></param>
        /// <returns></returns>
        Task RemoveJob(long jobId, string cancellationReason, string auth0OrganizationId);

        /// <summary>
        /// Gets all the jobs that are relevant for a given staff
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="staffAuth0OrganizationId"></param>
        /// <param name="pagingParameters"></param>
        /// <returns></returns>
        Task<PagedList<JobModel>> GetRelevantJobsForStaff(long staffId, string staffAuth0OrganizationId, PagingQueryStringParameters pagingParameters);

        /// <summary>
        /// Gets all the staff (their device Id's) that are relevant for the given job
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        Task<(JobSearchModel job, List<string> staffDeviceIds)> GetRelevantStaffDeviceIdsForJob(string jobId);

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
        /// <param name="staffId"></param>
        /// <param name="jobStatusChangedTo"></param>
        /// <param name="statusChangeReason"></param>
        /// <param name="eventOccurredDateTime"></param>
        /// <returns></returns>
        Task CreateJobsStatusChangeRecord(long jobId, long? staffId, JobStatus jobStatusChangedTo, string statusChangeReason, DateTime? eventOccurredDateTime = null);

        /// <summary>
        /// Gets all the jobs that have past their end datetime and needs to be expired
        /// </summary>
        /// <returns>Returns, the (jobid, Auth0organizationId) as key value pair</returns>
        Task<List<long>> MoveAllPublishedStatusJobsPastTimeLimitToExpiredState();

        /// <summary>
        /// Gets all accepted jobs that have past their start time and before end time
        /// </summary>
        /// <returns>Returns, the (jobid, Auth0organizationId) as key value pair</returns>
        Task<List<long>> MoveAllAcceptedStatusJobsPastStartTimeBeforeEndTimeToInProgressState();


        /// <summary>
        /// Gets all completed jobs
        /// </summary>
        /// <returns>Returns, the (jobid, Auth0organizationId) as key value pair</returns>
        Task<List<long>> MoveInProgressJobsPastEndDateToCompletedState();

        /// <summary>
        /// Gets all the jobs that are finished and the status needs to be set to feedback
        /// </summary>
        /// <returns>Returns, the (jobid, Auth0organizationId) as key value pair</returns>
        Task MoveAllJobsThatArePastEndDateToFeedbackStatus();

        /// <summary>
        /// Gets a list of jobs that are finished (in statuses completed or noshow)
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<long>> GetAllJobsThatAreFinished();
    }
}
