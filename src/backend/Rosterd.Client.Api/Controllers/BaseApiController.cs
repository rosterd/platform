using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rosterd.Client.Api.Services;
using Rosterd.Domain;

namespace Rosterd.Client.Api.Controllers
{
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)] //All validation failures and model state errors
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)] //Fatal error
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)] //Custom business validation error
    [ProducesResponseType(StatusCodes.Status200OK)] //Success
    [ProducesResponseType(StatusCodes.Status401Unauthorized)] //Sent if no JWT or bearer token sent to the API
    [ProducesResponseType(StatusCodes.Status403Forbidden)] //Sent if there is a valid JWT but the user (auth0Id) does not exist in our system or no valid API for anonymous API's
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        protected BaseApiController()
        {
        }

        /// <summary>
        /// Checks if the api key given is valid (ie: its compared against our constant)
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        protected bool IsProvidedApiKeyValid(string apiKey)
        {
            if (apiKey.IsNullOrWhiteSpace())
                return false;

            return RosterdConstants.ApplicationKeys.AnonymousApiKey == apiKey;
        }
    }
}
