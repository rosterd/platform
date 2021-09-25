using System;
using Rosterd.Domain.Search;

namespace Rosterd.Domain.Messaging
{
    public sealed class StaffCreatedOrUpdatedMessage : BaseRosterdMessage
    {
        public StaffCreatedOrUpdatedMessage(string staffId, string auth0OrganizationId)
        {
            MessageType = RosterdConstants.Messaging.StaffCreatedOrUpdatedMessage;
            SubjectId = staffId;
            MessageBody = staffId;
            Auth0OrganizationId = auth0OrganizationId;
        }

        /// <summary>
        /// Handy name method to get the staff id
        /// </summary>
        public string StaffId => SubjectId;
    }
}
