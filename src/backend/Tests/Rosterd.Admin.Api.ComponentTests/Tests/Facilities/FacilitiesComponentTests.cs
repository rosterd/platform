using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Flurl.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Rosterd.Admin.Api.Requests.Facility;
using Rosterd.ComponentTests.Fixture;
using Rosterd.ComponentTests.Helpers;
using Rosterd.Data.SqlServer.Context;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.FacilitiesModels;
using Rosterd.Domain.Models.OrganizationModels;
using Rosterd.Services.Facilities.Interfaces;
using Xunit;
using Xunit.Abstractions;

namespace Rosterd.ComponentTests.Tests.Facilities
{
    public class FacilitiesComponentTests
    {

        private readonly ApiHelper _apiHelper;

        [Fact]
        public async Task GivenFacilitiesWhenGetAllFacilitiesThenPagedListWithNumberOfFacilitiesRequested()
        {

            // GET FACILITIES
            var response = _apiHelper.GetApiRequest(ApiConstants.FACILITIES_ENDPOINT)
                .SetQueryParam("PageNumber", 1)
                .SetQueryParam("PageSize", 10)
                .GetAsync().Result;

            // ASSERT
            response.StatusCode.Should().Be(200);

            var responsePagedList = JsonConvert.DeserializeObject<PagedList<FacilityModel>>(response.ResponseMessage.Content.ReadAsStringAsync().Result);

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
            var response =_apiHelper.GetApiRequest(url).GetAsync().Result;

            // ASSERT
            response.StatusCode.Should().Be(200);

            var facilityModel = JsonConvert.DeserializeObject<FacilityModel>(response.ResponseMessage.Content.ReadAsStringAsync().Result);

            await deleteFacilityAsync(facilityId);
        }


        [Fact]
        public async Task GivenFacilityWhenGetFacilityByIdThenFacilityIsReturned()
        {
            var facilityId = createFacilityAsync().Result;
            // ENDPOINT URL
            var url = ApiConstants.FACILITIES_ENDPOINT + "/" + facilityId;

            // GET FACILITIES
            var response = _apiHelper.GetApiRequest(url).GetAsync().Result;

            // ASSERT
            response.StatusCode.Should().Be(200);

            var facility = JsonConvert.DeserializeObject<FacilityModel>(response.ResponseMessage.Content.ReadAsStringAsync().Result);

            await deleteFacilityAsync(facilityId);
        }


        [Fact]
        public async Task GivenFacilityWhenDeleteFacilityThenFacilityIsDeleted()
        {
            var facilityId = createFacilityAsync().Result;
            // ENDPOINT URL
            var url = ApiConstants.FACILITIES_ENDPOINT + "/" + facilityId;

            // GET FACILITIES
            var response = _apiHelper.GetApiRequest(url).GetAsync().Result;

            // ASSERT
            response.StatusCode.Should().Be(200);

            await deleteFacilityAsync(facilityId);
        }


        private async Task<int> createFacilityAsync() {
            var facilityId = new Random().Next(1000);
            var addUpdateFacilityRequest = new AddFacilityRequest
            {
                    FacilityName = "ComponentTestFacility",
                    Latitude = 10,
                    Longitude = 10,
                    City = "Auckland",
                    Suburb = "Mt Eden",
                    Address = "308 Shacks Road, Mt Eden",
                    Country = "NZ",
                    PhoneNumber1 = "0912345007",
                    PhoneNumber2 = "090898490"
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(addUpdateFacilityRequest), Encoding.UTF8, "application/json");
            var response = _apiHelper.GetApiRequest(ApiConstants.FACILITIES_ENDPOINT).PostAsync(stringContent).Result;
            response.StatusCode.Should().Be(204);
            return facilityId;
        }

        private async Task deleteFacilityAsync(int facilityId)
        {
            var response =  _apiHelper.GetApiRequest(ApiConstants.FACILITIES_ENDPOINT).AppendPathSegment(facilityId).DeleteAsync().Result;
            response.StatusCode.Should().Be(200);
        }
    }
}
