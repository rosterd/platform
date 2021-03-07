using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        public Task<IActionResult> DeleteSkill(long skillId) => throw new System.NotImplementedException();
        public Task<SkillModel> GetSkillById(long skillId) => throw new System.NotImplementedException();

        public async Task<PagedList<SkillModel>> GetSkills(PagingQueryStringParameters pagingParameters) 
        {
            var query = _context.Skill;
            var pagedList = await PagingHelper<Skill>.ToPagingHelper(query, pagingParameters.PageNumber, pagingParameters.PageSize);


            var domainModelSkill = pagedList.Select(s => new SkillModel {SkillId = s.SkillId}).ToList();

            return new PagedList<SkillModel>(domainModelSkill, domainModelSkill.Count, pagedList.CurrentPage, pagedList.PageSize,
                pagedList.TotalPages);
        }

        public Task<IActionResult> PostSkill(SkillModel skillModel) => throw new System.NotImplementedException();
    }
}
