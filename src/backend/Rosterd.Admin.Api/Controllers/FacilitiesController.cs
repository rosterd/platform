using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rosterd.Admin.Api.Requests.Facility;
using Rosterd.Domain;
using Rosterd.Domain.Models.FacilitiesModels;
using Rosterd.Domain.Models.OrganizationModels;
using Rosterd.Services.Facilities.Interfaces;
using Rosterd.Web.Infra.Filters.Swagger;
using Rosterd.Web.Infra.ValidationAttributes;
using PagingQueryStringParameters = Rosterd.Domain.Models.PagingQueryStringParameters;
using Microsoft.AspNetCore.Authorization;
using Rosterd.Domain.Settings;

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
        [HttpGet("{facilityId}")]
        [OperationOrderAttribute(2)]
        public async Task<ActionResult<FacilityModel>> GetFacilityById([ValidNumberRequired] long? facilityId)
        {
            var facilityModel = await _facilitiesService.GetFacility(facilityId.Value);
            return facilityModel;
        }

        /// <summary>
        ///     Adds a new Facility
        /// </summary>
        /// <param name="request">The Facility to add</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [OperationOrderAttribute(3)]
        public async Task<ActionResult<FacilityModel>> AddNewFacility([Required][FromBody] AddFacilityRequest request)
        {
            //TODO: remove hard-coding of organization now, this will need to come from JWT once auth is there
            request.FacilityToAdd.Organization = new OrganizationModel { OrganizationId = 7 };

            //Validate duplicates
            var duplicatesExist = await _facilitiesService.DoesFacilityWithSameNameExistForOrganization(request.FacilityToAdd);
            if (duplicatesExist)
            {
                ModelState.TryAddModelError("FacilityToAdd.FacilityName", $"Facility with name {request.FacilityToAdd.FacilityName} already exits");
                return BadRequest(ModelState);
            }

            var facilityAdded = await _facilitiesService.CreateFacility(request.FacilityToAdd);
            return facilityAdded;
        }

        /// <summary>
        ///     Update a Facility
        /// </summary>
        /// <param name="request">The Facility to update</param>
        /// <returns></returns>
        [HttpPut]
        [Authorize("create:facility")]
        [OperationOrderAttribute(4)]
        public async Task<ActionResult<FacilityModel>> UpdateFacility([Required] UpdateFacilityRequest request)
        {
            //TODO: remove hard-coding of organization now, this will need to come from JWT once auth is there
            request.FacilityToUpdate.Organization = new OrganizationModel { OrganizationId = 7 };

            //Validate duplicates
            var duplicatesExist = await _facilitiesService.DoesFacilityWithSameNameExistForOrganization(request.FacilityToUpdate);
            if (duplicatesExist)
            {
                ModelState.TryAddModelError("FacilityToUpdate.FacilityName", $"Facility with name {request.FacilityToUpdate.FacilityName} already exits");
                return BadRequest(ModelState);
            }

            var updatedFacility = await _facilitiesService.UpdateFacility(request.FacilityToUpdate);

            return updatedFacility;
        }


        /// <summary>
        ///     Deletes Facility
        /// </summary>
        /// <param name="facilityId">The Facility to be deleted</param>
        /// <returns></returns>
        [HttpDelete("{facilityId}")]
        [OperationOrderAttribute(5)]
        public async Task<ActionResult> RemoveFacility([ValidNumberRequired] long? facilityId)
        {
            await _facilitiesService.RemoveFacility(facilityId.Value);
            return Ok();
        }
    }
}
