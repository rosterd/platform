using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rosterd.Admin.Api.Infrastructure.Filters.Swagger;
using Rosterd.Admin.Api.Requests.Job;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.Resources;
using Rosterd.Services.Jobs.Interfaces;

namespace Rosterd.Admin.Api.Controllers
{
    /// <summary>
    /// All actions related to resources
    /// </summary>
    [ApiVersion("1.0")]
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
        /// Gets all the resources 
        /// </summary>
        /// <param name="pagingParameters"></param>
        /// <returns></returns>
        [HttpGet]
        [OperationOrderAttribute(1)]
        public async Task<ActionResult<PagedList<JobModel>>> GetAllJobs([FromQuery] PagingQueryStringParameters pagingParameters)
        {
            pagingParameters ??= new PagingQueryStringParameters();
            PagedList<JobModel> pagedList;

            pagedList = await _jobService.GetAllJobs(pagingParameters);

            return pagedList;
        }

        /// Get Job by Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        [OperationOrderAttribute(2)]
        public async Task<ActionResult<JobModel>> GetJobById(string? id)
        {
            var jobModel = await _jobService.GetJob(long.Parse(id));
            return jobModel;
        }

        /// <summary>
        /// Adds a new Job
        /// </summary>
        /// <param name="request">The Job to add</param>
        /// <returns></returns>
        [HttpPost]
        [OperationOrderAttribute(3)]
        public async Task<ActionResult> AddNewJob([FromBody] AddUpdateJobRequest request)
        {
            await _jobService.CreateJob(request.JobToAddOrUpdate);
            return Ok();
        }

        /// <summary>
        /// Update a Job
        /// </summary>
        /// <param name="request">The Job to update</param>
        /// <returns></returns>
        [HttpPut]
        [OperationOrderAttribute(4)]
        public async Task<ActionResult> UpdateJob([FromBody] AddUpdateJobRequest request)
        {
            await _jobService.UpdateJob(request.JobToAddOrUpdate);
            return Ok();
        }


        /// <summary>
        /// Deletes Job
        /// </summary>
        /// <param name="jobId">The Job to be deleted</param>
        /// <returns></returns>
        [HttpDelete]
        [OperationOrderAttribute(5)]
        public async Task<ActionResult> RemoveJob([FromQuery][Required] long? jobId)
        {
            await _jobService.RemoveJob(jobId.Value);
            return Ok();
        }

    }
}
