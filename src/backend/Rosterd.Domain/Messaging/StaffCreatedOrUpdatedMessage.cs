using System;
using Rosterd.Domain.Search;

namespace Rosterd.Domain.Messaging
{
    public sealed class StaffCreatedOrUpdatedMessage : BaseMessage
    {
        public StaffCreatedOrUpdatedMessage(StaffSearchModel staffSearchModel)
        {
            MessageType = RosterdConstants.Messaging.StaffCreatedOrUpdatedMessage;
            SubjectId = staffSearchModel.StaffId.ToString();
            MessageBody = BinaryData.FromObjectAsJson(staffSearchModel);
        }
    }
}
