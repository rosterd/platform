using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Rosterd.Domain.Models.SkillsModels;
using Rosterd.Domain.Models.StaffModels;
using Rosterd.Domain.ValidationAttributes;

namespace Rosterd.Admin.Api.Requests.Staff
{
    public class AddUpdateStaffRequest
    {
        public StaffModel Staff { get; set; }
    }
}
