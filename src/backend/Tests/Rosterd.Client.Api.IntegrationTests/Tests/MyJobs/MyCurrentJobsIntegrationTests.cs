using System.Threading.Tasks;
using FluentAssertions;
using Flurl.Http;
using Newtonsoft.Json;
using Rosterd.Client.Api.IntegrationTests.Helpers;
using Rosterd.Client.Api.IntegrationTests.Utitlities;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.JobModels;
using Xunit;

namespace Rosterd.Client.Api.IntegrationTests.Tests.MyJobs
{
    public class MyCurrentJobsIntegrationTests
    {
        private readonly ApiHelper _apiHelper;

        public MyCurrentJobsIntegrationTests()
        {
            _apiHelper = new ApiHelper();
        }

        [Fact]
        public async Task GivenAcceptedJobsWhenGetMyCurrentJobsThenReturnAcceptedJobs()
        {
            //setup
            var jobId = new JobDataHelper().publishJob(26, 60);

            //call api
            var response = _apiHelper.GetClientApiRequest(ApiConstants.MY_CURRENT_JOBS)
                .SetQueryParam("PageSize", 50)
                .GetAsync().Result;

            //assert
            response.StatusCode.Should().Be(200);

            var responsePagedList = JsonConvert.DeserializeObject<JobPagedList>(response.ResponseMessage.Content.ReadAsStringAsync().Result);

        }
    }
}
