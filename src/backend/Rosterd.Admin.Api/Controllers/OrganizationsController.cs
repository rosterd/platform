using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rosterd.Admin.Api.Requests.Organization;
using Rosterd.Domain;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.OrganizationModels;
using Rosterd.Domain.Settings;
using Rosterd.Services.Organizations.Interfaces;
using Rosterd.Web.Infra.Filters.Swagger;
using Rosterd.Web.Infra.Security;
using Rosterd.Web.Infra.ValidationAttributes;

namespace Rosterd.Admin.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "Organizations")]
    [AuthorizeByRole(RosterdConstants.RosterdRoleNames.RosterdAdmin)]
    public class OrganizationsController : BaseApiController
    {
        private readonly ILogger<OrganizationsController> _logger;
        private readonly IOrganizationsService _organizationService;

        public OrganizationsController(ILogger<OrganizationsController> logger, IOrganizationsService organizationService, IOptions<AppSettings> appSettings) :
            base(appSettings)
        {
            _logger = logger;
            _organizationService = organizationService;
        }

        /// <summary>
        ///     Gets all the organizations
        /// </summary>
        /// <param name="pagingParameters"></param>
        /// <param name="activeOrganizationsOnly">True = only active organizations will be returned.
        /// False = all organization will be returned
        /// The default is true</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<PagedList<OrganizationModel>>> GetAllOrganizations([FromQuery] PagingQueryStringParameters pagingParameters, [FromQuery] bool activeOrganizationsOnly = true)
        {
            pagingParameters ??= new PagingQueryStringParameters();
            var pagedList = await _organizationService.GetAllOrganizations(pagingParameters, activeOrganizationsOnly);

            return pagedList;
        }

        /// <summary>
        ///     Get Organization by Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{organizationId}")]
        public async Task<ActionResult<OrganizationModel>> GetOrganization([ValidNumberRequired] long? organizationId)
        {
            var organizationModel = await _organizationService.GetOrganization(organizationId.Value);
            return organizationModel;
        }

        /// <summary>
        ///     Adds a new Organization
        /// </summary>
        /// <param name="request">The Organization to add</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<OrganizationModel>> CreateNewOrganization([Required] [FromBody] AddOrganizationRequest request)
        {
            //TODO: send a message to create index

            var organization = await _organizationService.CreateOrganization(request.ToOrganizationModel());
            return organization;
        }

        /// <summary>
        /// Update an Organization.
        /// THIS IS NOT IMPLEMENTED, WILL RETURN A 500
        /// </summary>
        /// <param name="request">The Organization to update</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult<OrganizationModel>> UpdateOrganization([Required] [FromBody] UpdateOrganizationRequest request) => throw new NotImplementedException("Not Implemented");


        /// <summary>
        ///     Deletes Organization
        /// </summary>
        /// <param name="organizationId">The Organization to be deleted</param>
        /// <returns></returns>
        [HttpDelete("{organizationId}")]
        public async Task<ActionResult> RemoveOrganization([ValidNumberRequired] long? organizationId)
        {
            await _organizationService.RemoveOrganization(organizationId.Value);
            return Ok();
        }
    }
}
