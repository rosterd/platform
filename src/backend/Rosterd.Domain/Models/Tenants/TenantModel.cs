using System.ComponentModel.DataAnnotations;
using Rosterd.Domain.Enums;

namespace Rosterd.Domain.Models.Tenants
{
    public class TenantModel
    {
        public long TenantId { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(1000, MinimumLength = 1)]
        public string TenantName { get; set; } = null;

        [StringLength(1000, MinimumLength = 1)]
        public string BusinessName { get; set; } = null;

        [StringLength(1000)]
        public string Description { get; set; }

        [StringLength(1000)]
        public string WebsiteLink { get; set; }
    }
}
