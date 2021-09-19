using System;

namespace Rosterd.Domain.Messaging
{
    public sealed class StaffDeletedMessage : BaseMessage
    {
        public StaffDeletedMessage(string staffId, string auth0OrganizationId)
        {
            MessageType = RosterdConstants.Messaging.StaffDeletedMessage;
            SubjectId = staffId;
            MessageBody = staffId;
            Auth0OrganizationId = auth0OrganizationId;
        }

        // <summary>
        /// Handy name method to get the staff id
        /// </summary>
        public string StaffId => SubjectId;
    }
}
