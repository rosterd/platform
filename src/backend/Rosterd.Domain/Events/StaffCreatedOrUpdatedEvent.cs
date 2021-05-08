using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Rosterd.Domain.Models.StaffModels;

namespace Rosterd.Domain.Events
{
    public class StaffCreatedOrUpdatedEvent : Microsoft.Azure.EventGrid.Models.EventGridEvent
    {
        public StaffCreatedOrUpdatedEvent(string environmentThisEventIsBeingGenerateFrom, StaffModel staffModel)
        {
            Id = new Guid().ToString();
            EventTime = DateTime.UtcNow;
            EventType = RosterdConstants.Events.StaffCreatedOrUpdatedEvent.Format(environmentThisEventIsBeingGenerateFrom);
            DataVersion = RosterdConstants.Events.Version1;
            Subject = staffModel.StaffId.ToString();
            Data = staffModel;
        }

    }
}
