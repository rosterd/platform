using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Rosterd.Data.SqlServer.Context;
using Rosterd.Data.SqlServer.Models;
using Rosterd.Domain.Exceptions;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.FacilitiesModels;
using Rosterd.Infrastructure.Security.Interfaces;
using Rosterd.Services.Facilities;
using Rosterd.UnitTests.Rosterd.Admin.Api.Utilities;
using Xunit;

namespace Rosterd.UnitTests.Rosterd.Admin.Api.Tests.Services
{
    public class FacilitiesServiceTests
    {
        private readonly FacilitiesService _facilitiesService;
        private readonly IRosterdDbContext _context;
        private readonly Mock<IBelongsToValidator> _belongsToValidatorMock;

        public FacilitiesServiceTests()
        {
            _context = new RosterdDbContext(new DbContextOptionsBuilder<RosterdDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options);
            _belongsToValidatorMock = new Mock<IBelongsToValidator>();
            _facilitiesService = new FacilitiesService(_context, _belongsToValidatorMock.Object);
        }

        [Fact]
        public async Task GivenValidOrIdWhenGetAllFacilitiesThenReturnPagedList()
        {
            //Build Request
            FacilitiesDataHelper.ArrangeFacilitiesTestData(_context);
            var pagingQueryStringParameters = new PagingQueryStringParameters
            {
                PageNumber = 1,
                PageSize = 10
            };
            var organizationId = 1L;
            var auth0OrganizationId= "auth0|Test";

            //Mock Service Response Setup
            _belongsToValidatorMock.Setup(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()))
                .ReturnsAsync(new Organization { OrganizationId = organizationId});

            //Call GetAllFacilities function
            var response = _facilitiesService.GetAllFacilities(pagingQueryStringParameters, auth0OrganizationId).Result;;

