using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rosterd.Data.SqlServer.Context;
using Rosterd.Data.SqlServer.Helpers;
using Rosterd.Data.SqlServer.Models;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.Resources;
using Rosterd.Services.Resources.Interfaces;

namespace Rosterd.Services.Resources
{
    public class StaffService : IStaffService
    {
        private readonly IRosterdDbContext _context;

        public StaffService(IRosterdDbContext context) => _context = context;

        public Task<IActionResult> DeleteStaff(long staffId) => throw new System.NotImplementedException();

        public async Task<PagedList<StaffModel>> GetStaff(PagingQueryStringParameters pagingParameters)
        {
            var query = _context.Staff;
            var pagedList = await PagingHelper<Staff>.ToPagingHelper(query, pagingParameters.PageNumber, pagingParameters.PageSize);


            var domainModelStaff = pagedList.Select(s => new StaffModel {FirstName = s.FirstName, LastName = s.LastName}).ToList();

            return new PagedList<StaffModel>(domainModelStaff, domainModelStaff.Count, pagedList.CurrentPage, pagedList.PageSize,
                pagedList.TotalPages);
        }

        public Task<StaffModel> GetStaffById(long staffId) => throw new System.NotImplementedException();

        public Task<IActionResult> PostStaff(StaffModel staffModel) => throw new System.NotImplementedException();
    }
}
