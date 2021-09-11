using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Auth0.Core.Exceptions;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Rosterd.Domain.Exceptions;
using Rosterd.Domain.Settings;
using Rosterd.Infrastructure.Security.Interfaces;

namespace Rosterd.Infrastructure.Security
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

        public async Task SendPasswordResetEmailToUser(string usersEmailAddress) =>
            await _auth0AuthenticationApiClient.ChangePasswordAsync(new ChangePasswordRequest
            {
                ClientId = _auth0Settings.ClientId, Connection = _auth0Settings.ConnectionName, Email = usersEmailAddress
            });

        public async Task<User> CreateUserAndAddToOrganization(string auth0OrganizationId, string email, string firstName, string lastName, string phoneNumber)
        {
            try
            {
                var auth0ApiManagementClient = await GetAuth0ApiManagementClient();

                //Create the user in auth0 with a random password
                var userCreatedInAuth0 = await auth0ApiManagementClient.Users.CreateAsync(new UserCreateRequest
                {
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    //MobilePhoneNumber = phoneNumber,
                    FullName = $"{firstName} {lastName}",
                    Password = Guid.NewGuid().ToString(),
                    Connection = _auth0Settings.ConnectionName,
                    VerifyEmail = false //setting this to true will send a verify your email to the user, we don't want to do that at this stage
                });

                //Add this user to the organization
                await auth0ApiManagementClient.Organizations.AddMembersAsync(auth0OrganizationId,
                    new OrganizationAddMembersRequest { Members = new List<string> { userCreatedInAuth0.UserId } });

                return userCreatedInAuth0;

            }
            catch (ErrorApiException ex)
            {
                if (ex.StatusCode == HttpStatusCode.Conflict || ex.Message.ToLower().Contains("already exists"))
                    throw new EntityAlreadyExistsException($"User with email {email} already exists");

                throw;
            }
        }
    }
}
