using System.Linq;
using System.Threading.Tasks;
using Rosterd.Data.SqlServer.Context;
using Rosterd.Data.SqlServer.Helpers;
using Rosterd.Data.SqlServer.Models;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.SkillsModels;
using Rosterd.Services.Resources.Interfaces;

namespace Rosterd.Services.Resources
{
    public class SkillService: ISkillService
    {
        private readonly IRosterdDbContext _context;

        public SkillService(IRosterdDbContext context) => _context = context;

        public async Task<PagedList<SkillModel>> GetSkills(PagingQueryStringParameters pagingParameters) => null;
    }
}
