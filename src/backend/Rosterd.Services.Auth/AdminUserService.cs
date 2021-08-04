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
using Rosterd.Domain.Enums;
using Rosterd.Domain.Exceptions;
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

        ///<inheritdoc/>
        public async Task<AdminUserModel> AddOrganizationAdmin(string auth0OrganizationId, AdminUserModel adminUserModel) => await AddAdmin(auth0OrganizationId, adminUserModel, RosterdRoleEnum.OrganizationAdmin);

        ///<inheritdoc/>
        public async Task<AdminUserModel> AddFacilityAdmin(string auth0OrganizationId, AdminUserModel adminUserModel) => await AddAdmin(auth0OrganizationId, adminUserModel, RosterdRoleEnum.FacilityAdmin);

        public async Task<AdminUserModel> AddAdmin(string auth0OrganizationId, AdminUserModel adminUserModel, RosterdRoleEnum roleToAddForUser)
        {
            var auth0ApiManagementClient = await _auth0AuthenticationService.GetAuth0ApiManagementClient();

            //1. Create the user in auth0 with a random password
            //2. Add this user to the organization
            var userCreatedInAuth0 = await _auth0AuthenticationService.CreateUserAndAddToOrganization(auth0OrganizationId, adminUserModel.Email,
                adminUserModel.FirstName, adminUserModel.LastName, adminUserModel.PhoneNumber);

            //3. Add the organization admin role to this user
            var roleToAdd = await _rolesService.GetRole(roleToAddForUser);
            if (roleToAdd == null)
                throw new RoleDoesNotExistException();
            
            await auth0ApiManagementClient.Organizations.AddMemberRolesAsync(auth0OrganizationId, userCreatedInAuth0.UserId, new OrganizationAddMemberRolesRequest {Roles = new List<string> {roleToAdd.RoleId}});

            //By this point we have created the user, add them to the to the organization and assigned them the role org-admin
            //4. Now, final thing we need to do is trigger a password-change event so the user will get an email
            await _auth0AuthenticationService.SendPasswordResetEmailToUser(adminUserModel.Email);

            adminUserModel.AdminUserId = userCreatedInAuth0.UserId;
            return adminUserModel;
        }
    }
}
