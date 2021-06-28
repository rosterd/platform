using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rosterd.Domain;
using Rosterd.Domain.Models.FacilitiesModels;
using Rosterd.Domain.Models.OrganizationModels;
using Rosterd.Domain.Requests.Facility;
using Rosterd.Services.Facilities.Interfaces;
using Rosterd.Web.Infra.Filters.Swagger;
using PagingQueryStringParameters = Rosterd.Domain.Models.PagingQueryStringParameters;

namespace Rosterd.Admin.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "Facilities")]
    public class FacilitiesController : BaseApiController
    {
        private readonly IFacilitiesService _facilitiesService;
        private readonly ILogger<FacilitiesController> _logger;

        public FacilitiesController(ILogger<FacilitiesController> logger, IFacilitiesService facilitiesService, IOptions<AppSettings> appSettings) : base(appSettings)
        {
            _logger = logger;
            _facilitiesService = facilitiesService;
        }

        /// <summary>
        ///     Gets all the facilities
        /// </summary>
        /// <param name="pagingParameters"></param>
        /// <returns></returns>
        [HttpGet]
        [OperationOrder(1)]
        public async Task<ActionResult<Domain.Models.PagedList<FacilityModel>>> GetAllFacilities([FromQuery] PagingQueryStringParameters pagingParameters)
        {
            pagingParameters ??= new PagingQueryStringParameters();

            var pagedList = await _facilitiesService.GetAllFacilities(pagingParameters);

            return pagedList;
        }

        /// <summary>
        ///     Get Facility by Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        [OperationOrderAttribute(2)]
        public async Task<ActionResult<FacilityModel>> GetFacilityById([Required] long? id)
        {
            var facilityModel = await _facilitiesService.GetFacility(id.Value);
            return facilityModel;
        }

        /// <summary>
        ///     Adds a new Facility
        /// </summary>
        /// <param name="request">The Facility to add</param>
        /// <returns></returns>
        [HttpPost]
        [OperationOrderAttribute(3)]
        public async Task<ActionResult> AddNewFacility([Required][FromBody] AddFacilityRequest request)
        {
            //TODO: remove hard-coding of facility now, this will need to come from JWT once auth is there
            request.FacilityToAdd.Organization = new OrganizationModel { OrganizationId = 7 };

            var facilityId = await _facilitiesService.CreateFacility(request.FacilityToAdd);
            return Ok(facilityId);
        }

        /// <summary>
        ///     Update a Facility
        /// </summary>
        /// <param name="request">The Facility to update</param>
        /// <returns></returns>
        [HttpPut]
        [OperationOrderAttribute(4)]
        public async Task<ActionResult> UpdateFacility([Required] UpdateFacilityRequest request)
        {
            await _facilitiesService.UpdateFacility(request.FacilityToUpdate);
            return Ok();
        }


        /// <summary>
        ///     Deletes Facility
        /// </summary>
        /// <param name="facilityId">The Facility to be deleted</param>
        /// <returns></returns>
        [HttpDelete]
        [OperationOrderAttribute(5)]
        public async Task<ActionResult> RemoveFacility([FromQuery] [Required] long? facilityId)
        {
            await _facilitiesService.RemoveFacility(facilityId.Value);
            return Ok();
        }
    }
}
