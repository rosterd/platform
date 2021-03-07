using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        public StaffController(ILogger<StaffController> logger, IStaffService resourcesService) : base()
        {
            _logger = logger;
            _staffService = resourcesService;
        }

        /// <summary>
        /// Gets all the resources 
        /// </summary>
        /// <param name="facilityId">The facility id to filter all the list of Staff by</param>
        /// <param name="pagingParameters"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<PagedList<StaffModel>>> GetAllStaff([FromQuery] int? facilityId, [FromQuery] PagingQueryStringParameters pagingParameters)
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
        public async Task<ActionResult> RemoveStaffMember([FromQuery] [Required] int? staffId)
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
        public async Task<ActionResult> MoveStaffMemberToAnotherFacility([FromQuery] [Required] int? facilityId, [Required] int? staffId)
        {
            await _staffService.MoveStaffMemberToAnotherFacility(staffId.Value, facilityId.Value);
            return Ok();
        }

        //TODO:
        //PATCH add skills to staff
        //PATCH add faility to staff
        //DELETE remove facility from staff
        //DELETE remove all skills from staff
    }
}
