using System.Threading.Tasks;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.StaffModels;
using Rosterd.Domain.Requests.Staff;

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
        Task<long> CreateStaffMember(AddUpdateStaffRequest staffModel);

        /// <summary>
        /// Updates an existing Staff member
        /// </summary>
        /// <param name="staffModel"></param>
        /// <returns></returns>
        Task UpdateStaffMember(AddUpdateStaffRequest staffModel);

        /// <summary>
        /// Marks a Staff member as inactive
        /// </summary>
        /// <param name="staffId"></param>
        /// <returns></returns>
        Task RemoveStaffMember(long staffId);

        /// <summary>
        /// Moves a Staff member to another facility
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="facilityId"></param>
        /// <returns></returns>
        Task MoveStaffMemberToAnotherFacility(long staffId, long facilityId);
    }
}
