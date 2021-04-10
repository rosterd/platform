using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rosterd.Admin.Api.Requests.Job;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.JobModels;
using Rosterd.Services.Jobs.Interfaces;
using Rosterd.Web.Infra.Filters.Swagger;

namespace Rosterd.Admin.Api.Controllers
{
    /// <summary>
    /// All actions related to Jobs
    /// </summary>
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "Jobs")]
    public class JobsController : BaseApiController
    {
        private readonly ILogger<JobsController> _logger;
        private readonly IJobsService _jobService;

        public JobsController(ILogger<JobsController> logger, IJobsService jobsService) : base()
        {
            _logger = logger;
            _jobService = jobsService;
        }

        /// <summary>
        /// Gets all the jobs 
        /// </summary>
        /// <param name="pagingParameters"></param>
        /// <returns></returns>
        [HttpGet]
        [OperationOrder(1)]
        public async Task<ActionResult<PagedList<JobModel>>> GetAllJobs([FromQuery] PagingQueryStringParameters pagingParameters)
        {
            pagingParameters ??= new PagingQueryStringParameters();
            var pagedList = await _jobService.GetAllJobs(pagingParameters);

            return pagedList;
        }

        /// <summary>
        /// Get Job by Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        [OperationOrderAttribute(2)]
        public async Task<ActionResult<JobModel>> GetJobById([Required] long? id)
        {
            var jobModel = await _jobService.GetJob(id.Value);
            return jobModel;
        }

        /// <summary>
        /// Adds a new Job
        /// </summary>
        /// <param name="request">The Job to add</param>
        /// <returns></returns>
        [HttpPost]
        [OperationOrderAttribute(3)]
        public async Task<ActionResult> AddNewJob([FromBody] AddJobRequest request)
        {
            var domainModelToSave = request.ToDomainModel();
            await _jobService.CreateJob(domainModelToSave);
            return Ok();
        }

        /// <summary>
        /// Sets the job status to 'Cancelled' 
        /// </summary>
        /// <param name="jobId">The Job to be removed</param>
        /// <returns></returns>
        [HttpDelete]
        [OperationOrderAttribute(4)]
        public async Task<ActionResult> RemoveJob([FromQuery][Required] long? jobId)
        {
            await _jobService.RemoveJob(jobId.Value);
            return Ok();
        }

    }
}
