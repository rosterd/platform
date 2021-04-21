using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rosterd.Data.SqlServer.Context;
using Rosterd.Data.SqlServer.Helpers;
using Rosterd.Domain.Exceptions;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.SkillsModels;
using Rosterd.Services.Mappers;
using Rosterd.Services.Skills.Interfaces;

namespace Rosterd.Services.Skills
{
    public class SkillsService : ISkillsService
    {
        private readonly IRosterdDbContext _context;

        public SkillsService(IRosterdDbContext context) => _context = context;

        public async Task<PagedList<SkillModel>> GetAllSkills(PagingQueryStringParameters pagingParameters)
        {
            var query = _context.Skills;
            var pagedList = await PagingList<Data.SqlServer.Models.Skill>.ToPagingList(query, pagingParameters.PageNumber, pagingParameters.PageSize);

            var domainModels = pagedList.ToDomainModels();
            return new PagedList<SkillModel>(domainModels, pagedList.TotalCount, pagedList.CurrentPage, pagedList.PageSize, pagedList.TotalPages);
        }

        public async Task<SkillModel> GetSkill(long skillId)
        {
            var skill = await _context.Skills.FindAsync(skillId);
            return skill?.ToDomainModel();
        }

        public async Task CreateSkill(SkillModel skillModel)
        {
            await ThrowDuplicateExceptionIfSkillAlreadyExists(skillModel.SkillName);

            var skillToCreate = skillModel.ToNewSkill();

            await _context.Skills.AddAsync(skillToCreate);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveSkill(long skillId)
        {
            var skill = await _context.Skills.FindAsync(skillId);
            if (skill != null)
            { 
                _context.Skills.Remove(skill);
                await _context.SaveChangesAsync();
            }
        }
        public async Task UpdateSkill(SkillModel skillModel)
        {
            await ThrowDuplicateExceptionIfSkillAlreadyExists(skillModel.SkillName);

            var skillModelToUpdate = skillModel.ToDataModel();

            _context.Skills.Update(skillModelToUpdate);
            await _context.SaveChangesAsync();
        }

        private async Task ThrowDuplicateExceptionIfSkillAlreadyExists(string skillName)
        {
            var existingSkillsWithSameName = await _context.Skills.AnyAsync(s => s.SkillName == skillName.ToLower());
            if(existingSkillsWithSameName)
                throw new EntityAlreadyExistsException();
        }
    }
}
