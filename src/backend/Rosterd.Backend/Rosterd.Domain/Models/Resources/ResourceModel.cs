using System.ComponentModel.DataAnnotations;

namespace Rosterd.Domain.Models.Resources
{
    public class StaffModel
    {
        public long ResourceId { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(1000, MinimumLength = 1)]
        public string FirstName { get; set; } = null!;

        [Required(AllowEmptyStrings = false)]
        [StringLength(1000, MinimumLength = 1)]
        public string LastName { get; set; } = null!;

        [Required(AllowEmptyStrings = false)]
        [StringLength(1000, MinimumLength = 1)]
        public string Email { get; set; } = null!;

        [StringLength(1000)]
        public string PhoneNumber { get; set; }

        public bool IsActive { get; set; }

        //[Required(AllowEmptyStrings = false)]
        //[StringLength(1000, MinimumLength = 1)]
        //public string Auth0Identifier { get; set; } = null!;

        ///// <summary>
        ///// This is the current role assigned to the staff, ignore this and dont send this to the UI
        ///// for security reasons, that's why it has the JsonIgnore on it
        ///// </summary>
        //[JsonIgnore]
        //public Role ApplicationSecurityRole { get; set; }
    }
}
