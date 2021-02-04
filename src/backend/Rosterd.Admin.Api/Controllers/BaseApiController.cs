using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rosterd.Domain;

namespace Rosterd.Admin.Api.Controllers
{
    //[Authorize]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)] //All validation failures and model state errors
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)] //Fatal error
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)] //Sent if too many requests coming in
    [ProducesResponseType(StatusCodes.Status200OK)] //Success
    [ProducesResponseType(StatusCodes.Status401Unauthorized)] //Sent if no JWT or bearer token sent to the API
    [ProducesResponseType(StatusCodes.Status403Forbidden)] //Sent if there is a valid JWT but the user (auth0Id) does not exist in our system or no valid API for anonymous API's
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        //protected readonly IUserContext UserContext;

        //protected BaseApiController(IUserContext userContext) => UserContext = userContext;

        /// <summary>
        /// Gets the currently logged in tenant id
        /// </summary>
        /// <returns></returns>
        //protected async Task<long> CurrentTenantId() => await UserContext.GetTenantIdForUser();

        /// <summary>
        /// Checks if the api key given is valid (ie: its compared against our constant)
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        protected bool IsProvidedApiKeyValid(string apiKey)
        {
            if (apiKey.IsNullOrWhiteSpace())
                return false;

            return Constants.ApplicationKeys.AnonymousApiKey == apiKey;
        }
    }
}
