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
using Rosterd.Domain.Models.AdminUserModels;
using Rosterd.Domain.Models.Roles;
using Rosterd.Domain.Settings;
using Rosterd.Infrastructure.Extensions;
using Rosterd.Services.Auth.Interfaces;

namespace Rosterd.Services.Auth
{
    public class AdminUserService : IAdminUserService
    {
        private readonly IAuth0AuthenticationService _auth0AuthenticationService;
        private readonly IRolesService _rolesService;

        public AdminUserService(IAuth0AuthenticationService auth0AuthenticationService, IRolesService rolesService)
        {
            _auth0AuthenticationService = auth0AuthenticationService;
            _rolesService = rolesService;
        }

        public async Task<RosterdRole> AddOrganizationAdmin(string auth0OrganizationId, AdminUserModel adminUserModel)
        {
            var auth0ApiManagementClient = await _auth0AuthenticationService.GetAuth0ApiManagementClient();

            //1. Create the user in auth0 with a random password
            var userCreatedInAuth0 = await auth0ApiManagementClient.Users.CreateAsync(new UserCreateRequest
            {
                Email = adminUserModel.Email,
                FirstName = adminUserModel.FirstName,
                LastName = adminUserModel.LastName,
                PhoneNumber = adminUserModel.MobilePhoneNumber,
                FullName = $"{adminUserModel.FirstName} {adminUserModel.MiddleName} {adminUserModel.LastName}",
                Password = Guid.NewGuid().ToString()
            });

            //2. Add this user to the organization
            await auth0ApiManagementClient.Organizations.AddMembersAsync(auth0OrganizationId,
                new OrganizationAddMembersRequest {Members = new List<string> {userCreatedInAuth0.UserId}});

            //TODO:
            //3. Add the organization admin role to this user
            return null;

            //public async Task<RosterdRole> GetRole(string roleId)
            //{
            //    var auth0ApiManagementClient = await _auth0AuthenticationService.GetAuth0ApiManagementClient();
            //    var role = await auth0ApiManagementClient.Roles.GetAsync(roleId);

            //    return new RosterdRole
            //    {
            //        RoleId = role.Id,
            //        RoleName = role.Name,
            //        RoleDescription = role.Description
            //    };
            //}

            //public async Task DeleteRole(string roleId)
            //{
            //    var auth0ApiManagementClient = await _auth0AuthenticationService.GetAuth0ApiManagementClient();
            //    await auth0ApiManagementClient.Roles.DeleteAsync(roleId);
            //}
        }
    }
}
