using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Rosterd.Domain.Models.FacilitiesModels;
using Rosterd.Domain.Models.SkillsModels;
using Rosterd.Domain.Models.StaffModels;
using Rosterd.Domain.ValidationAttributes;
using Rosterd.Infrastructure.Extensions;

namespace Rosterd.Admin.Api.Requests.Staff
{
    public class UpdateStaffRequest
    {
        [NumberIsRequiredAndShouldBeGreaterThanZero]
        public long? StaffId { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(1000, MinimumLength = 1)]
        public string FirstName { get; set; }

        [MaxLength(1000)]
        public string MiddleName { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(1000, MinimumLength = 1)]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(1000, MinimumLength = 1)]
        public string Email { get; set; }

        [StringLength(1000)]
        public string HomePhoneNumber { get; set; }

        [StringLength(1000)]
        public string MobilePhoneNumber { get; set; }

        [StringLength(1000)]
        public string OtherPhoneNumber { get; set; }

        [Required]
        public bool? IsActive { get; set; }

        public string JobTitle { get; set; }

        /// <summary>
        /// Skills this staff has
        /// </summary>
        [CollectionIsRequiredAndShouldNotBeEmptyAttribute]
        public List<long> Skills { get; set; }

        /// <summary>
        /// The facilities this staff is associated to
        /// </summary>
        public List<long> Facilities { get; set; }

        public static StaffModel ToStaffModel(UpdateStaffRequest request) =>
            new StaffModel
            {
                StaffId = request.StaffId,
                FirstName = request.FirstName,
                Email = request.Email,
                HomePhoneNumber = request.HomePhoneNumber,
                IsActive = request.IsActive.Value,
                JobTitle = request.JobTitle,
                LastName = request.LastName,
                MiddleName = request.MiddleName,
                MobilePhoneNumber = request.MobilePhoneNumber,
                OtherPhoneNumber = request.OtherPhoneNumber,
                Skills = request.Skills.AlwaysList().Select(s => new SkillModel { SkillId = s }).AlwaysList(),
                StaffFacilities = request.Facilities.AlwaysList().Select(s => new FacilityModel { FacilityId = s }).AlwaysList()
            };
    }
}
