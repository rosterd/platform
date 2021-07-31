using System.Threading.Tasks;
using Auth0.ManagementApi;

namespace Rosterd.Services.Auth.Interfaces
{
    public interface IAuth0AuthenticationService
    {
        Task<string> GetAccessTokenForManagementApi();

        Task<ManagementApiClient> GetAuth0ApiManagementClient();
    }
}
