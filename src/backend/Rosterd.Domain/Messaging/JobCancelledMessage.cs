using System;

namespace Rosterd.Domain.Messaging
{
    public sealed class JobCancelledMessage : BaseMessage
    {
        public JobCancelledMessage(long jobId)
        {
            MessageType = RosterdConstants.Messaging.JobCancelledMessage;
            SubjectId = jobId.ToString();
            MessageBody = BinaryData.FromString(jobId.ToString());
        }
    }
}
