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
        /// <returns>Returns the old auth0 id of the staff member</returns>
        Task<string> UpdateStaffToInactive(long staffId, string auth0OrganizationId);

        Task<StaffModel> UpdateStaffToInactive(string auth0Userid, string auth0OrganizationId);

        /// <summary>
        /// Updates the status of a given staff member to active
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="userAuth0Id"></param>
        /// <param name="auth0OrganizationId"></param>
        /// <returns></returns>
        Task UpdateStaffToActive(long staffId, string userAuth0Id, string auth0OrganizationId);

        /// <summary>
        /// Gets the staff's preferences
        /// </summary>
        /// <param name="userAuth0Id"></param>
        /// <returns></returns>
        Task<StaffAppUserPreferencesModel> GetStaffAppUserPreferences(string userAuth0Id);

        /// <summary>
        /// Updates the staff preferences
        /// </summary>
        /// <param name="staffAppUserPreferencesModel"></param>
        /// <param name="userAuth0Id"></param>
        /// <returns></returns>
        Task UpdateStaffAppUserPreferences(StaffAppUserPreferencesModel staffAppUserPreferencesModel, string userAuth0Id);

        /// <summary>
        /// Gets a list of facilities this staff member has access too manage
        /// </summary>
        /// <param name="auth0IdForStaff"></param>
        /// <param name="auth0OrganizationId"></param>
        /// <returns></returns>
        Task<List<FacilityLiteModel>> GetFacilitiesForStaff(string auth0IdForStaff, string auth0OrganizationId);

        /// <summary>
        /// Gets the first facility for the staff member
        /// </summary>
        /// <param name="auth0IdForStaff"></param>
        /// <returns></returns>
        Task<long> GetFacilityForStaffWhoIsFacilityAdmin(string auth0IdForStaff);
    }
}
