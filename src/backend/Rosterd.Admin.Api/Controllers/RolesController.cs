using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.EventGrid;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rosterd.Admin.Api.Requests.Roles;
using Rosterd.Admin.Api.Requests.Skills;
using Rosterd.Admin.Api.Requests.Staff;
using Rosterd.Domain;
using Rosterd.Domain.Models.Roles;
using Rosterd.Domain.Models.SkillsModels;
using Rosterd.Domain.Models.StaffModels;
using Rosterd.Domain.Settings;
using Rosterd.Services.Auth.Interfaces;
using Rosterd.Services.Staff.Interfaces;
using Rosterd.Web.Infra.Filters.Swagger;
using Rosterd.Web.Infra.ValidationAttributes;
using PagingQueryStringParameters = Rosterd.Domain.Models.PagingQueryStringParameters;

namespace Rosterd.Admin.Api.Controllers
{
    /// <summary>
    /// All actions related to admin users
    /// </summary>
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "Roles")]
    public class RolesController : BaseApiController
    {
        private readonly ILogger<RolesController> _logger;
        private readonly IRolesService _rolesService;

        public RolesController(ILogger<RolesController> logger, IOptions<AppSettings> appSettings, IRolesService rolesService) : base(appSettings)
        {
            _logger = logger;
            _rolesService = rolesService;
        }

        /// <summary>
        /// Gets all the roles
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [OperationOrder(1)]
        public async Task<ActionResult<List<RosterdRole>>> GetAllRoles() => await _rolesService.GetAllRoles();

        /// <summary>
        /// Get role by Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{roleId}")]
        [OperationOrderAttribute(2)]
        public async Task<ActionResult<RosterdRole>> GetRoleById([Required(AllowEmptyStrings = false)] string roleId) => await _rolesService.GetRole(roleId);

        /// <summary>
        /// Adds a new role
        /// </summary>
        /// <param name="request">The role to add</param>
        /// <returns></returns>
        [HttpPost]
        [OperationOrderAttribute(3)]
        public async Task<ActionResult> AddNewRole([FromBody] AddRoleRequest request)
        {
            await _rolesService.AddRole(new RosterdRole
            {
                RoleName = request.RoleName,
                RoleDescription = request.RoleDescription
            });

            return Ok();
        }

        /// <summary>
        /// Deletes a riven role
        /// </summary>
        /// <param name="roleId">The role to be deleted</param>
        /// <returns></returns>
        [HttpDelete("{roleId}")]
        [OperationOrderAttribute(4)]
        public async Task<ActionResult> RemoveRole([Required(AllowEmptyStrings = false)] string roleId)
        {
            await _rolesService.DeleteRole(roleId);
            return Ok();
        }
    }
}
