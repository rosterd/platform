using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Rosterd.Data.SqlServer.Context;
using Rosterd.Data.SqlServer.Models;
using Rosterd.Data.TableStorage.Context;
using Rosterd.Domain.Exceptions;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.FacilitiesModels;
using Rosterd.Domain.Models.SkillsModels;
using Rosterd.Domain.Models.StaffModels;
using Rosterd.Infrastructure.Search.Interfaces;
using Rosterd.Infrastructure.Security.Interfaces;
using Rosterd.Services.Staff;
using Rosterd.UnitTests.Rosterd.Admin.Api.Utilities;
using Xunit;

namespace Rosterd.UnitTests.Rosterd.Admin.Api.Tests.Services
{
    public class StaffServiceTests
    {
        private readonly StaffService _staffService;
        private readonly IRosterdDbContext _context;
        private readonly Mock<IBelongsToValidator> _belongsToValidatorMock;
        private readonly Mock<IAzureTableStorage> _azureTableStorageMock;
        private readonly Mock<ISearchIndexProvider> _searchIndexProviderMock;


        public StaffServiceTests()
        {
            _context = new RosterdDbContext(new DbContextOptionsBuilder<RosterdDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options);
            _belongsToValidatorMock = new Mock<IBelongsToValidator>();
            _azureTableStorageMock = new Mock<IAzureTableStorage>();
            _searchIndexProviderMock = new Mock<ISearchIndexProvider>();
            _staffService = new StaffService(_context, _azureTableStorageMock.Object,  _belongsToValidatorMock.Object, _searchIndexProviderMock.Object);
        }

        [Fact]
        public async Task GivenValidOrgIdWhenGetAllStaffThenReturnPagedList()
        {
            //Build Request
            StaffDataHelper.ArrangeStaffTestData(_context);
            var pagingQueryStringParameters = new PagingQueryStringParameters {PageNumber = 1, PageSize = 10};
            var organizationId = 1L;
            var auth0OrganizationId = "auth0|Test";

            //Mock Service Response Setup
            _belongsToValidatorMock.Setup(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()))
                .ReturnsAsync(new Organization {OrganizationId = organizationId});

            //Call GetAllStaff function
            var response = _staffService.GetAllStaff(pagingQueryStringParameters, auth0OrganizationId).Result;
            ;

