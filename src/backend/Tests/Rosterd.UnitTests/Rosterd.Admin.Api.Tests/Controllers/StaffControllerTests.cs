using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Rosterd.Admin.Api.Controllers;
using Rosterd.Admin.Api.Services;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.SkillsModels;
using Rosterd.Domain.Models.StaffModels;
using Rosterd.Domain.Settings;
using Rosterd.Infrastructure.Security.Interfaces;
using Rosterd.Services.Staff.Interfaces;
using Xunit;

namespace Rosterd.UnitTests.Rosterd.Admin.Api.Tests.Controllers
{
    public class StaffControllerTests
    {
        private readonly StaffController _staffController;
        private readonly Mock<ILogger<StaffController>> _loggerMock;
        private readonly Mock<IStaffService> _staffServiceMock;
        private readonly Mock<IStaffSkillsService> _staffSkillsServiceMock;
        private readonly Mock<IStaffEventsService> _staffEventsServiceMock;
        private readonly Mock<IAuth0UserService> _auth0UserServiceMock;
        private readonly Mock<IUserContext> _userContextMock;
        private readonly Mock<IBelongsToValidator> _belongsToValidatorMock;
        private readonly Mock<IOptions<AppSettings>> _appSettingsMock;

        public StaffControllerTests()
        {
            _loggerMock = new Mock<ILogger<StaffController>>();
            _staffServiceMock = new Mock<IStaffService>();
            _staffSkillsServiceMock = new Mock<IStaffSkillsService>();
            _staffEventsServiceMock = new Mock<IStaffEventsService>();
            _auth0UserServiceMock = new Mock<IAuth0UserService>();
            _userContextMock = new Mock<IUserContext>();
            _belongsToValidatorMock = new Mock<IBelongsToValidator>();
            _appSettingsMock = new Mock<IOptions<AppSettings>>();
            _staffController = new StaffController(_loggerMock.Object, _staffServiceMock.Object, _staffSkillsServiceMock.Object, _staffEventsServiceMock.Object,
                _appSettingsMock.Object, _auth0UserServiceMock.Object, _userContextMock.Object, _belongsToValidatorMock.Object);

        }

        [Fact]
        public async Task GivenStaffExistWhenGetAllStaffThenReturnsPageObject()
        {
            //Build Request
            var pagingQueryStringParameters = new PagingQueryStringParameters
            {
                PageNumber = 1,
                PageSize = 10
            };

            //Mock Service Response Setup
            _staffServiceMock.Setup(x => x.GetAllStaff(pagingQueryStringParameters, _userContextMock.Object.UserAuth0Id))
                .ReturnsAsync(new PagedList<StaffModel>(new List<StaffModel>(), 100, 1, 10, 10));

            //Call GetAllStaff function
            var response = _staffController.GetAllStaff(pagingQueryStringParameters).Result;;

            _staffServiceMock.Verify(x => x.GetAllStaff(pagingQueryStringParameters, _userContextMock.Object.UserAuth0Id));

            //Assert
            response.Value.CurrentPage.Should().Be(1);
            response.Value.PageSize.Should().Be(10);
            response.Value.Items.Count.Should().Be(0);
        }

        [Fact]
        public async Task GivenStaffExistWhenGetStaffByIdThenReturnsStaff()
        {
            //Build Request
            var staffId = 12L;
            var firstname = "Tester";
            var lastName = "Unit";

            //Mock Service Response Setup
            _staffServiceMock.Setup(x => x.GetStaff(staffId, _userContextMock.Object.UserAuth0Id))
                .ReturnsAsync(new StaffModel
                {
                    StaffId = staffId,
                    FirstName = firstname,
                    LastName = lastName
                });
            //Call GetSkillById function
            var response = _staffController.GetStaffById(staffId).Result;;

            _staffServiceMock.Verify(x => x.GetStaff(staffId, _userContextMock.Object.UserAuth0Id));

            //Assert
            response.Value.StaffId.Should().Be(staffId);
            response.Value.FirstName.Should().Be(firstname);
        }
    }
}
