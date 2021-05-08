using System;
using System.Collections.Generic;
using System.Text;

namespace Rosterd.AzureFunctions.Config
{
    public class FunctionSettings
    {
        public string EventGridConnectionString { get; set; }

        public string RosterdDbConnectionString { get; set; }

        public string StorageAccountConnectionString { get; set; }

        public string Environment { get; set; }

        public string SearchServiceEndpoint { get; set; }

        public string SearchServiceApiKey { get; set; }
    }
}
