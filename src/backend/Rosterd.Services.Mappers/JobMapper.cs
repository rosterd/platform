using System.Collections.Generic;
using System.Linq;
using Azure.Search.Documents.Models;
//using System.Runtime.Intrinsics.Arm;
using Rosterd.Data.SqlServer.Helpers;
using Rosterd.Data.SqlServer.Models;
using Rosterd.Domain.Enums;
using Rosterd.Domain.Models.FacilitiesModels;
using Rosterd.Domain.Models.JobModels;
using Rosterd.Domain.Search;
using Rosterd.Infrastructure.Extensions;

namespace Rosterd.Services.Mappers
{
    public static class JobMapper
    {
        public static JobModel ToDomainModel(this Job dataModel)
        {
            var jobModel = new JobModel
            {
                JobId = dataModel.JobId,
                JobTitle = dataModel.JobTitle,
                Description = dataModel.Description,
                JobStartDateTimeUtc = dataModel.JobStartDateTimeUtc,
                JobEndDateTimeUtc = dataModel.JobEndDateTimeUtc,
                Comments = dataModel.Comments,
                GracePeriodToCancelMinutes = dataModel.GracePeriodToCancelMinutes,
                NoGracePeriod = dataModel.NoGracePeriod,
                Facility = dataModel.Facility?.ToDomainModel(),
                Responsibilities = dataModel.Responsibilities,
                Experience = dataModel.Experience,
                IsDayShift = dataModel.IsDayShift ?? false,
                IsNightShift = dataModel.IsNightShift ?? false,
                JobPostedDateTimeUtc = dataModel.JobPostedDateTimeUtc,
                JobStatus = (JobStatus?)dataModel.JobStatusId,
                JobStatusName = ((JobStatus?)dataModel.JobStatusId).ToString(),
                JobSkills = new List<JobSkillModel>(),
            };

            if (dataModel.JobSkills.IsNotNullOrEmpty())
            {
                foreach (var jobModelJobSkill in jobModel.JobSkills)
                {
                    jobModel.JobSkills.Add(new JobSkillModel {JobId = dataModel.JobId, JobSkillId = jobModelJobSkill.JobSkillId, SkillId = jobModelJobSkill.SkillId, SkillName = jobModelJobSkill.SkillName});
                }
            }

            return jobModel;
        }

        public static JobSearchModel ToSearchModel(this Job dataModel)
        {
            var jobSearchModel = new JobSearchModel
            {
                JobId = dataModel.JobId.ToString(),
                JobTitle = dataModel.JobTitle,
                Description = dataModel.Description,
                JobStartDateTimeUtc = dataModel.JobStartDateTimeUtc,
                JobEndDateTimeUtc = dataModel.JobEndDateTimeUtc,
                Comments = dataModel.Comments,
                GracePeriodToCancelMinutes = dataModel.GracePeriodToCancelMinutes,
                NoGracePeriod = dataModel.NoGracePeriod,
                Responsibilities = dataModel.Responsibilities,
                Experience = dataModel.Experience,
                IsDayShift = dataModel.IsDayShift ?? false,
                IsNightShift = dataModel.IsNightShift ?? false,
                JobPostedDateTimeUtc = dataModel.JobPostedDateTimeUtc,
                JobStatusName = ((JobStatus?)dataModel.JobStatusId).ToString()
            };

            if (dataModel.JobSkills.IsNotNullOrEmpty())
                jobSearchModel.Skills = dataModel.JobSkills.Select(s => s.SkillName).ToArray();

            if (dataModel.Facility != null)
            {
                jobSearchModel.FacilityId = dataModel.Facility.FacilityId.ToString();
                jobSearchModel.FacilityName = dataModel.Facility.FacilityName;
                jobSearchModel.FacilityAddress = dataModel.Facility.Address;
                jobSearchModel.FacilityCity = dataModel.Facility.City;
                jobSearchModel.FacilityCountry = dataModel.Facility.Country;
                jobSearchModel.FacilityPhoneNumber1 = dataModel.Facility.PhoneNumber1;
                jobSearchModel.FacilityPhoneNumber2 = dataModel.Facility.PhoneNumber2;
                jobSearchModel.FacilitySuburb = dataModel.Facility.Suburb;
            }

            return jobSearchModel;
        }

