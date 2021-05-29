using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rosterd.Domain.Models.JobModels;
using Rosterd.Domain.Models.StaffModels;
using Rosterd.Domain.Search;

namespace Rosterd.Domain.Events
{
    public class NewJobCreatedEvent : Microsoft.Azure.EventGrid.Models.EventGridEvent
    {
        public NewJobCreatedEvent(string environmentThisEventIsBeingGenerateFrom, JobSearchModel jobModel)
        {
            Id = new Guid().ToString();
            EventTime = DateTime.UtcNow;
            EventType = RosterdConstants.Events.NewJobCreatedEvent.Format(environmentThisEventIsBeingGenerateFrom);
            DataVersion = RosterdConstants.Events.Version1;
            Subject = jobModel.JobId.ToString();
            Data = jobModel;
        }
    }

}
