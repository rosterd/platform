using System;
using Azure.Storage.Queues;

namespace Rosterd.Infrastructure.Messaging
{
    public class StaffQueueClient : IQueueClient<StaffQueueClient>
    {
        public QueueClient QueueClient { get; }

        public StaffQueueClient(string storageConnectionString, string queueName) => QueueClient = new QueueClient(storageConnectionString, queueName);
    }
}
