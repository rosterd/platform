using Rosterd.Data.SqlServer.Context;
using Rosterd.Data.SqlServer.Models;

namespace Rosterd.UnitTests.Rosterd.Admin.Api.Utilities
{
    public class StaffSkillsDataHelper
    {
        public static void ArrangeStaffSkillTestData(IRosterdDbContext context)
        {
            var staffSkill1 = new StaffSkill {StaffId = 1, SkillId = 1};
            var staffSkill2 = new StaffSkill {StaffId = 2, SkillId = 2};
            var staffSkill3 = new StaffSkill {StaffId = 3, SkillId = 3};
            var staffSkill4 = new StaffSkill {StaffId = 4, SkillId = 4};
            var staffSkill5 = new StaffSkill {StaffId = 5, SkillId = 5};
            var staffSkill6 = new StaffSkill {StaffId = 6, SkillId = 6};

            context.StaffSkills.Add(staffSkill1);
            context.StaffSkills.Add(staffSkill2);
            context.StaffSkills.Add(staffSkill3);
            context.StaffSkills.Add(staffSkill4);
            context.StaffSkills.Add(staffSkill5);
            context.StaffSkills.Add(staffSkill6);

            context.SaveChanges();
        }
    }
}
