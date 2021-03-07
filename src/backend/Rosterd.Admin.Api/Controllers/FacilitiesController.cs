using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.FacilitiesModels;
using Rosterd.Services.Facilities.Interfaces;

namespace Rosterd.Admin.Api.Controllers
{
    [ApiVersion("1.0")]
    public class FacilitiesController: BaseApiController
    {
        private readonly ILogger<FacilitiesController> _logger;
        private readonly IFacilitiesService _facilitiesService;

        public FacilitiesController(ILogger<FacilitiesController> logger, IFacilitiesService facilitiesService) : base()
        {
            _logger = logger;
            _facilitiesService = facilitiesService;
        }
    }
}
