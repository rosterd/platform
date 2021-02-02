using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.Resources;
using Rosterd.Services.Resources.Interfaces;

namespace Rosterd.Admin.Api.Controllers
{
    [ApiVersion("1.0")]
    public class FacilitiesController: BaseApiController
    {
        private readonly ILogger<FacilitiesController> _logger;
        private readonly IFacilityService _facilityService;

        public FacilitiesController(ILogger<FacilitiesController> logger, IFacilityService facilityService) : base()
        {
            _logger = logger;
            _facilityService = facilityService;
        }

        /// <summary>
        /// Gets all the resources 
        /// </summary>
        /// <param name="id">the resource id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PagedList<FacilityModel>> Get() =>
            (await _facilityService.GetFacilities(new PagingQueryStringParameters()));
    }
}
