using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Rosterd.Data.SqlServer.Context;
using Rosterd.Data.SqlServer.Models;
using Rosterd.Domain.Enums;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.FacilitiesModels;
using Rosterd.Domain.Models.JobModels;
using Rosterd.Infrastructure.Search.Interfaces;
using Rosterd.Infrastructure.Security.Interfaces;
using Rosterd.Services.Facilities;
using Rosterd.Services.Jobs;
using Rosterd.UnitTests.Rosterd.Admin.Api.Utilities;
using Xunit;

namespace Rosterd.UnitTests.Rosterd.Admin.Api.Tests.Services
{
    public class JobsServiceTests
    {
        private readonly JobsService _jobsService;
        private readonly IRosterdDbContext _context;
        private readonly Mock<ISearchIndexProvider> _searchIndexProviderMock;
        private readonly Mock<IBelongsToValidator> _belongsToValidatorMock;

        public JobsServiceTests()
        {
            _context = new RosterdDbContext(new DbContextOptionsBuilder<RosterdDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options);
            _searchIndexProviderMock = new Mock<ISearchIndexProvider>();
            _belongsToValidatorMock = new Mock<IBelongsToValidator>();
            _jobsService = new JobsService(_context, _searchIndexProviderMock.Object, _belongsToValidatorMock.Object);
        }

        [Fact]
        public async Task GivenValidIdWhenGetAllJobsThenReturnPagedList()
        {
            //Build Request
            JobsDataHelper.ArrangeJobsTestData(_context);
            var pagingQueryStringParameters = new PagingQueryStringParameters {PageNumber = 1, PageSize = 10};
            var organizationId = 1L;
            var auth0OrganizationId = "auth0|Test";

            //Mock Service Response Setup
            _belongsToValidatorMock.Setup(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()))
                .ReturnsAsync(new Organization {OrganizationId = organizationId});

            //Call GetAllJobs function
            var response = _jobsService.GetAllJobs(pagingQueryStringParameters, auth0OrganizationId).Result;
            ;

