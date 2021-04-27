using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosterd.Domain
{
    public class AppSettings
    {
        public int StaticDataCacheDurationMinutes { get; set; }

        public string Environment { get; set; }

        public string EventGridTopicEndpoint { get; set; }

        public string EventGridTopicKey { get; set; }
    }
}
