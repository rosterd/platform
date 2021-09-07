using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Rosterd.Domain.Models.AdminUserModels;
using Rosterd.Domain.Models.FacilitiesModels;
using Rosterd.Domain.Models.SkillsModels;
using Rosterd.Domain.Models.StaffModels;
using Rosterd.Infrastructure.Extensions;
using Rosterd.Web.Infra.ValidationAttributes;

namespace Rosterd.Admin.Api.Requests.AdminUser
{
    public class AddAdminWhoIsAlsoStaffRequest
    {
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

        [MaxLength(1000)]
        public string MiddleName { get; set; }

        [StringLength(1000)]
        public string MobilePhoneNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(1000)]
        public string Address { get; set; }

        [StringLength(1000)]
        public string Comments { get; set; }

        [StringLength(1000)]
        public string JobTitle { get; set; }

        /// <summary>
        /// The list of skills that the staff has, when creating should at least have one
        /// </summary>
        [CollectionIsRequiredAndShouldNotBeEmpty]
        public List<long> SkillIds { get; set; }

        /// <summary>
        /// Facilities this user has access too, when creating should at least have one
        /// </summary>
        [CollectionIsRequiredAndShouldNotBeEmpty]
        public List<long> FacilityIds { get; set; }

        public Auth0UserModel ToAdminUserModel() => new Auth0UserModel {FirstName = FirstName, LastName = LastName, Email = Email, MobilePhoneNumber = PhoneNumber};

        public StaffModel ToStaffModel() =>
            new StaffModel
            {
                FirstName = FirstName,
                Email = Email,
                IsActive = true,
                JobTitle = JobTitle,
                LastName = LastName,
                MobilePhoneNumber = MobilePhoneNumber,
                Comments = Comments,

                StaffSkills = SkillIds.AlwaysList().Select(s => new SkillModel { SkillId = s }).AlwaysList(),
                StaffFacilities = FacilityIds.AlwaysList().Select(s => new FacilityModel { FacilityId = s }).AlwaysList()
            };
    }
}
