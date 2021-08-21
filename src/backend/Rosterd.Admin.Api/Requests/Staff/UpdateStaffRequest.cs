using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Rosterd.Domain.Models.FacilitiesModels;
using Rosterd.Domain.Models.SkillsModels;
using Rosterd.Domain.Models.StaffModels;
using Rosterd.Infrastructure.Extensions;
using Rosterd.Web.Infra.ValidationAttributes;

namespace Rosterd.Admin.Api.Requests.Staff
{
    public class UpdateStaffRequest
    {
        [ValidNumberRequired]
        public long? StaffId { get; set; }

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

        [StringLength(1000)]
        public string Comments { get; set; }

        [StringLength(1000)]
        public string JobTitle { get; set; }

        public static StaffModel ToStaffModel(UpdateStaffRequest request) =>
            new StaffModel
            {
                StaffId = request.StaffId,
                FirstName = request.FirstName,
                Email = request.Email,
                JobTitle = request.JobTitle,
                LastName = request.LastName,
                MobilePhoneNumber = request.MobilePhoneNumber,
                Comments = request.Comments
            };
    }
}
