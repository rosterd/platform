using System.Linq;
using System.Threading.Tasks;
using Rosterd.Data.SqlServer.Context;
using Rosterd.Data.SqlServer.Helpers;
using Rosterd.Data.SqlServer.Models;
using Rosterd.Domain;
using Rosterd.Domain.Models.FacilitiesModels;
using Rosterd.Services.Facilities.Interfaces;

namespace Rosterd.Services.Facilities
{
    public class FacilitiesService: IFacilitiesService
    {
        private readonly IRosterdDbContext _context;

        public FacilitiesService(IRosterdDbContext context) => _context = context;


        public Task<Domain.Models.PagedList<FacilityModel>> GetFacilities(Domain.Models.PagingQueryStringParameters pagingParameters)
        {
            //TODO:
            return null;
        }
    }
}
