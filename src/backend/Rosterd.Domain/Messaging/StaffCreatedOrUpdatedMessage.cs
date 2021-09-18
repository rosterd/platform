using System;
using Rosterd.Domain.Search;

namespace Rosterd.Domain.Messaging
{
    public sealed class StaffCreatedOrUpdatedMessage : BaseMessage
    {
        public StaffCreatedOrUpdatedMessage(string staffId)
        {
            MessageType = RosterdConstants.Messaging.StaffCreatedOrUpdatedMessage;
            SubjectId = staffId;
            MessageBody = staffId;
        }

        /// <summary>
        /// Handy name method to get the staff id
        /// </summary>
        public string StaffId => SubjectId;
    }
}
