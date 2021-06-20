using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.EventGrid;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rosterd.Domain;
using Rosterd.Domain.Enums;
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
        private readonly IJobsValidationService _jobsValidationService;
        private readonly IJobEventsService _jobEventsService;
        private readonly IEventGridClient _eventGridClient;

        public JobsController(ILogger<JobsController> logger, IJobsService jobsService, IJobsValidationService jobsValidationService, IJobEventsService jobEventsService, IEventGridClient eventGridClient, IOptions<AppSettings> appSettings) : base(appSettings)
        {
            _logger = logger;
            _jobService = jobsService;
            _jobsValidationService = jobsValidationService;
            _jobEventsService = jobEventsService;
            _eventGridClient = eventGridClient;
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
            var (isJobValid, errorMessages) = await _jobsValidationService.IsJobStillValidToAccept(jobId);
            if (!isJobValid)
                return UnprocessableEntity(errorMessages);

            //TODO: When auth is ready pass in a proper staff id
            var isAcceptSuccessful = await _jobService.AcceptJobForStaff(jobId, 1);
            if (!isAcceptSuccessful)
                return UnprocessableEntity(RosterdConstants.ErrorMessages.GenericError);

            //Raise a job status change event (job is set to accepted status)
            await _jobEventsService.GenerateJobStatusChangedEvent(_eventGridClient, RosterdEventGridTopicHost, CurrentEnvironment, jobId, JobStatus.Accepted);
            
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
            //TODO: When auth is ready pass in a proper staff id
            var (isJobValid, errorMessages) = await _jobsValidationService.IsJobStillValidToCancelForStaff(jobId, 1);
            if (!isJobValid)
                return UnprocessableEntity(errorMessages);

            //TODO: When auth is ready pass in a proper staff id
            var isAcceptSuccessful = await _jobService.CancelJobForStaff(jobId, 1);
            if (!isAcceptSuccessful)
                return UnprocessableEntity(RosterdConstants.ErrorMessages.GenericError);

            //Raise a job status change event (job is set back to published status)
            await _jobEventsService.GenerateJobStatusChangedEvent(_eventGridClient, RosterdEventGridTopicHost, CurrentEnvironment, jobId, JobStatus.Published);

            return Ok();
        }
    }
}