        public static List<JobModel> ToDomainModels(this PagingList<Data.SqlServer.Models.Job> pagedDataModels)
        {
            var pagedList = pagedDataModels.AlwaysList();
            if (pagedList.IsNullOrEmpty())
                return new List<JobModel>();

            var jobModels = pagedList.Select(job => job.ToDomainModel()).AlwaysList();
            return jobModels;
        }

        public static List<JobModel> ToDomainModels(this SearchResults<JobSearchModel> searchResults)
        {
            if (searchResults == null)
                return new List<JobModel>();

            var jobModels = searchResults.GetResults().Select(job =>
                new JobModel
                {
                    JobId = job.Document.JobId.ToInt64(),
                    Comments = job.Document.Comments,
                    Description = job.Document.Description,
                    Experience = job.Document.Experience,
                    Facility = new FacilityModel
                    {
                        Address = job.Document.FacilityAddress,
                        City = job.Document.FacilityCity,
                        Country = job.Document.FacilityCountry,
                        FacilityId = job.Document.FacilityId.ToInt64(),
                        FacilityName = job.Document.FacilityName,
                        PhoneNumber1 = job.Document.FacilityPhoneNumber1,
                        PhoneNumber2 = job.Document.FacilityPhoneNumber2,
                        Suburb = job.Document.FacilitySuburb
                    },
                    GracePeriodToCancelMinutes = job.Document.GracePeriodToCancelMinutes,
                    IsDayShift = job.Document.IsDayShift,
                    IsNightShift = job.Document.IsNightShift,
                    JobEndDateTimeUtc = job.Document.JobEndDateTimeUtc,
                    JobPostedDateTimeUtc = job.Document.JobPostedDateTimeUtc,
                    JobStartDateTimeUtc = job.Document.JobStartDateTimeUtc,
                    JobStatus = job.Document.JobStatusName.ToEnum<JobStatus>(),
                    JobTitle = job.Document.JobTitle,
                    NoGracePeriod = job.Document.NoGracePeriod,
                    JobStatusName = job.Document.JobStatusName,
                    Responsibilities = job.Document.Responsibilities,
                    JobSkills = job.Document.Skills.AlwaysList().Select(s => new JobSkillModel
                    {
                        SkillName = s
                    }).ToList()
                }).AlwaysList();
            return jobModels;
        }

        public static Data.SqlServer.Models.Job ToNewJob(this JobModel domainModel)
        {
            var jobToSave = new Data.SqlServer.Models.Job
            {
                JobTitle = domainModel.JobTitle,
                Description = domainModel.Description,
                FacilityId = domainModel.Facility.FacilityId.Value,
                JobStartDateTimeUtc = domainModel.JobStartDateTimeUtc,
                JobEndDateTimeUtc = domainModel.JobEndDateTimeUtc,
                Comments = domainModel.Comments,
                GracePeriodToCancelMinutes = domainModel.GracePeriodToCancelMinutes,
                NoGracePeriod = domainModel.NoGracePeriod,
                Experience = domainModel.Experience,
                IsDayShift = domainModel.IsDayShift,
                IsNightShift = domainModel.IsNightShift,
                Responsibilities = domainModel.Responsibilities
            };

            var jobSkillModels = domainModel.JobSkills.AlwaysList();
            if (!jobSkillModels.IsNotNullOrEmpty())
                return jobToSave;

            foreach (var jobSkillModel in jobSkillModels)
            {
                var skill = new JobSkill
                {
                    JobSkillId = jobSkillModel.JobSkillId,
                    SkillId = jobSkillModel.SkillId,
                    SkillName = jobSkillModel.SkillName
                };
                jobToSave.JobSkills.Add(skill);
            }

            return jobToSave;
        }
    }
}
