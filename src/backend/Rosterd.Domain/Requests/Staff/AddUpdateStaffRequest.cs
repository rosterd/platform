using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Rosterd.Domain.Models.SkillsModels;
using Rosterd.Domain.ValidationAttributes;

namespace Rosterd.Domain.Requests.Staff
{
    public class AddUpdateStaffRequest
    {
        public long? StaffId { get; set; }

        [Required]
        [NumberIsRequiredAndShouldBeGreaterThanZero]

        public long? FacilityId { get; set; }

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

        public bool IsActive { get; set; }

        public string JobTitle { get; set; }

        public List<SkillModel> Skills { get; set; }
    }
}
