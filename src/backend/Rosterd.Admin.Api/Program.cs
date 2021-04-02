using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Rosterd.Admin.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ILogger<Program>? logger = null;

            try
            {
                var builder = CreateHostBuilder(args).Build();
                logger = builder.Services.GetService<ILogger<Program>>();

                logger.LogInformation("Starting web host");
                builder.Run();
            }
            catch (Exception ex)
            {
                logger?.LogCritical(ex, "Host unexpectedly terminated");
                throw;
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(configBuilder =>
                    configBuilder.AddJsonFile("appsettings.json", true, true)
                )
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureKestrel(options => options.AddServerHeader = false);
                });
    }
}
