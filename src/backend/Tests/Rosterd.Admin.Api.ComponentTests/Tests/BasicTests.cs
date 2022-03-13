using System.Threading.Tasks;
using Rosterd.Admin.Api;
using Rosterd.ComponentTests.Fixture;
using Xunit;

namespace Rosterd.ComponentTests.Tests
{
    public class BasicTests
        : IClassFixture<ComponentTestApplicationFactory<Startup>>
    {
        private readonly ComponentTestApplicationFactory<Startup> _factory;

        public BasicTests(ComponentTestApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/facilities")]
        [InlineData("/jobs")]
        [InlineData("/staff")]
        [InlineData("/skills")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
        }

    }
}
