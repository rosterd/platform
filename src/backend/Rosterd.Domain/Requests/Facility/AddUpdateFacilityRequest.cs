using Rosterd.Domain.Models.FacilitiesModels;

namespace Rosterd.Domain.Requests.Facility
{
    public class AddUpdateFacilityRequest
    {
        public FacilityModel FacilityToAddOrUpdate { get; set; }
    }
}
