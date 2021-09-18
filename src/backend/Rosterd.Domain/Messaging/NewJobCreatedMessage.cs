using System;
using Rosterd.Domain.Search;

namespace Rosterd.Domain.Messaging
{
    public sealed class NewJobCreatedMessage : BaseMessage
    {
        public NewJobCreatedMessage(JobSearchModel jobModel)
        {
            MessageType = RosterdConstants.Messaging.NewJobCreatedMessage;
            SubjectId = jobModel.JobId;
            MessageBody = BinaryData.FromObjectAsJson(jobModel);
        }
    }
}
