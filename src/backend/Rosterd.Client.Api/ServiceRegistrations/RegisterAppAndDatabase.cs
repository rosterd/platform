using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rosterd.Client.Api.Infrastructure.Configs;
using Rosterd.Data.SqlServer.Context;
using Rosterd.Data.TableStorage;
using Rosterd.Data.TableStorage.Context;
using Rosterd.Services.Facilities;
using Rosterd.Services.Facilities.Interfaces;
using Rosterd.Services.Jobs;
using Rosterd.Services.Jobs.Interfaces;
using Rosterd.Services.Skills;
using Rosterd.Services.Skills.Interfaces;
using Rosterd.Services.Users;
using Rosterd.Services.Users.Interfaces;

namespace Rosterd.Client.Api.ServiceRegistrations
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
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IJobsService, JobsService>();

            //User context

            //Db contexts
            services.AddScoped<IRosterdDbContext, RosterdDbContext>();
            services.AddScoped<IAzureTableStorage>(s => new AzureTableStorage(config.GetSection("ConnectionStrings:TableStorageConnectionString").Value));
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
                services.AddDbContextPool<RosterdDbContext>((sp, op) => op.UseSqlServer(connectionString));
            }
        }
    }
}
