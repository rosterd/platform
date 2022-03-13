using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Rosterd.Client.Api;
using Rosterd.Client.Api.Controllers;
using Rosterd.Client.Api.Services;
using Rosterd.Domain;
using Rosterd.Domain.Enums;
using Rosterd.Services.Jobs.Interfaces;
using Xunit;
using Xunit.Sdk;

namespace Rosterd.UnitTests.Rosterd.Client.Api.Tests.Controllers
{
    public class JobsControllerTests
    {
        private readonly JobsController _jobsController;
        private readonly Mock<ILogger<JobsController>>_loggerMock;
        private readonly Mock<IJobsService> _jobServiceMock;
        private readonly Mock<IJobsValidationService> _jobsValidationServiceMock;
        private readonly Mock<IJobEventsService> _jobEventsServiceMock;
        private readonly Mock<IOptions<AppSettings>> _appSettingsMock;
        private readonly Mock<IUserContext> _userContextMock;

        public JobsControllerTests()
        {
            _loggerMock = new Mock<ILogger<JobsController>>();
            _jobServiceMock = new Mock<IJobsService>();
            _jobsValidationServiceMock = new Mock<IJobsValidationService>();
            _jobEventsServiceMock = new Mock<IJobEventsService>();
            _appSettingsMock = new Mock<IOptions<AppSettings>>();
            _userContextMock = new Mock<IUserContext>();
            _jobsController = new JobsController(_loggerMock.Object, _jobServiceMock.Object, _jobsValidationServiceMock.Object, _jobEventsServiceMock.Object,
                _appSettingsMock.Object, _userContextMock.Object);
        }

        [Fact]
        public async Task GivenJobIsValidToAcceptWhenAcceptAndConfirmJobThenAccepted()
        {
            //Build request and setup
            var jobId = 1L;
            _jobsValidationServiceMock.Setup(x => x.IsJobStillValidToAccept(It.IsAny<long>()))
                .ReturnsAsync((true, null));
            _jobServiceMock.Setup(x => x.AcceptJobForStaff(It.IsAny<long>(), It.IsAny<long>()))
                .ReturnsAsync(true);
            _jobEventsServiceMock.Setup(x => x.UpdateStatusOfJobInSearch(It.IsAny<long>(), JobStatus.Accepted));

            //Call api
            var response = _jobsController.AcceptAndConfirmJob(jobId).Result;

            //Assert
            response.Should().BeOfType<OkResult>();
        }

        [Theory]
        [InlineData("Job is already scheduled to start, can not be accepted")]
        [InlineData("This job has already been accepted")]
        public async Task GivenJobIsInvalidToAcceptWhenAcceptAndConfirmJobThenException(string errorMessage)
        {
            //Build request and setup
            var jobId = 1L;
            _jobsValidationServiceMock.Setup(x => x.IsJobStillValidToAccept(It.IsAny<long>()))
                .ReturnsAsync((false, new List<string>{errorMessage}));
            _jobServiceMock.Setup(x => x.AcceptJobForStaff(It.IsAny<long>(), It.IsAny<long>()))
                .ReturnsAsync(true);
            _jobEventsServiceMock.Setup(x => x.UpdateStatusOfJobInSearch(It.IsAny<long>(), JobStatus.Accepted));

            //Call api
            var response = _jobsController.AcceptAndConfirmJob(jobId).Result;

            //Assert
            response.Should().BeOfType<UnprocessableEntityObjectResult>()
                .Subject.Value.Should().BeEquivalentTo(new List<string>{errorMessage});
        }

        [Fact]
        public async Task GivenInvalidAcceptJobForStaffWhenAcceptAndConfirmJobThenException()
        {
            //Build request and setup
            var jobId = 1L;
            _jobsValidationServiceMock.Setup(x => x.IsJobStillValidToAccept(It.IsAny<long>()))
                .ReturnsAsync((true, null));
            _jobServiceMock.Setup(x => x.AcceptJobForStaff(It.IsAny<long>(), It.IsAny<long>()))
                .ReturnsAsync(false);
            _jobEventsServiceMock.Setup(x => x.UpdateStatusOfJobInSearch(It.IsAny<long>(), JobStatus.Accepted));

            //Call api
            var response = _jobsController.AcceptAndConfirmJob(jobId).Result;

            //Assert
            response.Should().BeOfType<UnprocessableEntityObjectResult>()
                .Subject.Value.Should().BeEquivalentTo(RosterdConstants.ErrorMessages.GenericError);
        }

        [Fact]
        public async Task GivenJobIsAcceptedWhenCancelledThenCancelled()
        {
            //Build request and setup
            var jobId = 1L;
            _jobsValidationServiceMock.Setup(x => x.IsJobStillValidToCancelForStaff(It.IsAny<long>(), It.IsAny<long>()))
                .ReturnsAsync((true, null));
            _jobServiceMock.Setup(x => x.CancelJobForStaff(It.IsAny<long>(), It.IsAny<long>()))
                .ReturnsAsync(true);
            _jobEventsServiceMock.Setup(x => x.UpdateStatusOfJobInSearch(It.IsAny<long>(), JobStatus.Accepted));

            //Call api
            var response = _jobsController.CancelJob(jobId).Result;

            //Assert
            response.Should().BeOfType<OkResult>();
        }

        [Theory]
        [InlineData("This job can not be cancelled once it has been accepted.")]
        [InlineData("You have past the grace time to cancel this job.")]
        public async Task GivenJobInvalidStateWhenCancelJobThenException(string errorMessage)
        {
            //Build request and setup
            var jobId = 1L;
            _jobsValidationServiceMock.Setup(x => x.IsJobStillValidToCancelForStaff(It.IsAny<long>(),It.IsAny<long>()))
                .ReturnsAsync((false, new List<string>{errorMessage}));
            _jobServiceMock.Setup(x => x.CancelJobForStaff(It.IsAny<long>(), It.IsAny<long>()))
                .ReturnsAsync(true);
            _jobEventsServiceMock.Setup(x => x.UpdateStatusOfJobInSearch(It.IsAny<long>(), JobStatus.Accepted));

            //Call api
            var response = _jobsController.CancelJob(jobId).Result;

            //Assert
            response.Should().BeOfType<UnprocessableEntityObjectResult>()
                .Subject.Value.Should().BeEquivalentTo(new List<string>{errorMessage});
        }

        [Fact]
        public async Task GivenInvalidJobForStaffWhenCancelJobThenException()
        {
            //Build request and setup
            var jobId = 1L;
            _jobsValidationServiceMock.Setup(x => x.IsJobStillValidToCancelForStaff(It.IsAny<long>(), It.IsAny<long>()))
                .ReturnsAsync((true, null));
            _jobServiceMock.Setup(x => x.CancelJobForStaff(It.IsAny<long>(), It.IsAny<long>()))
                .ReturnsAsync(false);
            _jobEventsServiceMock.Setup(x => x.UpdateStatusOfJobInSearch(It.IsAny<long>(), JobStatus.Accepted));

            //Call api
            var response = _jobsController.CancelJob(jobId).Result;

            //Assert
            response.Should().BeOfType<UnprocessableEntityObjectResult>()
                .Subject.Value.Should().BeEquivalentTo(RosterdConstants.ErrorMessages.GenericError);
        }
    }
}
