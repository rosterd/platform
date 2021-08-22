using System.Threading.Tasks;
using Rosterd.Data.SqlServer.Models;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.FacilitiesModels;

namespace Rosterd.Services.Facilities.Interfaces
{
    public interface IFacilitiesService
    {
        Task<PagedList<FacilityModel>> GetAllFacilities(PagingQueryStringParameters pagingParameters, string auth0OrganizationId);

        /// <summary>
        /// Gets a specific facility
        /// </summary>
        /// <param name="facilityId"></param>
        /// <param name="auth0OrganizationId"></param>
        /// <returns></returns>
        Task<FacilityModel> GetFacility(long facilityId, string auth0OrganizationId);

        /// <summary>
        /// Adds a new facility
        /// </summary>
        /// <param name="facilityModel"></param>
        /// <param name="auth0OrganizationId"></param>
        /// <returns></returns>
        Task<FacilityModel> CreateFacility(FacilityModel facilityModel, string auth0OrganizationId);

        /// <summary>
        /// Updates an existing facility
        /// </summary>
        /// <param name="facilityModel"></param>
        /// <returns></returns>
        Task<FacilityModel> UpdateFacility(FacilityModel facilityModel);

        /// <summary>
        /// Deletes a facility
        /// </summary>
        /// <param name="facilityId"></param>
        /// <returns></returns>
        Task RemoveFacility(long facilityId);

        /// <summary>
        /// Un-deletes a facility
        /// </summary>
        /// <param name="facilityId"></param>
        /// <returns></returns>
        Task ReactivateFacility(long facilityId);

        Task<bool> DoesFacilityWithSameNameExistForOrganization(FacilityModel facilityModel, string auth0OrganizationId);
    }

}
