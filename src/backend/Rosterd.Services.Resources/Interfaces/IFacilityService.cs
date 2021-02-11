using System.Collections.Generic;
using System.Threading.Tasks;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.Resources;
using Microsoft.AspNetCore.Mvc;
namespace Rosterd.Services.Resources.Interfaces
{
    public interface IFacilityService
    {
        Task<PagedList<FacilityModel>> GetFacilities(PagingQueryStringParameters pagingParameters);
        Task<FacilityModel> GetFacilityById(long facilityId);
        Task<IActionResult> DeleteFacility(long staffId);
        Task<IActionResult> PostFacility(FacilityModel facilityModel);
    }
}
