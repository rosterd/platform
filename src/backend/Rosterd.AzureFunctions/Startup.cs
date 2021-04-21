using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rosterd.AzureFunctions;
using Rosterd.AzureFunctions.Config;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Rosterd.AzureFunctions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var storageAccountConnectionString = GetConfig<string>("FileProcessOptions:WdlBasketSalesStorageConnectionString");

            builder.Services
                .AddLogging()

                .AddOptions<FunctionSettings>()
                .Configure<IConfiguration>((settings, configuration) => configuration.GetSection("FunctionSettings").Bind(settings));
        }

        private static T GetConfig<T>(string key)
        {
            var value = Environment.GetEnvironmentVariable(key);
            return value != null ? (T)Convert.ChangeType(value, typeof(T)) : default;
        }
    }
}
