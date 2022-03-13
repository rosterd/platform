using System;
using System.Threading.Tasks;
using FluentAssertions;
using Flurl.Http;
using Newtonsoft.Json;
using Rosterd.Client.Api.IntegrationTests.Helpers;
using Rosterd.Client.Api.IntegrationTests.Utitlities;
using Xunit;

namespace Rosterd.Client.Api.IntegrationTests.Tests.MyJobs
{
    public class ConfirmMyCurrentJobIntegrationTests
    {
        private readonly ApiHelper _apiHelper;

        public ConfirmMyCurrentJobIntegrationTests()
        {
            _apiHelper = new ApiHelper();
        }

        [Fact]
        public async Task GivenJobIsPublishedAcceptedThenJobAccepted()
        {
            //setup
            var jobId = new JobDataHelper().publishJob(26);
            //call api
            var response = _apiHelper.GetClientApiRequest(String.Format(ApiConstants.ACCEPT_MY_CURRENT_JOBS, jobId))
                .PutAsync().Result;

            //assert
            response.StatusCode.Should().Be(200);

            var responsePagedList = JsonConvert.DeserializeObject<JobPagedList>(response.ResponseMessage.Content.ReadAsStringAsync().Result);

        }
    }
}
