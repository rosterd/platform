using System;
using System.Collections.Generic;
namespace Rosterd.Domain.Models.Resources
{
    public class JobModel
    {
        
        public long JobId { get; set; }
        
        public string JobTitle { get; set; }
        
        public string Description { get; set; }

        public long FacilityId { get; set; }
        
        public DateTime JobStartDateTimeUtc { get; set; }
       
        public DateTime JobEndDateTimeUtc { get; set; }
       
        public string Comments { get; set; }

        public long? GracePeriodToCancelMinutes { get; set; }

        public bool? NoGracePeriod { get; set; }

        public ICollection<JobSkillModel> JobSkills { get; set; }
      
        public ICollection<JobStatusChangeModel> JobStatusChanges { get; set; }
    }
}
