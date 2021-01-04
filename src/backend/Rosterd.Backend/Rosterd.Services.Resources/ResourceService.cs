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
    public class ResourceService : IResourceService
    {
        private readonly IRosterdDbContext _context;

        public ResourceService(IRosterdDbContext context) => _context = context;

        public async Task<PagedList<ResourceModel>> GetResources(PagingQueryStringParameters pagingParameters)
        {
            var query = _context.Resource;
            var pagedList = await PagingHelper<Resource>.ToPagingHelper(query, pagingParameters.PageNumber, pagingParameters.PageSize);


            var domainModelResources = pagedList.Select(s => new ResourceModel {FirstName = s.FirstName, LastName = s.LastName}).ToList();

            return new PagedList<ResourceModel>(domainModelResources, domainModelResources.Count, pagedList.CurrentPage, pagedList.PageSize,
                pagedList.TotalPages);
        }
    }
}
