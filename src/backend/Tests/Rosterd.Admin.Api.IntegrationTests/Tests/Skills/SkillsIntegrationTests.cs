using System.Threading.Tasks;
using FluentAssertions;
using Flurl.Http;
using Newtonsoft.Json;
using Rosterd.Admin.Api.IntegrationTests.Helpers;
using Rosterd.Admin.Api.IntegrationTests.Utitlities;
using Rosterd.Domain.Models.FacilitiesModels;
using Rosterd.Domain.Models.SkillsModels;
using Xunit;

namespace Rosterd.Admin.Api.IntegrationTests.Tests.Skills
{
    public class SkillsIntegrationTests
    {
        private ApiHelper _apiHelper;

        public SkillsIntegrationTests()
        {
            _apiHelper = new ApiHelper();
        }

        [Fact]
        public async Task GivenSkillsExistWhenGetAllSkillsThenReturnSkills()
        {
            //set up

            // call api
            var response =  _apiHelper.GetAdminApiRequest(ApiConstants.SKILLS_ENDPOINT).GetAsync().Result;

            // assert
            response.StatusCode.Should().Be(200);

            var responsePagedList = JsonConvert.DeserializeObject<TestPagedList<SkillModel>>(response.ResponseMessage.Content.ReadAsStringAsync().Result);

            responsePagedList.items.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GivenSkillsWhenGetSkillByIdThenReturnSkill()
        {
            //set up

            // call api
            var response =  _apiHelper.GetAdminApiRequest(ApiConstants.SKILLS_ENDPOINT).AppendPathSegment(26).GetAsync().Result;

            // assert
            response.StatusCode.Should().Be(200);

            var responsePagedList = JsonConvert.DeserializeObject<SkillModel>(response.ResponseMessage.Content.ReadAsStringAsync().Result);

            responsePagedList.SkillId.Should().Be(26);
            responsePagedList.SkillName.Should().Be("Developer");
        }
    }
}
