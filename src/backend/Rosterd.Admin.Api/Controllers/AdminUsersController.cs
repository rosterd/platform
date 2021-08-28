using System;
using System.Collections.Generic;
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
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.AdminUserModels;
using Rosterd.Domain.Models.StaffModels;
using Rosterd.Domain.Settings;
using Rosterd.Infrastructure.Security.Interfaces;
using Rosterd.Services.Staff.Interfaces;
using Rosterd.Web.Infra.Filters.Swagger;
using Rosterd.Web.Infra.Security;
using Rosterd.Web.Infra.ValidationAttributes;

namespace Rosterd.Admin.Api.Controllers
{
    /// <summary>
    /// All actions related to admin users
    /// </summary>
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "Admin User")]
    [AuthorizeByRole(RosterdConstants.RosterdRoleNames.OrganizationAdmin, RosterdConstants.RosterdRoleNames.RosterdAdmin)]
    public class AdminUsersController : BaseApiController
    {
        private readonly ILogger<StaffController> _logger;
        private readonly IAuth0UserService _adminUserService;
        private readonly IStaffEventsService _staffEventsService;
        private readonly IEventGridClient _eventGridClient;
        private readonly IUserContext _userContext;
        private readonly IStaffService _staffService;

        public AdminUsersController(ILogger<StaffController> logger, IAuth0UserService adminUserService,
            IStaffEventsService staffEventsService, IEventGridClient eventGridClient, IOptions<AppSettings> appSettings, IUserContext userContext,
            IStaffService staffService) : base(appSettings)
        {
            _logger = logger;
            _adminUserService = adminUserService;
            _staffEventsService = staffEventsService;
            _eventGridClient = eventGridClient;
            _userContext = userContext;
            _staffService = staffService;
        }

        /// <summary>
        /// Gets a list of all the organization admins for the organization
        /// </summary>
        /// <param name="pagingParameters"></param>
        /// <returns></returns>
        [HttpGet()]
        public async Task<ActionResult<PagedList<Auth0UserModel>>> GetListOfOrganizationAdmins([FromQuery] PagingQueryStringParameters pagingParameters)
        {
            var organizationAdmins = await _adminUserService.GetAdminUsers(_userContext.UsersAuth0OrganizationId, pagingParameters);
            return organizationAdmins;
        }

        /// <summary>
        /// Adds a new organization admin user
        /// </summary>
        /// <param name="request">The admin to add for the organization</param>
        /// <returns></returns>
        [HttpPost("organization-admins")]
        public async Task<ActionResult<Auth0UserModel>> AddOrganizationAdminUser([FromBody] AddAdminUserRequest request)
        {
            if (_userContext.IsUserRosterdAdmin() && request.Auth0OrganizationId.IsNullOrEmpty())
                return BadRequest("You are a RosterdAdmin.  Auth0OrganizationId is required");

            var auth0OrganizationId = _userContext.IsUserRosterdAdmin() ? request.Auth0OrganizationId : _userContext.UsersAuth0OrganizationId;
            var adminUserModel = await _adminUserService.AddOrganizationAdminToAuth0(auth0OrganizationId, request.ToModel());
            return adminUserModel;
        }

        /// <summary>
        /// Update a organization admin user
        /// </summary>
        /// <param name="request">The organization admin member to update</param>
        /// <returns></returns>
        [HttpPut("organization-admins")]
        public async Task<ActionResult<Auth0UserModel>> UpdateOrganizationAdminUser([FromBody] UpdateAdminUserRequest request)
        {
            if (_userContext.IsUserRosterdAdmin() && request.Auth0OrganizationId.IsNullOrEmpty())
                return BadRequest("You are a RosterdAdmin.  Auth0OrganizationId is required");

            var auth0OrganizationId = _userContext.IsUserRosterdAdmin() ? request.Auth0OrganizationId : _userContext.UsersAuth0OrganizationId;
            var adminUserModel = await _adminUserService.UpdateOrganizationAdminInAuth0(auth0OrganizationId, request.ToAuth0UserModel());
            return adminUserModel;
        }

        /// <summary>
        /// Adds a new facility admin user
        /// </summary>
        /// <param name="request">The admin to add for the facility</param>
        /// <returns></returns>
        [HttpPost("facility-admins")]
        public async Task<ActionResult<StaffModel>> AddFacilityAdminUser([FromBody] AddAdminWhoIsAlsoStaffRequest request)
        {
            //1. Create user in auth0
            var adminUserModel = await _adminUserService.AddFacilityAdminToAuth0(_userContext.UsersAuth0OrganizationId, request.ToAdminUserModel());

            //2. Create the staff record in our db
            var staffToCreate = request.ToStaffModel();
            staffToCreate.Auth0Id = adminUserModel.UserAuth0Id;
            var staffCreated = await _staffService.CreateStaff(staffToCreate, _userContext.UsersAuth0OrganizationId);

            return staffCreated;
        }
    }
}