            //Assert
            _belongsToValidatorMock.Verify(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()));
            response.PageSize.Should().Be(10);
            response.Items.Count.Should().Be(5);
        }

        [Fact]
        public async Task GivenStaffExistsWhenGetStaffThenStaffReturned()
        {
            //Build Request
            StaffDataHelper.ArrangeStaffTestData(_context);
            var staffId = 1L;
            var organizationId = 1L;
            var auth0OrganizationId = "auth0|Test";

            //Mock Service Response Setup
            _belongsToValidatorMock.Setup(x => x.ValidateStaffBelongsToOrganization(It.IsAny<long>(), It.IsAny<string>()));

            //Call GetStaff function
            var response = _staffService.GetStaff(1L, auth0OrganizationId).Result;
            ;

            //Assert
            _belongsToValidatorMock.Verify(x => x.ValidateStaffBelongsToOrganization(It.IsAny<long>(), It.IsAny<string>()));
            response.FirstName.Should().Be("One");
            response.LastName.Should().Be("Test");
        }

        [Fact]
        public async Task GivenStaffDoesNotExistWhenGetStaffThenException()
        {
            //Build Request
            StaffDataHelper.ArrangeStaffTestData(_context);
            var staffId = 8L;
            var organizationId = 1L;
            var auth0OrganizationId = "auth0|Test";

            //Mock Service Response Setup
            _belongsToValidatorMock.Setup(x => x.ValidateStaffBelongsToOrganization(It.IsAny<long>(), It.IsAny<string>()));

            try
            {
                //Call GetStaff function
                var response = _staffService.GetStaff(1L, auth0OrganizationId).Result;
            }
            catch (Exception exception)
            {
                //Assert
                _belongsToValidatorMock.Verify(x => x.ValidateStaffBelongsToOrganization(It.IsAny<long>(), It.IsAny<string>()));
                exception.Should().BeOfType<EntityNotFoundException>();
                exception.Message.Should().Be("The staff member does not exist");
            }
        }

        [Fact]
        public async Task GivenStaffWithAuthIdlWhenCreateStaffThenStaffCreated()
        {
            //Build Request
            StaffDataHelper.ArrangeStaffTestData(_context);
            var auth0OrganizationId = "auth0|Test";
            var staff7 = new StaffModel
            {
                StaffId = 7,
                FirstName = "Seven",
                LastName = "Test",
                JobTitle = "Tester",
                Email = "seven.test@gmail.com",
                Auth0Id = auth0OrganizationId,
                IsActive = false,
                StaffRole = "Admin",
                StaffSkills = new List<SkillModel> {
                    new SkillModel
                    {
                        SkillId = 1,
                        SkillName = "DotNet"
                    }
                },
                StaffFacilities = new List<FacilityModel>
                {
                    new FacilityModel
                    {
                        FacilityId = 1
                    }
                }
            };
            var organizationId = 1L;

            //Mock Service Response Setup
            _belongsToValidatorMock.Setup(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()))
                .ReturnsAsync(new Organization {OrganizationId = organizationId});

            //Call CreateStaff function
            var createResponse = _staffService.CreateStaff(staff7, auth0OrganizationId).Result;

            var getResponse = _staffService.GetStaff(7, auth0OrganizationId).Result;

            //Assert
            _belongsToValidatorMock.Verify(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()));
            getResponse.StaffId.Should().Be(7);
            getResponse.FirstName.Should().Be("Seven");
        }

        [Fact]
        public async Task GivenStaffWithoutAuthIdlWhenCreateStaffThenException()
        {
            //Build Request
            StaffDataHelper.ArrangeStaffTestData(_context);
            var staff7 = new StaffModel
            {
                StaffId = 7,
                FirstName = "Seven",
                LastName = "Test",
                JobTitle = "Tester",
                Email = "six.test@gmail.com",
                IsActive = false,
                StaffSkills = new List<SkillModel> {
                    new SkillModel
                    {
                        SkillId = 1,
                        SkillName = "DotNet"
                    }
                },
                StaffFacilities = new List<FacilityModel>
                {
                    new FacilityModel
                    {
                        FacilityId = 1
                    }
                }
            };
            var organizationId = 1L;
            var auth0OrganizationId = "auth0|Test";

            //Mock Service Response Setup
            _belongsToValidatorMock.Setup(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()))
                .ReturnsAsync(new Organization {OrganizationId = organizationId});
            try
            {
                //Call CreateStaff function
                var createResponse = _staffService.CreateStaff(staff7, auth0OrganizationId);
            }
            catch (Exception exception)
            {
                exception.Should().BeOfType<Auth0IdNotSetException>();
                exception.Message.Should().Be("Staff member is not created with the identity provider");
            }
        }

        [Fact]
        public async Task GivenStaffExistsWhenUpdateStaffThenStaffUpdated()
        {
            GivenStaffWithAuthIdlWhenCreateStaffThenStaffCreated();

            var auth0OrganizationId = "auth0|Test";
            var staff7 = new StaffModel
            {
                StaffId = 7,
                FirstName = "Seven Updated",
                LastName = "Test",
                JobTitle = "Tester",
                Email = "seven.test@gmail.com",
                Auth0Id = auth0OrganizationId,
                IsActive = false,
                StaffRole = "Admin",
                StaffSkills = new List<SkillModel> {
                    new SkillModel
                    {
                        SkillId = 1,
                        SkillName = "DotNet"
                    }
                },
                StaffFacilities = new List<FacilityModel>
                {
                    new FacilityModel
                    {
                        FacilityId = 1
                    }
                }
            };
            var organizationId = 1L;

            //Mock Service Response Setup
            _belongsToValidatorMock.Setup(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()))
                .ReturnsAsync(new Organization {OrganizationId = organizationId});

            //Call UpdateStaff function
            var updateResponse = _staffService.UpdateStaff(staff7, auth0OrganizationId).Result;

            //Assert
            _belongsToValidatorMock.Verify(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()));
            updateResponse.StaffId.Should().Be(7);
            updateResponse.FirstName.Should().Be("Seven Updated");
        }

        [Fact]
        public async Task GivenStaffDoesNotExistsWhenUpdateStaffThenException()
        {
            var auth0OrganizationId = "auth0|Test";
            var staff7 = new StaffModel
            {
                StaffId = 7,
                FirstName = "Seven Updated",
                LastName = "Test",
                JobTitle = "Tester",
                Email = "seven.test@gmail.com",
                Auth0Id = auth0OrganizationId,
                IsActive = false,
                StaffSkills = new List<SkillModel> {
                    new SkillModel
                    {
                        SkillId = 1,
                        SkillName = "DotNet"
                    }
                },
                StaffFacilities = new List<FacilityModel>
                {
                    new FacilityModel
                    {
                        FacilityId = 1
                    }
                }
            };
            var organizationId = 1L;

            //Mock Service Response Setup
            _belongsToValidatorMock.Setup(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()))
                .ReturnsAsync(new Organization {OrganizationId = organizationId});

            try
            {
                //Call UpdateStaff function
                var updateResponse = _staffService.UpdateStaff(staff7, auth0OrganizationId);
            }
            catch (Exception exception)
            {
                //Assert
                exception.Should().BeOfType<EntityNotFoundException>();
                exception.Message.Should().Be("The staff member does not exist");
            }
        }
    }
}
