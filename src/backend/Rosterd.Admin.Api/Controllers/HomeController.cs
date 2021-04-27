using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.FacilitiesModels;
using Rosterd.Services.Facilities.Interfaces;
using Rosterd.Web.Infra.Filters.Swagger;

namespace Rosterd.Admin.Api.Controllers
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
