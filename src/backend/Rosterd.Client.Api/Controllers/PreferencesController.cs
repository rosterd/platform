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
        private readonly IStaffService _staffService;
        private readonly IUserContext _userContext;

        public PreferencesController(ILogger<JobsController> logger, IStaffService staffService, IOptions<AppSettings> appSettings, IUserContext userContext) : base(appSettings)
        {
            _logger = logger;
            _staffService = staffService;
            _userContext = userContext;
        }

        /// <summary>
        /// Gets all user profile information for the current user
        /// </summary>
        /// <returns></returns>
        [HttpGet("my")]
        public async Task<ActionResult<StaffAppUserPreferencesModel>> GetUserPreferences() => await _staffService.GetStaffAppUserPreferences(_userContext.UserAuth0Id);

        /// <summary>
        /// Updates all user profile information for the current user
        /// </summary>
        /// <returns></returns>
        [HttpPut("my")]
        public async Task<ActionResult<StaffAppUserPreferencesModel>> UpdateUserPreferences([FromBody] StaffAppUserPreferencesModel staffAppUserPreferencesModel)
        {
            await _staffService.UpdateStaffAppUserPreferences(staffAppUserPreferencesModel, _userContext.UserAuth0Id);
            return Ok();
        }
    }
}
