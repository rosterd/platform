
using System;
using Azure.Storage.Queues;

namespace Rosterd.Infrastructure.Messaging
{
    public interface IQueueClient<T> where T : class
    {
        QueueClient QueueClient { get; }
    }
}
