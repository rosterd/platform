// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
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
using Rosterd.Domain.Messaging;

using Rosterd.Domain;
using Rosterd.Domain.Search;
//using Rosterd.Services.Jobs.Interfaces;
//using Rosterd.Services.Staff.Interfaces;

namespace Rosterd.AzureFunctions
{
    public class EventConsumerFunctions
    {
        private readonly ILogger<EventConsumerFunctions> _logger;
        private readonly IOptions<FunctionSettings> _settings;
        //private readonly IStaffEventsService _staffEventsService;
        //private readonly IJobEventsService _jobEventsService;

        public EventConsumerFunctions(ILogger<EventConsumerFunctions> logger, IOptions<FunctionSettings> settings)//, IStaffEventsService staffEventsService, IJobEventsService jobEventsService)
        {
            _logger = logger;
            _settings = settings;
            //_staffEventsService = staffEventsService;
            //_jobEventsService = jobEventsService;
        }

        [FunctionName(nameof(ProcessEvent))]
        public async Task ProcessEvent([QueueTrigger("staff-queue", Connection = "FunctionSettings:StorageAccount")] string queueItem, ILogger log)
        {
            _logger.LogInformation($"{nameof(ProcessEvent)} - triggered on UTC Time {DateTime.UtcNow}");

            var rosterdMessage = JsonSerializer.Deserialize<BaseRosterdMessage>(queueItem);
            if (rosterdMessage == null || rosterdMessage.MessageType.IsNullOrEmpty())
            {
                _logger.LogError($"{nameof(ProcessEvent)} - this is not a Rosterd message, can not process the message");
                return;
            }

            _logger.LogInformation(
                $"{nameof(ProcessEvent)} - processing message type {rosterdMessage.MessageType} for organization {rosterdMessage.Auth0OrganizationId} with subject {rosterdMessage.SubjectId}");

            //switch (rosterdMessage.MessageType)
            //{
            //    case

            //    default:
            //        throw new NotSupportedException($" message type {rosterdMessage.MessageType} for organization {rosterdMessage.Auth0OrganizationId} with subject {rosterdMessage.SubjectId} not supported");
            //}
        }

        //TODO:
        //switch (eventGridEvent)
        //{
        //    //New staff created or updated
        //    case { EventType: var eventType } when eventType.Contains(RosterdConstants.Events.StaffCreatedOrUpdatedMessage):
        //    {
        //        await _staffEventsService.HandleStaffCreatedOrUpdatedEvent(eventGridEvent);
        //        break;
        //    }

        //    //Staff deleted
        //    case { EventType: var eventType } when eventType.Contains(RosterdConstants.Events.StaffDeletedEvent):
        //    {
        //        await _staffEventsService.HandleStaffDeletedEvent(eventGridEvent);
        //        break;
        //    }

        //    //Job created
        //    case { EventType: var eventType } when eventType.Contains(RosterdConstants.Events.NewJobCreatedEvent):
        //    {
        //        await _jobEventsService.HandleNewJobCreatedEvent(eventGridEvent);
        //        break;
        //    }

        //    //Job cancelled
        //    case { EventType: var eventType } when eventType.Contains(RosterdConstants.Events.JobCancelledMessage):
        //    {
        //        await _jobEventsService.HandleJobCancelledEvent(eventGridEvent);
        //        break;
        //    }

        //    //Job status changed
        //    case { EventType: var eventType } when eventType.Contains(RosterdConstants.Events.JobStatusChangedEvent):
        //    {
        //        await _jobEventsService.HandleJobStatusChangedEvent(eventGridEvent);
        //        break;
        //    }

        //    default:
        //        throw new NotSupportedException($"EventType : {eventGridEvent.EventType} not supported");

        }
    }

