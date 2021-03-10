using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rosterd.Admin.Api.Infrastructure.Filters.Swagger;
using Rosterd.Admin.Api.Requests.Facility;
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

        /// <summary>
        /// Gets all the resources 
        /// </summary>
        /// <param name="pagingParameters"></param>
        /// <returns></returns>
        [HttpGet]
        [OperationOrderAttribute(1)]
        public async Task<ActionResult<PagedList<FacilityModel>>> GetAllFacilities([FromQuery] PagingQueryStringParameters pagingParameters)
        {
            pagingParameters ??= new PagingQueryStringParameters();
            PagedList<FacilityModel> pagedList;

            pagedList = await _facilitiesService.GetAllFacilities(pagingParameters);

            return pagedList;
        }

        /// <summary>
        /// Get Facility by Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        [OperationOrderAttribute(2)]
        public async Task<ActionResult<FacilityModel>> GetFacilityById(string? id)
        { 
            var facilityModel = await _facilitiesService.GetFacility(long.Parse(id));
            return facilityModel;
        }

        /// <summary>
        /// Adds a new Facility
        /// </summary>
        /// <param name="request">The Facility to add</param>
        /// <returns></returns>
        [HttpPost]
        [OperationOrderAttribute(3)]
        public async Task<ActionResult> AddNewFacility([FromBody] AddUpdateFacilityRequest request)
        {
            await _facilitiesService.CreateFacility(request.FacilityToAddOrUpdate);
            return Ok();
        }

        /// <summary>
        /// Update a Facility
        /// </summary>
        /// <param name="request">The Facility to update</param>
        /// <returns></returns>
        [HttpPut]
        [OperationOrderAttribute(4)]
        public async Task<ActionResult> UpdateFacility([FromBody] AddUpdateFacilityRequest request)
        {
            await _facilitiesService.UpdateFacility(request.FacilityToAddOrUpdate);
            return Ok();
        }


        /// <summary>
        /// Deletes Facility
        /// </summary>
        /// <param name="facilityId">The Facility to be deleted</param>
        /// <returns></returns>
        [HttpDelete]
        [OperationOrderAttribute(5)]
        public async Task<ActionResult> RemoveFacility([FromQuery][Required] long? facilityId)
        {
            await _facilitiesService.RemoveFacility(facilityId.Value);
            return Ok();
        }
    }
}
