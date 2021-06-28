using System.ComponentModel.DataAnnotations;

namespace Rosterd.Domain.Models.OrganizationModels
{
    public class OrganizationModel
    {

        public long OrganizationId { get; set; }
        public long TenantId { get; set; }

        [Required]
        [StringLength(1000)]
        public string OrganizationName { get; set; }

        [StringLength(1000)]
        public string Address { get; set; }
    }
}
