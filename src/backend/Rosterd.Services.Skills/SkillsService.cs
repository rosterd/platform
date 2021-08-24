using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rosterd.Data.SqlServer.Context;
using Rosterd.Data.SqlServer.Extensions;
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

        public async Task<PagedList<SkillModel>> GetAllSkills(PagingQueryStringParameters pagingParameters, string auth0OrganizationId)
        {
            var organization = await _context.GetOrganization(auth0OrganizationId);

            var query = _context.Skills.Where(s => s.OrganizationId == organization.OrganizationId);
            var pagedList = await PagingList<Data.SqlServer.Models.Skill>.ToPagingList(query, pagingParameters.PageNumber, pagingParameters.PageSize);

            var domainModels = pagedList.ToDomainModels();
            return new PagedList<SkillModel>(domainModels, pagedList.TotalCount, pagedList.CurrentPage, pagedList.PageSize, pagedList.TotalPages);
        }

        public async Task<SkillModel> GetSkill(long skillId, string auth0OrganizationId)
        {
            var organization = await _context.GetOrganization(auth0OrganizationId);

            var skill = await _context.Skills.FirstOrDefaultAsync(s => s.OrganizationId == organization.OrganizationId && s.SkillId == skillId);
            return skill?.ToDomainModel();
        }

        public async Task CreateSkill(SkillModel skillModel, string auth0OrganizationId)
        {
            await ThrowDuplicateExceptionIfSkillAlreadyExists(skillModel.SkillName);

            var organization = await _context.GetOrganization(auth0OrganizationId);
            var skillToCreate = skillModel.ToNewSkill();
            skillToCreate.OrganizationId = organization.OrganizationId;

            await _context.Skills.AddAsync(skillToCreate);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveSkill(long skillId, string auth0OrganizationId)
        {
            var organization = await _context.GetOrganization(auth0OrganizationId);

            var skill = await _context.Skills.FirstOrDefaultAsync(s => s.OrganizationId == organization.OrganizationId && s.SkillId == skillId);
            if (skill != null)
            {
                _context.Skills.Remove(skill);
                await _context.SaveChangesAsync();
            }
        }
        public async Task UpdateSkill(SkillModel skillModel, string auth0OrganizationId)
        {
            var organization = await _context.GetOrganization(auth0OrganizationId);

            var skillFromDb = await _context.Skills.FirstOrDefaultAsync(s => s.OrganizationId == organization.OrganizationId && s.SkillId == skillModel.SkillId);
            if (skillFromDb == null)
                throw new EntityNotFoundException();

            var skillModelToUpdate = skillModel.ToDataModel(skillFromDb);

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
