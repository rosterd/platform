//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;

//namespace Rosterd.Admin.Api.Controllers
//{
//    /// <summary>
//    /// All actions related to resources
//    /// </summary>
//    [ApiVersion("1.0")]
//    public class ResourcesController : BaseApiController
//    {
//        private readonly ILogger<ResourcesController> _logger;
//        private readonly IResourceService _resourcesService;

//        public ResourcesController(ILogger<ResourcesController> logger, IResourceService resourcesService, IUserContext userContext) : base(userContext)
//        {
//            _logger = logger;
//            _resourcesService = resourcesService;
//        }

//        /// <summary>
//        ///     Gets the resource details
//        ///     THIS IS JUST FOR TESTING API CONNECTIVITY FOR NOW
//        /// </summary>
//        /// <param name="id">the resource id</param>
//        /// <returns></returns>
//        [HttpGet]
//        public async Task<ActionResult<ResourceModel>> Get(long id) =>
//            //TODO: proper/correct implementation
//            (await _resourcesService.GetResourcesForTenantAsync(id)).FirstOrDefault();
//    }
//}
