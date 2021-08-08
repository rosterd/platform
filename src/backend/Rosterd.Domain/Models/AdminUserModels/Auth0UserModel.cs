using System;
using System.ComponentModel.DataAnnotations;

namespace Rosterd.Domain.Models.AdminUserModels
{
    public class Auth0UserModel
    {
        [Required]
        public string UserAuth0Id { get; set; }

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
    }
}
