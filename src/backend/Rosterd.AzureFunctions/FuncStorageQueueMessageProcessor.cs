// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Expo.Server.Client;
using Expo.Server.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rosterd.AzureFunctions.Config;
using Rosterd.Domain;
using Rosterd.Domain.Messaging;
using Rosterd.Infrastructure.Extensions;
using Rosterd.Services.Jobs.Interfaces;
using Rosterd.Services.Staff.Interfaces;

namespace Rosterd.AzureFunctions
{
    public class FuncStorageQueueMessageProcessor
    {
        private readonly IJobsService _jobsService;
        private readonly ILogger<FuncStorageQueueMessageProcessor> _logger;
        private readonly IOptions<FunctionSettings> _settings;
        private readonly PushApiClient _expoSdkClient;

        public FuncStorageQueueMessageProcessor(ILogger<FuncStorageQueueMessageProcessor> logger, IOptions<FunctionSettings> settings, IJobsService jobsService)
        {
            _logger = logger;
            _settings = settings;
            _jobsService = jobsService;
            _expoSdkClient = new PushApiClient();
        }

        [FunctionName(nameof(ProcessStorageQueueMessages))]
        public async Task ProcessStorageQueueMessages([QueueTrigger("job-queue", Connection = "FunctionSettings:StorageAccount")] string queueItem, ILogger log)
        {
            _logger.LogInformation($"{nameof(ProcessStorageQueueMessages)} - triggered on UTC Time {DateTime.UtcNow}");

            var rosterdMessage = JsonSerializer.Deserialize<BaseRosterdMessage>(queueItem);
            if (rosterdMessage == null || rosterdMessage.MessageType.IsNullOrEmpty())
            {
                _logger.LogError($"{nameof(ProcessStorageQueueMessages)} - this is not a Rosterd message, can not process the message");
                throw new Exception($"{nameof(ProcessStorageQueueMessages)} - this is not a Rosterd message, can not process the message");
            }

            //We currently only process the new job created message
            switch (rosterdMessage.MessageType)
            {
                case RosterdConstants.Messaging.NewJobCreatedMessage:
                {
                    var (job, staffDeviceIds) = await _jobsService.GetRelevantStaffDeviceIdsForJob(rosterdMessage.MessageBody);
                    if (staffDeviceIds.IsNotNullOrEmpty())
                    {
                        var pushTicketReq = new PushTicketRequest()
                        {
                            PushTo = staffDeviceIds.Select(s => $"ExponentPushToken[{s}]").ToList(),
                            PushBadgeCount = 1,
                            PushTitle = "Rosterd New Matching Job Alert",
                            PushBody = $"Found a new matching job for {job.FacilityName} in {job.FacilityCity}"
                        };

                        _logger.LogInformation($"Sending push notifications to device ids {staffDeviceIds.ToDelimitedString()}");
                        var result = await _expoSdkClient.PushSendAsync(pushTicketReq);

                        //TODO: Later (after the mvp) do some error handling for the result
                    }
                    break;
                }

                default:
                    throw new Exception($"Message type {rosterdMessage.MessageType} can not be processed.");
            }
        }
    }
}
