using System.ComponentModel.DataAnnotations;

namespace Rosterd.Domain.Models.FacilitiesModels
{
    public class FacilityModel
    {
        public long FacilityId { get; set; }

        public string FacilityName { get; set; }

        public string OrganizationName { get; set; }
    }
}
