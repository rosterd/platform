using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Rosterd.Client.Api.ServiceRegistrations
{
    public static class RegisterHealthChecks
    {
        public static void RegisterHealthCheckDependencies(this IServiceCollection services, IConfiguration config)
        {
            //Register HealthChecks and UI
            //services.AddHealthChecks()
            //    .AddCheck("Google Ping", new PingHealthCheck("www.google.com", 100))
            //    .AddCheck("Bing Ping", new PingHealthCheck("www.bing.com", 100))
            //    .AddSqlServer(
            //        config["ConnectionStrings:SQLDBConnectionString"],
            //        "SELECT 1;",
            //        "SQL",
            //        HealthStatus.Degraded,
            //        new[] {"db", "sql", "sqlserver"});

            // Set the maximum history entries by endpoint that will be served by the UI api middleware
            services.AddHealthChecksUI(setup => setup.MaximumHistoryEntriesPerEndpoint(100))
                .AddInMemoryStorage();
        }
    }
}
