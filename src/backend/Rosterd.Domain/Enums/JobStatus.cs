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
        [Display(Name = "No Notifications")]
        NoNotifications = 1,

        [Display(Name = "Email")]
        Email,

        [Display(Name = "SMS")]
        Sms,

        [Display(Name = "Email & SMS")]
        EmailAndSms
    }
}
