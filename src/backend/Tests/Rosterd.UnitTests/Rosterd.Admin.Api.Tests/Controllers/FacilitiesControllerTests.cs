using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Rosterd.Admin.Api.Controllers;
using Rosterd.Admin.Api.Requests.Facility;
using Rosterd.Admin.Api.Services;
using Rosterd.Domain.Exceptions;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.FacilitiesModels;
using Rosterd.Domain.Settings;
using Rosterd.Services.Facilities.Interfaces;
using Xunit;

namespace Rosterd.UnitTests.Rosterd.Admin.Api.Tests.Controllers
{
    public class FacilitiesControllerTests
    {
        private readonly Mock<IFacilitiesService>  _facilitiesServiceMock;
        private readonly Mock<IUserContext> _userContextMock;
        private readonly FacilitiesController _facilitiesController;
        private readonly Mock<IOptions<AppSettings>> _appSettingsMock;

        public FacilitiesControllerTests()
        {
            _facilitiesServiceMock = new Mock<IFacilitiesService>();
            _userContextMock = new Mock<IUserContext>();
            _appSettingsMock = new Mock<IOptions<AppSettings>>();
            _facilitiesController = new FacilitiesController(new Mock<ILogger<FacilitiesController>>().Object, _facilitiesServiceMock.Object, _appSettingsMock.Object, _userContextMock.Object);
        }

        [Fact]
        public async Task GivenFacilitiesExistsWhenGetAllFacilitiesThenReturnsPageObject()
        {
            //Build Request
            var pagingQueryStringParameters = new PagingQueryStringParameters
            {
                PageNumber = 1,
                PageSize = 10
            };

            //Mock Service Response Setup
            _facilitiesServiceMock.Setup(x => x.GetAllFacilities(pagingQueryStringParameters, _userContextMock.Object.UserAuth0Id))
                .ReturnsAsync(new PagedList<FacilityModel>(new List<FacilityModel>(), 100, 1, 10, 10));

            //Call GetAllFacilities function
            var response = _facilitiesController.GetAllFacilities(pagingQueryStringParameters).Result;;

            _facilitiesServiceMock.Verify(x => x.GetAllFacilities(pagingQueryStringParameters, _userContextMock.Object.UserAuth0Id));

            //Assert
            response.Value.CurrentPage.Should().Be(1);
            response.Value.PageSize.Should().Be(10);
            response.Value.Items.Count.Should().Be(0);
        }

        [Fact]
        public async Task GivenFacilityIdExistsWhenGetFacilityByIdThenReturnsFacility()
        {
            //Build Request
            var facilityId = 123L;

            //Mock Service Response Setup
            _facilitiesServiceMock.Setup(x => x.GetFacility(facilityId, _userContextMock.Object.UserAuth0Id))
                .ReturnsAsync(new FacilityModel
                {
                    FacilityId = facilityId
                });

            //Call GetFacilityById function
            var response = _facilitiesController.GetFacilityById(facilityId).Result;;

            _facilitiesServiceMock.Verify(x => x.GetFacility(facilityId, _userContextMock.Object.UserAuth0Id));

            //Assert
            response.Value.FacilityId.Should().Be(facilityId);
        }

        [Fact]
        public async Task GivenFacilityIdDoesNotExistsWhenGetFacilityByIdThenReturnsNotFound()
        {
            //Build Request
            var facilityId = 123L;

            //Mock Service Response Setup
            _facilitiesServiceMock.Setup(x => x.GetFacility(facilityId, _userContextMock.Object.UserAuth0Id))
                .ReturnsAsync(value: null as FacilityModel);

            //Call GetFacilityById function
            var response = _facilitiesController.GetFacilityById(facilityId).Result;

            _facilitiesServiceMock.Verify(x => x.GetFacility(facilityId, _userContextMock.Object.UserAuth0Id));

            //Assert
            response.Value.Should().BeNull();
        }


