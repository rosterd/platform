using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rosterd.Domain;
using Rosterd.Domain.Enums;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.JobModels;
using Rosterd.Services.Jobs.Interfaces;
using Rosterd.Web.Infra.Filters.Swagger;
using Rosterd.Web.Infra.Security;

namespace Rosterd.Client.Api.Controllers
{
    /// <summary>
    /// All actions related to Jobs for the user
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/jobs")]
    [ApiExplorerSettings(GroupName = "My Jobs")]
    [AuthorizeByRole(RosterdConstants.RosterdRoleNames.Staff)]
    public class MyJobsController : BaseApiController
    {
        private readonly ILogger<MyJobsController> _logger;
        private readonly IJobsService _jobService;

        public MyJobsController(ILogger<MyJobsController> logger, IJobsService jobsService, IOptions<AppSettings> appSettings) : base(appSettings)
        {
            _logger = logger;
            _jobService = jobsService;
        }

        /// <summary>
        /// Gets all the jobs that are relevant for the user
        /// </summary>
        /// <param name="pagingParameters"></param>
        /// <returns></returns>
        [HttpGet("my/relevant")]
        public async Task<ActionResult<PagedList<JobModel>>> GetAllRelevantJobsForUser([FromQuery] PagingQueryStringParameters pagingParameters)
        {
            //TODO: Once auth is done, get the right staff id to check
            pagingParameters ??= new PagingQueryStringParameters();
            var pagedList = await _jobService.GetRelevantJobsForStaff(1, pagingParameters);

            return pagedList;
        }

        /// <summary>
        /// Gets all the jobs that are currently in progress or upcoming for the user
        /// </summary>
        /// <param name="pagingParameters"></param>
        /// <returns></returns>
        [HttpGet("my/current")]
        public async Task<ActionResult<PagedList<JobModel>>> GetAllCurrentJobsForUser([FromQuery] PagingQueryStringParameters pagingParameters)
        {
            //TODO: Once auth is done, get the right staff id to check
            pagingParameters ??= new PagingQueryStringParameters();
            var pagedList = await _jobService.GetCurrentJobsForStaff(1, pagingParameters);

            return pagedList;
        }

        /// <summary>
        /// Gets all the historically completed jobs (only jobs that have been completed)
        /// </summary>
        /// <param name="pagingParameters"></param>
        /// <returns></returns>
        [HttpGet("my/history/completed")]
        public async Task<ActionResult<PagedList<JobModel>>> GetAllHistoricalCompletedJobsForUser([FromQuery] PagingQueryStringParameters pagingParameters)
        {
            //TODO: Once auth is done, get the right staff id to check
            pagingParameters ??= new PagingQueryStringParameters();
            var pagedList = await _jobService.GetJobsForStaff(1, new List<JobStatus> {JobStatus.Completed, JobStatus.FeedbackPending}, pagingParameters);

            return pagedList;
        }

        /// <summary>
        /// Gets all the historically cancelled jobs (only jobs that have been cancelled by the admin or the user opted out before the grace period)
        /// </summary>
        /// <param name="pagingParameters"></param>
        /// <returns></returns>
        [HttpGet("my/history/cancelled")]
        public async Task<ActionResult<PagedList<JobModel>>> GetAllHistoricalCancelledJobsForUser([FromQuery] PagingQueryStringParameters pagingParameters)
        {
            //TODO: Once auth is done, get the right staff id to check
            pagingParameters ??= new PagingQueryStringParameters();
            var pagedList = await _jobService.GetJobsForStaff(1, JobStatus.Cancelled, pagingParameters);

            return pagedList;
        }
    }
}
