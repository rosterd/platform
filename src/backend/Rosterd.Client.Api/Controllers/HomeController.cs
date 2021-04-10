using Microsoft.AspNetCore.Mvc;
using Rosterd.Web.Infra.Filters.Swagger;

namespace Rosterd.Client.Api.Controllers
{
    [ApiController]
    [Route("")]
    [ApiExplorerSettings(GroupName = "Home")]
    public class HomeController : ControllerBase
    {
        /// <summary>
        /// Default route (needed for the react app to return 200 so the react admin works with no issues)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [OperationOrder(1)]
        public ActionResult Get() => Ok("OK");
    }
}
