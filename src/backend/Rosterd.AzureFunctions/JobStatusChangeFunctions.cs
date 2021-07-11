// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using System;
using System.Threading.Tasks;
using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rosterd.AzureFunctions.Config;
using Rosterd.Domain;
using Rosterd.Domain.Search;
using Rosterd.Infrastructure.Extensions;
using Rosterd.Services.Jobs.Interfaces;
using Rosterd.Services.Staff.Interfaces;

namespace Rosterd.AzureFunctions
{
    public class JobStatusChangeFunctions
    {
        private readonly ILogger<EventConsumerFunctions> _logger;
        private readonly IOptions<FunctionSettings> _settings;
        private readonly IJobsService _jobsService;
        private readonly IJobEventsService _jobEventsService;

        public JobStatusChangeFunctions(ILogger<EventConsumerFunctions> logger, IOptions<FunctionSettings> settings, IJobsService jobsService, IJobEventsService jobEventsService)
        {
            _logger = logger;
            _settings = settings;
            _jobsService = jobsService;
            _jobEventsService = jobEventsService;
        }

        [FunctionName(nameof(MovedJobsPastTimeLimitToExpiredState))]
        public async Task MovedJobsPastTimeLimitToExpiredState([TimerTrigger("%FunctionSettings:MovedJobsPastTimeLimitToExpiredStateSchedule%", RunOnStartup = false)] TimerInfo myTimer)
        {
            _logger.LogInformation($"{nameof(MovedJobsPastTimeLimitToExpiredState)} - triggered on UTC Time {DateTime.UtcNow}");

            //Get all the jobs that need to be expired
            var jobsThatNeedsToBeExpired = (await _jobsService.GetAllJobsThatAreExpiredButStatusStillNotSetToExpired()).AlwaysList();

            //foreach (var jobToBeExpired in jobsThatNeedsToBeExpired)
            //{
            //    _jobEventsService.GenerateJobStatusChangedEvent()
            //}
        }

        [FunctionName(nameof(MoveFinishedJobsToFeedbackState))]
        public async Task MoveFinishedJobsToFeedbackState([TimerTrigger("%FunctionSettings:MoveFinishedJobsToFeedbackStateSchedule%", RunOnStartup = false)] TimerInfo myTimer)
        {
            _logger.LogInformation($"{nameof(MoveFinishedJobsToFeedbackState)} - triggered on UTC Time {DateTime.UtcNow}");


        }
    }
}
