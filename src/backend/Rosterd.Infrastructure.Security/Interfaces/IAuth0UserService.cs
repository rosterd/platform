using System.Collections.Generic;
using System.Threading.Tasks;
using Rosterd.Domain.Enums;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.AdminUserModels;

namespace Rosterd.Infrastructure.Security.Interfaces
{
    public interface IAuth0UserService
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
        Task<Auth0UserModel> AddOrganizationAdminToAuth0(string auth0OrganizationId, Auth0UserModel adminUserModel);

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
        Task<Auth0UserModel> AddFacilityAdminToAuth0(string auth0OrganizationId, Auth0UserModel adminUserModel);

        Task<Auth0UserModel> AddStaffToAuth0(string auth0OrganizationId, string firstName, string lastName, string email, string mobilePhoneNumber, string usersAuth0OrganizationId);

        Task<Auth0UserModel> AddUserToAuth0(string auth0OrganizationId, Auth0UserModel adminUserModel, RosterdRoleEnum roleToAddForUser);

        Task RemoveUserFromAuth0(string auth0Id);

        Task<PagedList<Auth0UserModel>> GetAdminUsers(string auth0OrganizationId, PagingQueryStringParameters pagingParams);
    }
}
