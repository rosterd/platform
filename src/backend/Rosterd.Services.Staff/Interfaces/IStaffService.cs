using System.Collections.Generic;
using System.Threading.Tasks;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.FacilitiesModels;
using Rosterd.Domain.Models.StaffModels;
using Rosterd.Domain.Models.Users;

namespace Rosterd.Services.Staff.Interfaces
{
    public interface IStaffService
    {
        /// <summary>
        /// Gets all the Staff for a given facility
        /// </summary>
        /// <param name="pagingParameters"></param>
        /// <param name="facilityId"></param>
        /// <returns></returns>
        Task<PagedList<StaffModel>> GetStaffForFacility(PagingQueryStringParameters pagingParameters, long facilityId);

        /// <summary>
        /// Gets all the Staff members
        /// </summary>
        /// <param name="pagingParameters"></param>
        /// <returns></returns>
        Task<PagedList<StaffModel>> GetAllStaff(PagingQueryStringParameters pagingParameters);

        /// <summary>
        /// Gets a specific staffmember
        /// </summary>
        /// <param name="staffId"></param>
        /// <returns></returns>
        Task<StaffModel> GetStaff(long staffId);

        /// <summary>
        /// Adds a new Staff member
        /// </summary>
        /// <param name="staffModel"></param>
        /// <returns>The id of the newly created staff</returns>
        Task<StaffModel> CreateStaff(StaffModel staffModel);

        /// <summary>
        /// Updates an existing Staff member
        /// </summary>
        /// <param name="staffModel"></param>
        /// <returns></returns>
        Task<StaffModel> UpdateStaff(StaffModel staffModel);

        /// <summary>
        /// Marks a Staff member as inactive
        /// </summary>
        /// <param name="staffId"></param>
        /// <returns></returns>
        Task UpdateStaffToInactive(long staffId);

        /// <summary>
        /// Updates the status of a given staff member to active
        /// </summary>
        /// <param name="staffId"></param>
        /// <returns></returns>
        Task UpdateStaffToActive(long staffId);

        /// <summary>
        /// Moves a Staff member to another facility
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="facilityModels"></param>
        /// <returns></returns>
        Task DeleteAllExistingFacilitiesForStaffAndAddNew(long staffId, List<FacilityModel> facilityModels);

        /// <summary>
        /// Associates the facility to the staff member
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="facilityId"></param>
        /// <returns></returns>
        Task AddFacilityToStaff(long staffId, long facilityId);

        /// <summary>
        /// Removes the facility association with a staff member
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="facilityId"></param>
        /// <returns></returns>
        Task RemoveFacilityFromStaff(long staffId, long facilityId);

        /// <summary>
        /// Gets the staff's preferences
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        public Task<UserPreferencesModel> GetStaffPreferences(string userEmail);

        /// <summary>
        /// Updates the staff preferences
        /// </summary>
        /// <param name="userPreferencesModel"></param>
        /// <returns></returns>
        public Task UpdateStaffPreferences(UserPreferencesModel userPreferencesModel);
    }
}
