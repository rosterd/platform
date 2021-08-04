using System.ComponentModel.DataAnnotations;

namespace Rosterd.Domain.Enums
{
    public enum RosterdRoleEnum
    {
        /// <summary>
        /// Administrator for a given organization
        /// </summary>
        [Display(Name = "Organization Admin", Description = "")]
        OrganizationAdmin,

        /// <summary>
        /// Super admin for the whole Rosterd platform, will have access to everything
        /// </summary>
        [Display(Name = "Super Admin", Description = "")]
        RosterdAdmin,

        [Display(Name = "Facility Admin", Description = "")]
        FacilityAdmin,

        /// <summary>
        /// A staff user. This user has no access to the admin portal, can only access the client app.
        /// </summary>
        [Display(Name = "Staff", Description = "")]
        Staff
    }
}
