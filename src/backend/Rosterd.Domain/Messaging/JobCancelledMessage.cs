using System;
using Azure.Storage.Queues;

namespace Rosterd.Domain.Messaging
{
    public sealed class JobCancelledMessage : BaseMessage
    {
        public JobCancelledMessage(long jobId)
        {
            MessageType = RosterdConstants.Messaging.JobCancelledMessage;
            SubjectId = jobId.ToString();
            MessageBody = jobId.ToString();
        }

        /// <summary>
        /// Handy name method to get the job id
        /// </summary>
        public string JobId => SubjectId;
    }
}
