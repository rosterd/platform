using System.Collections.Generic;
using System.Linq;
using Rosterd.Data.SqlServer.Helpers;
using Rosterd.Domain.Models.Resources;
using Rosterd.Domain.Models.SkillsModels;
using Rosterd.Infrastructure.Extensions;

namespace Rosterd.Services.Jobs.Mappers
{
    public static class JobMapper
    {
        public static JobModel ToDomainModel(this Data.SqlServer.Models.Job dataModel)
        {
            var jobModel = new JobModel
            {
                JobId = dataModel.JobId,
                JobTitle = dataModel.JobTitle,
                Description = dataModel.Description,
                FacilityId = dataModel.FacilityId,
                JobStartDateTimeUtc = dataModel.JobStartDateTimeUtc,
                JobEndDateTimeUtc = dataModel.JobEndDateTimeUtc,
                Comments = dataModel.Comments,
                GracePeriodToCancelMinutes = dataModel.GracePeriodToCancelMinutes,
                NoGracePeriod = dataModel.NoGracePeriod,
                //JobSkills = dataModel.JobSkills,
                //JobStatusChanges = dataModel.JobStatusChange

            };

            var jobSkills = dataModel.JobSkills.AlwaysList();
            if (jobSkills.IsNotNullOrEmpty())
            {
                foreach (var jobSkill in jobSkills)
                {
                    var skill = new JobSkillModel {
                        JobSkillId = jobSkill.JobSkillId,
                        SkillId = jobSkill.SkillId,
                        SkillName = jobSkill.SkillName,
                        JobId = jobSkill.JobId
                    };
                    jobModel.JobSkills.Add(skill);
                }
            }

            var jobStatusChanges = dataModel.JobStatusChanges.AlwaysList();
            if (jobStatusChanges.IsNotNullOrEmpty())
            {
                foreach (var jobStatusChange in jobStatusChanges)
                {
                    var jobStatusChangeModel = new JobStatusChangeModel {
                        JobStatusChangeId = jobStatusChange.JobStatusChangeId,
                        JobId = jobStatusChange.JobId,
                        JobStatusId = jobStatusChange.JobStatusId,
                        JobStatusChangeDateTimeUtc= jobStatusChange.JobStatusChangeDateTimeUtc,
                        JobStatusChangeReason = jobStatusChange.JobStatusChangeReason
                    };
                    jobModel.JobStatusChanges.Add(jobStatusChangeModel);
                }
            }

            return jobModel;
        }

        public static List<JobModel> ToDomainModels(this PagingHelper<Data.SqlServer.Models.Job> pagedDataModels)
        {
            var pagedList = pagedDataModels.AlwaysList();
            if (pagedList.IsNullOrEmpty())
                return new List<JobModel>();

            var jobModels = pagedList.Select(job => job.ToDomainModel()).AlwaysList();
            return jobModels;
        }

        public static Data.SqlServer.Models.Job ToDataModel(this JobModel domainModel)
        {
            var jobToUpdate = domainModel.ToNewJob();
            return jobToUpdate;
        }

        public static Data.SqlServer.Models.Job ToNewJob(this JobModel domainModel)
        {
            var jobToSave = new Data.SqlServer.Models.Job
            {
                JobId = domainModel.JobId,
                JobTitle = domainModel.JobTitle,
                Description = domainModel.Description,
                FacilityId = domainModel.FacilityId,
                JobStartDateTimeUtc = domainModel.JobStartDateTimeUtc,
                JobEndDateTimeUtc = domainModel.JobEndDateTimeUtc,
                Comments = domainModel.Comments,
                GracePeriodToCancelMinutes = domainModel.GracePeriodToCancelMinutes,
                NoGracePeriod = domainModel.NoGracePeriod
            };

            return jobToSave;
        }
    }
}
