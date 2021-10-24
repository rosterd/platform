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

namespace Rosterd.AzureFunctions
{
    public class PushNotificationsFunctions
    {
        private readonly ILogger<PushNotificationsFunctions> _logger;
        private readonly IOptions<FunctionSettings> _settings;

        public PushNotificationsFunctions(ILogger<PushNotificationsFunctions> logger, IOptions<FunctionSettings> settings)
        {
            _logger = logger;
            _settings = settings;
        }


    }
}
