using System;
using System.Collections.Generic;
using System.Globalization;
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
                    jobModel.JobSkills.Add(new JobSkillModel { JobSkillId = jobModelJobSkill.JobSkillId, SkillId = jobModelJobSkill.SkillId, SkillName = jobModelJobSkill.SkillName});
                }
            }

            return jobModel;
        }

        public static JobModel ToDomainModelWithNoFacilityDetails(this Job dataModel)
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
                Facility = new FacilityModel{FacilityId = dataModel.FacilityId},
                Responsibilities = dataModel.Responsibilities,
                Experience = dataModel.Experience,
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
                    jobModel.JobSkills.Add(new JobSkillModel { JobSkillId = jobModelJobSkill.JobSkillId, SkillId = jobModelJobSkill.SkillId, SkillName = jobModelJobSkill.SkillName});
                }
            }

            return jobModel;
        }

        public static JobSearchModel ToSearchModel(this Job dataModel, List<Skill> skillsForJob, string auth0OrganizationId)
        {
            var jobSearchModel = new JobSearchModel
            {
                JobId = dataModel.JobId.ToString(),
                Auth0OrganizationId = auth0OrganizationId,
                JobTitle = dataModel.JobTitle,
                Description = dataModel.Description,
                JobStartDateTimeUtc = dataModel.JobStartDateTimeUtc,
                JobEndDateTimeUtc = dataModel.JobEndDateTimeUtc,
                Comments = dataModel.Comments,
                GracePeriodToCancelMinutes = dataModel.GracePeriodToCancelMinutes.ToString(),
                NoGracePeriod = dataModel.NoGracePeriod.ToBooleanOrDefault(),
                Responsibilities = dataModel.Responsibilities,
                Experience = dataModel.Experience,
                IsNightShift = dataModel.IsNightShift.ToBooleanOrDefault(),
                JobPostedDateTimeUtc = dataModel.JobPostedDateTimeUtc,
                JobStatusName = ((JobStatus?)dataModel.JobStatusId).ToString()
            };

            if (skillsForJob.IsNotNullOrEmpty())
            {
                jobSearchModel.SkillsIds = skillsForJob.Select(s => s.SkillId.ToString()).ToArray();
                jobSearchModel.SkillNames = skillsForJob.Select(s => s.SkillName).ToArray();
            }

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
                jobSearchModel.FacilityLatitude = dataModel.Facility.Latitude.ToString(CultureInfo.InvariantCulture);
                jobSearchModel.FacilityLongitude = dataModel.Facility.Longitude.ToString(CultureInfo.InvariantCulture);
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
                    JobId = Convert.ToInt64(job.Document.JobId),
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
                        Longitude = job.Document.FacilityLongitude.ToDecimal(),
                        Latitude = job.Document.FacilityLatitude.ToDecimal(),
                        Suburb = job.Document.FacilitySuburb
                    },
                    GracePeriodToCancelMinutes = job.Document.GracePeriodToCancelMinutes.ToLong(),
                    IsNightShift = job.Document.IsNightShift.ToBooleanOrDefault(),
                    JobEndDateTimeUtc = job.Document.JobEndDateTimeUtc.UtcDateTime,
                    JobPostedDateTimeUtc = job.Document.JobPostedDateTimeUtc.UtcDateTime,
                    JobStartDateTimeUtc = job.Document.JobStartDateTimeUtc.UtcDateTime,
                    JobStatus = job.Document.JobStatusName.ToEnum<JobStatus>(),
                    JobTitle = job.Document.JobTitle,
                    NoGracePeriod = job.Document.NoGracePeriod.ToBooleanOrDefault(),
                    JobStatusName = job.Document.JobStatusName,
                    Responsibilities = job.Document.Responsibilities,
                    JobSkills = job.Document.SkillNames.AlwaysList().Select(s => new JobSkillModel
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
                FacilityId = domainModel.Facility.FacilityId,
                JobStartDateTimeUtc = domainModel.JobStartDateTimeUtc,
                JobEndDateTimeUtc = domainModel.JobEndDateTimeUtc,
                Comments = domainModel.Comments,
                GracePeriodToCancelMinutes = domainModel.GracePeriodToCancelMinutes,
                NoGracePeriod = domainModel.NoGracePeriod,
                Experience = domainModel.Experience,
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
                    SkillId = jobSkillModel.SkillId
                };
                jobToSave.JobSkills.Add(skill);
            }

            return jobToSave;
        }
    }
}
