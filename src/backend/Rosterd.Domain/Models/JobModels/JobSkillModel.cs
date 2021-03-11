
using System.ComponentModel.DataAnnotations;

namespace Rosterd.Domain.Models.JobModels
{
    public class JobSkillModel
    {
        public long JobSkillId { get; set; }
        public long JobId { get; set; }
        public long SkillId { get; set; }
        [StringLength(1000)]
        public string SkillName { get; set; }
    }
}
