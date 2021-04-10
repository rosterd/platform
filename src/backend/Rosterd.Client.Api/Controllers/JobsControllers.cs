using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.JobModels;
using Rosterd.Services.Jobs.Interfaces;
using Rosterd.Web.Infra.Filters.Swagger;

namespace Rosterd.Client.Api.Controllers
{
    /// <summary>
    /// All actions related to Jobs for the user
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
        /// Marks the current user as accepting the job
        /// </summary>
        /// <param name="jobId">The job id</param>
        /// <returns></returns>
        [HttpPut("{jobId}/confirmation")]
        [OperationOrder(1)]
        public async Task<ActionResult> AcceptAndConfirmJob(long jobId)
        {
            //Validate start time

            //Record history of this

            //Assign user

            //Change status to assigned

            //TODO:
            return Ok();
        }

        /// <summary>
        /// Removes the current user from the job, if the grace period has run out then a HTTP 422 is thrown back
        /// </summary>
        /// <param name="jobId">The job id</param>
        /// <returns></returns>
        [HttpDelete("{jobId}/cancellations")]
        [OperationOrder(2)]
        public async Task<ActionResult> CancelJob(long jobId)
        {
            //TODO:
            //1. Validate grace period

            //2. Record history of this

            //3. Remove assignment of user

            //4. Change status of job back to published

            return Ok();
        }
    }
}
