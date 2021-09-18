using System;
using Rosterd.Domain.Enums;

namespace Rosterd.Domain.Messaging
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

        /// <summary>
        /// Handy name method to get the job id
        /// </summary>
        public string JobId => Subject;
    }
}
