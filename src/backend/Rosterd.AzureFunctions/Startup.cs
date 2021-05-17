using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rosterd.AzureFunctions;
using Rosterd.AzureFunctions.Config;
using Rosterd.Infrastructure.Search;
using Rosterd.Infrastructure.Search.Interfaces;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Rosterd.AzureFunctions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var searchServiceEndpoint = builder.GetContext().Configuration.GetValue<string>("FunctionSettings:SearchServiceEndpoint");
            var searchServiceApiKey = builder.GetContext().Configuration.GetValue<string>("FunctionSettings:SearchServiceApiKey");

            builder.Services
                .AddLogging()

                .AddScoped<ISearchIndexProvider>(s => new SearchIndexProvider(searchServiceEndpoint, searchServiceApiKey))

                .AddOptions<FunctionSettings>()
                .Configure<IConfiguration>((settings, configuration) => configuration.GetSection("FunctionSettings").Bind(settings));
        }
    }
}
