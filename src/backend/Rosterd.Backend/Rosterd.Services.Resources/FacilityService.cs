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
    public class FacilityService: IFacilityService
    {
        private readonly IRosterdDbContext _context;

        public FacilityService(IRosterdDbContext context) => _context = context;

        public async Task<PagedList<FacilityModel>> GetFacilities(PagingQueryStringParameters pagingParameters)
        {
            var query = _context.Facility;
            var pagedList = await PagingHelper<Facility>.ToPagingHelper(query, pagingParameters.PageNumber, pagingParameters.PageSize);


            var domainModelFacilities = pagedList.Select(s => new FacilityModel { FacilityName = s.FacilityName}).ToList();

            return new PagedList<FacilityModel>(domainModelFacilities, domainModelFacilities.Count, pagedList.CurrentPage, pagedList.PageSize,
                pagedList.TotalPages);
        }
    }
}