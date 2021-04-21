// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;

namespace Rosterd.AzureFunctions
{
    public class JobEventFunctions
    {
        private readonly ILogger<JobEventFunctions> _logger;

        public JobEventFunctions(ILogger<JobEventFunctions> logger)
        {
            _logger = logger;
        }

        [FunctionName(nameof(ProcessNewJobCreatedEvent))]
        public async Task ProcessNewJobCreatedEvent([EventGridTrigger] EventGridEvent eventGridEvent)
        {
            _logger.LogInformation($"{nameof(ProcessNewJobCreatedEvent)} - triggered on UTC Time {DateTime.UtcNow} - {eventGridEvent.Data.ToString()}");
        }
    }
}
