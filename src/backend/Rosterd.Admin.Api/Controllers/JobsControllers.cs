using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rosterd.Admin.Api.Requests.Jobs;
using Rosterd.Admin.Api.Services;
using Rosterd.Domain;
using Rosterd.Domain.Models.JobModels;
using Rosterd.Domain.Settings;
using Rosterd.Infrastructure.Security.Interfaces;
using Rosterd.Services.Jobs.Interfaces;
using Rosterd.Services.Staff.Interfaces;
using Rosterd.Web.Infra.Filters.Swagger;
using Rosterd.Web.Infra.Security;
using Rosterd.Web.Infra.ValidationAttributes;
using PagingQueryStringParameters = Rosterd.Domain.Models.PagingQueryStringParameters;

namespace Rosterd.Admin.Api.Controllers
{
    /// <summary>
    /// All actions related to Jobs
    /// </summary>
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "Jobs")]
    [AuthorizeByRole(RosterdConstants.RosterdRoleNames.FacilityAdmin, RosterdConstants.RosterdRoleNames.OrganizationAdmin, RosterdConstants.RosterdRoleNames.RosterdAdmin)]
    public class JobsController : BaseApiController
    {
        private readonly ILogger<JobsController> _logger;
        private readonly IJobsService _jobService;
        private readonly IJobEventsService _jobEventsService;
        private readonly IUserContext _userContext;
        private readonly IBelongsToValidator _belongsToValidator;
        private readonly IStaffService _staffService;

        public JobsController(ILogger<JobsController> logger, IJobsService jobsService, IJobEventsService jobEventsService, IOptions<AppSettings> appSettings, IUserContext userContext, IBelongsToValidator belongsToValidator, IStaffService staffService) : base(appSettings)
        {
            _logger = logger;
            _jobService = jobsService;
            _jobEventsService = jobEventsService;
            _userContext = userContext;
            _belongsToValidator = belongsToValidator;
            _staffService = staffService;
        }

        /// <summary>
        /// Gets all the jobs
        /// </summary>
        /// <param name="pagingParameters"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<Domain.Models.PagedList<JobModel>>> GetAllJobs([FromQuery] PagingQueryStringParameters pagingParameters)
        {
            pagingParameters ??= new PagingQueryStringParameters();
            var pagedList = await _jobService.GetAllJobs(pagingParameters, _userContext.UsersAuth0OrganizationId);

            return pagedList;
        }

        /// <summary>
        /// Get Job by Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{jobId}")]
        public async Task<ActionResult<JobModel>> GetJobById([ValidNumberRequired] long? jobId)
        {
            var jobModel = await _jobService.GetJob(jobId.Value, _userContext.UsersAuth0OrganizationId);
            return jobModel;
        }

        /// <summary>
        /// Adds a new Job
        /// Validation:
        /// -------------------
        /// 1. "Either no-grace-period' or 'grace-period-to-cancel-minutes' must be specified"
        /// 2. Job end date time must be greater than start date time
        /// </summary>
        /// <param name="request">The Job to add</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> AddNewJob([FromBody] AddJobRequest request)
        {
            //If user is a facility admin we need to validate the user can create for this facility
            if (_userContext.IsUserFacilityAdmin())
            {
                if(request.FacilityId.HasValue)
                    return BadRequest("As a facility admin you don't need to specify the facility");

                request.FacilityId = await _staffService.GetFacilityForStaffWhoIsFacilityAdmin(_userContext.UserAuth0Id);
            }

            if(request.FacilityId == null || request.FacilityId < 0)
                return BadRequest("Facility Id needs to be specified.");

            //Create Job
            var domainModelToSave = request.ToDomainModel();
            var newJob = await _jobService.CreateJob(domainModelToSave, _userContext.UsersAuth0OrganizationId);

            //Generate a new job created event
            await _jobEventsService.GenerateNewJobCreatedEvent(newJob.JobId, _userContext.UsersAuth0OrganizationId);
            return Ok(newJob);
        }

        /// <summary>
        /// Sets the job status to 'Cancelled'
        /// </summary>
        /// <param name="jobId">The Job to be removed</param>
        /// <param name="jobCancellationReason">The reason for the job cancellation</param>
        /// <returns></returns>
        [HttpDelete("{jobId}")]
        public async Task<ActionResult> RemoveJob([ValidNumberRequired] long? jobId, [Required][FromBody] string jobCancellationReason)
        {
            await _jobService.RemoveJob(jobId.Value, jobCancellationReason, _userContext.UsersAuth0OrganizationId);

            //Generate a new job deleted event
            await _jobEventsService.GenerateJobCancelledEvent(jobId.Value, _userContext.UsersAuth0OrganizationId);
            return Ok();
        }

    }
}
