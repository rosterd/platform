namespace Rosterd.Client.Api
{
    public class AppSettings
    {
        public int StaticDataCacheDurationMinutes { get; set; }

        public string Environment { get; set; }

        public string StorageAccount { get; set; }

        public string SearchServiceEndpoint { get; set; }

        public string SearchServiceApiKey { get; set; }
    }
}
