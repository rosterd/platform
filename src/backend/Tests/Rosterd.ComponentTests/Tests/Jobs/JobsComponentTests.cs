using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Rosterd.ComponentTests.Fixture;
using Rosterd.ComponentTests.Helpers;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.JobModels;
using Rosterd.Domain.Requests.Job;
using Xunit;
using Xunit.Abstractions;

namespace Rosterd.ComponentTests.Tests.Jobs
{
    public class JobsComponentTests
    {
        private readonly ApplicationFixture _appFixture = new ApplicationFixture();


        [Fact]
        public async Task GivenJobssWhenGetAllJobsThenPagedListWithNumberOfJobsRequested()
        {
            // ENDPOINT URL
            var url = ApiConstants.JOBS_ENDPOINT + "?PageNumber=1&PageSize=1";

            // GET JOBS
            var response = await _appFixture.HttpClient.GetAsync(url);

            // ASSERT
            response.EnsureSuccessStatusCode();

            var responsePagedList = JsonConvert.DeserializeObject<PagedList<JobModel>>(response.Content.ReadAsStringAsync().Result);

            responsePagedList.PageSize.Should().Be(1);
            responsePagedList.Items.Count.Should().Be(1);
        }

        [Fact]
        public async Task GivenValidJobWhenPostedThenJobIsAdded()
        {
            var jobId = createJobAsync().Result;
            // ENDPOINT URL
            var url = ApiConstants.JOBS_ENDPOINT + "/" + jobId;

            // GET JOBS
            var response = await _appFixture.HttpClient.GetAsync(url);

            // ASSERT
            response.EnsureSuccessStatusCode();

            var jobModel = JsonConvert.DeserializeObject<JobModel>(response.Content.ReadAsStringAsync().Result);

            await deleteJobAsync(jobId);
        }


        [Fact]
        public async Task GivenJobWhenGetJobByIdThenJobIsReturned()
        {
            var jobId = createJobAsync().Result;
            // ENDPOINT URL
            var url = ApiConstants.JOBS_ENDPOINT + "/" + jobId;

            // GET JOBS
            var response = await _appFixture.HttpClient.GetAsync(url);

            // ASSERT
            response.EnsureSuccessStatusCode();

            var job = JsonConvert.DeserializeObject<JobModel>(response.Content.ReadAsStringAsync().Result);

            await deleteJobAsync(jobId);
        }


        [Fact]
        public async Task GivenJobWhenDeleteJobThenJobIsDeleted()
        {
            var jobId = createJobAsync().Result;
            // ENDPOINT URL
            var url = ApiConstants.JOBS_ENDPOINT + "/" + jobId;

            // GET JOBS
            var response = await _appFixture.HttpClient.GetAsync(url);

            // ASSERT
            response.EnsureSuccessStatusCode();

            await deleteJobAsync(jobId);
        }


        private async Task<int> createJobAsync()
        {
            var jobId = new Random().Next(1000);
            var addUpdateJobRequest = new AddJobRequest
            {
                
                    
                    JobTitle = "",
                    Description = "",
                    FacilityId = 0,
                    Comments = ""
                
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(addUpdateJobRequest), Encoding.UTF8, "application/json");
            var response = await _appFixture.HttpClient.PostAsync(ApiConstants.JOBS_ENDPOINT, stringContent);
            response.EnsureSuccessStatusCode();
            return jobId;
        }

        private async Task deleteJobAsync(int jobId)
        {
            var response = await _appFixture.HttpClient.DeleteAsync(ApiConstants.JOBS_ENDPOINT + "/" + jobId);
            response.EnsureSuccessStatusCode();
        }
    }
}
