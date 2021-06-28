using System.Threading.Tasks;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.FacilitiesModels;

namespace Rosterd.Services.Facilities.Interfaces
{
    public interface IFacilitiesService
    {
        Task<PagedList<FacilityModel>> GetAllFacilities(PagingQueryStringParameters pagingParameters);

        /// <summary>
        /// Gets a specific facility
        /// </summary>
        /// <param name="facilityId"></param>
        /// <returns></returns>
        Task<FacilityModel> GetFacility(long facilityId);

        /// <summary>
        /// Adds a new facility
        /// </summary>
        /// <param name="facilityModel"></param>
        /// <returns></returns>
        Task<long> CreateFacility(FacilityModel facilityModel);

        /// <summary>
        /// Updates an existing facility
        /// </summary>
        /// <param name="facilityModel"></param>
        /// <returns></returns>
        Task UpdateFacility(FacilityModel facilityModel);

        /// <summary>
        /// Deletes facility
        /// </summary>
        /// <param name="facilityId"></param>
        /// <returns></returns>
        Task RemoveFacility(long facilityId);
    }

}
