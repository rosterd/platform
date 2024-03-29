using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure;
using Azure.Search.Documents.Indexes;
using Expo.Server.Client;
using Expo.Server.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rosterd.AzureFunctions.Config;
using Rosterd.Domain;
using Rosterd.Domain.Search;
using Rosterd.Infrastructure.Search.Interfaces;
using Rosterd.Services.Jobs.Interfaces;
using Rosterd.Services.Staff.Interfaces;

namespace Rosterd.AzureFunctions
{
    public class FuncAzureSearchHelpers
    {
        private readonly IJobEventsService _jobEventsService;
        private readonly ILogger<FuncAzureSearchHelpers> _logger;
        private readonly ISearchIndexProvider _searchIndexProvider;
        private readonly IOptions<FunctionSettings> _settings;
        private readonly IStaffEventsService _staffEventsService;

        public FuncAzureSearchHelpers(ILogger<FuncAzureSearchHelpers> logger, IOptions<FunctionSettings> settings, ISearchIndexProvider searchIndexProvider,
            IStaffEventsService staffEventsService, IJobEventsService jobEventsService)
        {
            _logger = logger;
            _settings = settings;
            _searchIndexProvider = searchIndexProvider;
            _staffEventsService = staffEventsService;
            _jobEventsService = jobEventsService;
        }

        /// <summary>
        ///     Timer function that runs once a day, the primary purpose is to make sure the search indexes are there and if not create them.
        ///     This also runs on startup so every time this is deployed or restarted then it will do a check for the indexes and create if not there
        ///     SCHEDULE IS TO RUN EVERY 24 HOURS
        /// </summary>
        /// <param name="myTimer"></param>
        /// <param name="log"></param>
        [FunctionName(nameof(CreateSearchIndexes))]
        [Disable]
        public async Task CreateSearchIndexes([TimerTrigger("0 0 0 * * *"
#if DEBUG
                , RunOnStartup = false
#endif
            )]
            TimerInfo myTimer, ILogger log)
        {
            _logger?.LogInformation($"{nameof(CreateSearchIndexes)} - triggered on UTC Time {DateTime.UtcNow}");

            //Create a SearchIndexClient to create indexes
            var serviceEndpoint = new Uri(_settings.Value.SearchServiceEndpoint);
            var credential = new AzureKeyCredential(_settings.Value.SearchServiceApiKey);
            var searchIndexClient = new SearchIndexClient(serviceEndpoint, credential);

            //Create all the required indexes
            //Staff Index
            var (staffIndexExists, _) = await _searchIndexProvider.GetIndexStatus(RosterdConstants.Search.StaffIndex);
            if (!staffIndexExists)
                await _searchIndexProvider.CreateOrUpdateIndex<StaffSearchModel>(RosterdConstants.Search.StaffIndex, null);

            //Jobs Index
            var (jobIndexExists, _) = await _searchIndexProvider.GetIndexStatus(RosterdConstants.Search.JobsIndex);
            if (!jobIndexExists)
                await _searchIndexProvider.CreateOrUpdateIndex<JobSearchModel>(RosterdConstants.Search.JobsIndex, null);
        }

        /// <summary>
        ///     Helper function for populating all active staff and job to search,
        ///     handy when we have a fresh index and want to get all active entries from db.
        ///     THIS IS IN-EFFICIENT DESIGNED FOR TEST/DEV ENVIRONMENTS
        /// </summary>
        /// <param name="myTimer"></param>
        /// <param name="log"></param>
        [FunctionName(nameof(ReCreateSearchIndexesAndPopulateFromDb))]
        [Disable]
        public async Task ReCreateSearchIndexesAndPopulateFromDb([TimerTrigger("0 0 0 * * *"
#if DEBUG
                , RunOnStartup = false
#endif
            )]
            TimerInfo myTimer, ILogger log)
        {
            //Delete indexes
            await _searchIndexProvider.DeleteIndex(RosterdConstants.Search.StaffIndex);
            await _searchIndexProvider.DeleteIndex(RosterdConstants.Search.JobsIndex);

            //Create indexes
            await CreateSearchIndexes(null, null);

            //Take everything from db and put into search (in-efficient should not be used for prod)
            await _staffEventsService.AddAllActiveStaffToSearch();
            await _jobEventsService.AddAllActiveJobsToSearch();
        }

        /// <summary>
        ///     Helper function for sending some push notifications to a given device id
        /// </summary>
        /// <param name="myTimer"></param>
        /// <param name="log"></param>
        [FunctionName(nameof(SendPushNotificationToSampleDeviceId))]
        [Disable]
        public async Task SendPushNotificationToSampleDeviceId([TimerTrigger("0 0 0 * * *"
#if DEBUG
                , RunOnStartup = true
#endif
            )]
            TimerInfo myTimer, ILogger log)
        {
            var expoSdkClient = new PushApiClient();
            var pushTicketReq = new PushTicketRequest()
            {
                PushTo = new List<string>{ "ExponentPushToken[PQj7mKOJc3YNyBtsyds_uG]" },
                PushBadgeCount = 1,
                PushTitle = "Rosterd New Matching Job Alert",
                PushBody = $"Found a new matching job for Blah"
            };

            var result = await expoSdkClient.PushSendAsync(pushTicketReq);
        }
    }
}
