using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.EventGrid;
using Microsoft.Extensions.Logging;
using Rosterd.Domain;
using Rosterd.Domain.Models.StaffModels;
using Rosterd.Domain.Requests.Staff;
using Rosterd.Domain.ValidationAttributes;
using Rosterd.Services.Staff.Interfaces;
using Rosterd.Web.Infra.Filters.Swagger;
using PagingQueryStringParameters = Rosterd.Domain.Models.PagingQueryStringParameters;

namespace Rosterd.Admin.Api.Controllers
{
    /// <summary>
    /// All actions related to resources
    /// </summary>
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "Staff")]
    public class StaffController : BaseApiController
    {
        private readonly ILogger<StaffController> _logger;
        private readonly IStaffService _staffService;
        private readonly IStaffSkillsService _staffSkillsService;
        private readonly IStaffEventsService _staffEventsService;
        private readonly IEventGridClient _eventGridClient;

        public StaffController(ILogger<StaffController> logger, IStaffService staffService, IStaffSkillsService staffSkillsService, IStaffEventsService staffEventsService, IEventGridClient eventGridClient, AppSettings appSettings) : base(appSettings)
        {
            _logger = logger;
            _staffService = staffService;
            _staffSkillsService = staffSkillsService;
            _staffEventsService = staffEventsService;
            _eventGridClient = eventGridClient;
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
        public async Task<ActionResult<StaffModel>> GetStaffById([Required][NumberIsRequiredAndShouldBeGreaterThanZero] long? id)
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
        public async Task<ActionResult> AddNewStaffMember([FromBody] AddUpdateStaffRequest request)
        {
            //Create the staff
            var staffId = await _staffService.CreateStaffMember(request);

            //Generate a new staff created event
            await _staffEventsService.GenerateStaffCreatedOrUpdatedEvent(_eventGridClient, RosterdEventGridTopicHost, CurrentEnvironment, staffId);
            return Ok();
        }

        /// <summary>
        /// Update a Staff member details
        /// </summary>
        /// <param name="request">The Staff member to update</param>
        /// <returns></returns>
        [HttpPut]
        [OperationOrderAttribute(3)]
        public async Task<ActionResult> UpdateStaffMember([FromBody] AddUpdateStaffRequest request)
        {
            if (request.StaffId == null)
                return BadRequest("staffId field is required");

            await _staffService.UpdateStaffMember(request);
            await _staffEventsService.GenerateStaffCreatedOrUpdatedEvent(_eventGridClient, RosterdEventGridTopicHost, CurrentEnvironment, request.StaffId.Value);
            return Ok();
        }


        /// <summary>
        /// Makes a Staff member as inactive
        /// </summary>
        /// <param name="staffId">The Staff member to mark as inactive</param>
        /// <returns></returns>
        [HttpDelete]
        [OperationOrderAttribute(4)]
        public async Task<ActionResult> RemoveStaffMember([FromQuery] [Required][NumberIsRequiredAndShouldBeGreaterThanZero] long? staffId)
        {
            await _staffService.RemoveStaffMember(staffId.Value);
            await _staffEventsService.GenerateStaffDeletedEvent(_eventGridClient, RosterdEventGridTopicHost, CurrentEnvironment, staffId.Value);
            return Ok();
        }

        /// <summary>
        /// Moves a Staff member from their existing facility to another facility
        /// </summary>
        /// <param name="facilityId">The facility id to move</param>
        /// <param name="staffId">The Staff id</param>
        /// <returns></returns>
        [HttpPut("facilities")]
        [OperationOrderAttribute(5)]
        public async Task<ActionResult> MoveStaffMemberToAnotherFacility([FromQuery] [Required] long? facilityId, [Required][NumberIsRequiredAndShouldBeGreaterThanZero] long? staffId)
        {
            await _staffService.MoveStaffMemberToAnotherFacility(staffId.Value, facilityId.Value);
            await _staffEventsService.GenerateStaffCreatedOrUpdatedEvent(_eventGridClient, RosterdEventGridTopicHost, CurrentEnvironment, staffId.Value);
            return Ok();
        }

        /// <summary>
        /// Adds a collection of skills to the Staff member
        /// </summary>
        /// <param name="staffId">The Staff id</param>
        /// <param name="request">The skills to add</param>
        /// <returns></returns>
        [HttpPut("skills")]
        [OperationOrderAttribute(6)]
        public async Task<ActionResult> AddSkillToStaff([FromQuery][Required][NumberIsRequiredAndShouldBeGreaterThanZero] long? staffId, [FromBody] AddSkillsToStaffRequest request)
        {
            await _staffSkillsService.UpdateAllSkillsForStaff(staffId.Value, request.SkillsToAdd);
            await _staffEventsService.GenerateStaffCreatedOrUpdatedEvent(_eventGridClient, RosterdEventGridTopicHost, CurrentEnvironment, staffId.Value);
            return Ok();
        }

        /// <summary>
        /// Removes all skills from a Staff member
        /// </summary>
        /// <param name="staffId">The Staff id</param>
        /// <returns></returns>
        [HttpDelete("skills")]
        [OperationOrderAttribute(7)]
        public async Task<ActionResult> DeleteAllSkillsForStaff([FromQuery][Required][NumberIsRequiredAndShouldBeGreaterThanZero] long? staffId)
        {
            await _staffSkillsService.RemoveAllSkillsForStaff(staffId.Value);
            await _staffEventsService.GenerateStaffCreatedOrUpdatedEvent(_eventGridClient, RosterdEventGridTopicHost, CurrentEnvironment, staffId.Value);
            return Ok();
        }
    }
}
