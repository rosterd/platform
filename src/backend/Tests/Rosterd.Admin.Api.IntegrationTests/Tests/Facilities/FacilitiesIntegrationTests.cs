using System.Threading.Tasks;
using FluentAssertions;
using Flurl.Http;
using Newtonsoft.Json;
using Rosterd.Admin.Api.IntegrationTests.Helpers;
using Rosterd.Admin.Api.IntegrationTests.Utitlities;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.FacilitiesModels;
using Xunit;
using Xunit.Sdk;

namespace Rosterd.Admin.Api.IntegrationTests.Tests.Facilities
{
    public class FacilitiesIntegrationTests
    {
        private ApiHelper _apiHelper;

        public FacilitiesIntegrationTests()
        {
            _apiHelper = new ApiHelper();
        }

        [Fact]
        public async Task GivenFacilitiesExistWhenGetAllFacilitiesThenReturnFacilities()
        {
            //set up

            // call api
            var response =  _apiHelper.GetOrgAdminApiRequest(ApiConstants.FACILITIES_ENDPOINT).GetAsync().Result;

            // assert
            response.StatusCode.Should().Be(200);

            var responsePagedList = JsonConvert.DeserializeObject<TestPagedList<FacilityModel>>(response.ResponseMessage.Content.ReadAsStringAsync().Result);

            responsePagedList.items.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GivenFacilityExistsWhenGetFacilityByIdThenReturnFacility()
        {
            //set up

            // call api
            var response =  _apiHelper.GetOrgAdminApiRequest(ApiConstants.FACILITIES_ENDPOINT).AppendPathSegment(39).GetAsync().Result;

            // assert
            response.StatusCode.Should().Be(200);

            var responsePagedList = JsonConvert.DeserializeObject<FacilityModel>(response.ResponseMessage.Content.ReadAsStringAsync().Result);

            responsePagedList.FacilityId.Should().Be(39);
        }

        [Fact]
        public async Task GivenValidFacilityWhenCreateFacilityThenFacilityCreated()
        {
            //set up

            // call api
            var facilityId = new FacilitiesDataHelper().createFacility();
            // assert
            facilityId.Should().NotBe(null);
        }

    }
}
