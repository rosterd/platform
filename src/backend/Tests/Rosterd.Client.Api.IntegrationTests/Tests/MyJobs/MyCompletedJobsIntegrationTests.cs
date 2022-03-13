using System.Threading.Tasks;
using FluentAssertions;
using Flurl.Http;
using Newtonsoft.Json;
using Rosterd.Client.Api.IntegrationTests.Helpers;
using Rosterd.Client.Api.IntegrationTests.Utitlities;
using Rosterd.Domain.Models.JobModels;
using Xunit;

namespace Rosterd.Client.Api.IntegrationTests.Tests.MyJobs
{
    public class MyCompletedJobsIntegrationTests
    {
        private readonly ApiHelper _apiHelper;

        public MyCompletedJobsIntegrationTests()
        {
            _apiHelper = new ApiHelper();
        }

        [Fact]
        public async Task GivenCompletedJobsExistsfWhenCompletedJobsThenJobsReturned()
        {
            //setup

            //call api
            var response = _apiHelper.GetClientApiRequest(ApiConstants.MY_COMPLETED_JOBS)
                .GetAsync().Result;

            //assert
            response.StatusCode.Should().Be(200);

            var responsePagedList = JsonConvert.DeserializeObject<JobPagedList>(response.ResponseMessage.Content.ReadAsStringAsync().Result);
        }
    }
}
