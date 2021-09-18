using System;

namespace Rosterd.Domain.Messaging
{
    public class BaseMessage
    {
        public string Id { get; set; }

        public DateTime MessageTimeUtc { get; set;}

        //Id = new Guid().ToString();
        //EventTime = DateTime.UtcNow;
        //EventType = RosterdConstants.Events.JobCancelledEvent.Format(environmentThisEventIsBeingGenerateFrom);
        //DataVersion = RosterdConstants.Events.Version1;
        //Subject = jobId.ToString();
        //Data = jobId.ToString();
    }
}
