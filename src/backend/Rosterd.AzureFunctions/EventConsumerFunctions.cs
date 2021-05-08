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
using Rosterd.Services.Staff.Interfaces;

namespace Rosterd.AzureFunctions
{
    public class EventConsumerFunctions
    {
        private readonly ILogger<EventConsumerFunctions> _logger;
        private readonly IOptions<FunctionSettings> _settings;
        private readonly IStaffEventsService _staffEventsService;

        public EventConsumerFunctions(ILogger<EventConsumerFunctions> logger, IOptions<FunctionSettings> settings, IStaffEventsService staffEventsService)
        {
            _logger = logger;
            _settings = settings;
            _staffEventsService = staffEventsService;
        }

        [FunctionName(nameof(ProcessEvent))]
        public async Task ProcessEvent([EventGridTrigger] EventGridEvent eventGridEvent)
        {
            _logger.LogInformation($"{nameof(ProcessEvent)} - triggered on UTC Time {DateTime.UtcNow} - {eventGridEvent.Data.ToString()}");

            switch (eventGridEvent)
            {
                //New staff created or updated
                case {EventType: var eventType} when eventType.Contains(RosterdConstants.Events.StaffCreatedOrUpdatedEvent):
                {
                    await _staffEventsService.HandleStaffCreatedOrUpdatedEvent(eventGridEvent);
                    break;
                }

                //Staff deleted
                case {EventType: var eventType} when eventType.Contains(RosterdConstants.Events.StaffDeletedEvent):
                {
                    await _staffEventsService.HandleStaffDeletedEvent(eventGridEvent);
                    break;
                }
                default:
                    throw new NotSupportedException($"EventType : {eventGridEvent.EventType} not supported");
            }
        }
    }
}
