// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}

using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rosterd.AzureFunctions.Config;
using Rosterd.Domain;
using Rosterd.Domain.Messaging;
using Rosterd.Services.Jobs.Interfaces;
using Rosterd.Services.Staff.Interfaces;

namespace Rosterd.AzureFunctions
{
    public class StorageQueueFunctions
    {
        private readonly IJobEventsService _jobEventsService;
        private readonly ILogger<StorageQueueFunctions> _logger;
        private readonly IOptions<FunctionSettings> _settings;
        private readonly IStaffEventsService _staffEventsService;

        public StorageQueueFunctions(ILogger<StorageQueueFunctions> logger, IOptions<FunctionSettings> settings, IStaffEventsService staffEventsService,
            IJobEventsService jobEventsService)
        {
            _logger = logger;
            _settings = settings;
            _staffEventsService = staffEventsService;
            _jobEventsService = jobEventsService;
        }

        [FunctionName(nameof(ProcessEvent))]
        public async Task ProcessEvent([QueueTrigger("staff-queue", Connection = "FunctionSettings:StorageAccount")] string queueItem, ILogger log)
        {
            //_logger.LogInformation($"{nameof(ProcessEvent)} - triggered on NZ Time {DateTime.UtcNow.ToNzstFromUtc()}");

            var rosterdMessage = JsonSerializer.Deserialize<BaseRosterdMessage>(queueItem);
            if (rosterdMessage == null || rosterdMessage.MessageType.IsNullOrEmpty())
            {
                _logger.LogError($"{nameof(ProcessEvent)} - this is not a Rosterd message, can not process the message");
                return;
            }

            //We currently only process the new job created message
            if (rosterdMessage.MessageType != RosterdConstants.Messaging.NewJobCreatedMessage)
                throw new Exception($"Message type {rosterdMessage.MessageType} can not be processed.");

            //Get a list of all device id's this job is applicable too
            //TODO: 

            //Send push notification to these devices

            _logger.LogInformation(
                $"{nameof(ProcessEvent)} - processing message type {rosterdMessage.MessageType} for organization {rosterdMessage.Auth0OrganizationId} with subject {rosterdMessage.SubjectId}");
        }
    }
}
