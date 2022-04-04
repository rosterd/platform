using System.Threading.Tasks;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;

namespace Rosterd.Infrastructure.Security.Interfaces
{
    public interface IAuth0AuthenticationService
    {
        Task<string> GetAccessTokenForManagementApi();

        Task<ManagementApiClient> GetAuth0ApiManagementClient();

        Task SendPasswordResetEmailToUser(string usersEmailAddress);

        Task<User> CreateUserAndAddToOrganization(string auth0OrganizationId, string email, string firstName, string lastName,
            string phoneNumber);

        Task<string> GetPasswordResetLink(string auth0UserId);
    }
}
