using System.ComponentModel.DataAnnotations;

namespace Rosterd.Domain.Enums
{
    public enum ClientNotificationPreference
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
