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
            await _jobsService.MoveAllPublishedStatusJobsPastTimeLimitToExpiredState();
        }

        [FunctionName(nameof(MovedAllAcceptedStatusJobsPastStartTimeBeforeEndTimeToInProgressState))]
        public async Task MovedAllAcceptedStatusJobsPastStartTimeBeforeEndTimeToInProgressState([TimerTrigger("%FunctionSettings:MovedAllAcceptedStatusJobsPastStartTimeBeforeEndTimeToInProgressStateSchedule%", RunOnStartup = false)] TimerInfo myTimer)
        {
            _logger.LogInformation($"{nameof(MovedAllAcceptedStatusJobsPastStartTimeBeforeEndTimeToInProgressState)} - triggered on UTC Time {DateTime.UtcNow}");

            //All the accepted jobs, after start time and before end time move them to in progress status
             await _jobsService.MoveAllAcceptedStatusJobsPastStartTimeBeforeEndTimeToInProgressState();
        }

        // This is for MVP only
        [FunctionName(nameof(MoveInProgressJobsPastEndDateToCompletedState))]
        public async Task MoveInProgressJobsPastEndDateToCompletedState([TimerTrigger("%FunctionSettings:MoveInProgressJobsPastEndDateToCompletedState%", RunOnStartup = false)] TimerInfo myTimer)
        {
            _logger.LogInformation($"{nameof(MoveInProgressJobsPastEndDateToCompletedState)} - triggered on UTC Time {DateTime.UtcNow}");

            //Move all the inprogress after the endtime to completed
            await _jobsService.MoveInProgressJobsPastEndDateToCompletedState();
        }

        [FunctionName(nameof(MoveFinishedJobsFromSearch))]
        public async Task MoveFinishedJobsFromSearch([TimerTrigger("%FunctionSettings:MoveFinishedJobsFromSearchSchedule%", RunOnStartup = false)] TimerInfo myTimer)
        {
            _logger.LogInformation($"{nameof(MoveFinishedJobsFromSearch)} - triggered on UTC Time {DateTime.UtcNow}");

            //Get all the jobs that are finished and remove them from Azure Search
            var JobsThatHaveFinished = (await _jobsService.GetAllJobsThatAreFinished()).AlwaysList();
            await _jobEventsService.RemoveJobsFromSearch(JobsThatHaveFinished);
        }
    }
}
