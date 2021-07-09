using System;
using System.Collections.Generic;
using System.Text;

namespace Rosterd.AzureFunctions.Config
{
    public class FunctionSettings
    {
        public string EventGridTopicEndpoint { get; set; }

        public string EventGridTopicKey { get; set; }

        public string RosterdDbConnectionString { get; set; }

        public string TableStorageConnectionString { get; set; }

        public string Environment { get; set; }

        public string SearchServiceEndpoint { get; set; }

        public string SearchServiceApiKey { get; set; }
    }
}
