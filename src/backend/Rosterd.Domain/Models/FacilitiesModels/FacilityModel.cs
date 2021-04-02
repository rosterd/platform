using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Rosterd.Domain.Models.OrganizationModels;

namespace Rosterd.Domain.Models.FacilitiesModels
{
    public class FacilityModel
    {
        public long FacilityId { get; set; }

        [Required]
        [StringLength(1000)]
        public string FacilityName { get; set; }
        
        [Required]
        [StringLength(1000)]
        public string Address { get; set; }

        [Required]
        [StringLength(1000)]
        public string Suburb { get; set; }

        [Required]
        [StringLength(1000)]
        public string City { get; set; }

        [Required]
        [StringLength(1000)]
        public string Country { get; set; }

        [Required]
        [StringLength(1000)]
        public string PhoneNumber1 { get; set; }

        [StringLength(1000)]
        public string PhoneNumber2 { get; set; }

        public OrganizationModel Organization { get; set; }

        public List<FacilityCapabilityModel> FacilityCapabilities { get; set; }
    }
}
