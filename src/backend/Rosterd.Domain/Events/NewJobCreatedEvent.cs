using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rosterd.Domain.Models.JobModels;
using Rosterd.Domain.Models.StaffModels;

namespace Rosterd.Domain.Events
{
    public class NewJobCreatedEvent : Microsoft.Azure.EventGrid.Models.EventGridEvent
    {
        public NewJobCreatedEvent(string environmentThisEventIsBeingGenerateFrom, JobModel jobModel)
        {
            Id = new Guid().ToString();
            EventTime = DateTime.UtcNow;
            EventType = Constants.Events.NewJobCreatedEvent.Format(environmentThisEventIsBeingGenerateFrom);
            DataVersion = Constants.Events.Version1;
            Subject = jobModel.JobId.ToString();
            Data = jobModel;
        }
    }

}
