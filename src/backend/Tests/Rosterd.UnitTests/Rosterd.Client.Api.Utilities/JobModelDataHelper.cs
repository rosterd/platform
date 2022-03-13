using System;
using System.Collections.Generic;
using Rosterd.Domain.Enums;
using Rosterd.Domain.Models.FacilitiesModels;
using Rosterd.Domain.Models.JobModels;

namespace Rosterd.UnitTests.Rosterd.Client.Api.Utilities
{
    public class JobModelDataHelper
    {
        public static JobModel createJobModel(JobStatus status)
        {
            var jobModel = new JobModel()
            {
                JobId = new Random().Next(),
                JobTitle = "Level 1 HCA",
                Description = "Level 1 HCA to cover a shift of 8 hours",
                JobStartDateTimeUtc = new DateTime().AddDays(2),
                JobEndDateTimeUtc = new DateTime().AddDays(2).AddHours(8),
                JobPostedDateTimeUtc = new DateTime(),
                Facility = new FacilityModel
                {
                    FacilityId = 1
                },
                Comments = "Unit test job",
                GracePeriodToCancelMinutes = 60,
                Responsibilities = "Showering, Lifting",
                JobStatus = status,
                Experience = "1+ year",
                IsNightShift = false,
                JobSkills = new List<JobSkillModel>
                {
                    new JobSkillModel
                    {
                        SkillId = 1,
                        SkillName = "Tester"
                    }
                }
            };
            return jobModel;
        }
    }
}
