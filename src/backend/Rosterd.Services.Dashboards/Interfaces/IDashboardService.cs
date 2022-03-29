using System.Threading.Tasks;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.DashboardModels;
using Rosterd.Domain.Models.FacilitiesModels;

namespace Rosterd.Services.Dashboards.Interfaces
{
    public interface IDashboardService
    {
        /// <summary>
        /// Gets Dashboards
        /// </summary>
        /// <returns>DashboardModel</returns>
        Task<DashboardModel> GetDashBoard(string auth0OrganizationId);
    }
}
