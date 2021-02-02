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

        public StaffController(ILogger<StaffController> logger, IStaffService resourcesService) : base()
        {
            _logger = logger;
            _staffService = resourcesService;
        }

        /// <summary>
        /// Gets all the resources 
        /// </summary>
        /// <param name="id">the resource id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PagedList<StaffModel>> Get() =>
            (await _staffService.GetStaff(new PagingQueryStringParameters()));
    }
}
