using System;
using System.ComponentModel.DataAnnotations;

namespace Rosterd.Domain.Models.JobModels
{
    public class JobStatusChangeModel
    {
        public long JobStatusChangeId { get; set; }
        public long JobId { get; set; }
        public long JobStatusId { get; set; }
        public DateTime JobStatusChangeDateTimeUtc { get; set; }
        [StringLength(1000)]
        public string JobStatusChangeReason { get; set; }
    }
}
