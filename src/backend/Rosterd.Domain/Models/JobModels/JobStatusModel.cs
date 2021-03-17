using System.ComponentModel.DataAnnotations;

namespace Rosterd.Domain.Models.JobModels
{
    public class JobStatusModel
    {
        public long JobStatusId { get; set; }
        [Required]
        [StringLength(1000)]
        public string JobStatusName { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
    }
}
