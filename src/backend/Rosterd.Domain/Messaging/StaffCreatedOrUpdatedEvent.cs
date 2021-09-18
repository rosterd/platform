using System;
using Rosterd.Domain.Search;

namespace Rosterd.Domain.Messaging
{
    public class StaffCreatedOrUpdatedEvent : Microsoft.Azure.EventGrid.Models.EventGridEvent
    {
        public StaffCreatedOrUpdatedEvent(string environmentThisEventIsBeingGenerateFrom, StaffSearchModel staffSearchModel)
        {
            Id = new Guid().ToString();
            EventTime = DateTime.UtcNow;
            EventType = RosterdConstants.Events.StaffCreatedOrUpdatedEvent.Format(environmentThisEventIsBeingGenerateFrom);
            DataVersion = RosterdConstants.Events.Version1;
            Subject = staffSearchModel.StaffId.ToString();
            Data = staffSearchModel;
        }

    }
}
