using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosterd.Domain.Enums
{
    public enum JobStatus
    {
        [Display(Name = "Published", Description = "")]
        Published = 1,

        [Display(Name = "Accepted", Description = "")]
        Accepted = 2,

        [Display(Name = "No Show", Description = "")]
        NoShow = 3,

        [Display(Name = "In Progress", Description = "")]
        InProgress = 4,

        [Display(Name = "Feedback Pending", Description = "")]
        FeedbackPending = 5,

        [Display(Name = "Completed", Description = "")]
        Completed = 6,

        [Display(Name = "Expired", Description = "")]
        Expired = 7,

        [Display(Name = "Cancelled", Description = "")]
        Cancelled = 8,
    }
}
