using System;
using Rosterd.Domain.Enums;

namespace Rosterd.Domain.Messaging
{
    public sealed class JobStatusChangedMessage : BaseMessage
    {
        public JobStatusChangedMessage(long jobId, JobStatus newStatus)
        {
            MessageType = RosterdConstants.Messaging.JobStatusChangedMessage;
            SubjectId = jobId.ToString();
            MessageBody = newStatus.ToString();
        }

        /// <summary>
        /// Handy name method to get the job id
        /// </summary>
        public string JobId => SubjectId;
    }
}
