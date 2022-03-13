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
using Rosterd.Domain.Enums;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.FacilitiesModels;
using Rosterd.Domain.Models.JobModels;
using Rosterd.Services.Jobs.Interfaces;
using Rosterd.UnitTests.Rosterd.Client.Api.Utilities;
using Xunit;

namespace Rosterd.UnitTests.Rosterd.Client.Api.Tests.Controllers
{
    public class MyJobsControllerTests
    {
        private readonly MyJobsController _myJobsController;
        private readonly Mock<ILogger<MyJobsController>>_loggerMock;
        private readonly Mock<IJobsService> _jobServiceMock;
        private readonly Mock<IOptions<AppSettings>> _appSettingsMock;
        private readonly Mock<IUserContext> _userContextMock;

        public MyJobsControllerTests()
        {
            _loggerMock = new Mock<ILogger<MyJobsController>>();
            _jobServiceMock = new Mock<IJobsService>();
            _appSettingsMock = new Mock<IOptions<AppSettings>>();
            _userContextMock = new Mock<IUserContext>();
            _myJobsController = new MyJobsController(_loggerMock.Object, _jobServiceMock.Object, _appSettingsMock.Object, _userContextMock.Object);
        }


        [Fact]
        public async Task GivenJobsExistsWhenGetAllCurrentJobsForUserThenReturnPagedList()
        {
            //Build request and setup
            _userContextMock.Setup(x => x.UserStaffId).Returns(1);
            _jobServiceMock.Setup(x => x.GetCurrentJobsForStaff(It.IsAny<long>(), It.IsAny<PagingQueryStringParameters>()))
                .ReturnsAsync(new PagedList<JobModel>(new List<JobModel>
                {
                    JobModelDataHelper.createJobModel(JobStatus.Accepted),
                    JobModelDataHelper.createJobModel(JobStatus.Accepted)
                }, 2, 1, 10, 1));

            //Call api
            var response = _myJobsController.GetAllCurrentJobsForUser(new PagingQueryStringParameters{PageNumber = 1, PageSize = 10} ).Result;

            //Assert
            response.Value.PageSize.Should().Be(10);
            response.Value.Items.Count.Should().Be(2);
        }

        [Fact]
        public async Task GivenJobsCompletedAndFeedbackPendingWhenGetHistoricJobsForUserThenReturnPagedList()
        {
            //Build request and setup
            _userContextMock.Setup(x => x.UserStaffId).Returns(1);
            _jobServiceMock.Setup(x => x.GetJobsForStaff(It.IsAny<long>(), It.IsAny<List<JobStatus>>(),It.IsAny<PagingQueryStringParameters>()))
                .ReturnsAsync(new PagedList<JobModel>(new List<JobModel>
                {
                    JobModelDataHelper.createJobModel(JobStatus.Completed),
                    JobModelDataHelper.createJobModel(JobStatus.Completed),
                    JobModelDataHelper.createJobModel(JobStatus.FeedbackPending)
                }, 3, 1, 10, 1));

            //Call api
            var response = _myJobsController.GetAllHistoricalCompletedJobsForUser(new PagingQueryStringParameters{PageNumber = 1, PageSize = 10} ).Result;

            //Assert
            response.Value.PageSize.Should().Be(10);
            response.Value.Items.Count.Should().Be(3);
        }

        [Fact]
        public async Task GivenJobsCancelledWhenGetHistoricJobsForUserThenReturnPagedList()
        {
            //Build request and setup
            _userContextMock.Setup(x => x.UserStaffId).Returns(1);
            _jobServiceMock.Setup(x => x.GetJobsForStaff(It.IsAny<long>(), It.IsAny<JobStatus>(),It.IsAny<PagingQueryStringParameters>()))
                .ReturnsAsync(new PagedList<JobModel>(new List<JobModel>
                {
                    JobModelDataHelper.createJobModel(JobStatus.Cancelled),
                    JobModelDataHelper.createJobModel(JobStatus.Cancelled),
                    JobModelDataHelper.createJobModel(JobStatus.Cancelled)
                }, 3, 1, 10, 1));

            //Call api
            var response = _myJobsController.GetAllHistoricalCancelledJobsForUser(new PagingQueryStringParameters{PageNumber = 1, PageSize = 10} ).Result;

            //Assert
            response.Value.PageSize.Should().Be(10);
            response.Value.Items.Count.Should().Be(3);
        }
    }
}
