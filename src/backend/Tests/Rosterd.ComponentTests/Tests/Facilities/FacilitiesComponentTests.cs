using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Rosterd.ComponentTests.Fixture;
using Rosterd.ComponentTests.Helpers;
using Rosterd.Data.SqlServer.Context;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.FacilitiesModels;
using Rosterd.Domain.Models.OrganizationModels;
using Rosterd.Domain.Requests.Facility;
using Rosterd.Services.Facilities.Interfaces;
using Xunit;
using Xunit.Abstractions;

namespace Rosterd.ComponentTests.Tests.Facilities
{
    public class FacilitiesComponentTests
    {

        private readonly ApplicationFixture _appFixture = new ApplicationFixture();


        [Fact]
        public async Task GivenFacilitiesWhenGetAllFacilitiesThenPagedListWithNumberOfFacilitiesRequested()
        {
            // ENDPOINT URL
            var url = ApiConstants.FACILITIES_ENDPOINT + "?PageNumber=1&PageSize=1";

            // GET FACILITIES
            var response = await _appFixture.HttpClient.GetAsync(url);

            // ASSERT
            response.EnsureSuccessStatusCode();

            var responsePagedList = JsonConvert.DeserializeObject<PagedList<FacilityModel>>(response.Content.ReadAsStringAsync().Result);

            responsePagedList.PageSize.Should().Be(1);
            responsePagedList.Items.Count.Should().Be(1);
        }

        [Fact]
        public async Task GivenValidFacilityWhenPostedThenFacilityIsAdded()
        {
            var facilityId = createFacilityAsync().Result;
            // ENDPOINT URL
            var url = ApiConstants.FACILITIES_ENDPOINT + "/" + facilityId;

            // GET FACILITIES
            var response = await _appFixture.HttpClient.GetAsync(url);

            // ASSERT
            response.EnsureSuccessStatusCode();

            var facilityModel = JsonConvert.DeserializeObject<FacilityModel>(response.Content.ReadAsStringAsync().Result);

            await deleteFacilityAsync(facilityId);
        }


        [Fact]
        public async Task GivenFacilityWhenGetFacilityByIdThenFacilityIsReturned()
        {
            var facilityId = createFacilityAsync().Result;
            // ENDPOINT URL
            var url = ApiConstants.FACILITIES_ENDPOINT + "/" + facilityId;

            // GET FACILITIES
            var response = await _appFixture.HttpClient.GetAsync(url);

            // ASSERT
            response.EnsureSuccessStatusCode();

            var facility = JsonConvert.DeserializeObject<FacilityModel>(response.Content.ReadAsStringAsync().Result);

            await deleteFacilityAsync(facilityId);
        }


        [Fact]
        public async Task GivenFacilityWhenDeleteFacilityThenFacilityIsDeleted()
        {
            var facilityId = createFacilityAsync().Result;
            // ENDPOINT URL
            var url = ApiConstants.FACILITIES_ENDPOINT + "/" + facilityId;

            // GET FACILITIES
            var response = await _appFixture.HttpClient.GetAsync(url);

            // ASSERT
            response.EnsureSuccessStatusCode();

            await deleteFacilityAsync(facilityId);
        }


        private async Task<int> createFacilityAsync() {
            var facilityId = new Random().Next(1000);
            var addUpdateFacilityRequest = new AddFacilityRequest
            {
                FacilityToAdd = new FacilityModel
                {
                    FacilityId = facilityId,
                    FacilityName = "ComponentTestFacility",
                    Organization = new OrganizationModel {OrganizationId = 0},
                    City = "Auckland",
                    Suburb = "Mt Eden",
                    Address = "308 Shacks Road, Mt Eden",
                    Country = "NZ",
                    PhoneNumber1 = "0912345007"
                }
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(addUpdateFacilityRequest), Encoding.UTF8, "application/json");
            var response = await _appFixture.HttpClient.PostAsync(ApiConstants.FACILITIES_ENDPOINT, stringContent);
            response.EnsureSuccessStatusCode();
            return facilityId;
        }

        private async Task deleteFacilityAsync(int facilityId)
        {
            var response = await _appFixture.HttpClient.DeleteAsync(ApiConstants.FACILITIES_ENDPOINT+"/"+facilityId);
            response.EnsureSuccessStatusCode();
        }
    }
}
