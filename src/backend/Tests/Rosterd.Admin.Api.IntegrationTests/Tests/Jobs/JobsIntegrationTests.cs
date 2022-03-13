using System.Threading.Tasks;
using FluentAssertions;
using Flurl.Http;
using Newtonsoft.Json;
using Rosterd.Admin.Api.IntegrationTests.Helpers;
using Rosterd.Admin.Api.IntegrationTests.Utitlities;
using Rosterd.Domain.Models.JobModels;
using Rosterd.Domain.Models.SkillsModels;
using Xunit;

namespace Rosterd.Admin.Api.IntegrationTests.Tests.Jobs
{
    public class JobsIntegrationTests
    {
        private ApiHelper _apiHelper;

        public JobsIntegrationTests()
        {
            _apiHelper = new ApiHelper();
        }

        [Fact]
        public async Task GivenJobsExistWhenGetJobsThenReturnJobs()
        {
            //set up

            // call api
            var response =  _apiHelper.GetAdminApiRequest(ApiConstants.JOBS_ENDPOINT).GetAsync().Result;

            // assert
            response.StatusCode.Should().Be(200);

            var responsePagedList = JsonConvert.DeserializeObject<TestPagedList<JobModel>>(response.ResponseMessage.Content.ReadAsStringAsync().Result);

            responsePagedList.items.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GivenJobExistWhenGetJobByIdThenReturnJob()
        {
            //set up

            // call api
            var response =  _apiHelper.GetAdminApiRequest(ApiConstants.JOBS_ENDPOINT).AppendPathSegment(47).GetAsync().Result;

            // assert
            response.StatusCode.Should().Be(200);

            var responsePagedList = JsonConvert.DeserializeObject<JobModel>(response.ResponseMessage.Content.ReadAsStringAsync().Result);

            responsePagedList.JobId.Should().Be(47);
        }
    }
}
