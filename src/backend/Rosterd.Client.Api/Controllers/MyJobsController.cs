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
    [Route("api/v{version:apiVersion}/jobs")]
    [ApiExplorerSettings(GroupName = "My Jobs")]
    public class MyJobsController : BaseApiController
    {
        private readonly ILogger<MyJobsController> _logger;
        private readonly IJobsService _jobService;

        public MyJobsController(ILogger<MyJobsController> logger, IJobsService jobsService) : base()
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
        [OperationOrder(1)]
        public async Task<ActionResult<PagedList<JobModel>>> GetAllRelevantJobsForUser([FromQuery] PagingQueryStringParameters pagingParameters)
        {
            //TODO: get only relevant jobs for the user
            pagingParameters ??= new PagingQueryStringParameters();
            var pagedList = await _jobService.GetAllJobs(pagingParameters);

            return pagedList;
        }

        /// <summary>
        /// Gets all the jobs that are currently in progress or upcoming for the user
        /// </summary>
        /// <param name="pagingParameters"></param>
        /// <returns></returns>
        [HttpGet("my/current")]
        [OperationOrder(2)]
        public async Task<ActionResult<PagedList<JobModel>>> GetAllCurrentJobsForUser([FromQuery] PagingQueryStringParameters pagingParameters)
        {
            //TODO: get only currently accepted and ongoing jobs for this user
            pagingParameters ??= new PagingQueryStringParameters();
            var pagedList = await _jobService.GetAllJobs(pagingParameters);

            return pagedList;
        }

        /// <summary>
        /// Gets all the historically completed jobs (only jobs that have been completed)
        /// </summary>
        /// <param name="pagingParameters"></param>
        /// <returns></returns>
        [HttpGet("my/history/completed")]
        [OperationOrder(3)]
        public async Task<ActionResult<PagedList<JobModel>>> GetAllHistoricalCompletedJobsForUser([FromQuery] PagingQueryStringParameters pagingParameters)
        {
            //TODO: get only historical jobs for this user
            pagingParameters ??= new PagingQueryStringParameters();
            var pagedList = await _jobService.GetAllJobs(pagingParameters);

            return pagedList;
        }

        /// <summary>
        /// Gets all the historically cancelled jobs (only jobs that have been cancelled by the admin or the user opted out before the grace period)
        /// </summary>
        /// <param name="pagingParameters"></param>
        /// <returns></returns>
        [HttpGet("my/history/cancelled")]
        [OperationOrder(4)]
        public async Task<ActionResult<PagedList<JobModel>>> GetAllHistoricalCancelledJobsForUser([FromQuery] PagingQueryStringParameters pagingParameters)
        {
            //TODO: get only historical jobs for this user
            pagingParameters ??= new PagingQueryStringParameters();
            var pagedList = await _jobService.GetAllJobs(pagingParameters);

            return pagedList;
        }
    }
}
