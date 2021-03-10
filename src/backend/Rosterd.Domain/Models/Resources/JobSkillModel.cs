using System;
namespace Rosterd.Domain.Models.Resources
{
    public class JobSkillModel
    {
        public long JobSkillId { get; set; }
        public long JobId { get; set; }
        public long SkillId { get; set; }
        public string SkillName { get; set; }
    }
}
