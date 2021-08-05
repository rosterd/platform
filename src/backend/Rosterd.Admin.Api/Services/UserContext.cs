using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Rosterd.Domain;
using Rosterd.Domain.Enums;

namespace Rosterd.Admin.Api.Services
{
    /// <summary>
    ///     The context provides various bits of information about the logged in user, their roles and claims
    ///     The tenant this current user belongs to is cached for the specified duration
    /// </summary>
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContext(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;

        public string UserAuth0Id
        {
            get
            {
                var auth0Id = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                return auth0Id;
            }
        }

        public string UsersAuth0OrganizationId
        {
            get
            {
                var auth0OrganizationId = _httpContextAccessor.HttpContext.User.Claims.First(s => s.Type == RosterdConstants.AccessTokenFields.Auth0OrganizationId).Value;
                return auth0OrganizationId;
            }
        }
        public string AccessToken
        {
            get
            {
                var accessToken = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == RosterdConstants.AccessTokenFields.AccessToken).Value;
                return accessToken;
            }
        }

        //TODO Not working, need to debug why roles are not available
        public IEnumerable<string> Roles => _httpContextAccessor.HttpContext.User.Claims.Where(s => s.Type == RosterdConstants.AccessTokenFields.Roles).Select(s => s.Value);

        public bool IsUserInRole(RosterdRoleEnum rosterdRole) => !Roles.IsNullOrEmpty() && Roles.Contains(rosterdRole.ToString());

        public string UserEmailAddress =>
            throw new NotImplementedException(
                "Not yet implemented, when we need this we can add it to the access token and grab it from there, for now the access token does not have this.");

        public string UsersFirstName =>
            throw new NotImplementedException(
                "Not yet implemented, when we need this we can add it to the access token and grab it from there, for now the access token does not have this.");

        public string UsersLastName =>
            throw new NotImplementedException(
                "Not yet implemented, when we need this we can add it to the access token and grab it from there, for now the access token does not have this.");

        public string UsersPhoneNumber =>
            throw new NotImplementedException(
                "Not yet implemented, when we need this we can add it to the access token and grab it from there, for now the access token does not have this.");

    }
}
