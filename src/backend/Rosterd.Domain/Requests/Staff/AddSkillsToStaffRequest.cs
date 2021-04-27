using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Rosterd.Domain.Models.SkillsModels;

namespace Rosterd.Domain.Requests.Staff
{
    public class AddSkillsToStaffRequest
    {
        [Required]
        public List<SkillModel> SkillsToAdd { get; set; }
    }
}
