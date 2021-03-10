using System.Collections.Generic;
using System.Linq;
using Rosterd.Data.SqlServer.Helpers;
using Rosterd.Domain.Models.SkillsModels;
using Rosterd.Infrastructure.Extensions;

namespace Rosterd.Services.Skills.Mappers
{
    public static class SkillMapper
    {
        public static SkillModel ToDomainModel(this Data.SqlServer.Models.Skill dataModel)
        {
            var skillModel = new SkillModel
            {
                SkillId = dataModel.SkillId,
                SkillName = dataModel.SkillName,
                SkillDescription = dataModel.Description

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
            return skillToUpdate;
        }

        public static Data.SqlServer.Models.Skill ToNewSkill(this SkillModel domainModel)
        {
            var skillToSave = new Data.SqlServer.Models.Skill
            {
                SkillId = domainModel.SkillId,
                SkillName = domainModel.SkillName,
                Description = domainModel.SkillDescription
            };

            return skillToSave;
        }
    }
}
