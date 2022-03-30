using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rosterd.Admin.Api.Services;
using Rosterd.Domain;
using Rosterd.Domain.Enums;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.DashboardModels;
using Rosterd.Domain.Models.JobModels;
using Rosterd.Domain.Settings;
using Rosterd.Infrastructure.Security.Interfaces;
using Rosterd.Services.Dashboards.Interfaces;
using Rosterd.Services.Jobs.Interfaces;
using Rosterd.Services.Staff.Interfaces;
using Rosterd.Web.Infra.Security;

namespace Rosterd.Admin.Api.Controllers
{
        [ApiVersion("1.0")]
        [ApiExplorerSettings(GroupName = "Dashboards")]
        [AuthorizeByRole(RosterdConstants.RosterdRoleNames.FacilityAdmin, RosterdConstants.RosterdRoleNames.OrganizationAdmin, RosterdConstants.RosterdRoleNames.RosterdAdmin)]
        public class DashboardController : BaseApiController
        {
            private readonly ILogger<DashboardController> _logger;
            private readonly IDashboardService _dashboardService;
            private readonly IUserContext _userContext;


            public DashboardController(ILogger<DashboardController> logger, IDashboardService dashboardService, IOptions<AppSettings> appSettings, IUserContext userContext) : base(appSettings)
            {
                _logger = logger;
                _dashboardService = dashboardService;
                _userContext = userContext;

            }

            /// <summary>
            ///     Get Dashboards
            /// </summary>
            /// <returns></returns>
            [HttpGet]
            public async Task<ActionResult<DashboardModel>> GetDashBoard() => await _dashboardService.GetDashBoard(_userContext.UsersAuth0OrganizationId);
        }
}
