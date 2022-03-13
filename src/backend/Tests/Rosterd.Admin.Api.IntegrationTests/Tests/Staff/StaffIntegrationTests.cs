using System.Threading.Tasks;
using FluentAssertions;
using Flurl.Http;
using Newtonsoft.Json;
using Rosterd.Admin.Api.IntegrationTests.Helpers;
using Rosterd.Admin.Api.IntegrationTests.Utitlities;
using Rosterd.Domain.Models.SkillsModels;
using Rosterd.Domain.Models.StaffModels;
using Xunit;

namespace Rosterd.Admin.Api.IntegrationTests.Tests.Staff
{
    public class StaffIntegrationTests
    {
        private ApiHelper _apiHelper;

        public StaffIntegrationTests()
        {
            _apiHelper = new ApiHelper();
        }

        [Fact]
        public async Task GivenStaffExistWhenGetAllStaffThenReturnStaff()
        {
            //set up

            // call api
            var response =  _apiHelper.GetOrgAdminApiRequest(ApiConstants.STAFF_ENDPOINT).GetAsync().Result;

            // assert
            response.StatusCode.Should().Be(200);

            var responsePagedList = JsonConvert.DeserializeObject<TestPagedList<StaffModel>>(response.ResponseMessage.Content.ReadAsStringAsync().Result);

            responsePagedList.items.Should().NotBeEmpty();
        }
    }
}
