using System;
using Flurl;
using Flurl.Http;
using Rosterd.Client.Api.IntegrationTests.Config;

namespace Rosterd.Client.Api.IntegrationTests.Helpers
{
    public class ApiHelper
    {
        public Configuration configuration;

        public ApiHelper()
        {
            if (null == Environment.GetEnvironmentVariable("ENV")) Environment.SetEnvironmentVariable("ENV", "dev");
            configuration = ConfigurationManager.Get();
        }

        public IFlurlRequest GetClientApiRequest(string endpoint)
        {
            return configuration.ClientApiBaseUrl
                .AppendPathSegment(endpoint)
                .WithHeader("Content-Type", "application/json")
                .WithHeader("Authorization", "Bearer " + GetClientAuthToken());
        }

        public IFlurlRequest GetAdminApiRequest(string endpoint)
        {
            return configuration.AdminApiBaseUrl
                .AppendPathSegment(endpoint)
                .WithHeader("Content-Type", "application/json")
                .WithHeader("Authorization", "Bearer " + GetAdminAuthToken());
        }

        public string GetAdminAuthToken()
        {
            // string Endpoint = configuration.BaseUrl + configuration.AuthUrl;
            // var responseContent = Endpoint
            //    .WithHeader("username", configuration.UserName)
            //    .WithHeader("password", configuration.Password).GetAsync().Result.ResponseMessage.Content.ReadAsStringAsync().Result;
            return configuration.AdminAccessToken;
        }

        public string GetClientAuthToken()
        {
            // string Endpoint = configuration.BaseUrl + configuration.AuthUrl;
            // var responseContent = Endpoint
            //    .WithHeader("username", configuration.UserName)
            //    .WithHeader("password", configuration.Password).GetAsync().Result.ResponseMessage.Content.ReadAsStringAsync().Result;
            return configuration.ClientAccessToken;
        }
    }
}
