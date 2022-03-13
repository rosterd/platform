using FluentAssertions;
using Rosterd.Data.SqlServer.Context;
using Rosterd.Data.SqlServer.Models;

namespace Rosterd.UnitTests.Rosterd.Admin.Api.Utilities
{
    public class SkillsDataHelper
    {
        public static void ArrangeSkillsTestData(IRosterdDbContext context)
        {
            var skill1 = new Skill {SkillId = 1, OrganizationId = 1, SkillName = "DotNet", Description = "Progamming Language1"};
            var skill2 = new Skill {SkillId = 2, OrganizationId = 1, SkillName = "Java", Description = "Progamming Language2"};
            var skill3 = new Skill {SkillId = 3, OrganizationId = 1, SkillName = "Javascript", Description = "Progamming Language3"};
            var skill4 = new Skill {SkillId = 4, OrganizationId = 1, SkillName = "Ruby", Description = "Progamming Language4"};
            var skill5 = new Skill {SkillId = 5, OrganizationId = 1, SkillName = "Python", Description = "Progamming Language5"};
            var skill6 = new Skill {SkillId = 6, OrganizationId = 1, SkillName = "Scala", Description = "Progamming Language6"};

            context.Skills.Add(skill1);
            context.Skills.Add(skill2);
            context.Skills.Add(skill3);
            context.Skills.Add(skill4);
            context.Skills.Add(skill5);
            context.Skills.Add(skill6);
            context.SaveChanges();
        }
    }
}
