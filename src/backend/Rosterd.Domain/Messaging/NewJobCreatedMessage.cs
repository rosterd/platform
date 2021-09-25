using System;
using Rosterd.Domain.Search;

namespace Rosterd.Domain.Messaging
{
    public sealed class NewJobCreatedMessage : BaseRosterdMessage
    {
        public NewJobCreatedMessage(string jobId, string auth0OrganizationId)
        {
            MessageType = RosterdConstants.Messaging.NewJobCreatedMessage;
            SubjectId = jobId;
            MessageBody = jobId;
            Auth0OrganizationId = auth0OrganizationId;
        }

        /// <summary>
        /// Handy name method to get the job id
        /// </summary>
        public string JobId => SubjectId;
    }
}
