using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Rosterd.Domain.Models.SkillsModels;
using Rosterd.Domain.Models.StaffModels;

namespace Rosterd.Admin.Api.Requests.Staff
{
    public class AddSkillsToStaffRequest
    {
        [Required]
        public List<SkillModel> SkillsToAdd { get; set; }
    }
}
