using System.Collections.Generic;
using System.Linq;
using Rosterd.Data.SqlServer.Helpers;
using Rosterd.Domain.Models.SkillsModels;
using Rosterd.Infrastructure.Extensions;

namespace Rosterd.Services.Mappers
{
    public static class SkillMapper
    {
        public static SkillModel ToDomainModel(this Data.SqlServer.Models.Skill dataModel)
        {
            var skillModel = new SkillModel
            {
                SkillId = dataModel.SkillId,
                SkillName = dataModel.SkillName,
                Description = dataModel.Description

            };

            return skillModel;
        }

        public static List<SkillModel> ToDomainModels(this PagingHelper<Data.SqlServer.Models.Skill> pagedDataModels)
        {
            var pagedList = pagedDataModels.AlwaysList();
            if (pagedList.IsNullOrEmpty())
                return new List<SkillModel>();

            var skillModels = pagedList.Select(skill => skill.ToDomainModel()).AlwaysList();
            return skillModels;
        }

        public static Data.SqlServer.Models.Skill ToDataModel(this SkillModel domainModel)
        {
            var skillToUpdate = domainModel.ToNewSkill();
            skillToUpdate.SkillId = domainModel.SkillId;
            return skillToUpdate;
        }

        public static Data.SqlServer.Models.Skill ToNewSkill(this SkillModel domainModel)
        {
            var skillToSave = new Data.SqlServer.Models.Skill
            {
                SkillName = domainModel.SkillName,
                Description = domainModel.Description
            };

            return skillToSave;
        }
    }
}
