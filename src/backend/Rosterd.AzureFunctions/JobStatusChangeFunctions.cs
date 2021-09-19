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
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rosterd.AzureFunctions.Config;
using Rosterd.Domain;
using Rosterd.Domain.Enums;
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

        private string RosterdEventGridTopicHost { get; }

        public JobStatusChangeFunctions(ILogger<EventConsumerFunctions> logger, IOptions<FunctionSettings> settings, IJobsService jobsService, IJobEventsService jobEventsService)
        {
            _logger = logger;
            _settings = settings;
            _jobsService = jobsService;
            _jobEventsService = jobEventsService;

            RosterdEventGridTopicHost = new Uri(settings.Value.EventGridTopicEndpoint).Host;
        }

        //[FunctionName(nameof(MovedJobsPastTimeLimitToExpiredState))]
        //public async Task MovedJobsPastTimeLimitToExpiredState([TimerTrigger("%FunctionSettings:MovedJobsPastTimeLimitToExpiredStateSchedule%", RunOnStartup = false)] TimerInfo myTimer)
        //{
        //    _logger.LogInformation($"{nameof(MovedJobsPastTimeLimitToExpiredState)} - triggered on UTC Time {DateTime.UtcNow}");

        //    //Get all the jobs that need to be expired
        //    var jobIdsThatNeedExpiring = (await _jobsService.GetAllJobsThatAreExpiredButStatusStillNotSetToExpired()).AlwaysList();

        //    await _jobEventsService.GenerateJobStatusChangedEvent(jobIdsThatNeedExpiring, JobStatus.Expired,);
        //}

        //[FunctionName(nameof(MoveJobsPastEndDateToFeedbackState))]
        //public async Task MoveJobsPastEndDateToFeedbackState([TimerTrigger("%FunctionSettings:MoveJobsPastEndDateToFeedbackStateSchedule%", RunOnStartup = false)] TimerInfo myTimer)
        //{
        //    _logger.LogInformation($"{nameof(MoveJobsPastEndDateToFeedbackState)} - triggered on UTC Time {DateTime.UtcNow}");

        //    //Get all the jobs that need to be moved to feedback pending
        //    var jobIdsThatNeedFeedbackPending = (await _jobsService.GetAllJobsThatArePastEndDateButStatusStillNotSetToFeedback()).AlwaysList();

        //    await _jobEventsService.GenerateJobStatusChangedEvent(jobIdsThatNeedFeedbackPending, JobStatus.FeedbackPending, TODO);
        //}

        //[FunctionName(nameof(MoveFinishedJobsFromSearch))]
        //public async Task MoveFinishedJobsFromSearch([TimerTrigger("%FunctionSettings:MoveFinishedJobsFromSearchSchedule%", RunOnStartup = false)] TimerInfo myTimer)
        //{
        //    _logger.LogInformation($"{nameof(MoveFinishedJobsFromSearch)} - triggered on UTC Time {DateTime.UtcNow}");

        //    //Get all the jobs that are finished and remove them from Azure Search
        //    var JobsThatHaveFinished = (await _jobsService.GetAllJobsThatAreFinished()).AlwaysList();
        //    await _jobEventsService.RemoveFinishedJobsFromSearch(JobsThatHaveFinished);
        //}
    }
}
