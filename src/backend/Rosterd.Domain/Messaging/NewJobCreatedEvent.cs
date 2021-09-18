using System;
using Rosterd.Domain.Search;

namespace Rosterd.Domain.Messaging
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
