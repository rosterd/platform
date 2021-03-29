using System.Threading.Tasks;
using FluentAssertions;
using Flurl.Http;
using Newtonsoft.Json;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.FacilitiesModels;
using Rosterd.EndToEndTests.Helpers;
using Xunit;

namespace Rosterd.EndToEndTests.Tests.Facilities
{
    public class FacilitiesEndToEndTests
    {
        private ApiHelper apiHelper = new ApiHelper();

        [Fact]
        public async Task GivenFacilitiesWhenGetAllFacilitiesThenSuccessWithFacilitiesAsync()
        {
            var url = ApiConstants.FACILITIES_ENDPOINT + "?PageNumber=1&PageSize=1";

            // GET FACILITIES
            var response = await apiHelper.GetApiRequest(url).GetAsync();

            // ASSERT
            response.EnsureSuccessStatusCode();

            var responsePagedList = JsonConvert.DeserializeObject<PagedList<FacilityModel>>(response.Content.ReadAsStringAsync().Result);

            responsePagedList.PageSize.Should().Be(1);
            responsePagedList.Items.Count.Should().Be(1);
        }

    }
}
