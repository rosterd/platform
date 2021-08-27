using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Rosterd.Admin.Api.Requests.Skills;
using Rosterd.ComponentTests.Fixture;
using Rosterd.ComponentTests.Helpers;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.SkillsModels;
using Xunit;
using Xunit.Abstractions;

namespace Rosterd.ComponentTests.Tests.Skills
{
    public class SkillsComponentTests
    {
        private readonly ApplicationFixture _appFixture = new ApplicationFixture();


        [Fact]
        public async Task GivenSkillssWhenGetAllSkillsThenPagedListWithNumberOfSkillsRequested()
        {
            // ENDPOINT URL
            var url = ApiConstants.SKILLS_ENDPOINT + "?PageNumber=1&PageSize=1";

            // GET SKILLS
            var response = await _appFixture.HttpClient.GetAsync(url);

            // ASSERT
            response.EnsureSuccessStatusCode();

            var responsePagedList = JsonConvert.DeserializeObject<PagedList<SkillModel>>(response.Content.ReadAsStringAsync().Result);

            responsePagedList.PageSize.Should().Be(1);
            responsePagedList.Items.Count.Should().Be(1);
        }

        [Fact]
        public async Task GivenValidSkillWhenPostedThenSkillIsAdded()
        {
            var skillId = createSkillAsync().Result;
            // ENDPOINT URL
            var url = ApiConstants.SKILLS_ENDPOINT + "/" + skillId;

            // GET SKILLS
            var response = await _appFixture.HttpClient.GetAsync(url);

            // ASSERT
            response.EnsureSuccessStatusCode();

            var skillModel = JsonConvert.DeserializeObject<SkillModel>(response.Content.ReadAsStringAsync().Result);

            await deleteSkillAsync(skillId);
        }


        [Fact]
        public async Task GivenSkillWhenGetSkillByIdThenSkillIsReturned()
        {
            var skillId = createSkillAsync().Result;
            // ENDPOINT URL
            var url = ApiConstants.SKILLS_ENDPOINT + "/" + skillId;

            // GET SKILLS
            var response = await _appFixture.HttpClient.GetAsync(url);

            // ASSERT
            response.EnsureSuccessStatusCode();

            var skill = JsonConvert.DeserializeObject<SkillModel>(response.Content.ReadAsStringAsync().Result);

            await deleteSkillAsync(skillId);
        }


        [Fact]
        public async Task GivenSkillWhenDeleteSkillThenSkillIsDeleted()
        {
            var skillId = createSkillAsync().Result;
            // ENDPOINT URL
            var url = ApiConstants.SKILLS_ENDPOINT + "/" + skillId;

            // GET SKILLS
            var response = await _appFixture.HttpClient.GetAsync(url);

            // ASSERT
            response.EnsureSuccessStatusCode();

            await deleteSkillAsync(skillId);
        }


        private async Task<int> createSkillAsync()
        {
            var skillId = new Random().Next(1000);
            var addUpdateSkillRequest = new AddSkillRequest
            {
                SkillName = "test",
                Description = "test"
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(addUpdateSkillRequest), Encoding.UTF8, "application/json");
            var response = await _appFixture.HttpClient.PostAsync(ApiConstants.SKILLS_ENDPOINT, stringContent);
            response.EnsureSuccessStatusCode();
            return skillId;
        }

        private async Task deleteSkillAsync(int skillId)
        {
            var response = await _appFixture.HttpClient.DeleteAsync(ApiConstants.SKILLS_ENDPOINT + "/" + skillId);
            response.EnsureSuccessStatusCode();
        }
    }
}
