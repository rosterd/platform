using System;
using System.Threading.Tasks;
using Azure;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rosterd.AzureFunctions.Config;
using Rosterd.Domain;
using Rosterd.Domain.Search;
using Rosterd.Services.Staff.Interfaces;

namespace Rosterd.AzureFunctions
{
    public class AzureSearchFunctions
    {
        private readonly ILogger<AzureSearchFunctions> _logger;
        private readonly IOptions<FunctionSettings> _settings;

        public AzureSearchFunctions(ILogger<AzureSearchFunctions> logger, IOptions<FunctionSettings> settings)
        {
            _logger = logger;
            _settings = settings;
        }

        /// <summary>
        /// Timer function that runs once a day, the primary purpose is to make sure the search indexes are there and created
        /// </summary>
        /// <param name="myTimer"></param>
        /// <param name="log"></param>
        [FunctionName(nameof(CreateSearchIndexes))]
        public async Task CreateSearchIndexes([TimerTrigger("0 0 0 * * *", RunOnStartup = true)]TimerInfo myTimer, ILogger log)
        {
            _logger.LogInformation($"{nameof(CreateSearchIndexes)} - triggered on UTC Time {DateTime.UtcNow}");

            //Create a SearchIndexClient to create indexes
            var serviceEndpoint = new Uri(_settings.Value.SearchServiceEndpoint);
            var credential = new AzureKeyCredential(_settings.Value.SearchServiceApiKey);
            var searchIndexClient = new SearchIndexClient(serviceEndpoint, credential);

            //Create all the required indexes
            var staffSearchDefinitions = new SearchIndex(RosterdConstants.Search.StaffIndex, new FieldBuilder().Build(typeof(StaffSearchModel)));
            //var jobSearchDefinitions = new SearchIndex(RosterdConstants.Search.StaffIndex, new FieldBuilder().Build(typeof(StaffSearchModel));


            await searchIndexClient.CreateOrUpdateIndexAsync(staffSearchDefinitions);
            //await searchIndexClient.CreateOrUpdateIndexAsync(jobSearchDefinitions);
        }
    }
}
