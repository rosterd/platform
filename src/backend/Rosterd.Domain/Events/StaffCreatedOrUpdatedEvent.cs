using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Rosterd.Domain.Models.StaffModels;
using Rosterd.Domain.Search;

namespace Rosterd.Domain.Events
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
