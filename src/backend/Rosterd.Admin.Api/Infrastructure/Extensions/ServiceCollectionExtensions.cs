using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rosterd.Admin.Api.Infrastructure.ServiceRegistrations;

namespace Rosterd.Admin.Api.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCorsWithAllowAll(this IServiceCollection services) =>
            //Configure CORS to allow any origin, header and method. 
            //Change the CORS policy based on your requirements.
            //More info see: https://docs.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-3.0
            services.AddCors(options => options.AddPolicy("AllowAll",
                builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

        /// <summary>
        /// Configures caching for the application. Registers the <see cref="IDistributedCache"/> and
        /// <see cref="IMemoryCache"/> types with the services collection or IoC container. The
        /// <see cref="IDistributedCache"/> is intended to be used in cloud hosted scenarios where there is a shared
        /// cache, which is shared between multiple instances of the application. Use the <see cref="IMemoryCache"/>
        /// otherwise.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>The services with caching services added.</returns>
        public static IServiceCollection AddCustomCaching(this IServiceCollection services) =>
            services
                .AddMemoryCache()
                // Adds IDistributedCache which is a distributed cache shared between multiple servers.
                //For now memory cache will serve us ok, when the use case comes we can change to Redis with
                //AddDistributedRedisCache
                .AddDistributedMemoryCache();

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
            services.RegisterAuthenticationDependencies(config);
            return services;
        }
    }
}
