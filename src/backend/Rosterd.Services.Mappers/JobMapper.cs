using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using Rosterd.Data.SqlServer.Helpers;
using Rosterd.Data.SqlServer.Models;
using Rosterd.Domain.Enums;
using Rosterd.Domain.Models.FacilitiesModels;
using Rosterd.Domain.Models.JobModels;
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

        public static List<JobModel> ToDomainModels(this PagingList<Data.SqlServer.Models.Job> pagedDataModels)
        {
            var pagedList = pagedDataModels.AlwaysList();
            if (pagedList.IsNullOrEmpty())
                return new List<JobModel>();

            var jobModels = pagedList.Select(job => job.ToDomainModel()).AlwaysList();
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
                PreviouslyCancelledJobId = domainModel.PreviouslyCancelledJobId,
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
