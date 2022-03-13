using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rosterd.Admin.Api;
using Rosterd.Data.SqlServer.Context;

namespace Rosterd.ComponentTests.Fixture
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
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
                        .GetRequiredService<ILogger<CustomWebApplicationFactory>>();

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
        }
    }
}
