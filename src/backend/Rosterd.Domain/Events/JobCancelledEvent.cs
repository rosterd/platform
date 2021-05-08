using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Rosterd.Domain.Models.StaffModels;

namespace Rosterd.Domain.Events
{
    public class JobCancelledEvent : Microsoft.Azure.EventGrid.Models.EventGridEvent
    {
        public JobCancelledEvent(string environmentThisEventIsBeingGenerateFrom, long jobId)
        {
            Id = new Guid().ToString();
            EventTime = DateTime.UtcNow;
            EventType = RosterdConstants.Events.JobDeletedEvent.Format(environmentThisEventIsBeingGenerateFrom);
            DataVersion = RosterdConstants.Events.Version1;
            Subject = jobId.ToString();
            Data = jobId;
        }

    }
}
