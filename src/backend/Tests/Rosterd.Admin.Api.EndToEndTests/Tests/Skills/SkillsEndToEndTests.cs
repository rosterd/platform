using System.Threading.Tasks;
using FluentAssertions;
using Flurl.Http;
using Newtonsoft.Json;
using Rosterd.Domain;
using Rosterd.Domain.Models.JobModels;
using Rosterd.Domain.Models.SkillsModels;
using Rosterd.EndToEndTests.Helpers;
using Xunit;

namespace Rosterd.EndToEndTests.Tests.Skills
{
    public class SkillsEndToEndTests
    {
        private ApiHelper apiHelper = new ApiHelper();

        [Fact]
        public async Task GivenSkillsWhenGetAllSkillsThenSuccessWithSkills()
        {
            var url = ApiConstants.SKILLS_ENDPOINT + "?PageNumber=1&PageSize=1";

            // GET FACILITIES
            var response = await apiHelper.GetApiRequest(url).GetAsync();

            // ASSERT
            response.EnsureSuccessStatusCode();

            var responsePagedList = JsonConvert.DeserializeObject<PagedList<SkillModel>>(response.Content.ReadAsStringAsync().Result);

            responsePagedList.PageSize.Should().Be(1);
            responsePagedList.Items.Count.Should().Be(1);
        }
    }
}
