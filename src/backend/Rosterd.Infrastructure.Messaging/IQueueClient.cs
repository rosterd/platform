
using System;
using System.Threading.Tasks;
using Azure.Storage.Queues;

namespace Rosterd.Infrastructure.Messaging
{
    public interface IQueueClient<T> where T : class
    {
        QueueClient QueueClient { get; }

        /// <summary>
        /// Sends the message to the storage queue and marks the message as never to expire
        /// TimeToLive = -1 in this case to achieve this
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        async Task SendMessageWithNoExpiry<TType>(TType message) => await QueueClient.SendMessageAsync(BinaryData.FromObjectAsJson(message), timeToLive: TimeSpan.FromSeconds(-1));
    }
}
