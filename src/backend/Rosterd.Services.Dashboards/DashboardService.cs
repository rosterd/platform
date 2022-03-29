using System.Linq;
using System.Threading.Tasks;
using Rosterd.Data.SqlServer.Context;
using Rosterd.Domain.Enums;
using Rosterd.Domain.Models.DashboardModels;
using Rosterd.Infrastructure.Security.Interfaces;
using Rosterd.Services.Dashboards.Interfaces;

namespace Rosterd.Services.Dashboards
{
    public class DashboardService: IDashboardService
    {
        private readonly IRosterdDbContext _context;
        private readonly IBelongsToValidator _belongsToValidator;

        public DashboardService(IRosterdDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardModel> GetDashBoard(string auth0OrganizationId)
        {
            var completedStatus = (long) JobStatus.Completed;
            var expiredStatus = (long) JobStatus.Expired;
            var inProgressStatus = (long) JobStatus.InProgress;

            var organization = await _belongsToValidator.ValidateOrganizationExistsAndGetIfValid(auth0OrganizationId);

            //Get a count for all the three job status's
            var completedJobsCount = _context.Jobs.Count(s => s.OrganizationId == organization.OrganizationId && s.JobStatusId == completedStatus);
            var expiredJobsCount = _context.Jobs.Count(s => s.OrganizationId == organization.OrganizationId && s.JobStatusId == expiredStatus);
            var inprogressJobsCount = _context.Jobs.Count(s => s.OrganizationId == organization.OrganizationId && s.JobStatusId == inProgressStatus);
            var totalJobs = _context.Jobs.Count(s => s.OrganizationId == organization.OrganizationId);
            
            //Total staff registered for the organization
            var staff = _context.Staff.Count(s => s.OrganizationId == organization.OrganizationId);
            var amountSaved = completedJobsCount * 8 * 5;

            return new DashboardModel
            {
                TotalJobs = totalJobs,
                TotalStaff = staff,
                AmountSaved = amountSaved,
                TotalCompletedJobs = completedJobsCount,
                TotalExpiredJobs = expiredJobsCount,
                TotalInprogressJobs = inprogressJobsCount
            };
        }
    }
}
