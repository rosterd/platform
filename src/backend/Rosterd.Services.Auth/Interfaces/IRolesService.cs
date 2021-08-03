using System.Collections.Generic;
using System.Threading.Tasks;
using Auth0.AuthenticationApi;
using Auth0.ManagementApi;
using Microsoft.Extensions.Options;
using Rosterd.Domain.Enums;
using Rosterd.Domain.Models.Roles;
using Rosterd.Domain.Settings;

namespace Rosterd.Services.Auth.Interfaces
{
    public interface IRolesService
    {
        Task<List<RosterdRole>> GetAllRoles();

        Task<RosterdRole> GetRole(string roleId);

        Task<RosterdRole> GetRole(RosterdRoleEnum rosterdRoleEnum);

        Task<RosterdRole> AddRole(RosterdRole roleToAdd);

        Task DeleteRole(string roleId);
    }
}
