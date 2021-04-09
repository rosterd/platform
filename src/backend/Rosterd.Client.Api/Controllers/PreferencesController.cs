using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.JobModels;
using Rosterd.Domain.Models.Users;
using Rosterd.Services.Jobs.Interfaces;
using Rosterd.Services.Users.Interfaces;
using Rosterd.Web.Infra.Filters.Swagger;

namespace Rosterd.Client.Api.Controllers
{
    /// <summary>
    /// All actions related to the user
    /// </summary>
    [ApiVersion("1.0")]
    public class PreferencesController : BaseApiController
    {
        private readonly ILogger<JobsController> _logger;
        private readonly IUserService _userService;

        public PreferencesController(ILogger<JobsController> logger, IUserService userService) : base()
        {
            _logger = logger;
            _userService = userService;
        }

        /// <summary>
        /// Gets all user profile information for the current user
        /// </summary>
        /// <returns></returns>
        [HttpGet("my")]
        [OperationOrder(1)]
        public async Task<ActionResult<UserPreferencesModel>> GetUserPreferences([FromQuery] string userEmail) => await _userService.GetUserPreferences(userEmail);

        /// <summary>
        /// Updates all user profile information for the current user
        /// </summary>
        /// <returns></returns>
        [HttpPut("my")]
        [OperationOrder(2)]
        public async Task<ActionResult<UserPreferencesModel>> UpdateUserPreferences([FromBody] UserPreferencesModel userPreferencesModel)
        {
            await _userService.UpdateUserPreferences(userPreferencesModel);
            return Ok();
        }
    }
}
