using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth0.ManagementApi.Models;
using Auth0.ManagementApi.Paging;
using Rosterd.Domain.Enums;
using Rosterd.Domain.Exceptions;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.AdminUserModels;
using Rosterd.Infrastructure.Extensions;
using Rosterd.Infrastructure.Security.Interfaces;

namespace Rosterd.Infrastructure.Security
{
    public class Auth0UserService : IAuth0UserService
    {
        private readonly IAuth0AuthenticationService _auth0AuthenticationService;
        private readonly IRolesService _rolesService;

        public Auth0UserService(IAuth0AuthenticationService auth0AuthenticationService, IRolesService rolesService)
        {
            _auth0AuthenticationService = auth0AuthenticationService;
            _rolesService = rolesService;
        }

        ///<inheritdoc/>
        public async Task<Auth0UserModel> AddOrganizationAdminToAuth0(string auth0OrganizationId, Auth0UserModel adminUserModel) => await AddUserToAuth0(auth0OrganizationId, adminUserModel, RosterdRoleEnum.OrganizationAdmin);

        ///<inheritdoc/>
        public async Task<Auth0UserModel> AddFacilityAdminToAuth0(string auth0OrganizationId, Auth0UserModel adminUserModel) => await AddUserToAuth0(auth0OrganizationId, adminUserModel, RosterdRoleEnum.FacilityAdmin);

        public async Task<Auth0UserModel> AddStaffToAuth0(string auth0OrganizationId, string firstName, string lastName, string email, string mobilePhoneNumber) =>
            await AddUserToAuth0(auth0OrganizationId,
                new Auth0UserModel {Email = email, FirstName = firstName, LastName = lastName, MobilePhoneNumber = mobilePhoneNumber}, RosterdRoleEnum.Staff);

        public async Task RemoveUserFromAuth0(string auth0Id)
        {
            var auth0ApiManagementClient = await _auth0AuthenticationService.GetAuth0ApiManagementClient();
            await auth0ApiManagementClient.Users.DeleteAsync(auth0Id);
        }

        public async Task<Auth0UserModel> AddUserToAuth0(string auth0OrganizationId, Auth0UserModel adminUserModel, RosterdRoleEnum roleToAddForUser)
        {
            var auth0ApiManagementClient = await _auth0AuthenticationService.GetAuth0ApiManagementClient();

            //1. Create the user in auth0 with a random password
            //2. Add this user to the organization
            var userCreatedInAuth0 = await _auth0AuthenticationService.CreateUserAndAddToOrganization(auth0OrganizationId, adminUserModel.Email,
                adminUserModel.FirstName, adminUserModel.LastName, adminUserModel.MobilePhoneNumber);

            //3. Add the organization admin role to this user
            var roleToAdd = await _rolesService.GetRole(roleToAddForUser);
            if (roleToAdd == null)
                throw new RoleDoesNotExistException();
            
            await auth0ApiManagementClient.Organizations.AddMemberRolesAsync(auth0OrganizationId, userCreatedInAuth0.UserId, new OrganizationAddMemberRolesRequest {Roles = new List<string> {roleToAdd.RoleId}});

            //By this point we have created the user, add them to the to the organization and assigned them the role org-admin
            //4. Now, final thing we need to do is trigger a password-change event so the user will get an email
            await _auth0AuthenticationService.SendPasswordResetEmailToUser(adminUserModel.Email);

            adminUserModel.UserAuth0Id = userCreatedInAuth0.UserId;
            return adminUserModel;
        }

        public async Task<Domain.Models.PagedList<Auth0UserModel>> GetAdminUsers(string auth0OrganizationId, PagingQueryStringParameters pagingParams)
        {
            var auth0ApiManagementClient = await _auth0AuthenticationService.GetAuth0ApiManagementClient();

            var allOrganizationMembers = auth0ApiManagementClient.Organizations.GetAllMembersAsync(auth0OrganizationId,
                new PaginationInfo(pagingParams.PageNumber, pagingParams.PageSize, true));

            if (allOrganizationMembers == null || allOrganizationMembers.Result.IsNullOrEmpty())
                return Domain.Models.PagedList<Auth0UserModel>.EmptyPagedList();


            var auth0UserModels = new List<Auth0UserModel>();
            foreach (var organizationMember in allOrganizationMembers.Result)
            {
                var userRoles = auth0ApiManagementClient.Users.GetRolesAsync(organizationMember.UserId, new PaginationInfo());

                //Only admin users
                if (userRoles != null && userRoles.Result.IsNullOrEmpty())
                    continue;

                //If use ris staff role then not an admin
                var isStaff = userRoles.Result.FirstOrDefault(s => s.Name == RosterdRoleEnum.Staff.ToString());
                if(isStaff != null)
                    continue;

                auth0UserModels.Add(new Auth0UserModel
                {
                    Email = organizationMember.Email,
                    FirstName = organizationMember.Name,
                    LastName = string.Empty,
                    MobilePhoneNumber = string.Empty,
                    UserAuth0Id = organizationMember.UserId,
                    RosterdRolesForUser = userRoles.Result.AlwaysList().Select(s => s.Name.ToEnum<RosterdRoleEnum>()).AlwaysList()
                });
            }

            return new Domain.Models.PagedList<Auth0UserModel>(auth0UserModels, allOrganizationMembers.Result.Paging.Total, pagingParams.PageNumber,
                pagingParams.PageSize);
        }
    }
}
