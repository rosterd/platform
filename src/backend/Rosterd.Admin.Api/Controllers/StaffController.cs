using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.EventGrid;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rosterd.Admin.Api.Requests.Staff;
using Rosterd.Admin.Api.Services;
using Rosterd.Domain;
using Rosterd.Domain.Models.StaffModels;
using Rosterd.Domain.Settings;
using Rosterd.Infrastructure.Security.Interfaces;
using Rosterd.Services.Staff.Interfaces;
using Rosterd.Web.Infra.Filters.Swagger;
using Rosterd.Web.Infra.Security;
using Rosterd.Web.Infra.ValidationAttributes;
using PagingQueryStringParameters = Rosterd.Domain.Models.PagingQueryStringParameters;

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
        private readonly IEventGridClient _eventGridClient;
        private readonly IAuth0UserService _auth0UserService;
        private readonly IUserContext _userContext;

        public StaffController(ILogger<StaffController> logger, IStaffService staffService, IStaffSkillsService staffSkillsService, IStaffEventsService staffEventsService, IEventGridClient eventGridClient, IOptions<AppSettings> appSettings, IAuth0UserService auth0UserService, IUserContext userContext) : base(appSettings)
        {
            _logger = logger;
            _staffService = staffService;
            _staffSkillsService = staffSkillsService;
            _staffEventsService = staffEventsService;
            _eventGridClient = eventGridClient;
            _auth0UserService = auth0UserService;
            _userContext = userContext;
        }

        /// <summary>
        /// Gets all the resources 
        /// </summary>
        /// <param name="facilityId">The facility id to filter all the list of Staff by</param>
        /// <param name="pagingParameters"></param>
        /// <returns></returns>
        [HttpGet]
        [OperationOrder(1)]
        public async Task<ActionResult<Domain.Models.PagedList<StaffModel>>> GetAllStaff([FromQuery] long? facilityId, [FromQuery] PagingQueryStringParameters pagingParameters)
        {
            pagingParameters ??= new PagingQueryStringParameters();
            Domain.Models.PagedList<StaffModel> pagedList;

            if (facilityId == null)
                pagedList = await _staffService.GetAllStaff(pagingParameters);
            else
                pagedList = await _staffService.GetStaffForFacility(pagingParameters, facilityId.Value);

            return pagedList;
        }

        /// <summary>
        /// Get Staff by Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        [OperationOrderAttribute(2)]
        public async Task<ActionResult<StaffModel>> GetStaffById([ValidNumberRequired] long? id)
        {
            var staffModel = await _staffService.GetStaff(id.Value);
            return staffModel;
        }

        /// <summary>
        /// Adds a new Staff member
        /// </summary>
        /// <param name="request">The Staff member to add</param>
        /// <returns></returns>
        [HttpPost]
        [OperationOrderAttribute(2)]
        public async Task<ActionResult<StaffModel>> AddNewStaffMember([FromBody] AddStaffRequest request)
        {
            //1. Create the staff in auth0
            var userCreatedInAuth0 = await _auth0UserService.AddStaffToAuth0(_userContext.UsersAuth0OrganizationId, request.FirstName, request.LastName, request.Email, request.MobilePhoneNumber);
            
            //2. Create the staff in our db
            var staffToCreateInDb = request.ToStaffModel();
            staffToCreateInDb.Auth0Id = userCreatedInAuth0.UserAuth0Id;
            var staff = await _staffService.CreateStaff(staffToCreateInDb);

            //Generate a new staff created event
            await _staffEventsService.GenerateStaffCreatedOrUpdatedEvent(_eventGridClient, RosterdEventGridTopicHost, CurrentEnvironment, staff.StaffId.Value);
            return staff;
        }

        /// <summary>
        /// Update a Staff member details
        /// </summary>
        /// <param name="request">The Staff member to update</param>
        /// <returns></returns>
        [HttpPut]
        [OperationOrderAttribute(3)]
        public async Task<ActionResult<StaffModel>> UpdateStaffMember([FromBody] UpdateStaffRequest request)
        {
            var staff = await _staffService.UpdateStaff(UpdateStaffRequest.ToStaffModel(request));
            await _staffEventsService.GenerateStaffCreatedOrUpdatedEvent(_eventGridClient, RosterdEventGridTopicHost, CurrentEnvironment, request.StaffId.Value);
            return staff;
        }

        /// <summary>
        /// Reactivate the staff member, status will be set to active again
        /// </summary>
        /// <param name="staffId">The staff id to update</param>
        /// <returns></returns>
        [HttpPatch("{staffId}/reactivate")]

        [OperationOrderAttribute(3)]
        public async Task<ActionResult> ReactivateStaffMember([ValidNumberRequired] long? staffId)
        {
            await _staffService.UpdateStaffToActive(staffId.Value);
            await _staffEventsService.GenerateStaffCreatedOrUpdatedEvent(_eventGridClient, RosterdEventGridTopicHost, CurrentEnvironment, staffId.Value);
            return Ok();
        }
        
        /// <summary>
        /// Makes a Staff member as inactive
        /// </summary>
        /// <param name="staffId">The Staff member to mark as inactive</param>
        /// <returns></returns>
        [HttpDelete("{staffId}")]
        [OperationOrderAttribute(4)]
        public async Task<ActionResult> RemoveStaffMember([ValidNumberRequired] long? staffId)
        {
            //1. Mark as not active in our db
            var staffModel = await _staffService.UpdateStaffToInactive(staffId.Value);

            //2. Remove from auth0
            if (staffModel != null)
                await _auth0UserService.RemoveUserFromAuth0(staffModel.Auth0Id);

            await _staffEventsService.GenerateStaffDeletedEvent(_eventGridClient, RosterdEventGridTopicHost, CurrentEnvironment, staffId.Value);
            return Ok();
        }

        /// <summary>
        /// Adds a facility to a staff member
        /// </summary>
        /// <param name="staffId">The Staff id</param>
        /// /// <param name="facilityId">The facility id to add</param>
        /// <returns></returns>
        [HttpPost("{staffId}/facilities/{facilityId}")]
        [OperationOrderAttribute(5)]
        public async Task<ActionResult> AddStaffToFacility([ValidNumberRequired] long? staffId, [ValidNumberRequired] long facilityId)
        {
            await _staffService.AddFacilityToStaff(staffId.Value, facilityId);
            await _staffEventsService.GenerateStaffCreatedOrUpdatedEvent(_eventGridClient, RosterdEventGridTopicHost, CurrentEnvironment, staffId.Value);
            return Ok();
        }

        /// <summary>
        /// Remove a staff from facility
        /// </summary>
        /// <param name="staffId">The Staff id</param>
        /// /// <param name="facilityId">The facility id to add</param>
        /// <returns></returns>
        [HttpDelete("{staffId}/facilities/{facilityId}")]
        [OperationOrderAttribute(6)]
        public async Task<ActionResult> RemoveStaffToFacility([ValidNumberRequired] long? staffId, [ValidNumberRequired] long facilityId)
        {
            await _staffService.RemoveFacilityFromStaff(staffId.Value, facilityId);
            await _staffEventsService.GenerateStaffCreatedOrUpdatedEvent(_eventGridClient, RosterdEventGridTopicHost, CurrentEnvironment, staffId.Value);
            return Ok();
        }

        /// <summary>
        /// Adds a collection of skills to the Staff member
        /// </summary>
        /// <param name="staffId">The Staff id</param>
        /// <param name="request">The skills to add</param>
        /// <returns></returns>
        [HttpPut("{staffId}/skills")]
        [OperationOrderAttribute(7)]
        public async Task<ActionResult> AddSkillToStaff([FromQuery][ValidNumberRequired] long? staffId, [FromBody] AddSkillsToStaffRequest request)
        {
            await _staffSkillsService.UpdateAllSkillsForStaff(staffId.Value, AddSkillsToStaffRequest.ToSkillModels(request));
            await _staffEventsService.GenerateStaffCreatedOrUpdatedEvent(_eventGridClient, RosterdEventGridTopicHost, CurrentEnvironment, staffId.Value);
            return Ok();
        }

        /// <summary>
        /// Deletes a collection of skills from the Staff member
        /// </summary>
        /// <param name="staffId">The Staff id</param>
        /// <param name="request">The skills to delete</param>
        /// <returns></returns>
        [HttpDelete("{staffId}/skills")]
        [OperationOrderAttribute(8)]
        public async Task<ActionResult> DeleteSkillsForStaff([ValidNumberRequired] long? staffId, [FromBody] AddSkillsToStaffRequest request)
        {
            await _staffSkillsService.DeleteSkillsForStaff(staffId.Value, AddSkillsToStaffRequest.ToSkillModels(request));
            await _staffEventsService.GenerateStaffCreatedOrUpdatedEvent(_eventGridClient, RosterdEventGridTopicHost, CurrentEnvironment, staffId.Value);
            return Ok();
        }
    }
}
