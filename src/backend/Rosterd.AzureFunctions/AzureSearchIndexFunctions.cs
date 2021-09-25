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
using Rosterd.Domain.Search;
using Rosterd.Infrastructure.Search.Interfaces;

namespace Rosterd.AzureFunctions
{
    public class AzureSearchIndexFunctions
    {
        private readonly ILogger<AzureSearchIndexFunctions> _logger;
        private readonly IOptions<FunctionSettings> _settings;
        private readonly ISearchIndexProvider _searchIndexProvider;

        public AzureSearchIndexFunctions(ILogger<AzureSearchIndexFunctions> logger, IOptions<FunctionSettings> settings, ISearchIndexProvider searchIndexProvider)
        {
            _logger = logger;
            _settings = settings;
            _searchIndexProvider = searchIndexProvider;
        }

        /// <summary>
        /// Timer function that runs once a day, the primary purpose is to make sure the search indexes are there and if not create them.
        /// This also runs on startup so every time this is deployed or restarted then it will do a check for the indexes and create if not there
        ///
        /// SCHEDULE IS TO RUN EVERY 24 HOURS
        /// </summary>
        /// <param name="myTimer"></param>
        /// <param name="log"></param>
        [FunctionName(nameof(CreateSearchIndexes))]
        public async Task CreateSearchIndexes([TimerTrigger("0 0 0 * * *", RunOnStartup = true)] TimerInfo myTimer, ILogger log)
        {
            _logger.LogInformation($"{nameof(CreateSearchIndexes)} - triggered on UTC Time {DateTime.UtcNow}");

            //Create a SearchIndexClient to create indexes
            var serviceEndpoint = new Uri(_settings.Value.SearchServiceEndpoint);
            var credential = new AzureKeyCredential(_settings.Value.SearchServiceApiKey);
            var searchIndexClient = new SearchIndexClient(serviceEndpoint, credential);

            //await _searchIndexProvider.DeleteIndex(RosterdConstants.Search.StaffIndex);
            //await _searchIndexProvider.DeleteIndex(RosterdConstants.Search.JobsIndex);

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
    }
}
