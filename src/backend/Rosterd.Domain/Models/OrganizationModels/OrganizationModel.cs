using System.ComponentModel.DataAnnotations;

namespace Rosterd.Domain.Models.OrganizationModels
{
    public class OrganizationModel
    {
        public long? OrganizationId { get; set; }

        public string Auth0OrganizationId { get; set; }

        [Required]
        [StringLength(1000)]
        public string OrganizationName { get; set; }

        [Required]
        [StringLength(1000)]
        public string Auth0OrganizationName { get; set; }

        [StringLength(1000)]
        public string Phone { get; set; }

        [StringLength(1000)]
        public string Address { get; set; }

        [StringLength(1000)]
        public string Comments { get; set; }
    }
}
