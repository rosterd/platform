namespace Rosterd.Domain.Models.DashboardModels
{
    public class DashboardModel
    {
        public long TotalJobs { get; set; }

        public long TotalCompletedJobs { get; set; }

        public long TotalExpiredJobs { get; set; }

        public long TotalInprogressJobs { get; set; }

        public long TotalStaff { get; set; }

        public long AmountSaved { get; set; }
    }
}
