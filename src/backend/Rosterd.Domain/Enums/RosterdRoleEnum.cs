using System.ComponentModel.DataAnnotations;

namespace Rosterd.Domain.Enums
{
    public enum RosterdRoleEnum
    {
        [Display(Name = "Organization Admin", Description = "")]
        OrganizationAdmin,

        [Display(Name = "Super Admin", Description = "")]
        SuperAdmin,

        [Display(Name = "Facility Admin", Description = "")]
        FacilityAdmin,

        [Display(Name = "Staff", Description = "")]
        Staff
    }
}
