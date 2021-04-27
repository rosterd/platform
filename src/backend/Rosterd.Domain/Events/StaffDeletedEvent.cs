using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Rosterd.Domain.Models.StaffModels;

namespace Rosterd.Domain.Events
{
    public class StaffDeletedEvent : Microsoft.Azure.EventGrid.Models.EventGridEvent
    {
        public StaffDeletedEvent(string environmentThisEventIsBeingGenerateFrom, long staffId)
        {
            Id = new Guid().ToString();
            EventTime = DateTime.UtcNow;
            EventType = Constants.Events.StaffDeletedEvent.Format(environmentThisEventIsBeingGenerateFrom);
            DataVersion = Constants.Events.Version1;
            Subject = staffId.ToString();
            Data = staffId;
        }

    }
}