            //Assert
            _belongsToValidatorMock.Verify(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()));
            response.PageSize.Should().Be(10);
            response.Items.Count.Should().Be(5);
        }

        [Fact]
        public async Task GivenInValidOrgIdWhenGetAllFacilitiesThenReturnEmptyItemsPagedList()
        {
            //Build Request
            FacilitiesDataHelper.ArrangeFacilitiesTestData(_context);
            var pagingQueryStringParameters = new PagingQueryStringParameters
            {
                PageNumber = 1,
                PageSize = 10
            };
            var organizationId = 2L;
            var auth0OrganizationId= "auth0|Test";

            //Mock Service Response Setup
            _belongsToValidatorMock.Setup(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()))
                .ReturnsAsync(new Organization { OrganizationId = organizationId});

            //Call GetAllFacilities function
            var response = _facilitiesService.GetAllFacilities(pagingQueryStringParameters, auth0OrganizationId).Result;;

            //Assert
            _belongsToValidatorMock.Verify(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()));
            response.PageSize.Should().Be(10);
            response.Items.Count.Should().Be(0);
        }

        [Fact]
        public async Task GivenFacilityExistsWhenGetFacilityByIdThenReturnFacility()
        {
            //Build Request
            FacilitiesDataHelper.ArrangeFacilitiesTestData(_context);
            var facilityId = 1L;
            var organizationId = 1L;
            var auth0OrganizationId= "auth0|Test";

            //Mock Service Response Setup
            _belongsToValidatorMock.Setup(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()))
                .ReturnsAsync(new Organization { OrganizationId = organizationId});

            //Call GetFacility function
            var response = _facilitiesService.GetFacility(facilityId, auth0OrganizationId).Result;;

            //Assert
            _belongsToValidatorMock.Verify(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()));
            response.FacilityId.Should().Be(1);
            response.FacilityName.Should().Be("Everyl Orr");
            response.IsActive.Should().Be(true);
        }

        [Fact]
        public async Task GivenInActiveFacilityExistsWhenGetFacilityByIdThenReturnFacility()
        {
            //Build Request
            FacilitiesDataHelper.ArrangeFacilitiesTestData(_context);
            var facilityId = 6L;
            var organizationId = 1L;
            var auth0OrganizationId= "auth0|Test";

            //Mock Service Response Setup
            _belongsToValidatorMock.Setup(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()))
                .ReturnsAsync(new Organization { OrganizationId = organizationId});

            //Call GetFacility function
            var response = _facilitiesService.GetFacility(facilityId, auth0OrganizationId).Result;;

            //Assert
            _belongsToValidatorMock.Verify(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()));
            response.FacilityId.Should().Be(6);
            response.FacilityName.Should().Be("Everyl Orr Old");
            response.IsActive.Should().Be(false);
        }

        [Fact]
        public async Task GivenFacilityDoesNotExistsWhenGetFacilityByIdThenReturnNull()
        {
            //Build Request
            FacilitiesDataHelper.ArrangeFacilitiesTestData(_context);
            var facilityId = 7L;
            var organizationId = 1L;
            var auth0OrganizationId= "auth0|Test";

            //Mock Service Response Setup
            _belongsToValidatorMock.Setup(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()))
                .ReturnsAsync(new Organization { OrganizationId = organizationId});

            //Call GetFacility function
            var response = _facilitiesService.GetFacility(facilityId, auth0OrganizationId).Result;;

            //Assert
            _belongsToValidatorMock.Verify(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()));
            response.Should().BeNull();
        }

        [Fact]
        public async Task GivenFacilityWithRequiredFieldsWhenCreateFacilityThenFacilityCreated()
        {
            //Build Request
            FacilitiesDataHelper.ArrangeFacilitiesTestData(_context);

            var facility7 = new FacilityModel {
                FacilityName = "Everyl Orr Test",
                Address = "63 Allendale Rd",
                City = "Auckland",
                PhoneNumber1 = "09-1234567"
            };
            var organizationId = 1L;
            var auth0OrganizationId= "auth0|Test";

            //Mock Service Response Setup
            _belongsToValidatorMock.Setup(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()))
                .ReturnsAsync(new Organization { OrganizationId = organizationId});

            //Call CreateFacility function
            var createResponse = _facilitiesService.CreateFacility(facility7, auth0OrganizationId).Result;;

            var getResponse = _facilitiesService.GetFacility(createResponse.FacilityId, auth0OrganizationId).Result;

            //Assert
            _belongsToValidatorMock.Verify(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()));

            createResponse.City.Should().Be("Auckland");
            getResponse.FacilityId.Should().Be(createResponse.FacilityId);
        }

        [Fact]
        public async Task GivenFacilityExistsWhenRemoveFacilityThenFacilityRemoved()
        {
            //Build Request
            FacilitiesDataHelper.ArrangeFacilitiesTestData(_context);

            var facility7 = new FacilityModel {
                FacilityName = "Everyl Orr Test",
                Address = "63 Allendale Rd",
                City = "Auckland",
                PhoneNumber1 = "09-1234567"
            };
            var organizationId = 1L;
            var auth0OrganizationId= "auth0|Test";

            //Mock Service Response Setup
            _belongsToValidatorMock.Setup(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()))
                .ReturnsAsync(new Organization { OrganizationId = organizationId});

            //Call CreateFacility function
            var createResponse = _facilitiesService.CreateFacility(facility7, auth0OrganizationId).Result;;

            _facilitiesService.RemoveFacility(createResponse.FacilityId, auth0OrganizationId);

            var getResponse = _facilitiesService.GetFacility(createResponse.FacilityId, auth0OrganizationId).Result;;

            //Assert
            _belongsToValidatorMock.Verify(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()));

            createResponse.City.Should().Be("Auckland");
            getResponse.FacilityId.Should().Be(createResponse.FacilityId);
            getResponse.IsActive.Should().Be(false);
        }

        [Fact]
        public async Task GivenFacilityDoesNotExistsWhenRemoveFacilityThenFacilityRemoved()
        {
            //Build Request
            FacilitiesDataHelper.ArrangeFacilitiesTestData(_context);

            var organizationId = 1L;
            var auth0OrganizationId= "auth0|Test";

            //Mock Service Response Setup
            _belongsToValidatorMock.Setup(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()))
                .ReturnsAsync(new Organization { OrganizationId = organizationId});

            //Call RemoveFacility function
            _facilitiesService.RemoveFacility(7L, auth0OrganizationId);

            var getResponse = _facilitiesService.GetFacility(7L, auth0OrganizationId).Result;;

            //Assert
            _belongsToValidatorMock.Verify(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()));

            getResponse.Should().Be(null);
        }

        [Fact]
        public async Task GivenFacilityIsNotActiveWhenReactivateFacilityThenFacilityIsActive()
        {
            //Build Request
            FacilitiesDataHelper.ArrangeFacilitiesTestData(_context);

            var organizationId = 1L;
            var auth0OrganizationId= "auth0|Test";

            //Mock Service Response Setup
            _belongsToValidatorMock.Setup(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()))
                .ReturnsAsync(new Organization { OrganizationId = organizationId});

            var getInactiveResponse = _facilitiesService.GetFacility(6L, auth0OrganizationId).Result;

            getInactiveResponse.IsActive.Should().Be(false);

            //Call ReactivateFacility function
            _facilitiesService.ReactivateFacility(6L, auth0OrganizationId);

            var getResponse = _facilitiesService.GetFacility(6L, auth0OrganizationId).Result;

            //Assert
            _belongsToValidatorMock.Verify(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()));

            getResponse.IsActive.Should().Be(true);
        }

        [Fact]
        public async Task GivenFacilityExistsWhenUpdateFacilityThenFacilityIsUpdated()
        {
            //Build Request
            FacilitiesDataHelper.ArrangeFacilitiesTestData(_context);

            var facilityModel = new FacilityModel
            {
                FacilityId = 1,
                FacilityName = "Everyl Orr New",
                Address = "63 Allendale Rd",
                Suburb = "Mt Albert",
                City = "Auckland",
                PhoneNumber1 = "09-1234567",
                IsActive = true
            };

            var organizationId = 1L;
            var auth0OrganizationId= "auth0|Test";

            //Mock Service Response Setup
            _belongsToValidatorMock.Setup(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()))
                .ReturnsAsync(new Organization { OrganizationId = organizationId});

            //Call UpdateFacility function
            var updateFacilityResponse = _facilitiesService.UpdateFacility(facilityModel, auth0OrganizationId).Result;

            var getResponse = _facilitiesService.GetFacility(1L, auth0OrganizationId).Result;
            //Assert
            _belongsToValidatorMock.Verify(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()));

            getResponse.FacilityName.Should().Be("Everyl Orr New");
        }

        [Fact]
        public async Task GivenFacilityDoesNotExistsWhenUpdateFacilityThenException()
        {
            //Build Request
            FacilitiesDataHelper.ArrangeFacilitiesTestData(_context);
            var facilityId = 7L;

            var facilityModel = new FacilityModel
            {
                FacilityId = facilityId,
                FacilityName = "Everyl Orr New",
                Address = "63 Allendale Rd",
                Suburb = "Mt Albert",
                City = "Auckland",
                PhoneNumber1 = "09-1234567",
                IsActive = true
            };

            var organizationId = 1L;
            var auth0OrganizationId= "auth0|Test";

            //Mock Service Response Setup
            _belongsToValidatorMock.Setup(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()))
                .ReturnsAsync(new Organization { OrganizationId = organizationId});

            //Call UpdateFacility function
            try
            {
                _facilitiesService.UpdateFacility(facilityModel, auth0OrganizationId);
            }
            catch (Exception exception)
            {
                _belongsToValidatorMock.Verify(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()));

                exception.Should().BeOfType<EntityNotFoundException>();
                exception.Message.Should().Be($"No existing facility with id {facilityId} was not found");
            }
        }

        [Fact]
        public async Task GivenDuplicateFacilityNameWhenEvaluatedThenReturnTrue()
        {
            //Build Request
            FacilitiesDataHelper.ArrangeFacilitiesTestData(_context);

            var facilityId = 7L;

            var facilityModel = new FacilityModel
            {
                FacilityId = facilityId,
                FacilityName = "Everyl Orr",
                Address = "63 Allendale Rd",
                Suburb = "Mt Albert",
                City = "Auckland",
                PhoneNumber1 = "09-1234567",
                IsActive = true
            };

            var organizationId = 1L;
            var auth0OrganizationId= "auth0|Test";

            //Mock Service Response Setup
            _belongsToValidatorMock.Setup(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()))
                .ReturnsAsync(new Organization { OrganizationId = organizationId});

            //Call DoesFacilityWithSameNameExistForOrganization function
            var facilityExists = _facilitiesService.DoesFacilityWithSameNameExistForOrganization(facilityModel, auth0OrganizationId).Result;

            //Assert
            facilityExists.Should().Be(true);
        }

        [Fact]
        public async Task GivenFacilityNameUniqueWhenEvaluatedThenReturnFalse()
        {
            //Build Request
            FacilitiesDataHelper.ArrangeFacilitiesTestData(_context);

            var facilityId = 7L;

            var facilityModel = new FacilityModel
            {
                FacilityId = facilityId,
                FacilityName = "Everyl Orr New",
                Address = "63 Allendale Rd",
                Suburb = "Mt Albert",
                City = "Auckland",
                PhoneNumber1 = "09-1234567",
                IsActive = true
            };

            var organizationId = 1L;
            var auth0OrganizationId= "auth0|Test";

            //Mock Service Response Setup
            _belongsToValidatorMock.Setup(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()))
                .ReturnsAsync(new Organization { OrganizationId = organizationId});

            //Call DoesFacilityWithSameNameExistForOrganization function
            var facilityExists = _facilitiesService.DoesFacilityWithSameNameExistForOrganization(facilityModel, auth0OrganizationId).Result;

            //Assert
            facilityExists.Should().Be(false);
        }

    }
}
