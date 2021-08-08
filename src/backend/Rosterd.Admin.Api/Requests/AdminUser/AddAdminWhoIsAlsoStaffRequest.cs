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
        /// The facility this staff belongs too
        /// Initially when creating a staff, a staff will need to belong to at least one facility, later you can add more or remove
        /// </summary>
        [ValidNumberRequired]
        public long? FacilityId { get; set; }

        /// <summary>
        /// The list of skills that the staff has, when creating should at least have one
        /// </summary>
        [CollectionIsRequiredAndShouldNotBeEmpty]
        public List<long> SkillIds { get; set; }

        public Auth0UserModel ToAdminUserModel() => new Auth0UserModel {FirstName = FirstName, LastName = LastName, Email = Email, MobilePhoneNumber = PhoneNumber};

        public StaffModel ToStaffModel() =>
            new StaffModel
            {
                FirstName = FirstName,
                Email = Email,
                IsActive = true,
                JobTitle = JobTitle,
                LastName = LastName,
                MiddleName = MiddleName,
                MobilePhoneNumber = MobilePhoneNumber,
                IsAvailable = true,
                DateOfBirth = DateOfBirth,
                Address = Address,
                Comments = Comments,

                StaffFacilities = new List<FacilityModel> { new FacilityModel { FacilityId = FacilityId.Value } },
                StaffSkills = SkillIds.AlwaysList().Select(s => new SkillModel { SkillId = s }).AlwaysList()
            };
    }
}
