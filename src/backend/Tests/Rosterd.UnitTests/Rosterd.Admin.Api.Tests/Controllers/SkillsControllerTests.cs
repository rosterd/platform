using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Rosterd.Admin.Api.Controllers;
using Rosterd.Admin.Api.Requests.Skills;
using Rosterd.Admin.Api.Services;
using Rosterd.Domain.Enums;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.JobModels;
using Rosterd.Domain.Models.SkillsModels;
using Rosterd.Domain.Settings;
using Rosterd.Services.Skills.Interfaces;
using Xunit;

namespace Rosterd.UnitTests.Rosterd.Admin.Api.Tests.Controllers
{
    public class SkillsControllerTests
    {
        private readonly SkillsController _skillsController;
        private readonly Mock<ILogger<SkillsController>> _loggerMock;
        private readonly Mock<ISkillsService> _skillServiceMock;
        private readonly Mock<IUserContext> _userContextMock;
        private readonly Mock<IOptions<AppSettings>> _appSettingsMock;


        public SkillsControllerTests()
        {
            _loggerMock = new Mock<ILogger<SkillsController>>();
            _skillServiceMock = new Mock<ISkillsService>();
            _userContextMock = new Mock<IUserContext>();
            _appSettingsMock = new Mock<IOptions<AppSettings>>();
            _skillsController = new SkillsController(_loggerMock.Object, _skillServiceMock.Object, _appSettingsMock.Object, _userContextMock.Object);
        }

        [Fact]
        public async Task GivenSkillsExistWhenGetAllSkillsThenReturnsPageObject()
        {
            //Build Request
            var pagingQueryStringParameters = new PagingQueryStringParameters
            {
                PageNumber = 1,
                PageSize = 10
            };

            //Mock Service Response Setup
            _skillServiceMock.Setup(x => x.GetAllSkills(pagingQueryStringParameters, _userContextMock.Object.UserAuth0Id))
                .ReturnsAsync(new PagedList<SkillModel>(new List<SkillModel>(), 100, 1, 10, 10));

            //Call GetAllSkills function
            var response = _skillsController.GetAllSkills(pagingQueryStringParameters).Result;;

            _skillServiceMock.Verify(x => x.GetAllSkills(pagingQueryStringParameters, _userContextMock.Object.UserAuth0Id));

            //Assert
            response.Value.CurrentPage.Should().Be(1);
            response.Value.PageSize.Should().Be(10);
            response.Value.Items.Count.Should().Be(0);
        }

        [Fact]
        public async Task GivenSkillExistWhenGetSkillByIdThenReturnsSkill()
        {
            //Build Request
            var skillId = 12L;
            var skillName= "Tester";

            //Mock Service Response Setup
            _skillServiceMock.Setup(x => x.GetSkill(skillId, _userContextMock.Object.UserAuth0Id))
                .ReturnsAsync(new SkillModel
                {
                 SkillId   = skillId,
                 SkillName = skillName
                });
            //Call GetSkillById function
            var response = _skillsController.GetSkillById(skillId).Result;;

            _skillServiceMock.Verify(x => x.GetSkill(skillId, _userContextMock.Object.UserAuth0Id));

            //Assert
            response.Value.SkillId.Should().Be(skillId);
            response.Value.SkillName.Should().Be(skillName);
        }

        [Fact]
        public async Task GivenSkillDoesNotExistWhenGetSkillByIdThenReturnsNotFound()
        {
            //Build Request
            var skillId = 12L;
            var skillName= "Tester";

            //Mock Service Response Setup
            _skillServiceMock.Setup(x => x.GetSkill(skillId, _userContextMock.Object.UserAuth0Id))
                .ReturnsAsync(value: null as SkillModel);
            //Call GetSkillById function
            var response = _skillsController.GetSkillById(skillId).Result;;

            _skillServiceMock.Verify(x => x.GetSkill(skillId, _userContextMock.Object.UserAuth0Id));

            //Assert
            response.Value.Should().BeNull();
        }

        [Fact]
        public async Task GivenValidSkillWhenCreateSkillThenSkillCreated()
        {
            //Build Request
            var skillName= "Tester";
            var addSkillRequest = new AddSkillRequest {SkillName = skillName};

            //Mock Service Response Setup
            _skillServiceMock.Setup(x => x.CreateSkill(It.IsAny<SkillModel>(), _userContextMock.Object.UserAuth0Id))
                .ReturnsAsync(new SkillModel
                {
                    SkillName = skillName
                });

            //Call AddNewSkill function
            var response = _skillsController.AddNewSkill(addSkillRequest).Result;

            _skillServiceMock.Verify(x => x.CreateSkill(It.IsAny<SkillModel>(), _userContextMock.Object.UserAuth0Id));

            //Assert
            response.Value.SkillName.Should().Be(skillName);
        }

        [Fact]
        public async Task GivenValidUpdateSkillRequestWhenUpdateSkillThenSkillUpdated()
        {
            //Build Request
            var skillName= "Tester Updated";
            var updateSkillRequest = new UpdateSkillRequest() {SkillName = skillName};

            //Mock Service Response Setup
            _skillServiceMock.Setup(x => x.UpdateSkill(It.IsAny<SkillModel>(), _userContextMock.Object.UserAuth0Id))
                .ReturnsAsync(new SkillModel
                {
                    SkillName = skillName
                });

            //Call UpdateSkill function
            var response = _skillsController.UpdateSkill(updateSkillRequest).Result;

            _skillServiceMock.Verify(x => x.UpdateSkill(It.IsAny<SkillModel>(), _userContextMock.Object.UserAuth0Id));

            //Assert
            response.Value.SkillName.Should().Be(skillName);
        }

        [Fact]
        public async Task GivenSkillExistsWhenRemoveSkillThenSkillDeleted()
        {
            //Build Request
            var skillId = 123L;


            //Mock Service Response Setup
            _skillServiceMock.Setup(x => x.RemoveSkill(It.IsAny<long>(), _userContextMock.Object.UserAuth0Id));

            //Call RemoveSkill function
            var response = _skillsController.RemoveSkill(skillId).Result;

            _skillServiceMock.Verify(x => x.RemoveSkill(It.IsAny<long>(), _userContextMock.Object.UserAuth0Id));

            //Assert
            response.Should().BeOfType<OkResult>();
        }
    }
}
