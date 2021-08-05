using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth0.ManagementApi.Models;
using Auth0.ManagementApi.Paging;
using Rosterd.Domain.Enums;
using Rosterd.Domain.Models.Roles;
using Rosterd.Infrastructure.Extensions;
using Rosterd.Infrastructure.Security.Interfaces;

namespace Rosterd.Infrastructure.Security
{
    public class RolesService : IRolesService
    {
        private readonly IAuth0AuthenticationService _auth0AuthenticationService;

        public RolesService(IAuth0AuthenticationService auth0AuthenticationService) => _auth0AuthenticationService = auth0AuthenticationService;

        public async Task<List<RosterdRole>> GetAllRoles()
        {
            var auth0ApiManagementClient = await _auth0AuthenticationService.GetAuth0ApiManagementClient();
            var allRoles = await auth0ApiManagementClient.Roles.GetAllAsync(new GetRolesRequest(), new PaginationInfo());

            if (allRoles.IsNullOrEmpty())
                return new List<RosterdRole>();

            var rolesToReturn = allRoles.Select(s => new RosterdRole
            {
                RoleId = s.Id,
                RoleName = s.Name,
                RoleDescription = s.Description
            }).AlwaysList();
            return rolesToReturn;
        }

        public async Task<RosterdRole> GetRole(string roleId)
        {
            var auth0ApiManagementClient = await _auth0AuthenticationService.GetAuth0ApiManagementClient();
            var role = await auth0ApiManagementClient.Roles.GetAsync(roleId);

            return new RosterdRole
            {
                RoleId = role.Id,
                RoleName = role.Name,
                RoleDescription = role.Description
            };
        }

        public async Task<RosterdRole> GetRole(RosterdRoleEnum rosterdRoleEnum)
        {
            var allRoles = await GetAllRoles();
            var matchingRole = allRoles.FirstOrDefault(s => s.RoleName == rosterdRoleEnum.ToString());

            return matchingRole;
        }

        public async Task<RosterdRole> AddRole(RosterdRole roleToAdd)
        {
            var auth0ApiManagementClient = await _auth0AuthenticationService.GetAuth0ApiManagementClient();
            var roleAdded = await auth0ApiManagementClient.Roles.CreateAsync(new RoleCreateRequest {Name = roleToAdd.RoleName, Description = roleToAdd.RoleDescription});

            return new RosterdRole {RoleId = roleAdded.Id, RoleName = roleAdded.Name, RoleDescription = roleToAdd.RoleDescription};
        }

        public async Task DeleteRole(string roleId)
        {
            var auth0ApiManagementClient = await _auth0AuthenticationService.GetAuth0ApiManagementClient();
            await auth0ApiManagementClient.Roles.DeleteAsync(roleId);
        }
    }
}
