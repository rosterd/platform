using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.Resources;
using Rosterd.Services.Resources.Interfaces;

namespace Rosterd.Admin.Api.Controllers
{
    /// <summary>
    /// All actions related to resources
    /// </summary>
    [ApiVersion("1.0")]
    public class JobsController : BaseApiController
    {
        private readonly ILogger<JobsController> _logger;
        private readonly IJobService _jobservice;

        public JobsController(ILogger<JobsController> logger, IJobService jobservice) : base()
        {
            _logger = logger;
            _jobservice = jobservice;
        }

        /// <summary>
        /// Gets all the resources 
        /// </summary>
        /// <param name="id">the resource id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PagedList<JobModel>> Get() =>
            (await _jobservice.GetJobs(new PagingQueryStringParameters()));

        [HttpGet("{jobId:long}")]
        public async Task<JobModel> GetFacilityById(long jobId) =>
         (await _jobservice.GetJobById(jobId));

        [HttpDelete("{jobId:long}")]
        public async Task<IActionResult> DeleteJob(long jobId) =>
            (await _jobservice.DeleteJob(jobId));

        [HttpPost]
        public async Task<IActionResult> CreateJob(JobModel jobModel) =>
            (await _jobservice.PostJob(jobModel));
    }
}