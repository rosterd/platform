using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.Resources;
using Rosterd.Services.Resources.Interfaces;

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

        public StaffController(ILogger<StaffController> logger, IStaffService staffService) : base()
        {
            _logger = logger;
            _staffService = staffService;
        }

        /// <summary>
        /// Gets all the resources 
        /// </summary>
        /// <param name="id">the resource id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PagedList<StaffModel>> Get() =>
            (await _staffService.GetStaff(new PagingQueryStringParameters()));

        [HttpGet("{staffId:long}")]
        public async Task<StaffModel> GetStaffById(long staffId) =>
             (await _staffService.GetStaffById(staffId));

        [HttpDelete("{staffId:long}")]
        public async Task<IActionResult> DeleteStaff(long staffId) =>
            (await _staffService.DeleteStaff(staffId));

        [HttpPost]
        public async Task<IActionResult> CreateStaff(StaffModel staffModel) =>
            (await _staffService.PostStaff(staffModel));
    }
}
