using System;
using System.IO;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rosterd.Admin.Api;
using Rosterd.Data.SqlServer.Context;
using Xunit.Abstractions;

namespace Rosterd.ComponentTests.Fixture
{
    public class ApplicationFixture
    {
        private readonly ServiceCollection _services = new ServiceCollection();
        private readonly string _hostingEnvironment = "Component";

        private IHost _applicationHost;
        public IHost ApplicationHost => _applicationHost ??= ConfigureApplicationHost();

        private HttpClient _client;
        public HttpClient HttpClient => _client ??= ConfigureClient();

        private ITestOutputHelper _outputHelper;

        public void ConfigureServices(Action<ServiceCollection> configure) => configure(_services);

        public void ConfigureLogging(ITestOutputHelper logger) => _outputHelper = logger;

      
        public T GetService<T>()
        {
            return ApplicationHost.Services.GetService<T>();
        }

        public void Dispose()
        {
            HttpClient?.Dispose();
            ApplicationHost?.Dispose();
        }

        private IHost ConfigureApplicationHost()
        {
            var host = Program.CreateHostBuilder(new string[] { }, hostBuilder =>
            {
                hostBuilder.UseTestServer();
                hostBuilder.UseEnvironment(_hostingEnvironment);
                var projectDir = Directory.GetCurrentDirectory();
                var configPath = Path.Combine(projectDir, "appSettings.testing.json");
                hostBuilder.ConfigureAppConfiguration((context, conf) => conf.AddJsonFile(configPath));
                hostBuilder.ConfigureTestServices(collection => { });
            }).Build();
            host.Start();
            return host;
        }

        private HttpClient ConfigureClient()
        {
            var client = ApplicationHost.GetTestServer().CreateClient();
            return client;
        }

        private RosterdDbContext ConfigureDbContext()
        {
            var options = new DbContextOptionsBuilder<RosterdDbContext>()
                          .UseInMemoryDatabase(Guid.NewGuid().ToString())
                          .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            return new RosterdDbContext(options.Options);
        }
    }
}
