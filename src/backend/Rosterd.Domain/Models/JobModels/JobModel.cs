using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Rosterd.Domain.Enums;
using Rosterd.Domain.Models.FacilitiesModels;

namespace Rosterd.Domain.Models.JobModels
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

        public FacilityModel Facility { get; set; }

        [Required]
        public DateTime JobStartDateTimeUtc { get; set; }

        [Required]
        public DateTime JobEndDateTimeUtc { get; set; }

        [Required]
        public DateTime JobPostedDateTimeUtc { get; set; }

        [StringLength(1000)]
        public string Comments { get; set; }

        [Required]
        public long? GracePeriodToCancelMinutes { get; set; }

        public bool? NoGracePeriod { get; set; }

        public DateTime? JobGracePeriodEndDateTimeUtc => GracePeriodToCancelMinutes == null
            ? (DateTime?)null
            : JobStartDateTimeUtc.Date.AddMinutes(-GracePeriodToCancelMinutes.Value);

        [Required]
        public JobStatus? JobStatus { get; set; }

        public string JobStatusName { get; set; }

        [StringLength(2000)]
        public string Responsibilities { get; set; }

        [StringLength(2000)]
        public string Experience { get; set; }

        public bool IsNightShift { get; set; }

        public virtual List<JobSkillModel> JobSkills { get; set; }
    }
}
