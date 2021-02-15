using System.Collections.Generic;
using System.Threading.Tasks;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.Resources;
using Microsoft.AspNetCore.Mvc;
namespace Rosterd.Services.Resources.Interfaces
{
    public interface IJobService
    {
        Task<PagedList<JobModel>> GetJobs(PagingQueryStringParameters pagingParameters);
        Task<JobModel> GetJobById(long jobId);
        Task<IActionResult> PostJob(JobModel jobModel);
        Task<IActionResult> DeleteJob(long jobId);
    }
}
