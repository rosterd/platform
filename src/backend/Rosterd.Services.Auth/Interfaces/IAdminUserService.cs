using System.Collections.Generic;
using System.Threading.Tasks;
using Auth0.AuthenticationApi;
using Auth0.ManagementApi;
using Microsoft.Extensions.Options;
using Rosterd.Domain.Enums;
using Rosterd.Domain.Models.AdminUserModels;
using Rosterd.Domain.Models.Roles;
using Rosterd.Domain.Settings;

namespace Rosterd.Services.Auth.Interfaces
{
    public interface IAdminUserService
    {
        /// <summary>
        /// Create organization admin user for organization
	    /// 1. Create a user in user store
	    /// 2. Add this user to the Organization
	    /// 3. Add role(s) to this user (only admin, this is a constant) where our constant matches auth0 role name
	    /// 4. authentication api - password change to trigger email
        /// </summary>
        /// <param name="auth0OrganizationId"></param>
        /// <param name="adminUserModel"></param>
        /// <returns></returns>
        Task<AdminUserModel> AddOrganizationAdmin(string auth0OrganizationId, AdminUserModel adminUserModel);

        /// <summary>
        /// Create facility admin for organization
        /// 1. Create a user in user store
        /// 2. Add this user to the Organization
        /// 3. Add role(s) to this user (sent by UI, Auth0 role id)
        /// 4. authentication api - password change to trigger email
        /// 5. Create user in our user table
        /// </summary>
        /// <param name="auth0OrganizationId"></param>
        /// <param name="adminUserModel"></param>
        /// <returns></returns>
        Task<AdminUserModel> AddFacilityAdmin(string auth0OrganizationId, AdminUserModel adminUserModel);

        Task<AdminUserModel> AddAdmin(string auth0OrganizationId, AdminUserModel adminUserModel, RosterdRoleEnum roleToAddForUser);
    }
}
