using System;

namespace Rosterd.Domain.Messaging
{
    public class JobCancelledEvent : Microsoft.Azure.EventGrid.Models.EventGridEvent
    {
        public JobCancelledEvent(string environmentThisEventIsBeingGenerateFrom, long jobId)
        {
            Id = new Guid().ToString();
            EventTime = DateTime.UtcNow;
            EventType = RosterdConstants.Events.JobCancelledEvent.Format(environmentThisEventIsBeingGenerateFrom);
            DataVersion = RosterdConstants.Events.Version1;
            Subject = jobId.ToString();
            Data = jobId.ToString();
        }

    }
}
