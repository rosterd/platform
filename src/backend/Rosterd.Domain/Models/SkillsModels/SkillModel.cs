using System.ComponentModel.DataAnnotations;

namespace Rosterd.Domain.Models.SkillsModels
{
    public class SkillModel
    {
        public long SkillId { get; set; }

        [Required]
        [StringLength(1000)]
        public string SkillName { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }
    }
}
