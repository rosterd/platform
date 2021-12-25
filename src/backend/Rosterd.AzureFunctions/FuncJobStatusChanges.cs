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

namespace Rosterd.AzureFunctions
{
    public class FuncJobStatusChanges
    {
        private readonly ILogger<FuncStorageQueueMessageProcessor> _logger;
        private readonly IOptions<FunctionSettings> _settings;
        private readonly IJobsService _jobsService;
        private readonly IJobEventsService _jobEventsService;

        public FuncJobStatusChanges(ILogger<FuncStorageQueueMessageProcessor> logger, IOptions<FunctionSettings> settings, IJobsService jobsService, IJobEventsService jobEventsService)
        {
            _logger = logger;
            _settings = settings;
            _jobsService = jobsService;
            _jobEventsService = jobEventsService;
        }

        [FunctionName(nameof(MovedAllPublishedStatusJobsPastTimeLimitToExpiredState))]
        public async Task MovedAllPublishedStatusJobsPastTimeLimitToExpiredState([TimerTrigger("%FunctionSettings:MovedJobsPastTimeLimitToExpiredStateSchedule%", RunOnStartup = false)] TimerInfo myTimer)
        {
            _logger.LogInformation($"{nameof(MovedAllPublishedStatusJobsPastTimeLimitToExpiredState)} - triggered on UTC Time {DateTime.UtcNow}");

            //All the published jobs (ie: jobs that didn't get taken) if they have expired then move them to expired status
            await _jobsService.MovedAllPublishedStatusJobsPastTimeLimitToExpiredState();
        }

        [FunctionName(nameof(MoveJobsPastEndDateToFeedbackState))]
        public async Task MoveJobsPastEndDateToFeedbackState([TimerTrigger("%FunctionSettings:MoveJobsPastEndDateToFeedbackStateSchedule%", RunOnStartup = false)] TimerInfo myTimer)
        {
            _logger.LogInformation($"{nameof(MoveJobsPastEndDateToFeedbackState)} - triggered on UTC Time {DateTime.UtcNow}");

            //Move all the jobs that need to be moved to feedback pending
            await _jobsService.MoveAllJobsThatArePastEndDateToFeedbackStatus();
        }

        [FunctionName(nameof(MoveFinishedJobsFromSearch))]
        public async Task MoveFinishedJobsFromSearch([TimerTrigger("%FunctionSettings:MoveFinishedJobsFromSearchSchedule%", RunOnStartup = false)] TimerInfo myTimer)
        {
            _logger.LogInformation($"{nameof(MoveFinishedJobsFromSearch)} - triggered on UTC Time {DateTime.UtcNow}");

            //Get all the jobs that are finished and remove them from Azure Search
            var JobsThatHaveFinished = (await _jobsService.GetAllJobsThatAreFinished()).AlwaysList();
            await _jobEventsService.RemoveFinishedJobsFromSearch(JobsThatHaveFinished);
        }
    }
}
