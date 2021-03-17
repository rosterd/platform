using System.Collections.Generic;
using System.Linq;
using Rosterd.Data.SqlServer.Helpers;
using Rosterd.Data.SqlServer.Models;
using Rosterd.Domain.Models.JobModels;
using Rosterd.Infrastructure.Extensions;

namespace Rosterd.Services.Mappers
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
            };

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

            var jobSkillModels = domainModel.JobSkills.AlwaysList();
            if (jobSkillModels.IsNotNullOrEmpty())
            {
                foreach (var jobSkillModel in jobSkillModels)
                {
                    var skill = new JobSkill
                    {
                        JobSkillId = jobSkillModel.JobSkillId,
                        SkillId = jobSkillModel.SkillId,
                        SkillName = jobSkillModel.SkillName,
                        JobId = jobSkillModel.JobId
                    };
                    jobToSave.JobSkills.Add(skill);
                }
            }

            return jobToSave;
        }
    }
}