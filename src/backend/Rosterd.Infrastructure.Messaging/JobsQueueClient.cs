using System;
using Azure.Storage.Queues;

namespace Rosterd.Infrastructure.Messaging
{
    public class JobsQueueClient : IQueueClient<JobsQueueClient>
    {
        public QueueClient QueueClient { get; }

        public JobsQueueClient(string storageConnectionString, string queueName) => QueueClient = new QueueClient(storageConnectionString, queueName, new QueueClientOptions
        {
            MessageEncoding = QueueMessageEncoding.Base64
        });
    }
}
