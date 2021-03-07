using System.Threading.Tasks;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.FacilitiesModels;

namespace Rosterd.Services.Facilities.Interfaces
{
    public interface IFacilitiesService
    {
        Task<PagedList<FacilityModel>> GetFacilities(PagingQueryStringParameters pagingParameters);
    }
}
