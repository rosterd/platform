using System;
using Flurl;
using Flurl.Http;
using Rosterd.Admin.Api.IntegrationTests.Config;

namespace Rosterd.Admin.Api.IntegrationTests.Helpers
{
    public class ApiHelper
    {
        public Configuration configuration;

        public ApiHelper()
        {
            if (null == Environment.GetEnvironmentVariable("ENV")) Environment.SetEnvironmentVariable("ENV", "local");
            configuration = ConfigurationManager.Get();
        }

        public IFlurlRequest GetApiRequest(string endpoint)
        {
            return configuration.BaseUrl
                .AppendPathSegment(endpoint)
                .WithHeader("Content-Type", "application/json")
                .WithHeader("Authorization", "Bearer " + GetAuthToken());
        }

        public string GetAuthToken()
        {
            // string Endpoint = configuration.BaseUrl + configuration.AuthUrl;
            // var responseContent = Endpoint
            //    .WithHeader("username", configuration.UserName)
            //    .WithHeader("password", configuration.Password).GetAsync().Result.ResponseMessage.Content.ReadAsStringAsync().Result;
            return configuration.AccessToken;
        }
    }
}
