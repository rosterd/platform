using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Rosterd.Admin.Api.Controllers;
using Rosterd.Admin.Api.Requests.Jobs;
using Rosterd.Admin.Api.Services;
using Rosterd.Domain.Enums;
using Rosterd.Domain.Exceptions;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.FacilitiesModels;
using Rosterd.Domain.Models.JobModels;
using Rosterd.Domain.Settings;
using Rosterd.Infrastructure.Security.Interfaces;
using Rosterd.Services.Jobs.Interfaces;
using Rosterd.Services.Staff.Interfaces;
using Xunit;

namespace Rosterd.UnitTests.Rosterd.Admin.Api.Tests.Controllers
{
    public class JobsControllerTests
    {
        private readonly JobsController _jobsController;
        private readonly Mock<ILogger<JobsController>> _loggerMock;
        private readonly Mock<IJobsService> _jobServiceMock;
        private readonly Mock<IJobEventsService> _jobEventsServiceMock;
        private readonly Mock<IUserContext> _userContextMock;
        private readonly Mock<IBelongsToValidator> _belongsToValidatorMock;
        private readonly Mock<IStaffService> _staffServiceMock;
        private readonly Mock<IOptions<AppSettings>> _appSettingsMock;

        public JobsControllerTests()
        {
            _loggerMock = new Mock<ILogger<JobsController>>();
            _jobServiceMock = new Mock<IJobsService>();
            _jobEventsServiceMock = new Mock<IJobEventsService>();
            _userContextMock = new Mock<IUserContext>();
            _belongsToValidatorMock = new Mock<IBelongsToValidator>();
            _staffServiceMock = new Mock<IStaffService>();
            _appSettingsMock = new Mock<IOptions<AppSettings>>();
            _jobsController = new JobsController(_loggerMock.Object, _jobServiceMock.Object, _jobEventsServiceMock.Object, _appSettingsMock.Object, _userContextMock.Object, _belongsToValidatorMock.Object, _staffServiceMock.Object);
        }

        [Fact]
        public async Task GivenJobsExistsWhenGetAllJobsThenReturnsPageObject()
        {
            //Build Request
            var pagingQueryStringParameters = new PagingQueryStringParameters
            {
                PageNumber = 1,
                PageSize = 10
            };

            //Mock Service Response Setup
            _jobServiceMock.Setup(x => x.GetAllJobs(pagingQueryStringParameters, _userContextMock.Object.UserAuth0Id, It.IsAny<JobStatus>()))
                .ReturnsAsync(new PagedList<JobModel>(new List<JobModel>(), 100, 1, 10, 10));

            //Call GetAllJobs function
            var response = _jobsController.GetAllJobs(pagingQueryStringParameters, "Published").Result;;

            _jobServiceMock.Verify(x => x.GetAllJobs(pagingQueryStringParameters, _userContextMock.Object.UserAuth0Id, It.IsAny<JobStatus>()));

            //Assert
            response.Value.CurrentPage.Should().Be(1);
            response.Value.PageSize.Should().Be(10);
            response.Value.Items.Count.Should().Be(0);
        }

        [Fact]
        public async Task GivenJobsExistsWhenGetAllJobsWithNullStatusThenReturnsPageObject()
        {
            //Build Request
            var pagingQueryStringParameters = new PagingQueryStringParameters
            {
                PageNumber = 1,
                PageSize = 10
            };

            //Mock Service Response Setup
            _jobServiceMock.Setup(x => x.GetAllJobs(pagingQueryStringParameters, _userContextMock.Object.UserAuth0Id, null))
                .ReturnsAsync(new PagedList<JobModel>(new List<JobModel>(), 100, 1, 10, 10));

            //Call GetAllJobs function
            var response = _jobsController.GetAllJobs(pagingQueryStringParameters, null).Result;;

            _jobServiceMock.Verify(x => x.GetAllJobs(pagingQueryStringParameters, _userContextMock.Object.UserAuth0Id, null));

            //Assert
            response.Value.CurrentPage.Should().Be(1);
            response.Value.PageSize.Should().Be(10);
            response.Value.Items.Count.Should().Be(0);
        }

        [Fact]
        public async Task GivenJobExistWhenGetJobByIdThenReturnsJob()
        {
            //Build Request
            var jobId = 12L;

            //Mock Service Response Setup
            _jobServiceMock.Setup(x => x.GetJob(jobId, _userContextMock.Object.UserAuth0Id))
                .ReturnsAsync(new JobModel
                {
                    JobId = jobId,
                    JobStatus = JobStatus.Published
                });

            //Call GetJobById function
            var response = _jobsController.GetJobById(jobId).Result;;

            _jobServiceMock.Verify(x => x.GetJob(jobId, _userContextMock.Object.UserAuth0Id));

            //Assert
            response.Value.JobId.Should().Be(jobId);
            response.Value.JobStatus.Should().Be(JobStatus.Published);

        }

        [Fact]
        public async Task GivenValidAddJobRequestWhenAddNewJobThenJobCreated()
        {
            //Build Request
            var jobTitle = ".NET Tester Wanted";
            var facilityId = 1L;
            var addJobRequest = new AddJobRequest
            {
                JobTitle = jobTitle,
                FacilityId = facilityId
            };

            //Mock Service Response Setup
            _jobServiceMock.Setup(x => x.CreateJob(It.IsAny<JobModel>(), _userContextMock.Object.UserAuth0Id))
                .ReturnsAsync(new JobModel()
                {
                    JobId = 1,
                    JobTitle = jobTitle
                });

            //Call AddNewJob function
            var response = _jobsController.AddNewJob(addJobRequest).Result;

            _jobServiceMock.Verify(x => x.CreateJob(It.IsAny<JobModel>(), _userContextMock.Object.UserAuth0Id));

            //Assert
            response.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GivenInValidAddJobRequestWhenAddNewJobThenBadRequest()
        {
            //Build Request
            var jobTitle = ".NET Tester Wanted";
            var addJobRequest = new AddJobRequest
            {
                JobTitle = jobTitle
            };

            try
            {
                var response = _jobsController.AddNewJob(addJobRequest);
            }
            catch (Exception exception)
            {
                //Assert
                exception.Should().BeOfType<BadRequestException>();
                exception.Message.Should().Be("Facility Id needs to be specified");
            }
        }

        [Fact]
        public async Task GivenJobExistsWhenRemoveJobThenJobDeleted()
        {
            //Build Request
            var jobId = 123L;


            //Mock Service Response Setup
            _jobServiceMock.Setup(x => x.RemoveJob(It.IsAny<long>(), It.IsAny<string>(), _userContextMock.Object.UserAuth0Id));

            //Call RemoveJob function
            var response = _jobsController.RemoveJob(jobId, new DeleteJobRequest {JobCancellationReason = "Testing"}).Result;

            _jobServiceMock.Verify(x => x.RemoveJob(It.IsAny<long>(), It.IsAny<string>(), _userContextMock.Object.UserAuth0Id));

            //Assert
            response.Should().BeOfType<OkResult>();
        }
    }
}
