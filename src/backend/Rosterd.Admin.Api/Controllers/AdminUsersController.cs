using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rosterd.Admin.Api.Requests.AdminUser;
using Rosterd.Admin.Api.Requests.Staff;
using Rosterd.Admin.Api.Services;
using Rosterd.Domain;
using Rosterd.Domain.Enums;
using Rosterd.Domain.Exceptions;
using Rosterd.Domain.Messaging;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.AdminUserModels;
using Rosterd.Domain.Models.StaffModels;
using Rosterd.Domain.Settings;
using Rosterd.Infrastructure.Extensions;
using Rosterd.Infrastructure.Security.Interfaces;
using Rosterd.Services.Organizations.Interfaces;
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
        private readonly IUserContext _userContext;
        private readonly IStaffService _staffService;
        private readonly IAuth0UserService _auth0UserService;
        private readonly IOrganizationsService _organizationsService;

        public AdminUsersController(ILogger<StaffController> logger, IAuth0UserService adminUserService,
            IStaffEventsService staffEventsService, IOptions<AppSettings> appSettings, IUserContext userContext,
            IStaffService staffService, IAuth0UserService auth0UserService, IOrganizationsService organizationsService) : base(appSettings)
        {
            _logger = logger;
            _adminUserService = adminUserService;
            _staffEventsService = staffEventsService;
            _userContext = userContext;
            _staffService = staffService;
            _auth0UserService = auth0UserService;
            _organizationsService = organizationsService;
        }

        /// <summary>
        /// Gets a list of all the organization admins for the organization
        /// </summary>
        /// <param name="pagingParameters"></param>
        /// <returns></returns>
        [HttpGet()]
        public async Task<ActionResult<PagedList<StaffModel>>> GetListOfAdmins([FromQuery] PagingQueryStringParameters pagingParameters)
        {
            var adminUsers = await _staffService.GetAllAdmins(pagingParameters, _userContext.UsersAuth0OrganizationId);
            return adminUsers;
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
                throw new BadRequestException($"You are a RosterdAdmin. Auth0OrganizationId is required");

            var auth0OrganizationId = _userContext.IsUserRosterdAdmin() ? request.Auth0OrganizationId : _userContext.UsersAuth0OrganizationId;
            var adminUserModel = await _adminUserService.AddOrganizationAdminToAuth0(auth0OrganizationId, request.ToModel());

            if (_userContext.IsUserOrganizationAdmin())
            {
                //2. Create the staff record in our db
                var staffToCreate = request.ToStaffModel();
                staffToCreate.Auth0Id = adminUserModel.UserAuth0Id;
                staffToCreate.StaffRole = RosterdRoleEnum.OrganizationAdmin.ToString();

                var staffCreated = await _staffService.CreateStaff(staffToCreate, _userContext.UsersAuth0OrganizationId);
            }

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
                throw new BadRequestException($"You are a RosterdAdmin.  Auth0OrganizationId is required");

            var auth0OrganizationId = _userContext.IsUserRosterdAdmin() ? request.Auth0OrganizationId : _userContext.UsersAuth0OrganizationId;
            var adminUserModel = await _adminUserService.UpdateOrganizationAdminInAuth0(auth0OrganizationId, request.ToAuth0UserModel());

            if (_userContext.IsUserOrganizationAdmin())
            {
                //2. Create the staff record in our db
                var staffToCreate = request.ToStaffModel();
                staffToCreate.Auth0Id = adminUserModel.UserAuth0Id;
                staffToCreate.StaffRole = RosterdRoleEnum.OrganizationAdmin.ToString();

                var staffCreated = await _staffService.UpdateStaff(staffToCreate, _userContext.UsersAuth0OrganizationId);
            }

            return adminUserModel;
        }

        /// <summary>
        /// Removes the organization admin from auth-0
        /// </summary>
        /// <param name="auth0UserId">The admin to remove from auth0</param>
        /// <returns></returns>
        [HttpDelete("organization-admins/{auth0UserId}")]
        public async Task<ActionResult> RemoveOrganizationAdmin([Required] string auth0UserId)
        {
            //Remove from auth0
            await _auth0UserService.RemoveUserFromAuth0(auth0UserId);
            return Ok();
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
            var adminUserModel = request.ToAdminUserModel();
            adminUserModel.Auth0UserMetaData = new Auth0UserMetaData { FacilityIdsCsvString = request.FacilityIds.ToDelimitedString() };
            var auth0AdminUser = await _adminUserService.AddFacilityAdminToAuth0(_userContext.UsersAuth0OrganizationId, adminUserModel);

            //2. Create the staff record in our db
            var staffToCreate = request.ToStaffModel();
            staffToCreate.Auth0Id = auth0AdminUser.UserAuth0Id;
            staffToCreate.StaffRole = RosterdRoleEnum.FacilityAdmin.ToString();
            var staffCreated = await _staffService.CreateStaff(staffToCreate, _userContext.UsersAuth0OrganizationId);

            //3. Send Staff welcome email
            var passwordResetLink = await _auth0UserService.GetPasswordResetLink(auth0AdminUser.UserAuth0Id);
            await _auth0UserService.SendWelcomeEmailToStaff(passwordResetLink, AppSettings.SendGridEmailApiKey,
                $"{staffCreated.FirstName} {staffCreated.LastName}", staffCreated.Email, string.Empty);

            return staffCreated;
        }

        /// <summary>
        /// Removes the facility admin from auth-0 and marks staff as inactive in our db
        /// </summary>
        /// <param name="staffId"></param>
        /// <returns></returns>
        [HttpDelete("facility-admins/{staffId}")]
        public async Task<ActionResult<StaffModel>> RemoveFacilityAdmin([Required] long staffId)
        {
            //1. Mark as not active in our db
            var staffAuth0Id = await _staffService.UpdateStaffToInactive(staffId, _userContext.UsersAuth0OrganizationId);

            //2. Remove from auth0
            await _auth0UserService.RemoveUserFromAuth0(staffAuth0Id);

            await _staffEventsService.HandleStaffDeletedEvent(new StaffDeletedMessage(staffId.ToString(), _userContext.UsersAuth0OrganizationId));
            return Ok();
        }
    }
}
