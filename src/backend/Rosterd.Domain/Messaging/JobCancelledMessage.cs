using System;
using Azure.Storage.Queues;

namespace Rosterd.Domain.Messaging
{
    public sealed class JobCancelledMessage : BaseRosterdMessage
    {
        public JobCancelledMessage(long jobId, string auth0OrganizationId)
        {
            MessageType = RosterdConstants.Messaging.JobCancelledMessage;
            SubjectId = jobId.ToString();
            MessageBody = jobId.ToString();
            Auth0OrganizationId = auth0OrganizationId;
        }

        /// <summary>
        /// Handy name method to get the job id
        /// </summary>
        public string JobId => SubjectId;
    }
}
