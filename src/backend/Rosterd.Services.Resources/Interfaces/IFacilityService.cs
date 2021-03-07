using System.Collections.Generic;
using System.Threading.Tasks;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.FacilitiesModels;

namespace Rosterd.Services.Resources.Interfaces
{
    public interface IFacilityService
    {
        Task<PagedList<FacilityModel>> GetFacilities(PagingQueryStringParameters pagingParameters);
    }
}
