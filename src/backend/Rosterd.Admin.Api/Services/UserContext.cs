using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Rosterd.Admin.Api.Services;
using Rosterd.Domain.Exceptions;
using Rosterd.Domain.Models.StaffModels;
using Rosterd.Domain.Models.Tenants;
using Rosterd.Domain.Settings;
using Rosterd.Web.Infra.Extensions;

namespace Rosterd.Admin.Api.Services
{
    /// <summary>
    /// The context provides various bits of information about the logged in user, their roles and claims
    /// The tenant this current user belongs to is cached for the specified duration
    /// </summary>
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public UserContext(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;

        public string Auth0Id {
            get
            {
                var auth0Id = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                return auth0Id;
            }
        }

        public string UsersOrganizationId { get; }

        public string UserEmailAddress { get; set; }

        public string UsersFirstName { get; set; }

        public string UsersLastName { get; set; }

        public string UsersPhoneNumber { get; set; }

        public IEnumerable<string> Roles { get; }

        public string AccessToken { get; }
    }
}
