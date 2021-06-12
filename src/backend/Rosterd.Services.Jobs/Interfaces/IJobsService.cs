using System.Threading.Tasks;
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
    }
}
