using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Rosterd.Admin.Api.Services;
using Rosterd.Domain.Enums;
using Rosterd.Domain.Models.AdminUserModels;
using Rosterd.Domain.Models.FacilitiesModels;
using Rosterd.Domain.Models.SkillsModels;
using Rosterd.Domain.Models.StaffModels;
using Rosterd.Infrastructure.Extensions;
using Rosterd.Web.Infra.ValidationAttributes;

namespace Rosterd.Admin.Api.Requests.AdminUser
{
    public class AddAdminUserRequest
    {
        /// <summary>
        /// The auth0 organization id to add this admin too
        /// </summary>
        [StringLength(130, MinimumLength = 1)]
        public string Auth0OrganizationId { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(1000, MinimumLength = 1)]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(1000, MinimumLength = 1)]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(1000, MinimumLength = 1)]
        public string Email { get; set; }

        [StringLength(1000)]
        public string PhoneNumber { get; set; }

        public Auth0UserModel ToModel() => new Auth0UserModel {FirstName = FirstName, LastName = LastName, Email = Email, MobilePhoneNumber = PhoneNumber};

        public StaffModel ToStaffModel() =>
           new StaffModel
           {
               FirstName = FirstName,
               Email = Email,
               IsActive = true,
               JobTitle = RosterdRoleEnum.OrganizationAdmin.ToString(),
               LastName = LastName,
               MobilePhoneNumber = "NA",
               Comments = "NA",

               StaffSkills = new List<SkillModel>(),
               StaffFacilities = new List<FacilityModel>()
           };
    }
}
