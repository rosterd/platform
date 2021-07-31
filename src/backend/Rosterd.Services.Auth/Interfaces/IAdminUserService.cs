using System.Collections.Generic;
using System.Threading.Tasks;
using Auth0.AuthenticationApi;
using Auth0.ManagementApi;
using Microsoft.Extensions.Options;
using Rosterd.Domain.Models.AdminUserModels;
using Rosterd.Domain.Models.Roles;
using Rosterd.Domain.Settings;

namespace Rosterd.Services.Auth.Interfaces
{
    public interface IAdminUserService
    {
        Task<RosterdRole> AddOrganizationAdmin(string auth0OrganizationId, AdminUserModel adminUserModel);
    }
}
