using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Rosterd.Client.Api.Services;
using Rosterd.Domain.Exceptions;
using Rosterd.Web.Infra.Extensions;

namespace Rosterd.Client.Api.Infrastructure.Middleware
{
    public class UserContextBuilderMiddleware : IMiddleware
    {
        private readonly ILogger<UserContextBuilderMiddleware> _logger;
        private readonly IUserContext _userContext;

        public UserContextBuilderMiddleware(ILogger<UserContextBuilderMiddleware> logger, IUserContext userContext)
        {
            _logger = logger;
            _userContext = userContext;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            //If its a swagger route then we dont need to worry about jwt validation.  For swagger dashboard we hae basic auth.
            if (context.Request.IsNotSwaggerRoute())
            {
                //Populate the required fields of the user context
                var rosterdAppUser = _userContext.GetRosterdAppUserOrCreateIfNotExists().GetAwaiter().GetResult();

                //Set all the relevant ids to the context
                _userContext.UsersAuth0OrganizationId = rosterdAppUser.Auth0OrganizationId;
                _userContext.UserStaffId = rosterdAppUser.StaffId;
                _userContext.UsersOrganizationId = rosterdAppUser.OrganizationId;
            }

            await next.Invoke(context);
        }
    }
}
