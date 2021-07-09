namespace Rosterd.Domain.Models.StaffModels
{
    public class StaffAppUserPreferencesModel
    {
        public string Email { get; set; }

        public string IdmUserName { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string DeviceId { get; set; }

        public AvailableDays AvailableDays { get; set; }

        public Shift Shift { get; set; }

        public bool TurnAllNotificationsOff { get; set; }
    }

    public class AvailableDays
    {
        public bool Monday { get; set; }

        public bool Tuesday { get; set; }

        public bool Wednesday { get; set; }

        public bool Thursday { get; set; }

        public bool Friday { get; set; }

        public bool Saturday { get; set; }

        public bool Sunday { get; set; }
    }

    public class Shift
    {
        public bool DayShift { get; set; }

        public bool NightShift { get; set; }
    }
}
