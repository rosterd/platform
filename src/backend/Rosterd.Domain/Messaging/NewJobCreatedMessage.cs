using System;
using Rosterd.Domain.Search;

namespace Rosterd.Domain.Messaging
{
    public sealed class NewJobCreatedMessage : BaseMessage
    {
        public NewJobCreatedMessage(string jobId)
        {
            MessageType = RosterdConstants.Messaging.NewJobCreatedMessage;
            SubjectId = jobId;
            MessageBody = BinaryData.FromString(jobId);
        }

        /// <summary>
        /// Handy name method to get the job id
        /// </summary>
        public string JobId => SubjectId;
    }
}