        [Fact]
        public async Task GivenValidAddFacilityRequestWhenAddNewFacilityThenFacilityCreated()
        {
            //Build Request\
            var facilityName = "The Sands";
            var addFacilityRequest = buildAddFacilityRequest(facilityName);

            //Mock Service Response Setup
            _facilitiesServiceMock.Setup(x => x.CreateFacility(It.IsAny<FacilityModel>(), _userContextMock.Object.UserAuth0Id))
                .ReturnsAsync(new FacilityModel()
                {
                    FacilityName = facilityName
                });

            //Call AddNewFacility function
            var response = _facilitiesController.AddNewFacility(addFacilityRequest).Result;

            _facilitiesServiceMock.Verify(x => x.CreateFacility(It.IsAny<FacilityModel>(), _userContextMock.Object.UserAuth0Id));

            //Assert
            response.Value.FacilityName.Should().Be(facilityName);
        }

        [Fact]
        public async Task GivenAddFacilityRequestWithDuplicateNameWhenAddNewFacilityThenBadRequest()
        {
            //Build Request
            var facilityName = "The Sands";
            var addFacilityRequest = buildAddFacilityRequest(facilityName);

            //Mock Service Response Setup
            _facilitiesServiceMock.Setup(x =>
                    x.DoesFacilityWithSameNameExistForOrganization(It.IsAny<FacilityModel>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            //Call AddNewFacility function
            try
            {
                var response = _facilitiesController.AddNewFacility(addFacilityRequest);
            }
            catch (Exception exception)
            {
                //Assert
                _facilitiesServiceMock.Verify(x =>
                    x.DoesFacilityWithSameNameExistForOrganization(It.IsAny<FacilityModel>(), _userContextMock.Object.UserAuth0Id, It.IsAny<string>()));
                exception.Should().BeOfType<BadRequestException>();
                exception.Message.Should().Be($"Facility with name {facilityName} already exits");
            }

        }

        [Fact]
        public async Task GivenValidUpdateFacilityRequestWhenUpdateFacilityThenFacilityUpdated()
        {
            //Build Request\
            var facilityName = "The Sands";
            var address = "9 Bay View Road, Brownsbay";
            var updateFacilityRequest = buildUpdateFacilityRequest(facilityName);

            //Mock Service Response Setup

            _facilitiesServiceMock.Setup(x => x.UpdateFacility(It.IsAny<FacilityModel>(), _userContextMock.Object.UsersAuth0OrganizationId))
                .ReturnsAsync(new FacilityModel()
                {
                    FacilityName = facilityName,
                    Address = address
                });

            _facilitiesServiceMock.Setup(x =>
                    x.DoesFacilityWithSameNameExistForOrganization(updateFacilityRequest.ToFacilityModel(), _userContextMock.Object.UsersAuth0OrganizationId,
                        facilityName))
                .ReturnsAsync(false);

            _facilitiesServiceMock.Setup(x => x.GetFacility(It.IsAny<long>(), _userContextMock.Object.UsersAuth0OrganizationId))
                .ReturnsAsync(new FacilityModel() {FacilityName = facilityName});

            //Call UpdateFacility function
            var response = _facilitiesController.UpdateFacility(updateFacilityRequest);

            _facilitiesServiceMock.Verify(x => x.GetFacility(It.IsAny<long>(), _userContextMock.Object.UsersAuth0OrganizationId));

            _facilitiesServiceMock.Verify(x =>  x.UpdateFacility(It.IsAny<FacilityModel>(), _userContextMock.Object.UsersAuth0OrganizationId));

            //Assert
            response.Result.Value.FacilityName.Should().Be(facilityName);
            response.Result.Value.Address.Should().Be(address);

        }


        [Fact]
        public async Task GivenUpdateFacilityDuplicateNameRequestWhenUpdateFacilityThenBadRequest()
        {
            //Build Request\
            var facilityName = "The Sands";
            var address = "9 Bay View Road, Brownsbay";
            var updateFacilityRequest = buildUpdateFacilityRequest(facilityName);

            //Mock Service Response Setup

            _facilitiesServiceMock.Setup(x => x.UpdateFacility(It.IsAny<FacilityModel>(), _userContextMock.Object.UsersAuth0OrganizationId))
                .ReturnsAsync(new FacilityModel()
                {
                    FacilityName = facilityName,
                    Address = address
                });

            _facilitiesServiceMock.Setup(x =>
                    x.DoesFacilityWithSameNameExistForOrganization(updateFacilityRequest.ToFacilityModel(), _userContextMock.Object.UsersAuth0OrganizationId,
                        facilityName))
                .ReturnsAsync(true);

            _facilitiesServiceMock.Setup(x => x.GetFacility(It.IsAny<long>(), _userContextMock.Object.UsersAuth0OrganizationId))
                .ReturnsAsync(new FacilityModel() {FacilityName = facilityName});

            //Call UpdateFacility function
            try
            {
                var response = _facilitiesController.UpdateFacility(updateFacilityRequest);
            }
            catch (Exception exception)
            {
                //Assert
                _facilitiesServiceMock.Verify(x => x.GetFacility(It.IsAny<long>(), _userContextMock.Object.UsersAuth0OrganizationId));

                _facilitiesServiceMock.Verify(x =>  x.UpdateFacility(It.IsAny<FacilityModel>(), _userContextMock.Object.UsersAuth0OrganizationId));
                exception.Should().BeOfType<BadRequestException>();
                exception.Message.Should().Be($"Facility with name {facilityName} already exits");
            }

        }


        [Fact]
        public async Task GivenFacilityDeactivatedWhenReactivateThenFacilityActivated()
        {
            //Build Request\
            var facilityId = 123L;

            //Mock Service Response Setup
            _facilitiesServiceMock.Setup(x => x.ReactivateFacility(It.IsAny<long>(), _userContextMock.Object.UserAuth0Id));

            //Call ReactivateFacility function
            var response = _facilitiesController.ReactivateFacility(facilityId).Result;

            _facilitiesServiceMock.Verify(x => x.ReactivateFacility(It.IsAny<long>(), _userContextMock.Object.UserAuth0Id));

            //Assert
            response.Should().BeOfType<OkResult>();
        }


        [Fact]
        public async Task GivenFacilityIdExistsWhenDeleteFacilityByIdThenDeleted()
        {
            //Build Request
            var facilityId = 123L;

            //Mock Service Response Setup
            _facilitiesServiceMock.Setup(x => x.RemoveFacility(facilityId, _userContextMock.Object.UserAuth0Id));

            //Call DeleteFacility function
            var response = _facilitiesController.RemoveFacility(facilityId).Result;;

            _facilitiesServiceMock.Verify(x => x.RemoveFacility(facilityId, _userContextMock.Object.UserAuth0Id));

            //Assert
            response.Should().BeOfType<OkResult>();
        }


        private AddFacilityRequest buildAddFacilityRequest(string facilityName)
        {
            var addFacilityRequest = new AddFacilityRequest
            {
                FacilityName = facilityName,
                Latitude = 10,
                Longitude = 10,
                City = "Auckland",
                Suburb = "Mt Eden",
                Address = "308 Shacks Road, Mt Eden",
                Country = "NZ",
                PhoneNumber1 = "0912345007",
                PhoneNumber2 = "090898490"
            };
            return addFacilityRequest;
        }

        private UpdateFacilityRequest buildUpdateFacilityRequest(string facilityName)
        {
            var updateFacilityRequest = new UpdateFacilityRequest
            {
                FacilityName = facilityName,
                Latitude = 10,
                Longitude = 10,
                City = "Auckland",
                Suburb = "Mt Eden",
                Address = "308 Shacks Road, Mt Eden",
                Country = "NZ",
                PhoneNumber1 = "0912345007",
                PhoneNumber2 = "090898490"
            };
            return updateFacilityRequest;
        }
    }
}
