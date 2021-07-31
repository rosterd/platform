using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Auth0.ManagementApi.Paging;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Rosterd.Domain.Models.Roles;
using Rosterd.Domain.Settings;
using Rosterd.Infrastructure.Extensions;
using Rosterd.Services.Auth.Interfaces;

namespace Rosterd.Services.Auth
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
