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
        Task<long> CreateJob(JobModel jobModel);

        /// <summary>
        /// Deletes job
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        Task RemoveJob(long jobId);

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
    }
}
