using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rosterd.Admin.Api.Requests.Staff;
using Rosterd.Admin.Api.Services;
using Rosterd.Domain;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.FacilitiesModels;
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
    /// All actions related to staff
    /// </summary>
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "Staff")]
    [AuthorizeByRole(RosterdConstants.RosterdRoleNames.FacilityAdmin, RosterdConstants.RosterdRoleNames.OrganizationAdmin, RosterdConstants.RosterdRoleNames.RosterdAdmin)]
    public class StaffController : BaseApiController
    {
        private readonly ILogger<StaffController> _logger;
        private readonly IStaffService _staffService;
        private readonly IStaffSkillsService _staffSkillsService;
        private readonly IStaffEventsService _staffEventsService;
        private readonly IAuth0UserService _auth0UserService;
        private readonly IUserContext _userContext;
        private readonly IBelongsToValidator _belongsToValidator;

        public StaffController(ILogger<StaffController> logger, IStaffService staffService, IStaffSkillsService staffSkillsService, IStaffEventsService staffEventsService, IOptions<AppSettings> appSettings, IAuth0UserService auth0UserService, IUserContext userContext, IBelongsToValidator belongsToValidator) : base(appSettings)
        {
            _logger = logger;
            _staffService = staffService;
            _staffSkillsService = staffSkillsService;
            _staffEventsService = staffEventsService;
            _auth0UserService = auth0UserService;
            _userContext = userContext;
            _belongsToValidator = belongsToValidator;
        }

        /// <summary>
        /// Gets all the resources
        /// </summary>
        /// <param name="pagingParameters"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<Domain.Models.PagedList<StaffModel>>> GetAllStaff([FromQuery] PagingQueryStringParameters pagingParameters)
        {
            pagingParameters ??= new PagingQueryStringParameters();

            var pagedList = await _staffService.GetAllStaff(pagingParameters, _userContext.UsersAuth0OrganizationId);
            return pagedList;
        }

        /// <summary>
        /// Get Staff by Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<StaffModel>> GetStaffById([ValidNumberRequired] long? id)
        {
            var staffModel = await _staffService.GetStaff(id.Value, _userContext.UsersAuth0OrganizationId);
            return staffModel;
        }

        /// <summary>
        /// Adds a new Staff member
        /// </summary>
        /// <param name="request">The Staff member to add</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<StaffModel>> AddNewStaffMember([FromBody] AddStaffRequest request)
        {
            //Validation checks before we create the user in Auth0
            await _belongsToValidator.ValidateOrganizationExistsAndGetIfValid(_userContext.UsersAuth0OrganizationId);
            await _belongsToValidator.ValidateSkillsBelongsToOrganization(request.SkillIds, _userContext.UsersAuth0OrganizationId);

            //1. Create the staff in auth0
            var userCreatedInAuth0 = await _auth0UserService.AddStaffToAuth0(_userContext.UsersAuth0OrganizationId, request.FirstName, request.LastName, request.Email, request.MobilePhoneNumber, _userContext.UsersAuth0OrganizationId);

            //2. Create the staff in our db
            var staffToCreateInDb = request.ToStaffModel();
            staffToCreateInDb.Auth0Id = userCreatedInAuth0.UserAuth0Id;
            var staff = await _staffService.CreateStaff(staffToCreateInDb, _userContext.UsersAuth0OrganizationId);

            //Generate a new staff created event
            await _staffEventsService.GenerateStaffCreatedOrUpdatedEvent(staff.StaffId.Value);
            return staff;
        }

        /// <summary>
        /// Update a Staff member details
        /// </summary>
        /// <param name="request">The Staff member to update</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult<StaffModel>> UpdateStaffMember([FromBody] UpdateStaffRequest request)
        {
            var staff = await _staffService.UpdateStaff(UpdateStaffRequest.ToStaffModel(request), _userContext.UsersAuth0OrganizationId);
            await _staffEventsService.GenerateStaffCreatedOrUpdatedEvent(request.StaffId.Value);
            return staff;
        }

        /// <summary>
        /// Reactivate the staff member, status will be set to active again
        /// </summary>
        /// <param name="staffId">The staff id to update</param>
        /// <returns></returns>
        [HttpPatch("{staffId}/reactivate")]
        public async Task<ActionResult> ReactivateStaffMember([ValidNumberRequired] long? staffId)
        {
            //1. Gets the existing staff
            var existingInactiveStaff = await _staffService.GetStaff(staffId.Value, _userContext.UsersAuth0OrganizationId);

            //2. Re-create the user in Auth0
            var userCreatedInAuth0 = await _auth0UserService.AddStaffToAuth0(_userContext.UsersAuth0OrganizationId, existingInactiveStaff.FirstName, existingInactiveStaff.LastName, existingInactiveStaff.Email, existingInactiveStaff.MobilePhoneNumber, _userContext.UsersAuth0OrganizationId);

            await _staffService.UpdateStaffToActive(staffId.Value, userCreatedInAuth0.UserAuth0Id, _userContext.UsersAuth0OrganizationId);
            await _staffEventsService.GenerateStaffCreatedOrUpdatedEvent(staffId.Value);
            return Ok();
        }

        /// <summary>
        /// Makes a Staff member as inactive
        /// </summary>
        /// <param name="staffId">The Staff member to mark as inactive</param>
        /// <returns></returns>
        [HttpDelete("{staffId}")]
        public async Task<ActionResult> RemoveStaffMember([ValidNumberRequired] long? staffId)
        {
            //1. Mark as not active in our db
            var staffModel = await _staffService.UpdateStaffToInactive(staffId.Value, _userContext.UsersAuth0OrganizationId);

            //2. Remove from auth0
            await _auth0UserService.RemoveUserFromAuth0(staffModel.Auth0Id);

            await _staffEventsService.GenerateStaffDeletedEvent(staffId.Value);
            return Ok();
        }

        /// <summary>
        /// Adds a collection of skills to the Staff member
        /// </summary>
        /// <param name="staffId">The Staff id</param>
        /// <param name="request">The skills to add</param>
        /// <returns></returns>
        [HttpPut("{staffId}/skills")]
        public async Task<ActionResult> AddSkillToStaff([ValidNumberRequired] long? staffId, [FromBody] SkillsToStaffRequest request)
        {
            await _staffSkillsService.UpdateAllSkillsForStaff(staffId.Value, SkillsToStaffRequest.ToSkillModels(request), _userContext.UsersAuth0OrganizationId);
            await _staffEventsService.GenerateStaffCreatedOrUpdatedEvent(staffId.Value);
            return Ok();
        }

        /// <summary>
        /// Deletes a collection of skills from the Staff member
        /// </summary>
        /// <param name="staffId">The Staff id</param>
        /// <param name="request">The skills to delete</param>
        /// <returns></returns>
        [HttpDelete("{staffId}/skills")]
        public async Task<ActionResult> DeleteSkillsForStaff([ValidNumberRequired] long? staffId, [FromBody] SkillsToStaffRequest request)
        {
            await _staffSkillsService.DeleteSkillsForStaff(staffId.Value, SkillsToStaffRequest.ToSkillModels(request), _userContext.UsersAuth0OrganizationId);
            await _staffEventsService.GenerateStaffCreatedOrUpdatedEvent(staffId.Value);
            return Ok();
        }

        /// <summary>
        /// Gets a list of facilities that a facility admin has access too
        /// </summary>
        /// <returns></returns>
        [HttpGet("facilities")]
        public async Task<ActionResult<List<FacilityLiteModel>>> GetFacilitiesForFacilityAdmin()
        {
            var facilities = await _staffService.GetFacilitiesForStaff(_userContext.UserAuth0Id, _userContext.UsersAuth0OrganizationId);
            return facilities;
        }
    }
}
