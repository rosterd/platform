using System.Threading.Tasks;
using FluentAssertions;
using Flurl.Http;
using Newtonsoft.Json;
using Rosterd.Domain;
using Rosterd.Domain.Models.JobModels;
using Rosterd.EndToEndTests.Helpers;
using Xunit;

namespace Rosterd.EndToEndTests.Tests.Jobs
{
    public class JobsEndToEndTests
    {
        private ApiHelper apiHelper = new ApiHelper();

        [Fact]
        public async Task GivenJobsWhenGetAllJobsThenSuccessWithJobs()
        {
            var url = ApiConstants.JOBS_ENDPOINT + "?PageNumber=1&PageSize=1";

            // GET FACILITIES
            var response = await apiHelper.GetApiRequest(url).GetAsync();

            // ASSERT
            response.EnsureSuccessStatusCode();

            var responsePagedList = JsonConvert.DeserializeObject<PagedList<JobModel>>(response.Content.ReadAsStringAsync().Result);

            responsePagedList.PageSize.Should().Be(1);
            responsePagedList.Items.Count.Should().Be(1);
        }

    }
}
