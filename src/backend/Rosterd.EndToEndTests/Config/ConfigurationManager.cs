using System;
using Newtonsoft.Json;

namespace Rosterd.EndToEndTests.Config
{
    public class ConfigurationManager
    {
        private const string local = "Environment.local.json";
        private const string dev = "Environment.dev.json";
   

        private static Configuration configuration;

        public static Configuration Get()
        {
            if (configuration == null)
            {
                if (Environment.GetEnvironmentVariable("ENV").Equals("local"))
                {
                    var localEnv = ResourceFinder.ReadFromResourceFile(local);
                    configuration = JsonConvert.DeserializeObject<Configuration>(localEnv);
                }
                if (Environment.GetEnvironmentVariable("ENV").Equals("dev"))
                {
                    var devEnv = ResourceFinder.ReadFromResourceFile(dev);
                    configuration = JsonConvert.DeserializeObject<Configuration>(devEnv);
                }
            }

            return configuration;
        }

    }
}
