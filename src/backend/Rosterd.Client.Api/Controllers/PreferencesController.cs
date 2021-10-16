using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rosterd.Client.Api.Services;
using Rosterd.Domain;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.JobModels;
using Rosterd.Domain.Models.StaffModels;
using Rosterd.Services.Jobs.Interfaces;
using Rosterd.Services.Staff.Interfaces;
using Rosterd.Web.Infra.Filters.Swagger;
using Rosterd.Web.Infra.Security;

namespace Rosterd.Client.Api.Controllers
{
    /// <summary>
    /// All actions related to the user
    /// </summary>
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "Preferences")]
    [AuthorizeByRole(RosterdConstants.RosterdRoleNames.Staff)]
    public class PreferencesController : BaseApiController
    {
        private readonly ILogger<JobsController> _logger;
        private readonly IRosterdAppUserService _appUserService;

        public PreferencesController(ILogger<JobsController> logger, IRosterdAppUserService appUserService, IOptions<AppSettings> appSettings, IUserContext userContext) : base(userContext)
        {
            _logger = logger;
            _appUserService = appUserService;
        }

        /// <summary>
        /// Gets all user profile information for the current user
        /// </summary>
        /// <returns></returns>
        [HttpGet("my")]
        public async Task<ActionResult<StaffAppUserPreferencesModel>> GetUserPreferences() => await _appUserService.GetStaffAppUserPreferences(UserContext.UserAuth0Id);

        /// <summary>
        /// Updates all user profile information for the current user
        /// </summary>
        /// <returns></returns>
        [HttpPut("my")]
        public async Task<ActionResult<StaffAppUserPreferencesModel>> UpdateUserPreferences([FromBody] StaffAppUserPreferencesModel staffAppUserPreferencesModel)
        {
            await _appUserService.UpdateStaffAppUserPreferences(staffAppUserPreferencesModel, UserContext.UserAuth0Id, UserContext.UserStaffId);
            return Ok();
        }
    }
}
