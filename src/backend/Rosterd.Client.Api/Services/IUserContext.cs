using System.Collections.Generic;
using System.Threading.Tasks;
using Rosterd.Data.TableStorage.Models;
using Rosterd.Domain.Enums;

namespace Rosterd.Client.Api.Services
{
    public interface IUserContext
    {
        /// <summary>
        /// Creates a rosterd app user
        /// </summary>
        /// <returns></returns>
        Task<RosterdAppUser> GetRosterdAppUserOrCreateIfNotExists();

        /// <summary>
        /// Gets the user id from auth 0
        /// </summary>
        string UserAuth0Id { get; }

        /// <summary>
        /// Gets the users organization id (from auth 0, the auth0 id of the organization)
        /// </summary>
        string UsersAuth0OrganizationId { get; set; }

        long UserStaffId { get; set; }

        long UsersOrganizationId { get; set; }

        /// <summary>
        /// The email address of the user
        /// </summary>
        string UserEmailAddress { get; }

        /// <summary>
        /// The users first name
        /// </summary>
        string UsersFirstName { get; }

        /// <summary>
        /// The users last name
        /// </summary>
        string UsersLastName { get; }

        /// <summary>
        /// The users phone number
        /// </summary>
        string UsersPhoneNumber { get; }

        /// <summary>
        /// Gets all the roles for the user
        /// </summary>
        IEnumerable<string> Roles { get; }

        /// <summary>
        /// Gets the raw access token received from Auth0,
        /// This can be useful if we need to call profile api in auth0 or anything external call we need to make for the user
        /// </summary>
        string AccessToken { get; }

        bool IsUserInRole(RosterdRoleEnum rosterdRole);

        bool IsUserRosterdAdmin();
        bool IsUserFacilityAdmin();
        bool IsUserOrganizationAdmin();
        bool IsUserAStaff();
    }
}
