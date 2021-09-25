using System;
using Rosterd.Domain.Enums;

namespace Rosterd.Domain.Messaging
{
    public sealed class JobStatusChangedMessage : BaseRosterdMessage
    {
        public JobStatusChangedMessage(long jobId, JobStatus newStatus, string auth0OrganizationId)
        {
            MessageType = RosterdConstants.Messaging.JobStatusChangedMessage;
            SubjectId = jobId.ToString();
            MessageBody = newStatus.ToString();
            Auth0OrganizationId = auth0OrganizationId;
        }

        /// <summary>
        /// Handy name method to get the job id
        /// </summary>
        public string JobId => SubjectId;
    }
}
