using System.Threading.Tasks;
using FluentAssertions;
using Flurl.Http;
using Newtonsoft.Json;
using Rosterd.Domain;
using Rosterd.Domain.Models.StaffModels;
using Rosterd.EndToEndTests.Helpers;
using Xunit;

namespace Rosterd.EndToEndTests.Tests.Staff
{
    public class StaffEndToEndTests
    {
        private ApiHelper apiHelper = new ApiHelper();

        [Fact]
        public async Task GivenStaffWhenGetAllStaffThenSuccessWithStaff()
        {
            var url = ApiConstants.STAFF_ENDPOINT + "?PageNumber=1&PageSize=1";

            // GET FACILITIES
            var response = await apiHelper.GetApiRequest(url).GetAsync();

            // ASSERT
            response.EnsureSuccessStatusCode();

            var responsePagedList = JsonConvert.DeserializeObject<PagedList<StaffModel>>(response.Content.ReadAsStringAsync().Result);

            responsePagedList.PageSize.Should().Be(1);
            responsePagedList.Items.Count.Should().Be(1);
        }
    }
}
