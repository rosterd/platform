using System;

namespace Rosterd.Admin.Api.IntegrationTests.Config
{
    public class Configuration
    {
        public string BaseUrl { get; set; }
        public string AuthUrl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string AccessToken { get; set; }
    }
}
