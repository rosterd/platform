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
using Rosterd.Admin.Api.Services;
using Rosterd.Domain.Settings;
using Rosterd.Web.Infra.Security;

namespace Rosterd.Admin.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "Facilities")]
    [AuthorizeByRole(RosterdConstants.RosterdRoleNames.OrganizationAdmin, RosterdConstants.RosterdRoleNames.RosterdAdmin)]
    public class FacilitiesController : BaseApiController
    {
        private readonly IFacilitiesService _facilitiesService;
        private readonly ILogger<FacilitiesController> _logger;
        private readonly IUserContext _userContext;

        public FacilitiesController(ILogger<FacilitiesController> logger, IFacilitiesService facilitiesService, IOptions<AppSettings> appSettings, IUserContext userContext) : base(appSettings)
        {
            _logger = logger;
            _facilitiesService = facilitiesService;
            _userContext = userContext;
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
            var pagedList = await _facilitiesService.GetAllFacilities(pagingParameters, _userContext.UsersAuth0OrganizationId);

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
            var facilityModel = await _facilitiesService.GetFacility(facilityId.Value, _userContext.UsersAuth0OrganizationId);
            return facilityModel;
        }

        /// <summary>
        ///     Adds a new Facility
        /// </summary>
        /// <param name="request">The Facility to add</param>
        /// <returns></returns>
        [HttpPost]
        [OperationOrderAttribute(3)]
        public async Task<ActionResult<FacilityModel>> AddNewFacility([Required][FromBody] AddFacilityRequest request)
        {
            var facilityModelToAdd = request.ToFacilityModel();
            facilityModelToAdd.Organization = new OrganizationModel { Auth0OrganizationId = _userContext.UsersAuth0OrganizationId };

            //Validate duplicates
            var duplicatesExist = await _facilitiesService.DoesFacilityWithSameNameExistForOrganization(facilityModelToAdd, _userContext.UsersAuth0OrganizationId);
            if (duplicatesExist)
            {
                ModelState.TryAddModelError("FacilityToAdd.FacilityName", $"Facility with name {facilityModelToAdd.FacilityName} already exits");
                return BadRequest(ModelState);
            }

            var facilityAdded = await _facilitiesService.CreateFacility(facilityModelToAdd, _userContext.UsersAuth0OrganizationId);
            return facilityAdded;
        }

        /// <summary>
        ///     Update a Facility
        /// </summary>
        /// <param name="request">The Facility to update</param>
        /// <returns></returns>
        [HttpPut]
        [OperationOrderAttribute(4)]
        public async Task<ActionResult<FacilityModel>> UpdateFacility([Required] UpdateFacilityRequest request)
        {
            var facilityToUpdate = request.ToFacilityModel();
            facilityToUpdate.Organization = new OrganizationModel { Auth0OrganizationId = _userContext.UsersAuth0OrganizationId };

            //Existing facility
            var existingFacility = await _facilitiesService.GetFacility(request.FacilityId, _userContext.UsersAuth0OrganizationId);

            //Validate duplicates
            var duplicatesExist = await _facilitiesService.DoesFacilityWithSameNameExistForOrganization(facilityToUpdate, _userContext.UsersAuth0OrganizationId, existingFacility.FacilityName);
            if (duplicatesExist)
            {
                ModelState.TryAddModelError("FacilityToUpdate.FacilityName", $"Facility with name {facilityToUpdate.FacilityName} already exits");
                return BadRequest(ModelState);
            }

            var updatedFacility = await _facilitiesService.UpdateFacility(facilityToUpdate);
            return updatedFacility;
        }

        /// <summary>
        /// Reactivate the facility, status will be set to active again
        /// </summary>
        /// <param name="facilityId">The facility id to update</param>
        /// <returns></returns>
        [HttpPatch("{facilityId}/reactivate")]
        [OperationOrderAttribute(3)]
        public async Task<ActionResult> ReactivateFacility([ValidNumberRequired] long? facilityId)
        {
            await _facilitiesService.ReactivateFacility(facilityId.Value);
            return Ok();
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
