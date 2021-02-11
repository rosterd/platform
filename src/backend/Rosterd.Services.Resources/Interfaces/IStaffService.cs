using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.Resources;

namespace Rosterd.Services.Resources.Interfaces
{
    public interface IStaffService
    {
        Task<PagedList<StaffModel>> GetStaff(PagingQueryStringParameters pagingParameters);
        Task<StaffModel> GetStaffById(long staffId);
        Task<IActionResult> PostStaff(StaffModel staffModel);
        Task<IActionResult> DeleteStaff(long staffId);
    }
}
