using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rosterd.Client.Api.ServiceRegistrations;

namespace Rosterd.Client.Api.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers all database dependencies and all app dependencies
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <param name="hostingEnvironment"></param>
        /// <returns></returns>
        public static IServiceCollection AddAppAndDatabaseDependencies(this IServiceCollection services, IConfiguration config, IWebHostEnvironment hostingEnvironment)
        {
            services.RegisterDatabaseDependencies(config, hostingEnvironment);
            services.RegisterAppDependencies(config, hostingEnvironment);

            return services;
        }

        /// <summary>
        /// Adds all the required health checks
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services, IConfiguration config)
        {
            services.RegisterHealthCheckDependencies(config);
            return services;
        }

        /// <summary>
        /// Adds all the swagger UI and open api spec
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
        {
            services.RegisterSwaggerDependencies();
            return services;
        }

        /// <summary>
        /// Adds all the custom authenticate required for Auth0 and jwt/bearer tokens
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomAuthenticationWithJwtBearer(this IServiceCollection services, IConfiguration config)
        {
            //services.RegisterAuthenticationDependencies(config);
            return services;
        }
    }
}
