using Rosterd.Data.SqlServer.Context;
using Rosterd.Data.SqlServer.Models;

namespace Rosterd.UnitTests.Rosterd.Admin.Api.Utilities
{
    public class JobSkillsDataHelper
    {
        public static void ArrangeJobSkillTestData(IRosterdDbContext context)
        {
            var jobSkills1 = new JobSkill {SkillId = 1, JobId = 1, JobSkillId = 1};
            var jobSkills2 = new JobSkill {SkillId = 2, JobId = 2, JobSkillId = 2};
            var jobSkills3 = new JobSkill {SkillId = 3, JobId = 3, JobSkillId = 3};
            var jobSkills4 = new JobSkill {SkillId = 4, JobId = 4, JobSkillId = 4};
            var jobSkills5 = new JobSkill {SkillId = 5, JobId = 5, JobSkillId = 5};

            context.JobSkills.Add(jobSkills1);
            context.JobSkills.Add(jobSkills2);
            context.JobSkills.Add(jobSkills3);
            context.JobSkills.Add(jobSkills4);
            context.JobSkills.Add(jobSkills5);

            context.SaveChanges();
        }
    }
}
