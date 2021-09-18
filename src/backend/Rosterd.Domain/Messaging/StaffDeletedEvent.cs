using System;

namespace Rosterd.Domain.Messaging
{
    public class StaffDeletedEvent : Microsoft.Azure.EventGrid.Models.EventGridEvent
    {
        public StaffDeletedEvent(string environmentThisEventIsBeingGenerateFrom, long staffId)
        {
            Id = new Guid().ToString();
            EventTime = DateTime.UtcNow;
            EventType = RosterdConstants.Events.StaffDeletedEvent.Format(environmentThisEventIsBeingGenerateFrom);
            DataVersion = RosterdConstants.Events.Version1;
            Subject = staffId.ToString();
            Data = staffId;
        }

    }
}
