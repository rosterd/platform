using System;
using System.Data;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Rosterd.Data.SqlServer.Context;
using Rosterd.Data.SqlServer.Models;
using Rosterd.Domain.Exceptions;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.SkillsModels;
using Rosterd.Infrastructure.Security.Interfaces;
using Rosterd.Services.Mappers;
using Rosterd.Services.Skills;
using Rosterd.UnitTests.Rosterd.Admin.Api.Utilities;
using Xunit;

namespace Rosterd.UnitTests.Rosterd.Admin.Api.Tests.Services
{
    public class SkillsServiceTests
    {
        private readonly SkillsService _skillService;
        private readonly IRosterdDbContext _context;
        private readonly Mock<IBelongsToValidator> _belongsToValidatorMock;

        public SkillsServiceTests()
        {
            _context = new RosterdDbContext(new DbContextOptionsBuilder<RosterdDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options);
             _belongsToValidatorMock = new Mock<IBelongsToValidator>();
             _skillService = new SkillsService(_context, _belongsToValidatorMock.Object);
        }

        [Fact]
        public async Task GivenValidIdWhenGetAllSkillsThenReturnPagedList()
        {
            //Build Request
            SkillsDataHelper.ArrangeSkillsTestData(_context);
            var pagingQueryStringParameters = new PagingQueryStringParameters {PageNumber = 1, PageSize = 10};
            var organizationId = 1L;
            var auth0OrganizationId = "auth0|Test";

            //Mock Service Response Setup
            _belongsToValidatorMock.Setup(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()))
                .ReturnsAsync(new Organization {OrganizationId = organizationId});

            //Call GetAllSkills function
            var response = _skillService.GetAllSkills(pagingQueryStringParameters, auth0OrganizationId).Result;
            ;

