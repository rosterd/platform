using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Flurl.Http;
using Newtonsoft.Json;
using Rosterd.Admin.Api.Requests.Jobs;
using Rosterd.Client.Api.IntegrationTests.Helpers;
using Rosterd.Domain.Models.JobModels;
using Rosterd.Domain.Models.StaffModels;

namespace Rosterd.Client.Api.IntegrationTests.Utitlities
{
    public class JobDataHelper
    {
        private ApiHelper _apiHelper;

        public JobDataHelper()
        {
            _apiHelper = new ApiHelper();
        }

        public long publishJob(long skillId, long graceperiod = 60)
        {
            //setup
            var response = _apiHelper.GetAdminApiRequest(ApiConstants.JOBS_ENDPOINT)
                .PostJsonAsync(new JobRequestBuilder().buildAddJobRequest(skillId, graceperiod)).Result;
            //assert
            response.StatusCode.Should().Be(200);
            var jobsModel = JsonConvert.DeserializeObject<JobModel>(response.ResponseMessage.Content.ReadAsStringAsync().Result);
            return jobsModel.JobId;
        }

        class JobRequestBuilder
        {
            public AddJobRequest buildAddJobRequest(long skillId, long graceperiod = 60)
            {
                var addJobRequest = new AddJobRequest
                {
                    JobTitle = "Developer",
                    Description = "IntegrationTests",
                    JobStartDateTimeUtc = DateTime.Now.AddDays(2),
                    JobEndDateTimeUtc = DateTime.Now.AddDays(2).AddHours(8),
                    Comments = "Integration test job",
                    GracePeriodToCancelMinutes = graceperiod,
                    Responsibilities = "Development, Testing, Devops",
                    Experience = "5+ year",
                    IsNightShift = false,
                    SkillsRequiredForJob = new List<long> {skillId}
                };
                return addJobRequest;
            }
        }
    }
}
