using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rosterd.Data.SqlServer.Context;
using Rosterd.Data.TableStorage.Context;
using Rosterd.Infrastructure.Search;
using Rosterd.Infrastructure.Search.Interfaces;
using Rosterd.Services.Jobs;
using Rosterd.Services.Jobs.Interfaces;
using Rosterd.Services.Users;
using Rosterd.Services.Users.Interfaces;

namespace Rosterd.Client.Api.Infrastructure.ServiceRegistrations
{
    public static class RegisterAppAndDatabase
    {
        public static void RegisterAppDependencies(this IServiceCollection services, IConfiguration config, IWebHostEnvironment hostingEnvironment)
        {
            //Register all the settings
            services.Configure<AppSettings>(config.GetSection(nameof(AppSettings)));

            // Add useful interface for accessing the ActionContext && HttpContext && IUrlHelper outside a controller (ie: in the middleware)
            services.AddHttpContextAccessor();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //Services
            services.AddScoped<IAdminUserService, AdminUserService>();
            services.AddScoped<IJobsService, JobsService>();

            //User context

            //Search
            services.AddScoped<ISearchIndexProvider>(s => new SearchIndexProvider(config.GetValue<string>("AppSettings:SearchServiceEndpoint"),
                config.GetValue<string>("AppSettings:SearchServiceApiKey")));

            //Eventing
            services.AddScoped<IJobEventsService, JobEventsService>();

            //Db contexts
            services.AddScoped<IRosterdDbContext, RosterdDbContext>();
            services.AddScoped<IAzureTableStorage>(s => new AzureTableStorage(config.GetConnectionString("TableStorageConnectionString")));
        }

        public static void RegisterDatabaseDependencies(this IServiceCollection services, IConfiguration config, IWebHostEnvironment hostingEnvironment)
        {
            //Main SQL Server connection
            var connectionString = config.GetConnectionString("SQLDBConnectionString");
            if (hostingEnvironment.IsDevelopment() && string.IsNullOrWhiteSpace(connectionString))
            {
                services.AddDbContextPool<RosterdDbContext>((sp, op) =>
                    op.UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                        .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning)));
            }
            else
            {
                services.AddDbContextPool<RosterdDbContext>((sp, op) => op.UseSqlServer(connectionString,
                    sqlOptions => sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorNumbersToAdd: null)));
            }
        }
    }
}
