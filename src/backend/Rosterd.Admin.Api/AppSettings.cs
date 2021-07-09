namespace Rosterd.Admin.Api
{
    public class AppSettings
    {
        public int StaticDataCacheDurationMinutes { get; set; }

        public string Environment { get; set; }

        public string EventGridTopicEndpoint { get; set; }

        public string EventGridTopicKey { get; set; }

        public string SearchServiceEndpoint { get; set; }

        public string SearchServiceApiKey { get; set; }
    }
}
