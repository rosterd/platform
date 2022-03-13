using System.Threading.Tasks;
using FluentAssertions;
using Flurl.Http;
using Newtonsoft.Json;
using Rosterd.Client.Api.IntegrationTests.Helpers;
using Rosterd.Domain.Models.StaffModels;
using Xunit;

namespace Rosterd.Client.Api.IntegrationTests.Tests.MyPreferences
{
    public class MyPreferencesIntegrationTests
    {
        private readonly ApiHelper _apiHelper;

        public MyPreferencesIntegrationTests()
        {
            _apiHelper = new ApiHelper();
        }

        [Fact]
        public async Task GivenStaffExistsWhenGetMyPreferencesThenPreferencesReturned()
        {
            //setup

            //call api
            var response = _apiHelper.GetClientApiRequest(ApiConstants.MY_PREFERENCES)
                .GetAsync().Result;

            //assert
            response.StatusCode.Should().Be(200);

            var preferencesModel = JsonConvert.DeserializeObject<StaffAppUserPreferencesModel>(response.ResponseMessage.Content.ReadAsStringAsync().Result);

            preferencesModel.City.Should().Be("Auckland");
        }

        [Fact]
        public async Task GivenStaffExistsWhenPutPreferencesThenPreferenceUpdated()
        {
            //setup
            var response = _apiHelper.GetClientApiRequest(ApiConstants.MY_PREFERENCES)
                .GetAsync().Result;
            //assert
            response.StatusCode.Should().Be(200);
            var preferencesModel = JsonConvert.DeserializeObject<StaffAppUserPreferencesModel>(response.ResponseMessage.Content.ReadAsStringAsync().Result);
            preferencesModel.FirstName.Should().NotBe(preferencesModel.FirstName + 1);
            preferencesModel.FirstName = preferencesModel.FirstName + 1;

            var updateResponse = _apiHelper.GetClientApiRequest(ApiConstants.MY_PREFERENCES).PutJsonAsync(preferencesModel).Result;
            updateResponse.StatusCode.Should().Be(200);
        }
    }
}
