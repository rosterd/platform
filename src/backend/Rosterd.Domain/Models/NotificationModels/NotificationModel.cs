using System;
using System.ComponentModel.DataAnnotations;

namespace Rosterd.Domain.Models.NotificationModels
{
    public class NotificationModel
    {

        public long NotificationId { get; set; }

        [Required]
        [StringLength(1000)]
        public string Title { get; set; }

        [Required]
        [StringLength(8000)]
        public string Description { get; set; }

        [Required]
        public DateTime CreatedTime { get; set; }

        public long JobId { get; set; }


        public Boolean IsRead { get; set; }
    }
}
