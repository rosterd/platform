using System;

namespace Rosterd.Domain.Messaging
{
    /// <summary>
    /// Base messages
    /// AzureStorageQueueMessage - MAX size 64KB, but recommended to be 1KB (max throughput for storage queues  is 2000 messages of 1KB a second for one queue)
    /// </summary>
    public abstract class BaseMessage
    {
        /// <summary>
        /// The unique id of this message
        /// </summary>
        public string Id => new Guid().ToString();

        /// <summary>
        /// The date time (in utc) this message was created
        /// </summary>
        public DateTime MessageCreatedDateTimeUtc => DateTime.UtcNow;

        /// <summary>
        /// The message type
        /// eg: staffCreated, staffUpdated etc
        /// will need to be implemented by the inheriting message classes
        /// </summary>
        public virtual string MessageType { get; set; }

        /// <summary>
        /// The subject Id of the message
        /// The id of the message subject
        /// eg: if its a job related message the subject id will be job id
        /// </summary>
        public virtual string SubjectId { get; set; }

        //The body of the actual message
        public virtual BinaryData MessageBody { get; set; }
    }
}
