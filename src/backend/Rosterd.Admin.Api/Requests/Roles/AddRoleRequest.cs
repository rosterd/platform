using System.ComponentModel.DataAnnotations;
using Rosterd.Domain.Models.SkillsModels;

namespace Rosterd.Admin.Api.Requests.Roles
{
    public class AddRoleRequest
    {
        [Required]
        [StringLength(maximumLength: 25, MinimumLength = 1)]
        public string RoleName { get; set; }

        [Required]
        [StringLength(maximumLength:100, MinimumLength = 1)]
        public string RoleDescription { get; set; }
    }
}
