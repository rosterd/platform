using System.Linq;
using System.Threading.Tasks;
using Rosterd.Data.SqlServer.Context;
using Rosterd.Domain.Models.DashboardModels;
using Rosterd.Services.Dashboards.Interfaces;

namespace Rosterd.Services.Dashboards
{
    public class DashboardService: IDashboardService
    {
        private readonly IRosterdDbContext _context;


        public DashboardService(IRosterdDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardModel> GetDashBoard()
        {
            //var publishedJobsCount = _context.Jobs.Count();
            var completedJobsCount = _context.Jobs.Count();
            var staff = _context.Staff.Count();
            var amountSaved = completedJobsCount * 5;

            return new DashboardModel {TotalJobs = completedJobsCount, TotalStaff = "" + staff, AmountSaved = "" + amountSaved};
        }
    }
}
