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
    public class ResourcesController : BaseApiController
    {
        private readonly ILogger<ResourcesController> _logger;
        private readonly IResourceService _resourcesService;

        public ResourcesController(ILogger<ResourcesController> logger, IResourceService resourcesService) : base()
        {
            _logger = logger;
            _resourcesService = resourcesService;
        }

        /// <summary>
        /// Gets all the resources 
        /// </summary>
        /// <param name="id">the resource id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PagedList<ResourceModel>> Get() =>
            (await _resourcesService.GetResources(new PagingQueryStringParameters()));
    }
}
