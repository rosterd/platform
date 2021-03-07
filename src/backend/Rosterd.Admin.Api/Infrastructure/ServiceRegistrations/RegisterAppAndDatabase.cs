using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rosterd.Admin.Api.Infrastructure.Configs;
using Rosterd.Data.SqlServer.Context;
using Rosterd.Services.Facilities;
using Rosterd.Services.Facilities.Interfaces;
using Rosterd.Services.Jobs;
using Rosterd.Services.Jobs.Interfaces;
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

            // Add useful interface for accessing the ActionContext && HttpContext && IUrlHelper outside a controller (ie: in the middleware)
            services.AddHttpContextAccessor();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //Services
            services.AddScoped<IStaffService, StaffService>();
            services.AddScoped<IStaffFacilitiesService, StaffFacilitiesService>();
            services.AddScoped<IStaffSkillsService, StaffSkillsService>();
            services.AddScoped<IFacilitiesService, FacilitiesService>();
            services.AddScoped<ISkillsService, SkillsService>();
            services.AddScoped<IJobsService, JobsService>();

            //User context

            //Db contexts
            services.AddScoped<IRosterdDbContext, RosterdDbContext>();
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
