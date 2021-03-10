using System;
namespace Rosterd.Domain.Models.Resources
{
    public class JobStatusChangeModel
    {
        public long JobStatusChangeId { get; set; }
        public long JobId { get; set; }
        public long JobStatusId { get; set; }
        public DateTime JobStatusChangeDateTimeUtc { get; set; }
        public string JobStatusChangeReason { get; set; }
    }
}
