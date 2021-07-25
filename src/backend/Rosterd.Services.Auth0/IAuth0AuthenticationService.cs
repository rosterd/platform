using System;
using System.Threading.Tasks;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Auth0.ManagementApi;
using Microsoft.Extensions.Caching.Memory;

namespace Rosterd.Services.Auth0
{
    public interface IAuth0AuthenticationService
    {
        Task<string> GetAccessTokenForManagementApi();

        Task<ManagementApiClient> GetAuth0ApiManagementClient();
    }
}
