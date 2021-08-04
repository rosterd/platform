using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.EventGrid;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rosterd.Admin.Api.Requests.AdminUser;
using Rosterd.Admin.Api.Requests.Staff;
using Rosterd.Domain;
using Rosterd.Domain.Models.AdminUserModels;
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
    [ApiExplorerSettings(GroupName = "Admin User")]
    public class AdminUsersController : BaseApiController
    {
        private readonly ILogger<StaffController> _logger;
        private readonly IAdminUserService _adminUserService;
        private readonly IStaffEventsService _staffEventsService;
        private readonly IEventGridClient _eventGridClient;

        public AdminUsersController(ILogger<StaffController> logger, IAdminUserService adminUserService,
            IStaffEventsService staffEventsService, IEventGridClient eventGridClient, IOptions<AppSettings> appSettings) : base(appSettings)
        {
            _logger = logger;
            _adminUserService = adminUserService;
            _staffEventsService = staffEventsService;
            _eventGridClient = eventGridClient;
        }

        /// <summary>
        /// Gets all the admins for this organization
        /// </summary>
        /// <param name="organizationId">The Organization to get all admins for</param>
        /// <returns></returns>
        [HttpGet("{organizationId}")]
        [OperationOrderAttribute(1)]
        public async Task<ActionResult> GetAllAdminsForOrganization()
        {
            //TODO: once we get org from token
            return null;
        }

        /// <summary>
        /// Adds a new organization admin user
        /// </summary>
        /// <param name="request">The admin to add for the organization</param>
        /// <returns></returns>
        [HttpPost]
        [OperationOrderAttribute(2)]
        public async Task<ActionResult<AdminUserModel>> AddOrganizationAdminUser([FromBody] AddAdminUserRequest request)
        {
            var adminUserModel = await _adminUserService.AddOrganizationAdmin("org_pbP7xVjEopDANVRF", request.ToModel());
            return adminUserModel;

        }
    }
}
