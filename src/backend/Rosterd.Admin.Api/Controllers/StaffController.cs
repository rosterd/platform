using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rosterd.Admin.Api.Infrastructure.Filters.Swagger;
using Rosterd.Admin.Api.Requests.Staff;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.StaffModels;
using Rosterd.Services.Staff;
using Rosterd.Services.Staff.Interfaces;

namespace Rosterd.Admin.Api.Controllers
{
    /// <summary>
    /// All actions related to resources
    /// </summary>
    [ApiVersion("1.0")]
    public class StaffController : BaseApiController
    {
        private readonly ILogger<StaffController> _logger;
        private readonly IStaffService _staffService;
        private readonly IStaffSkillsService _staffSkillsService;

        public StaffController(ILogger<StaffController> logger, IStaffService staffService, IStaffSkillsService staffSkillsService) : base()
        {
            _logger = logger;
            _staffService = staffService;
            _staffSkillsService = staffSkillsService;
        }

        /// <summary>
        /// Gets all the resources 
        /// </summary>
        /// <param name="facilityId">The facility id to filter all the list of Staff by</param>
        /// <param name="pagingParameters"></param>
        /// <returns></returns>
        [HttpGet]
        [OperationOrderAttribute(1)]
        public async Task<ActionResult<PagedList<StaffModel>>> GetAllStaff([FromQuery] long? facilityId, [FromQuery] PagingQueryStringParameters pagingParameters)
        {
            pagingParameters ??= new PagingQueryStringParameters();
            PagedList<StaffModel> pagedList;

            if (facilityId == null)
                pagedList = await _staffService.GetAllStaff(pagingParameters);
            else
                pagedList = await _staffService.GetStaffForFacility(pagingParameters, facilityId.Value);

            return pagedList;
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
            await _staffService.CreateStaffMember(request.StaffToAddOrUpdate);
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
            await _staffService.UpdateStaffMember(request.StaffToAddOrUpdate);
            return Ok();
        }


        /// <summary>
        /// Makes a Staff member as inactive
        /// </summary>
        /// <param name="staffId">The Staff member to mark as inactive</param>
        /// <returns></returns>
        [HttpDelete]
        [OperationOrderAttribute(4)]
        public async Task<ActionResult> RemoveStaffMember([FromQuery] [Required] long? staffId)
        {
            await _staffService.RemoveStaffMember(staffId.Value);
            return Ok();
        }

        /// <summary>
        /// Moves a staff member from their existing facility to another facility
        /// </summary>
        /// <param name="facilityId">The facility id to move</param>
        /// <param name="staffId">The staff id</param>
        /// <returns></returns>
        [HttpPut("facilities")]
        [OperationOrderAttribute(5)]
        public async Task<ActionResult> MoveStaffMemberToAnotherFacility([FromQuery] [Required] long? facilityId, [Required] long? staffId)
        {
            await _staffService.MoveStaffMemberToAnotherFacility(staffId.Value, facilityId.Value);
            return Ok();
        }

        /// <summary>
        /// Adds a collection of skills to the staff member
        /// </summary>
        /// <param name="staffId">The staff id</param>
        /// <param name="request">The skills to add</param>
        /// <returns></returns>
        [HttpPut("skills")]
        [OperationOrderAttribute(6)]
        public async Task<ActionResult> AddSkillToStaff([FromQuery][Required] long? staffId, [FromBody] AddSkillsToStaffRequest request)
        {
            await _staffSkillsService.UpdateAllSkillsForStaff(staffId.Value, request.SkillsToAdd);
            return Ok();
        }

        /// <summary>
        /// Removes all skills from a staff member
        /// </summary>
        /// <param name="staffId">The staff id</param>
        /// <returns></returns>
        [HttpDelete("skills")]
        [OperationOrderAttribute(7)]
        public async Task<ActionResult> DeleteAllSkillsForStaff([FromQuery][Required] long? staffId)
        {
            await _staffSkillsService.RemoveAllSkillsForStaff(staffId.Value);
            return Ok();
        }
    }
}
