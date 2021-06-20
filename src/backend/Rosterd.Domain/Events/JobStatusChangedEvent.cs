using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Rosterd.Domain.Enums;
using Rosterd.Domain.Models.StaffModels;

namespace Rosterd.Domain.Events
{
    public class JobStatusChangedEvent : Microsoft.Azure.EventGrid.Models.EventGridEvent
    {
        public JobStatusChangedEvent(string environmentThisEventIsBeingGenerateFrom, long jobId, JobStatus newStatus)
        {
            Id = new Guid().ToString();
            EventTime = DateTime.UtcNow;
            EventType = RosterdConstants.Events.JobStatusChangedEvent.Format(environmentThisEventIsBeingGenerateFrom);
            DataVersion = RosterdConstants.Events.Version1;
            Subject = jobId.ToString();
            Data = newStatus.ToString();
        }
    }
}
