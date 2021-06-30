using System.ComponentModel.DataAnnotations;
using Rosterd.Domain.ValidationAttributes;

namespace Rosterd.Domain.Models.OrganizationModels
{
    public class OrganizationModel
    {
        public long? OrganizationId { get; set; }

        public long? TenantId { get; set; }

        [Required]
        [StringLength(1000)]
        public string OrganizationName { get; set; }

        [Required]
        [StringLength(1000)]
        public string Address { get; set; }
    }
}
