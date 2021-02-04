using System.Linq;
using System.Threading.Tasks;
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

        public async Task<PagedList<StaffModel>> GetStaff(PagingQueryStringParameters pagingParameters)
        {
            var query = _context.Resource;
            var pagedList = await PagingHelper<Resource>.ToPagingHelper(query, pagingParameters.PageNumber, pagingParameters.PageSize);


            var domainModelStaff = pagedList.Select(s => new StaffModel {FirstName = s.FirstName, LastName = s.LastName}).ToList();

            return new PagedList<StaffModel>(domainModelStaff, domainModelStaff.Count, pagedList.CurrentPage, pagedList.PageSize,
                pagedList.TotalPages);
        }
    }
}
