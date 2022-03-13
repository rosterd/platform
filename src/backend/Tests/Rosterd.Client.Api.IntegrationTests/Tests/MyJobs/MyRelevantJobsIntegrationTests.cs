using System.Threading.Tasks;
using FluentAssertions;
using Flurl.Http;
using Newtonsoft.Json;
using Rosterd.Client.Api.IntegrationTests.Helpers;
using Rosterd.Client.Api.IntegrationTests.Utitlities;
using Xunit;

namespace Rosterd.Client.Api.IntegrationTests.Tests.MyJobs
{
    public class MyRelevantJobsIntegrationTests
    {
        private readonly ApiHelper _apiHelper;

            public MyRelevantJobsIntegrationTests()
            {
                _apiHelper = new ApiHelper();
            }

            [Fact]
            public async Task GivenJobPublishedWhenRelevantJobsThenReturnPublishedJobs()
            {
                //setup

                //call api
                var response = _apiHelper.GetClientApiRequest(ApiConstants.MY_RELEVANT_JOBS)
                    .GetAsync().Result;

                //assert
                response.StatusCode.Should().Be(200);

                var responsePagedList = JsonConvert.DeserializeObject<JobPagedList>(response.ResponseMessage.Content.ReadAsStringAsync().Result);
            }
    }
}
