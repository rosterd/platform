using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rosterd.AzureFunctions;
using Rosterd.AzureFunctions.Config;
using Rosterd.Data.SqlServer.Context;
using Rosterd.Data.TableStorage.Context;
using Rosterd.Domain;
using Rosterd.Infrastructure.Messaging;
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

            //Storage Queues
            var staffQueueClient = new StaffQueueClient(tableStorageConnectionString, RosterdConstants.Messaging.StaffQueueName);
            staffQueueClient.QueueClient.CreateIfNotExists();

            var jobsQueueClient = new JobsQueueClient(tableStorageConnectionString, RosterdConstants.Messaging.JobQueueName);
            jobsQueueClient.QueueClient.CreateIfNotExists();

            builder.Services
                .AddLogging()

                //Services
                .AddScoped<IStaffEventsService, StaffEventsService>()
                .AddScoped<IJobEventsService, JobEventsService>()

                .AddSingleton<IQueueClient<StaffQueueClient>>(s => staffQueueClient)
                .AddSingleton<IQueueClient<JobsQueueClient>>(s => jobsQueueClient)

                .AddScoped<IAzureTableStorage>(s => new AzureTableStorage(tableStorageConnectionString))

                //Azure Search
                .AddScoped<ISearchIndexProvider>(s => new SearchIndexProvider(searchServiceEndpoint, searchServiceApiKey))

                //DB
                .AddDbContextPool<RosterdDbContext>((sp, op) => op.UseSqlServer(rosterdDbConnectionString,
                    sqlOptions => sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorNumbersToAdd: null)))


                .AddOptions<FunctionSettings>()
                .Configure<IConfiguration>((settings, configuration) => configuration.GetSection("FunctionSettings").Bind(settings));
        }
    }
}
