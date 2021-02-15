using System;
using System.ComponentModel.DataAnnotations;
namespace Rosterd.Domain.Models.Resources
{
    public class JobModel
    {
        public long JobId { get; set; }
        [Required]
        [StringLength(1000)]
        public string JobTitle { get; set; }
        [Required]
        [StringLength(8000)]
        public string Description { get; set; }
        public long FacilityId { get; set; }
        public DateTime JobStartDateTimeUtc { get; set; }
        public DateTime JobEndDateTimeUtc { get; set; }
        [StringLength(1000)]
        public string Comments { get; set; }
        public long? GracePeriodToCancelMinutes { get; set; }
        public bool? NoGracePeriod { get; set; }
    }
}
