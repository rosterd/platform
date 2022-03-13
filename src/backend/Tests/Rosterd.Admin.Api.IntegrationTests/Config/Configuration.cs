using System;

namespace Rosterd.Admin.Api.IntegrationTests.Config
{
    public class Configuration
    {
        public string AdminApiBaseUrl { get; set; }
        public string ClientApiBaseUrl { get; set; }
        public string AuthUrl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string AdminAccessToken { get; set; }
        public string OrgAdminAccessToken { get; set; }
    }
}
