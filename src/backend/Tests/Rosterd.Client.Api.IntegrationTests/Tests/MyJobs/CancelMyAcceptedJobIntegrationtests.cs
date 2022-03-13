using System;
using System.Threading.Tasks;
using FluentAssertions;
using Flurl.Http;
using Rosterd.Client.Api.IntegrationTests.Helpers;
using Rosterd.Client.Api.IntegrationTests.Utitlities;
using Xunit;

namespace Rosterd.Client.Api.IntegrationTests.Tests.MyJobs
{
    public class CancelMyAcceptedJobIntegrationtests
    {
        private readonly ApiHelper _apiHelper;

        public CancelMyAcceptedJobIntegrationtests()
        {
            _apiHelper = new ApiHelper();
        }

        [Fact]
        public async Task GivenAcceptedJobInGracePeriodWhenCancelJobsThenJobCancelled()
        {
            //setup
            var jobId = new JobDataHelper().publishJob(26);
            //call api
            var response = _apiHelper.GetClientApiRequest(String.Format(ApiConstants.CANCEL_MY_ACCEPTED_JOBS, jobId))
                .DeleteAsync().Result;

            //assert
            response.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GivenAcceptedJobOutsideGracePeriodWhenCancelJobsThenJobCancelled()
        {
            //setup
            var jobId = new JobDataHelper().publishJob(26, 2880);

            //call api
            var response = _apiHelper.GetClientApiRequest(String.Format(ApiConstants.CANCEL_MY_ACCEPTED_JOBS, jobId))
                .DeleteAsync().Result;

            response.StatusCode.Should().Be(422);
            response.ResponseMessage.Content.Should().Be("You have past the grace time to cancel this job.");
        }
    }
}
