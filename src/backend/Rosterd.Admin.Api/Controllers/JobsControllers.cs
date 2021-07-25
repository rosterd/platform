using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.EventGrid;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rosterd.Admin.Api.Requests.Job;
using Rosterd.Domain;
using Rosterd.Domain.Models.JobModels;
using Rosterd.Domain.Settings;
using Rosterd.Services.Jobs.Interfaces;
using Rosterd.Services.Staff.Interfaces;
using Rosterd.Web.Infra.Filters.Swagger;
using Rosterd.Web.Infra.ValidationAttributes;
using PagingQueryStringParameters = Rosterd.Domain.Models.PagingQueryStringParameters;

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
        private readonly IJobEventsService _jobEventsService;
        private readonly IEventGridClient _eventGridClient;

        public JobsController(ILogger<JobsController> logger, IJobsService jobsService, IJobEventsService jobEventsService, IEventGridClient eventGridClient, IOptions<AppSettings> appSettings) : base(appSettings)
        {
            _logger = logger;
            _jobService = jobsService;
            _jobEventsService = jobEventsService;
            _eventGridClient = eventGridClient;
        }

        /// <summary>
        /// Gets all the jobs 
        /// </summary>
        /// <param name="pagingParameters"></param>
        /// <returns></returns>
        [HttpGet]
        [OperationOrder(1)]
        public async Task<ActionResult<Domain.Models.PagedList<JobModel>>> GetAllJobs([FromQuery] PagingQueryStringParameters pagingParameters)
        {
            pagingParameters ??= new PagingQueryStringParameters();
            var pagedList = await _jobService.GetAllJobs(pagingParameters);

            return pagedList;
        }

        /// <summary>
        /// Get Job by Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{jobId}")]
        [OperationOrderAttribute(2)]
        public async Task<ActionResult<JobModel>> GetJobById([ValidNumberRequired] long? jobId)
        {
            var jobModel = await _jobService.GetJob(jobId.Value);
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
            //Create Job
            var domainModelToSave = request.ToDomainModel();
            var newJob = await _jobService.CreateJob(domainModelToSave);

            //Generate a new job created event
            await _jobEventsService.GenerateNewJobCreatedEvent(_eventGridClient, RosterdEventGridTopicHost, CurrentEnvironment, newJob.JobId);
            return Ok(newJob);
        }

        /// <summary>
        /// Sets the job status to 'Cancelled' 
        /// </summary>
        /// <param name="jobId">The Job to be removed</param>
        /// <param name="jobCancellationReason">The reason for the job cancellation</param>
        /// <returns></returns>
        [HttpDelete("{jobId}")]
        [OperationOrderAttribute(4)]
        public async Task<ActionResult> RemoveJob([ValidNumberRequired] long? jobId, [Required][FromBody] string jobCancellationReason)
        {
            await _jobService.RemoveJob(jobId.Value, jobCancellationReason);

            //Generate a new job deleted event
            await _jobEventsService.GenerateJobCancelledEvent(_eventGridClient, RosterdEventGridTopicHost, CurrentEnvironment, jobId.Value);
            return Ok();
        }

    }
}
