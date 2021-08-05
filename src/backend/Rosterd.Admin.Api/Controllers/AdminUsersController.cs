using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.EventGrid;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestSharp;
using Rosterd.Admin.Api.Requests.AdminUser;
using Rosterd.Admin.Api.Requests.Staff;
using Rosterd.Admin.Api.Services;
using Rosterd.Domain;
using Rosterd.Domain.Models.AdminUserModels;
using Rosterd.Domain.Models.StaffModels;
using Rosterd.Domain.Settings;
using Rosterd.Infrastructure.Security.Interfaces;
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
    [ApiExplorerSettings(GroupName = "Admin User")]
    public class AdminUsersController : BaseApiController
    {
        private readonly ILogger<StaffController> _logger;
        private readonly IAdminUserService _adminUserService;
        private readonly IStaffEventsService _staffEventsService;
        private readonly IEventGridClient _eventGridClient;
        private readonly IUserContext _userContext;

        public AdminUsersController(ILogger<StaffController> logger, IAdminUserService adminUserService,
            IStaffEventsService staffEventsService, IEventGridClient eventGridClient, IOptions<AppSettings> appSettings, IUserContext userContext) : base(appSettings)
        {
            _logger = logger;
            _adminUserService = adminUserService;
            _staffEventsService = staffEventsService;
            _eventGridClient = eventGridClient;
            _userContext = userContext;
        }

        /// <summary>
        /// Adds a new organization admin user
        /// </summary>
        /// <param name="request">The admin to add for the organization</param>
        /// <returns></returns>
        [HttpPost("organization-admins")]
        public async Task<ActionResult<AdminUserModel>> AddOrganizationAdminUser([FromBody] AddAdminUserRequest request)
        {
            //var sampleOrgId = "org_pbP7xVjEopDANVRF";
            var adminUserModel = await _adminUserService.AddOrganizationAdmin(_userContext.UsersAuth0OrganizationId, request.ToModel());
            return adminUserModel;
        }

        /// <summary>
        /// Adds a new facility admin user
        /// </summary>
        /// <param name="request">The admin to add for the facility</param>
        /// <returns></returns>
        [HttpPost("facility-admins")]
        public async Task<ActionResult<AdminUserModel>> AddFacilityAdminUser([FromBody] AddAdminUserRequest request)
        {
            var adminUserModel = await _adminUserService.AddFacilityAdmin(_userContext.UsersAuth0OrganizationId, request.ToModel());
            return adminUserModel;
        }
    }
}