            //Assert
            _belongsToValidatorMock.Verify(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()));
            response.PageSize.Should().Be(10);
            response.Items.Count.Should().Be(6);
        }

        [Fact]
        public async Task GivenSkillExistsWhenGetSkillThenSkillReturned()
        {
            //Build Request
            SkillsDataHelper.ArrangeSkillsTestData(_context);
            var skillId = 1;
            var organizationId = 1L;
            var auth0OrganizationId = "auth0|Test";

            //Mock Service Response Setup
            _belongsToValidatorMock.Setup(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()))
                .ReturnsAsync(new Organization {OrganizationId = organizationId});

            //Call GetSkill function
            var response = _skillService.GetSkill(skillId, auth0OrganizationId).Result;
            ;

            //Assert
            _belongsToValidatorMock.Verify(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()));
            response.SkillId.Should().Be(skillId);
            response.SkillName.Should().Be("DotNet");
        }

        [Fact]
        public async Task GivenSkillDoesNotExistWhenGetSkillThenReturnedNull()
        {
            //Build Request
            SkillsDataHelper.ArrangeSkillsTestData(_context);
            var skillId = 8;
            var organizationId = 1L;
            var auth0OrganizationId = "auth0|Test";

            //Mock Service Response Setup
            _belongsToValidatorMock.Setup(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()))
                .ReturnsAsync(new Organization {OrganizationId = organizationId});

            //Call GetSkill function
            var response = _skillService.GetSkill(skillId, auth0OrganizationId).Result;
            ;

            //Assert
            _belongsToValidatorMock.Verify(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()));
            response.Should().BeNull();
        }

        [Fact]
        public async Task GivenUniqueSkillWhenCreateSkillThenSkillCreated()
        {
            //Build Request
            SkillsDataHelper.ArrangeSkillsTestData(_context);
            var skill8 = new SkillModel {SkillId = 7, SkillName = "Telugu", Description = "Spoken Language"};

            var organizationId = 1L;
            var auth0OrganizationId = "auth0|Test";

            //Mock Service Response Setup
            _belongsToValidatorMock.Setup(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()))
                .ReturnsAsync(new Organization {OrganizationId = organizationId});

            //Call CreateSkill function
            var createResponse = _skillService.CreateSkill(skill8, auth0OrganizationId).Result;

            var getResponse = _skillService.GetSkill(createResponse.SkillId, auth0OrganizationId).Result;

            //Assert
            _belongsToValidatorMock.Verify(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()));
            getResponse.SkillId.Should().Be(createResponse.SkillId);
            getResponse.SkillName.Should().Be(createResponse.SkillName);
        }

        [Fact]
        public async Task GivenDuplicateSkillWhenCreateSkillThenException()
        {
            //Build Request
            SkillsDataHelper.ArrangeSkillsTestData(_context);
            var skill8 = new SkillModel {SkillId = 7, SkillName = "DotNet", Description = "Programming Language"};

            var organizationId = 1L;
            var auth0OrganizationId = "auth0|Test";

            //Mock Service Response Setup
            _belongsToValidatorMock.Setup(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()))
                .ReturnsAsync(new Organization {OrganizationId = organizationId});

            //Call CreateSkill function
            try
            {
                var createResponse = _skillService.CreateSkill(skill8, auth0OrganizationId).Result;
            }
            catch (Exception exception)
            {
                //Assert
                _belongsToValidatorMock.Verify(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()));
                exception.Should().BeOfType<EntityAlreadyExistsException>();
                exception.Message.Should().Be("The skill with the same name already exists");

            }
        }

        [Fact]
        public async Task GivenSkillExistsWhenRemoveSkillThenSkillRemoved()
        {
            //Build Request
            SkillsDataHelper.ArrangeSkillsTestData(_context);
            var skill8 = new SkillModel {SkillId = 7, SkillName = "Telugu", Description = "Spoken Language"};

            var organizationId = 1L;
            var auth0OrganizationId = "auth0|Test";

            //Mock Service Response Setup
            _belongsToValidatorMock.Setup(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()))
                .ReturnsAsync(new Organization {OrganizationId = organizationId});

            //Call CreateSkill function
            var createResponse = _skillService.CreateSkill(skill8, auth0OrganizationId).Result;

            var getResponse = _skillService.GetSkill(createResponse.SkillId, auth0OrganizationId).Result;
            //Assert
            _belongsToValidatorMock.Verify(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()));
            getResponse.SkillId.Should().Be(createResponse.SkillId);

            _skillService.RemoveSkill(createResponse.SkillId, auth0OrganizationId);

            var getResponse2 = _skillService.GetSkill(createResponse.SkillId, auth0OrganizationId).Result;
            getResponse2.Should().BeNull();
        }

        [Fact]
        public async Task GivenSkillDoesNotExistWhenRemoveSkillThenException()
        {
            //Build Request
            SkillsDataHelper.ArrangeSkillsTestData(_context);

            var organizationId = 1L;
            var auth0OrganizationId = "auth0|Test";

            //Mock Service Response Setup
            _belongsToValidatorMock.Setup(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()))
                .ReturnsAsync(new Organization {OrganizationId = organizationId});

            //Call RemoveSkill function
            try
            {
                _skillService.RemoveSkill(9, auth0OrganizationId);
            }
            catch (Exception exception)
            {
                exception.Should().BeOfType<EntityNotFoundException>();
                exception.Message.Should().Be("The skill does not exist");
            }
        }

        [Fact]
        public async Task GivenSkillExistsWhenUpdateSkillThenSkillUpdated()
        {
            //Build Request
            SkillsDataHelper.ArrangeSkillsTestData(_context);
            var skill1 = new SkillModel {SkillId = 1, SkillName = "C# DotNet", Description = "Programming Language"};

            var organizationId = 1L;
            var auth0OrganizationId = "auth0|Test";

            //Mock Service Response Setup
            _belongsToValidatorMock.Setup(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()))
                .ReturnsAsync(new Organization {OrganizationId = organizationId});

            //Call UpdateSkill function
            var updateResposne = _skillService.UpdateSkill(skill1, auth0OrganizationId).Result;

            var getResponse = _skillService.GetSkill(updateResposne.SkillId, auth0OrganizationId).Result;

            //Assert
            _belongsToValidatorMock.Verify(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()));
            getResponse.Description.Should().Be(updateResposne.Description);
            getResponse.SkillName.Should().Be(updateResposne.SkillName);
        }

        [Fact]
        public async Task GivenSkillDoesNotExistWhenUpdateSkillThenException()
        {
            //Build Request
            SkillsDataHelper.ArrangeSkillsTestData(_context);
            var skill1 = new SkillModel {SkillId = 8, SkillName = "C# DotNet", Description = "Programming Language"};

            var organizationId = 1L;
            var auth0OrganizationId = "auth0|Test";

            //Mock Service Response Setup
            _belongsToValidatorMock.Setup(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()))
                .ReturnsAsync(new Organization {OrganizationId = organizationId});
            try
            {
                //Call UpdateSkill function
                _skillService.UpdateSkill(skill1, auth0OrganizationId);
            }
            catch (Exception exception)
            {
                exception.Should().BeOfType<EntityNotFoundException>();
                exception.Message.Should().Be("The skill does not exist");
            }

        }
    }
}
