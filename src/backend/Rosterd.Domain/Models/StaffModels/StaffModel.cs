using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Rosterd.Domain.Models.FacilitiesModels;
using Rosterd.Domain.Models.SkillsModels;

namespace Rosterd.Domain.Models.StaffModels
{
    public class StaffModel
    {
        public long? StaffId { get; set; }

        public string Auth0Id { get; set; }

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
        public string MobilePhoneNumber { get; set; }

        public bool IsActive { get; set; }

        [StringLength(1000)]
        public string Comments { get; set; }

        [StringLength(1000)]
        public string JobTitle { get; set; }

        public List<SkillModel> StaffSkills { get; set; }
    }
}
