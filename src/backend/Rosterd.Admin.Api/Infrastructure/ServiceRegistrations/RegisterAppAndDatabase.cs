using System;
using Auth0.AuthenticationApi;
using Auth0.ManagementApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rosterd.Admin.Api.Services;
using Rosterd.Data.SqlServer.Context;
using Rosterd.Data.TableStorage.Context;
using Rosterd.Domain;
using Rosterd.Domain.Settings;
using Rosterd.Infrastructure.Messaging;
using Rosterd.Infrastructure.Search;
using Rosterd.Infrastructure.Search.Interfaces;
using Rosterd.Infrastructure.Security;
using Rosterd.Infrastructure.Security.Interfaces;
using Rosterd.Services.Dashboards;
using Rosterd.Services.Dashboards.Interfaces;
using Rosterd.Services.Facilities;
using Rosterd.Services.Facilities.Interfaces;
using Rosterd.Services.Jobs;
using Rosterd.Services.Jobs.Interfaces;
using Rosterd.Services.Organizations;
using Rosterd.Services.Organizations.Interfaces;
using Rosterd.Services.Skills;
using Rosterd.Services.Skills.Interfaces;
using Rosterd.Services.Staff;
using Rosterd.Services.Staff.Interfaces;


namespace Rosterd.Admin.Api.Infrastructure.ServiceRegistrations
{
    public static class RegisterAppAndDatabase
    {
        public static void RegisterAppDependencies(this IServiceCollection services, IConfiguration config, IWebHostEnvironment hostingEnvironment)
        {
            //Register all the settings
            services.Configure<AppSettings>(config.GetSection(nameof(AppSettings)));
            services.Configure<Auth0Settings>(config.GetSection(nameof(Auth0Settings)));

            // Add useful interface for accessing the ActionContext && HttpContext && IUrlHelper outside a controller (ie: in the middleware)
            services.AddHttpContextAccessor();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //Services
            services.AddScoped<IStaffService, StaffService>();
            services.AddScoped<IStaffFacilitiesService, StaffFacilitiesService>();
            services.AddScoped<IStaffSkillsService, StaffSkillsService>();
            services.AddScoped<IFacilitiesService, FacilitiesService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<ISkillsService, SkillsService>();
            services.AddScoped<IJobsService, JobsService>();
            services.AddScoped<IOrganizationsService, OrganizationsService>();

            services.AddScoped<IJobsValidationService, JobsValidationService>();
            services.AddScoped<IBelongsToValidator, BelongsToValidator>();

            //Search
            services.AddScoped<ISearchIndexProvider>(s => new SearchIndexProvider(config.GetValue<string>("AppSettings:SearchServiceEndpoint"),
                config.GetValue<string>("AppSettings:SearchServiceApiKey")));

            //Eventing
            services.AddScoped<IStaffEventsService, StaffEventsService>();
            services.AddScoped<IJobEventsService, JobEventsService>();

            //Orchestrators

            //User context
            services.AddScoped<IUserContext, UserContext>();

            //Db contexts
            services.AddScoped<IRosterdDbContext, RosterdDbContext>();
            services.AddScoped<IAzureTableStorage>(s => new AzureTableStorage(config.GetConnectionString("TableStorageConnectionString")));

            //Storage Queues
            var staffQueueClient = new StaffQueueClient(config.GetConnectionString("TableStorageConnectionString"), RosterdConstants.Messaging.StaffQueueName);
            staffQueueClient.QueueClient.CreateIfNotExists();

            var jobsQueueClient = new JobsQueueClient(config.GetConnectionString("TableStorageConnectionString"), RosterdConstants.Messaging.JobQueueName);
            jobsQueueClient.QueueClient.CreateIfNotExists();

            services.AddSingleton<IQueueClient<StaffQueueClient>>(s => staffQueueClient);
            services.AddSingleton<IQueueClient<JobsQueueClient>>(s => jobsQueueClient);

            //Auth0, auth, roles
            var domain = $"{config["Auth0Settings:Domain"]}/";
            services.AddSingleton<IManagementConnection, HttpClientManagementConnection>(); //part of auth0 sdk, auth0 recommends this to be singleton due to reuse of httpclient under the hood
            services.AddSingleton<AuthenticationApiClient>(s => new AuthenticationApiClient(domain)); //part of auth0 sdk, auth0 recommends this to be singleton due to reuse of httpclient under the hood
            services.AddScoped<IAuth0AuthenticationService, Auth0AuthenticationService>();
            services.AddScoped<IRolesService, RolesService>();
            services.AddScoped<IAuth0UserService, Auth0UserService>();
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
