using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rosterd.Data.SqlServer.Context;
using Rosterd.Data.SqlServer.Helpers;
using Rosterd.Data.SqlServer.Models;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.SkillsModels;
using Rosterd.Domain.Models.StaffModels;
using Rosterd.Services.Staff.Interfaces;

namespace Rosterd.Services.Staff
{
    public class StaffSkillsService : IStaffSkillsService
    {
        private readonly IRosterdDbContext _context;

        public StaffSkillsService(IRosterdDbContext context) => _context = context;

        ///<inheritdoc/>
        public async Task UpdateAllSkillsForStaff(long staffId, List<SkillModel> skillModels)
        {
            if (skillModels.IsNullOrEmpty())
                return;

            foreach (var skillModel in skillModels)
            {
                //Only bother adding this skill if its a valid skill in our db
                var existingSkill = await _context.Skills.FindAsync(skillModel.SkillId);
                if (existingSkill != null)
                {
                    //If the Staff member already has this skill associated then don't need to do anything
                    var alreadyExists = await _context.StaffSkills.AnyAsync(s => s.StaffId == staffId && s.SkillId == skillModel.SkillId);
                    if(alreadyExists)
                        continue;

                    await _context.StaffSkills.AddAsync(new StaffSkill {SkillId = existingSkill.SkillId, SkillName = existingSkill.SkillName, StaffId = staffId});
                }
            }

            await _context.SaveChangesAsync();
        }

    public async Task DeleteSkillsForStaff(long staffId, List<SkillModel> skillModels)
    {
        if (skillModels.IsNullOrEmpty())
            return;

        foreach (var skillModel in skillModels)
        {
            var skill = await _context.StaffSkills.FirstOrDefaultAsync(s => s.SkillId == skillModel.SkillId && s.StaffId == staffId);
            if (skill != null) {
                _context.StaffSkills.Remove(skill);
            }
        }

        await _context.SaveChangesAsync();
    }

        ///<inheritdoc/>
        public async Task RemoveAllSkillsForStaff(long staffId)
        {
            var toDelete = _context.StaffSkills.Where(s => s.StaffId == staffId);
            _context.StaffSkills.RemoveRange(toDelete);

            await _context.SaveChangesAsync();
        }
    }
}
