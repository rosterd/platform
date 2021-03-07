using System.Threading.Tasks;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.StaffModels;

namespace Rosterd.Services.Staff.Interfaces
{
    public interface IStaffService
    {
        /// <summary>
        /// Gets all the staff for a given facility
        /// </summary>
        /// <param name="pagingParameters"></param>
        /// <param name="facilityId"></param>
        /// <returns></returns>
        Task<PagedList<StaffModel>> GetStaffForFacility(PagingQueryStringParameters pagingParameters, int facilityId);

        /// <summary>
        /// Gets all the staff members
        /// </summary>
        /// <param name="pagingParameters"></param>
        /// <returns></returns>
        Task<PagedList<StaffModel>> GetAllStaff(PagingQueryStringParameters pagingParameters);

        /// <summary>
        /// Gets a specific staffmember
        /// </summary>
        /// <param name="staffId"></param>
        /// <returns></returns>
        Task<StaffModel> GetStaff(int staffId);

        /// <summary>
        /// Adds a new staff member
        /// </summary>
        /// <param name="staffModel"></param>
        /// <returns></returns>
        Task CreateStaffMember(StaffModel staffModel);

        /// <summary>
        /// Updates an existing staff member
        /// </summary>
        /// <param name="staffModel"></param>
        /// <returns></returns>
        Task UpdateStaffMember(StaffModel staffModel);

        /// <summary>
        /// Marks a staff member as inactive
        /// </summary>
        /// <param name="staffId"></param>
        /// <returns></returns>
        Task RemoveStaffMember(int staffId);

        /// <summary>
        /// Moves a staff member to another facility
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="facilityId"></param>
        /// <returns></returns>
        Task MoveStaffMemberToAnotherFacility(int staffId, int facilityId);
    }
}
