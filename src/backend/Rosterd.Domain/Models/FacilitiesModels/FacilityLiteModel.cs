using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Rosterd.Domain.Models.OrganizationModels;

namespace Rosterd.Domain.Models.FacilitiesModels
{
    public class FacilityLiteModel
    {
        public long FacilityId { get; set; }

        [Required]
        [StringLength(1000)]
        public string FacilityName { get; set; }
    }
}
