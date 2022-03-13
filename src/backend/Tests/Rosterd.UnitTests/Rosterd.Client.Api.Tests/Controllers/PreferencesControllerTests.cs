using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Rosterd.Client.Api;
using Rosterd.Client.Api.Controllers;
using Rosterd.Client.Api.Services;
using Rosterd.Domain.Models.StaffModels;
using Rosterd.Services.Jobs.Interfaces;
using Rosterd.Services.Mappers;
using Rosterd.Services.Staff.Interfaces;
using Rosterd.UnitTests.Rosterd.Client.Api.Utilities;
using Xunit;

namespace Rosterd.UnitTests.Rosterd.Client.Api.Tests.Controllers
{
    public class PreferencesControllerTests
    {
        private readonly PreferencesController _preferencesController;
        private readonly Mock<ILogger<JobsController>>_loggerMock;
        private readonly Mock<IRosterdAppUserService> _appUserServiceMock;
        private readonly Mock<IOptions<AppSettings>> _appSettingsMock;
        private readonly Mock<IUserContext> _userContextMock;

        public PreferencesControllerTests()
        {
            _loggerMock = new Mock<ILogger<JobsController>>();
            _appUserServiceMock = new Mock<IRosterdAppUserService>();
            _appSettingsMock = new Mock<IOptions<AppSettings>>();
            _userContextMock = new Mock<IUserContext>();
            _preferencesController = new PreferencesController(_loggerMock.Object, _appUserServiceMock.Object, _appSettingsMock.Object, _userContextMock.Object);
        }

        [Fact]
        public async Task GivenStaffExistsWhenGetUserPreferencesThenReturnPreferences()
        {
            //build request
            _appUserServiceMock.Setup(x => x.GetStaffAppUserPreferences(It.IsAny<string>()))
                .ReturnsAsync(PreferencesModelDataHelper.createStaffAppUserPreferencesModel);

            //call api
            var response = _preferencesController.GetUserPreferences().Result;
            //assert
            response.Value.LastName.Should().Be("Test");
        }

        [Fact]
        public async Task GivenStaffExistsWhenUpdateUserPreferencesThenUpdated()
        {
            //build request
            _appUserServiceMock.Setup(x => x.UpdateStaffAppUserPreferences(It.IsAny<StaffAppUserPreferencesModel>(), It.IsAny<string>(), It.IsAny<long>()));

            //call api
            var response = _preferencesController.UpdateUserPreferences(PreferencesModelDataHelper.createStaffAppUserPreferencesModel()).Result;

            //assert
            response.Result.Should().BeOfType<OkResult>();
        }

    }
}
