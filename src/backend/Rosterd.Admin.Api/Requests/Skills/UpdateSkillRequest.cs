using System.ComponentModel.DataAnnotations;
using Rosterd.Domain.Models.SkillsModels;
using Rosterd.Web.Infra.ValidationAttributes;

namespace Rosterd.Admin.Api.Requests.Skills
{
    public class UpdateSkillRequest
    {
        [Required]
        [ValidNumberRequired]
        public long SkillId { get; set; }

        [Required]
        [StringLength(1000)]
        public string SkillName { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public SkillModel ToSkillModel() => new SkillModel { Description = Description, SkillName = SkillName, SkillId = SkillId };
    }
}
