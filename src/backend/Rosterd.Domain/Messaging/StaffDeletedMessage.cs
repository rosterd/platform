using System;

namespace Rosterd.Domain.Messaging
{
    public sealed class StaffDeletedMessage : BaseMessage
    {
        public StaffDeletedMessage(string staffId)
        {
            MessageType = RosterdConstants.Messaging.StaffDeletedMessage;
            SubjectId = staffId;
            MessageBody = staffId;
        }

        // <summary>
        /// Handy name method to get the staff id
        /// </summary>
        public string StaffId => SubjectId;
    }
}
