using System.ComponentModel.DataAnnotations;

namespace Rosterd.Domain.Models.Resources
{
    public class FacilityModel
    {
        public long FacilityId { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(1000, MinimumLength = 1)]
        public string FacilityName { get; set; }

        [MaxLength(1000)]
        public string OrganizationName { get; set; }
    }
}
