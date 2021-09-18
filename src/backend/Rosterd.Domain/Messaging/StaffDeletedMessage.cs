using System;

namespace Rosterd.Domain.Messaging
{
    public sealed class StaffDeletedMessage : BaseMessage
    {
        public StaffDeletedMessage(long staffId)
        {
            MessageType = RosterdConstants.Messaging.StaffDeletedMessage;
            SubjectId = staffId.ToString();
            MessageBody = BinaryData.FromString(staffId.ToString());
        }
    }
}
