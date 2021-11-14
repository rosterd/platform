using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rosterd.Client.Api.Services;
using Rosterd.Domain;
using Rosterd.Web.Infra.Filters.Swagger;
using Rosterd.Web.Infra.Security;

namespace Rosterd.Client.Api.Controllers
{
    [ApiController]
    [Route("")]
    [ApiExplorerSettings(GroupName = "Home")]
    [Authorize]
    public class HomeController : ControllerBase
    {
        /// <summary>
        /// Default route (needed for the react app to return 200 so the react admin works with no issues)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Get() => Ok("OK");
    }
}
