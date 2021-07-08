using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rosterd.Admin.Api.Requests.Organization;
using Rosterd.Domain;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.OrganizationModels;
using Rosterd.Services.Organizations.Interfaces;
using Rosterd.Web.Infra.Filters.Swagger;
using Rosterd.Web.Infra.ValidationAttributes;

namespace Rosterd.Admin.Api.Controllers
{
    //TODO: This should only be available to rosterd super admins
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "Organizations")]
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
        /// <returns></returns>
        [HttpGet]
        [OperationOrder(1)]
        public async Task<ActionResult<PagedList<OrganizationModel>>> GetAllOrganizations([FromQuery] PagingQueryStringParameters pagingParameters)
        {
            pagingParameters ??= new PagingQueryStringParameters();

            var pagedList = await _organizationService.GetAllOrganizations(pagingParameters);

            return pagedList;
        }

        /// <summary>
        ///     Get Organization by Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{organizationId}")]
        [OperationOrderAttribute(2)]
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
        [OperationOrderAttribute(3)]
        public async Task<ActionResult<OrganizationModel>> CreateNewOrganization([Required] [FromBody] AddOrganizationRequest request)
        {
            var organization = await _organizationService.CreateOrganization(request.Organization);
            return organization;
        }

        /// <summary>
        ///     Update an Organization
        /// </summary>
        /// <param name="request">The Organization to update</param>
        /// <returns></returns>
        [HttpPut]
        [OperationOrderAttribute(4)]
        public async Task<ActionResult<OrganizationModel>> UpdateOrganization([Required] [FromBody] UpdateOrganizationRequest request)
        {
            var organization = await _organizationService.UpdateOrganization(request.Organization);
            return organization;
        }


        /// <summary>
        ///     Deletes Organization
        /// </summary>
        /// <param name="organizationId">The Organization to be deleted</param>
        /// <returns></returns>
        [HttpDelete]
        [OperationOrderAttribute(5)]
        public async Task<ActionResult> RemoveOrganization([ValidNumberRequired] long? organizationId)
        {
            await _organizationService.RemoveOrganization(organizationId.Value);
            return Ok();
        }
    }
}
