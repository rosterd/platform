using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rosterd.AzureFunctions;
using Rosterd.AzureFunctions.Config;
using Rosterd.Data.SqlServer.Context;
using Rosterd.Data.TableStorage.Context;
using Rosterd.Infrastructure.Search;
using Rosterd.Infrastructure.Search.Interfaces;
using Rosterd.Services.Jobs;
using Rosterd.Services.Jobs.Interfaces;
using Rosterd.Services.Staff;
using Rosterd.Services.Staff.Interfaces;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Rosterd.AzureFunctions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var searchServiceEndpoint = builder.GetContext().Configuration.GetValue<string>("FunctionSettings:SearchServiceEndpoint");
            var searchServiceApiKey = builder.GetContext().Configuration.GetValue<string>("FunctionSettings:SearchServiceApiKey");

            var eventGridTopicEndpoint = builder.GetContext().Configuration.GetValue<string>("FunctionSettings:EventGridTopicEndpoint");
            var eventGridTopicKey = builder.GetContext().Configuration.GetValue<string>("FunctionSettings:EventGridTopicKey");

            var rosterdDbConnectionString = builder.GetContext().Configuration.GetValue<string>("FunctionSettings:RosterdDbConnectionString");
            var tableStorageConnectionString = builder.GetContext().Configuration.GetValue<string>("FunctionSettings:TableStorageConnectionString");

            builder.Services
                .AddLogging()

                //Services
                .AddScoped<IStaffEventsService, StaffEventsService>()
                .AddScoped<IJobEventsService, JobEventsService>()

                //Azure Search
                .AddScoped<ISearchIndexProvider>(s => new SearchIndexProvider(searchServiceEndpoint, searchServiceApiKey))

                //Table Storage
                .AddScoped<IAzureTableStorage>(s => new AzureTableStorage(tableStorageConnectionString))

                //DB
                .AddDbContextPool<RosterdDbContext>((sp, op) => op.UseSqlServer(rosterdDbConnectionString,
                    sqlOptions => sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorNumbersToAdd: null)))

                //Event Grid
                //.AddScoped<IEventGridClient>(provider => new EventGridClient(new TopicCredentials(eventGridTopicKey)))

                .AddOptions<FunctionSettings>()
                .Configure<IConfiguration>((settings, configuration) => configuration.GetSection("FunctionSettings").Bind(settings));
        }
    }
}
