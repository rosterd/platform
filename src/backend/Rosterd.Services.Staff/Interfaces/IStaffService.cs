using System.Collections.Generic;
using System.Threading.Tasks;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.FacilitiesModels;
using Rosterd.Domain.Models.StaffModels;

namespace Rosterd.Services.Staff.Interfaces
{
    public interface IStaffService
    {
        /// <summary>
        /// Gets all the Staff for a given facility
        /// </summary>
        /// <param name="pagingParameters"></param>
        /// <param name="facilityId"></param>
        /// <param name="auth0OrganizationId"></param>
        /// <returns></returns>
        Task<PagedList<StaffModel>> GetStaffForFacility(PagingQueryStringParameters pagingParameters, long facilityId, string auth0OrganizationId);

        /// <summary>
        /// Gets all the Staff members
        /// </summary>
        /// <param name="pagingParameters"></param>
        /// <param name="auth0OrganizationId"></param>
        /// <returns></returns>
        Task<PagedList<StaffModel>> GetAllStaff(PagingQueryStringParameters pagingParameters, string auth0OrganizationId);

        /// <summary>
        /// Gets a specific staff member
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="auth0OrganizationId"></param>
        /// <returns></returns>
        Task<StaffModel> GetStaff(long staffId, string auth0OrganizationId);

        /// <summary>
        /// Adds a new Staff member
        /// </summary>
        /// <param name="staffModel"></param>
        /// <param name="auth0OrganizationId"></param>
        /// <returns>The id of the newly created staff</returns>
        Task<StaffModel> CreateStaff(StaffModel staffModel, string auth0OrganizationId);

        /// <summary>
        /// Updates an existing Staff member
        /// </summary>
        /// <param name="staffModel"></param>
        /// <param name="auth0OrganizationId"></param>
        /// <returns></returns>
        Task<StaffModel> UpdateStaff(StaffModel staffModel, string auth0OrganizationId);

        /// <summary>
        /// Marks a Staff member as inactive
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="auth0OrganizationId"></param>
        /// <returns></returns>
        Task<StaffModel> UpdateStaffToInactive(long staffId, string auth0OrganizationId);

        /// <summary>
        /// Updates the status of a given staff member to active
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="auth0OrganizationId"></param>
        /// <returns></returns>
        Task UpdateStaffToActive(long staffId, string auth0OrganizationId);

        /// <summary>
        /// Associates the facility to the staff member
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="facilityId"></param>
        /// <param name="auth0OrganizationId"></param>
        /// <returns></returns>
        Task AddFacilityToStaff(long staffId, long facilityId, string auth0OrganizationId);

        /// <summary>
        /// Removes the facility association with a staff member
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="facilityId"></param>
        /// <param name="auth0OrganizationId"></param>
        /// <returns></returns>
        Task RemoveFacilityFromStaff(long staffId, long facilityId, string auth0OrganizationId);

        /// <summary>
        /// Gets the staff's preferences
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        public Task<StaffAppUserPreferencesModel> GetStaffAppUserPreferences(string userEmail);

        /// <summary>
        /// Updates the staff preferences
        /// </summary>
        /// <param name="staffAppUserPreferencesModel"></param>
        /// <returns></returns>
        public Task UpdateStaffAppUserPreferences(StaffAppUserPreferencesModel staffAppUserPreferencesModel);
    }
}
