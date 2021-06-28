using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rosterd.Domain;
using Rosterd.Domain.Models.OrganizationModels;
using Rosterd.Domain.Organization;
using Rosterd.Services.Organizations.Interfaces;
using Rosterd.Web.Infra.Filters.Swagger;
using PagingQueryStringParameters = Rosterd.Domain.Models.PagingQueryStringParameters;

namespace Rosterd.Admin.Api.Controllers
{
    [ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "Organizations")]
public class OrganizationController : BaseApiController
{
    private readonly IOrganizationsService _organizationService;
    private readonly ILogger<OrganizationController> _logger;

    public OrganizationController(ILogger<OrganizationController> logger, IOrganizationsService organizationService, IOptions<AppSettings> appSettings) : base(appSettings)
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
    public async Task<ActionResult<Domain.Models.PagedList<OrganizationModel>>> GetAllOrganizations([FromQuery] PagingQueryStringParameters pagingParameters)
    {
        pagingParameters ??= new PagingQueryStringParameters();

        var pagedList = await _organizationService.GetAllOrganizations(pagingParameters);

        return pagedList;
    }

    /// <summary>
    ///     Get Organization by Id
    /// </summary>
    /// <returns></returns>
    [HttpGet("{id}")]
    [OperationOrderAttribute(2)]
    public async Task<ActionResult<OrganizationModel>> GetOrganization([Required] long? id)
    {
        var organizationModel = await _organizationService.GetOrganization(id.Value);
        return organizationModel;
    }

    /// <summary>
    ///     Adds a new Organization
    /// </summary>
    /// <param name="request">The Organization to add</param>
    /// <returns></returns>
    [HttpPost]
    [OperationOrderAttribute(3)]
    public async Task<ActionResult> CreateNewOrganization([FromBody] AddUpdateOrganizationRequest request)
    {
        await _organizationService.Createorganization(request.OrganizationToAddOrUpdate);
        return Ok();
    }

    /// <summary>
    ///     Update a Organization
    /// </summary>
    /// <param name="request">The Organization to update</param>
    /// <returns></returns>
    [HttpPut]
    [OperationOrderAttribute(4)]
    public async Task<ActionResult> UpdateOrganization([FromBody] AddUpdateOrganizationRequest request)
    {
        await _organizationService.UpdateOrganization(request.OrganizationToAddOrUpdate);
        return Ok();
    }


    /// <summary>
    ///     Deletes Organization
    /// </summary>
    /// <param name="organizationId">The Organization to be deleted</param>
    /// <returns></returns>
    [HttpDelete]
    [OperationOrderAttribute(5)]
    public async Task<ActionResult> RemoveOrganization([FromQuery][Required] long? organizationId)
    {
        await _organizationService.RemoveOrganization(organizationId.Value);
        return Ok();
    }
}
}

