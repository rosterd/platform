using System;
using Rosterd.Domain.Models.FacilitiesModels;

namespace Rosterd.Admin.Api.Requests.Facility
{
    public class AddUpdateFacilityRequest
    {
        public FacilityModel FacilityToAddOrUpdate { get; set; }
    }
}
