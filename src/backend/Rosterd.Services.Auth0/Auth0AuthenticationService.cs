using System;
using System.Threading.Tasks;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Auth0.ManagementApi;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Rosterd.Domain.Settings;

namespace Rosterd.Services.Auth0
{
    public class Auth0AuthenticationService : IAuth0AuthenticationService
    {
        private readonly AuthenticationApiClient _auth0AuthenticationApiClient;
        private readonly IManagementConnection _auth0ManagementConnection;
        private readonly Auth0Settings _auth0Settings;
        private readonly IMemoryCache _cache;

        private const string AccessTokenKey = "AccessTokenKey";

        public Auth0AuthenticationService(AuthenticationApiClient auth0AuthenticationApiClient, IMemoryCache cache, IManagementConnection auth0ManagementConnection, IOptions<Auth0Settings> auth0Settings)
        {
            _auth0AuthenticationApiClient = auth0AuthenticationApiClient;
            _cache = cache;
            _auth0ManagementConnection = auth0ManagementConnection;
            _auth0Settings = auth0Settings.Value;
        }

        public async Task<string> GetAccessTokenForManagementApi()
        {
            if (!_cache.TryGetValue(AccessTokenKey, out var cacheEntry))
            {
                // Set cache options (Auth0 access expires in 24, we are using 23 to be on the safe side)
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(23));

                var token = await _auth0AuthenticationApiClient.GetTokenAsync(new ClientCredentialsTokenRequest
                {
                    Audience = $"https://{_auth0Settings.Domain}/api/v2/",
                    ClientId = _auth0Settings.ClientId,
                    ClientSecret = _auth0Settings.ClientSecret
                });

                // Save data in cache.
                cacheEntry = token.AccessToken;
                _cache.Set(AccessTokenKey, token.AccessToken, cacheEntryOptions);
            }

            return cacheEntry.ToString();
        }

        public async Task<ManagementApiClient> GetAuth0ApiManagementClient()
        {
            var token = await GetAccessTokenForManagementApi();

            var managementClient = new ManagementApiClient(token, _auth0Settings.Domain, _auth0ManagementConnection);
            return managementClient;
        }

    }
}
