using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rosterd.Domain.Models.Users;

namespace Rosterd.Services.Users.Interfaces
{
    public interface IAdminUserService
    {
        /// <summary>
        /// Creates an admin mapping in our db for the given auth0 id
        /// </summary>
        /// <param name="adminUserModel"></param>
        /// <returns></returns>
        Task CreateAdminUser(AdminUserModel adminUserModel);

        /// <summary>
        /// Gets the admin user we have in our db for the auth0 id
        /// </summary>
        /// <param name="auth0Id"></param>
        /// <returns></returns>
        Task<AdminUserModel> GetAdminUser(string auth0Id);

        /// <summary>
        /// Updates the admin user in our db for the given auth0 id
        /// </summary>
        /// <param name="adminUserModel"></param>
        /// <returns></returns>
        Task UpdateAdminUser(AdminUserModel adminUserModel);
    }
}