            //Assert
            _belongsToValidatorMock.Verify(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()));
            response.PageSize.Should().Be(10);
            response.Items.Count.Should().Be(1);
        }

        [Fact]
        public async Task GivenJobsWithStatusWhenGetAllJobsThenReturnJobsInStatus()
        {
            //Build Request
            JobsDataHelper.ArrangeJobsTestData(_context);
            var pagingQueryStringParameters = new PagingQueryStringParameters {PageNumber = 1, PageSize = 10};
            var organizationId = 1L;
            var auth0OrganizationId = "auth0|Test";

            //Mock Service Response Setup
            _belongsToValidatorMock.Setup(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()))
                .ReturnsAsync(new Organization {OrganizationId = organizationId});

            //Call GetAllJobs function
            var response = _jobsService.GetAllJobs(pagingQueryStringParameters, auth0OrganizationId, JobStatus.Published).Result;
            ;

            //Assert
            _belongsToValidatorMock.Verify(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()));
            response.PageSize.Should().Be(10);
            response.Items.Count.Should().Be(1);
            response.Items.ToArray()[0].JobStatus.Should().Be(JobStatus.Published);
        }

        [Fact]
        public async Task GivenJobExistsWhenGetJobThenReturnJob()
        {
            //Build Request
            JobsDataHelper.ArrangeJobsTestData(_context);
            var jobId = 1L;
            var organizationId = 1L;
            var auth0OrganizationId = "auth0|Test";

            //Mock Service Response Setup
            _belongsToValidatorMock.Setup(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()))
                .ReturnsAsync(new Organization {OrganizationId = organizationId});

            //Call GetAllJobs function
            var response = _jobsService.GetJob(jobId, auth0OrganizationId).Result;
            ;

            //Assert
            _belongsToValidatorMock.Verify(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()));
            response.JobId.Should().Be(1);
        }

        [Fact]
        public async Task GivenJobDoesNotExistsWhenGetJobThenReturnNull()
        {
            //Build Request
            JobsDataHelper.ArrangeJobsTestData(_context);
            var jobId = 8L;
            var organizationId = 1L;
            var auth0OrganizationId = "auth0|Test";

            //Mock Service Response Setup
            _belongsToValidatorMock.Setup(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()))
                .ReturnsAsync(new Organization {OrganizationId = organizationId});

        try
        {
            var getResponse = _jobsService.GetJob(8L, auth0OrganizationId).Result;
        }catch(Exception exception){
            //Assert
            _belongsToValidatorMock.Verify(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()));
            exception.Should().BeOfType<AggregateException>();
            }
        }

        [Fact]
        public async Task GivenJobWithRequiredFieldsWhenCreateJobThenJobCreated()
        {
            //Build Request
            var job = buildJobModel(DateTime.Now.AddDays(2), DateTime.Now.AddDays(2).AddHours(8));
            var organizationId = 1L;
            var auth0OrganizationId= "auth0|Test";

            //Mock Service Response Setup
            _belongsToValidatorMock.Setup(x => x.ValidateFacilityBelongsToOrganization(It.IsAny<long>(), It.IsAny<string>()))
                .ReturnsAsync(new Organization { OrganizationId = organizationId});
            _belongsToValidatorMock.Setup(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()))
                .ReturnsAsync(new Organization { OrganizationId = organizationId});

            //Call CreateJob function
            var createResponse = _jobsService.CreateJob(job, auth0OrganizationId).Result;;

            var getResponse = _jobsService.GetJob(createResponse.JobId, auth0OrganizationId).Result;

            //Assert
            _belongsToValidatorMock.Verify(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()));

            getResponse.JobId.Should().Be(createResponse.JobId);
            getResponse.JobStatus.Should().Be(JobStatus.Published);
        }

         [Fact]
        public async Task GivenJobExistsWhenRemoveJobThenJobRemoved()
        {
            //Build Request
            var organizationId = 1L;
            var auth0OrganizationId= "auth0|Test";
            GivenJobWithRequiredFieldsWhenCreateJobThenJobCreated();

            //Call RemoveJob function
            _jobsService.RemoveJob(8L, "Cant make it", auth0OrganizationId);;

            try
            {
                var getResponse = _jobsService.GetJob(8L, auth0OrganizationId).Result;
            }catch(Exception exception){
                //Assert
            _belongsToValidatorMock.Verify(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()));
            exception.Should().BeOfType<AggregateException>();
            }
        }

        [Fact]
        public async Task GivenPublishedJobPastEndTimeWhenChronJobThenMovedToExpiry()
        {
            //Build Request
            var job = buildJobModel(DateTime.UtcNow.AddHours(-12), DateTime.UtcNow.AddHours(-4));

            var organizationId = 1L;
            var auth0OrganizationId= "auth0|Test";

            //Mock Service Response Setup
            _belongsToValidatorMock.Setup(x => x.ValidateFacilityBelongsToOrganization(It.IsAny<long>(), It.IsAny<string>()))
                .ReturnsAsync(new Organization { OrganizationId = organizationId});
            _belongsToValidatorMock.Setup(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()))
                .ReturnsAsync(new Organization { OrganizationId = organizationId});

            //Call CreateJob function
            var createResponse = _jobsService.CreateJob(job, auth0OrganizationId).Result;

            await _jobsService.MoveAllPublishedStatusJobsPastTimeLimitToExpiredState();

            var getResponse = _jobsService.GetJob(createResponse.JobId, auth0OrganizationId).Result;

            //Assert
            _belongsToValidatorMock.Verify(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()));
            getResponse.JobStatus.Should().Be(JobStatus.Expired);

        }

         [Fact]
        public async Task GivenAcceptedJobPastStartTimeWhenChronJobThenMovedToInProgress()
        {
            //Build Request
            StaffDataHelper.ArrangeStaffTestData(_context);

            var job = buildJobModel(DateTime.UtcNow.AddMinutes(-5), DateTime.UtcNow.AddHours(7).AddMinutes(55));

            var organizationId = 1L;
            var auth0OrganizationId= "auth0|Test";

            //Mock Service Response Setup
            _belongsToValidatorMock.Setup(x => x.ValidateFacilityBelongsToOrganization(It.IsAny<long>(), It.IsAny<string>()))
                .ReturnsAsync(new Organization { OrganizationId = organizationId});
            _belongsToValidatorMock.Setup(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()))
                .ReturnsAsync(new Organization { OrganizationId = organizationId});

            //Call CreateJob function
            var createResponse = _jobsService.CreateJob(job, auth0OrganizationId).Result;

            await _jobsService.AcceptJobForStaff(createResponse.JobId, 1);

            await _jobsService.MoveAllAcceptedStatusJobsPastStartTimeBeforeEndTimeToInProgressState();

            var getResponse = _jobsService.GetJob(createResponse.JobId, auth0OrganizationId).Result;

            //Assert
            _belongsToValidatorMock.Verify(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()));
            getResponse.JobStatus.Should().Be(JobStatus.InProgress);

        }

        [Fact]
        public async Task GivenInProgressJobPastEndTimeWhenChronJobThenMovedCompleted()
        {
            //Build Request
            StaffDataHelper.ArrangeStaffTestData(_context);

            var job = buildJobModel(DateTime.UtcNow.AddHours(-7).AddMinutes(-59), DateTime.UtcNow.AddMilliseconds(1000));

            var organizationId = 1L;
            var auth0OrganizationId= "auth0|Test";

            //Mock Service Response Setup
            _belongsToValidatorMock.Setup(x => x.ValidateFacilityBelongsToOrganization(It.IsAny<long>(), It.IsAny<string>()))
                .ReturnsAsync(new Organization { OrganizationId = organizationId});
            _belongsToValidatorMock.Setup(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()))
                .ReturnsAsync(new Organization { OrganizationId = organizationId});

            //Call CreateJob function
            var createResponse = _jobsService.CreateJob(job, auth0OrganizationId).Result;

            await _jobsService.AcceptJobForStaff(createResponse.JobId, 1);

            await _jobsService.MoveAllAcceptedStatusJobsPastStartTimeBeforeEndTimeToInProgressState();

            Thread.Sleep(1000);

            await _jobsService.MoveInProgressJobsPastEndDateToCompletedState();

            var getResponse = _jobsService.GetJob(createResponse.JobId, auth0OrganizationId).Result;

            //Assert
            _belongsToValidatorMock.Verify(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()));
            getResponse.JobStatus.Should().Be(JobStatus.Completed);

        }

        // [Fact]
        // public async Task GivenCompletedJobWhenChronJobThenMovedToFeebackPending()
        // {
        //     //Build Request
        //     StaffDataHelper.ArrangeStaffTestData(_context);
        //
        //     var job = buildJobModel(DateTime.UtcNow.AddHours(-7).AddMinutes(-59), DateTime.UtcNow.AddSeconds(1));
        //
        //     var organizationId = 1L;
        //     var auth0OrganizationId= "auth0|Test";
        //
        //     //Mock Service Response Setup
        //     _belongsToValidatorMock.Setup(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()))
        //         .ReturnsAsync(new Organization { OrganizationId = organizationId});
        //
        //     //Call CreateJob function
        //     var createResponse = _jobsService.CreateJob(job, auth0OrganizationId).Result;
        //
        //     await _jobsService.AcceptJobForStaff(createResponse.JobId, 1);
        //
        //     await _jobsService.MoveAllAcceptedStatusJobsPastStartTimeBeforeEndTimeToInProgressState();
        //
        //     Thread.Sleep(1000);
        //
        //     await _jobsService.MoveAllJobsThatArePastEndDateToFeedbackStatus();
        //
        //     var getResponse = _jobsService.GetJob(createResponse.JobId, auth0OrganizationId).Result;
        //
        //     //Assert
        //     _belongsToValidatorMock.Verify(x => x.ValidateOrganizationExistsAndGetIfValid(It.IsAny<string>()));
        //     getResponse.JobStatus.Should().Be(JobStatus.FeedbackPending);
        //
        // }




        private JobModel buildJobModel(DateTime startTime, DateTime endTime)
        {
            JobsDataHelper.ArrangeJobsTestData(_context);
            return new JobModel
            {
                JobTitle = "Level 1 HCA",
                Description = "Level 1 HCA to cover a shift of 8 hours",
                JobStartDateTimeUtc = startTime,
                JobEndDateTimeUtc = endTime,
                Facility = new FacilityModel {FacilityId = 1},
                Comments = "Unit test job",
                GracePeriodToCancelMinutes = 60,
                Responsibilities = "Showering, Lifting",
                Experience = "1+ year",
                IsNightShift = false,
                JobSkills = new List<JobSkillModel> {new JobSkillModel {SkillId = 1, SkillName = "Tester"}}
            };
        }


    }
}
