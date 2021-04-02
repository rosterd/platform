using System.Collections.Generic;
using System.Threading.Tasks;
using Rosterd.Domain.Models.StaffModels;
using Rosterd.Domain.Models.Tenants;

namespace Rosterd.Admin.Api.Services
{
    public interface IUserContext
    {
        /// <summary>
        /// Checks if a tenant and user exists for the currently logged in user
        /// </summary>
        /// <returns></returns>
        Task<bool> DoesTenantAndUserExist();

        /// <summary>
        /// Gets the tenant of the current logged in user.
        /// NB: This gets the full tenant object and can be a bit expensive if you just need the tenant id
        /// then calls the GetTenantIdForUser() method
        /// </summary>
        Task<long> GetTenantIdForUser();

        /// <summary>
        /// Gets the tenant of the current logged in user.
        /// NB: This gets the full tenant object and StaffToAuth0Id object from cache
        /// can be a bit expensive due to the weight of the object (serializing/deserializing from cache)
        /// if you just need the tenant id
        /// then call the GetTenantIdForUser() method which is a very light weight and fast
        /// </summary>
        Task<(TenantModel Tenant, StaffModel Staff)> GetTenantAndUser();

        /// <summary>
        /// Gets all the roles for the user
        /// </summary>
        IEnumerable<string> Roles { get; }

        /// <summary>
        /// Gets the unique id from Azure B2C for the user
        /// </summary>
        string IdmUserId { get; }

        /// <summary>
        /// Gets the raw access token received from Auth0,
        /// This can be useful if we need to call profile in b2c or anything external call we need to make for the user
        /// </summary>
        string AccessToken { get; }

        string UserName { get; }

        string UserNickname { get; }

        string UserPictureHref { get; }

        string UserEmail { get; }

        bool IsUserEmailVerified { get; }
    }
}
