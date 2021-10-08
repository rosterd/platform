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

            //TODO: for mvp rather than eventing, just call one method in the service calls that will do this
            ////Get all the jobs that need to be expired
            //var jobIdsThatNeedExpiring = (await _jobsService.GetAllJobsThatAreExpiredButStatusStillNotSetToExpired()).AlwaysList();
            //foreach (var job in jobIdsThatNeedExpiring)
            //{
            //    await _jobEventsService.GenerateJobStatusChangedEvent(job.Key, JobStatus.Expired, job.Value);
            //}
        }

        [FunctionName(nameof(MoveJobsPastEndDateToFeedbackState))]
        public async Task MoveJobsPastEndDateToFeedbackState([TimerTrigger("%FunctionSettings:MoveJobsPastEndDateToFeedbackStateSchedule%", RunOnStartup = false)] TimerInfo myTimer)
        {
            _logger.LogInformation($"{nameof(MoveJobsPastEndDateToFeedbackState)} - triggered on UTC Time {DateTime.UtcNow}");

            //TODO: for mvp rather than eventing, just call one method in the service calls that will do this
            ////Get all the jobs that need to be moved to feedback pending
            //var jobIdsThatNeedFeedbackPending = (await _jobsService.GetAllJobsThatArePastEndDateButStatusStillNotSetToFeedback()).AlwaysList();
            //foreach (var job in jobIdsThatNeedFeedbackPending)
            //{
            //    await _jobEventsService.GenerateJobStatusChangedEvent(job.Key, JobStatus.Expired, job.Value);
            //}
        }

        [FunctionName(nameof(MoveFinishedJobsFromSearch))]
        public async Task MoveFinishedJobsFromSearch([TimerTrigger("%FunctionSettings:MoveFinishedJobsFromSearchSchedule%", RunOnStartup = false)] TimerInfo myTimer)
        {
            _logger.LogInformation($"{nameof(MoveFinishedJobsFromSearch)} - triggered on UTC Time {DateTime.UtcNow}");

            //TODO: for mvp rather than eventing, just call one method in the service calls that will do this
            ////Get all the jobs that are finished and remove them from Azure Search
            //var JobsThatHaveFinished = (await _jobsService.GetAllJobsThatAreFinished()).AlwaysList();
            //await _jobEventsService.RemoveFinishedJobsFromSearch(JobsThatHaveFinished);
        }
    }
}
