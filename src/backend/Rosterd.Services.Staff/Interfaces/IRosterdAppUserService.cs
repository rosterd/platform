using System.Collections.Generic;
using System.Threading.Tasks;
using Rosterd.Data.TableStorage.Models;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.FacilitiesModels;
using Rosterd.Domain.Models.StaffModels;

namespace Rosterd.Services.Staff.Interfaces
{
    public interface IRosterdAppUserService
    {
        /// <summary>
        /// Gets the rosterd app user, if user does not exist a null is returned
        /// </summary>
        /// <param name="userAuth0Id"></param>
        /// <returns></returns>
        Task<RosterdAppUser> GetStaffAppUser(string userAuth0Id);

        /// <summary>
        /// Creates or replaces the existing roster app user
        /// </summary>
        /// <param name="userAuth0Id"></param>
        /// <param name="staffId"></param>
        /// <param name="organizationAuth0Id"></param>
        /// <param name="organizationId"></param>
        Task CreateOrUpdateStaffAppUser(string userAuth0Id, long staffId, string organizationAuth0Id, long organizationId);

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
        /// <param name="staffId"></param>
        /// <returns></returns>
        Task UpdateStaffAppUserPreferences(StaffAppUserPreferencesModel staffAppUserPreferencesModel, string userAuth0Id, long staffId);
    }
}
