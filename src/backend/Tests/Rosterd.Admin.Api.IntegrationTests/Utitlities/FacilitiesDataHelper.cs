using System;
using System.Collections.Generic;
using FluentAssertions;
using Flurl.Http;
using Newtonsoft.Json;
using Rosterd.Admin.Api.IntegrationTests.Helpers;
using Rosterd.Admin.Api.Requests.Facility;
using Rosterd.Admin.Api.Requests.Jobs;
using Rosterd.Domain.Models.FacilitiesModels;
using Rosterd.Domain.Models.JobModels;

namespace Rosterd.Admin.Api.IntegrationTests.Utitlities
{
    public class FacilitiesDataHelper
    {
        private ApiHelper _apiHelper;

        public FacilitiesDataHelper()
        {
            _apiHelper = new ApiHelper();
        }

        public long createFacility()
        {
            //setup
            var response = _apiHelper.GetOrgAdminApiRequest(ApiConstants.FACILITIES_ENDPOINT)
                .PostJsonAsync(new FacilitiesRequestBuilder().buildAAddFacilityRequest()).Result;
            //assert
            response.StatusCode.Should().Be(200);

            var facilityModel = JsonConvert.DeserializeObject<FacilityModel>(response.ResponseMessage.Content.ReadAsStringAsync().Result);
            return facilityModel.FacilityId;
        }

        class FacilitiesRequestBuilder
        {
            public AddFacilityRequest buildAAddFacilityRequest()
            {
                var addFacilityRequest = new AddFacilityRequest
                {
                    FacilityName = "Integration Test" + new Random().Next(),
                    Address = "30 Athenic Avenue",
                    Suburb = "Lynfield",
                    City = "Auckland",
                    Country = "New Zealand",
                    Latitude = (decimal) -36.931047199,
                    Longitude = (decimal) 174.721187471d,
                    PhoneNumber1 = "091234567",
                };
                return addFacilityRequest;
            }
        }
    }
}

