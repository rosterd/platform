using System;
using Flurl.Http;
using Newtonsoft.Json;
using Rosterd.EndToEndTests.Config;

namespace Rosterd.EndToEndTests.Helpers
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
            var Endpoint = configuration.BaseUrl + endpoint;
            return Endpoint
                .WithHeader("Content-Type", "application/json");
        }

        public string GetAuthToken()
        {
            string Endpoint = configuration.BaseUrl + configuration.AuthUrl;
            var responseContent = Endpoint
               .WithHeader("username", configuration.UserName)
               .WithHeader("password", configuration.Password).GetAsync().Result.Content.ReadAsStringAsync().Result;
            return "";
        }
    }
}
