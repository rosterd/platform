using System.Threading.Tasks;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Rosterd.Domain.Models.AdminUserModels;

namespace Rosterd.Infrastructure.Security.Interfaces
{
    public interface IAuth0AuthenticationService
    {
        Task<string> GetAccessTokenForManagementApi();

        Task<ManagementApiClient> GetAuth0ApiManagementClient();

        Task<User> CreateUserAndAddToOrganization(string auth0OrganizationId, string email, string firstName, string lastName,
            string phoneNumber, Auth0UserMetaData auth0UserMetaData);

        Task<string> GetPasswordResetLink(string auth0UserId);
    }
}
