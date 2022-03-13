using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rosterd.Admin.Api;
using Rosterd.Admin.Api.Infrastructure.Extensions;
using Rosterd.Data.SqlServer.Context;
using Rosterd.Data.TableStorage.Context;

namespace Rosterd.ComponentTests.Fixture
{
    public class ComponentTestApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {

        public IConfiguration Configuration { get; }

        protected override IWebHostBuilder CreateWebHostBuilder()
        {

            var builder = new WebHostBuilder().ConfigureAppConfiguration(configBuilder =>
                    configBuilder.AddJsonFile("testappsettings.json", true, true)
                ).ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                         typeof(DbContextOptions<RosterdDbContext>));

                services.Remove(descriptor);

                services.AddDbContext<RosterdDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<RosterdDbContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<ComponentTestApplicationFactory<TStartup>>>();

                    db.Database.EnsureCreated();

                    // try
                    // {
                    //     Utilities.InitializeDbForTests(db);
                    // }
                    // catch (Exception ex)
                    // {
                    //     logger.LogError(ex, "An error occurred seeding the " +
                    //                         "database with test messages. Error: {Message}", ex.Message);
                    // }
                }
            });
            builder.UseStartup<TStartup>();
            return builder;
        }

    }
}
