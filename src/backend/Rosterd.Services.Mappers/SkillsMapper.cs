using System.Collections.Generic;
using System.Linq;
using Rosterd.Data.SqlServer.Helpers;
using Rosterd.Data.SqlServer.Models;
using Rosterd.Domain.Models.FacilitiesModels;
using Rosterd.Domain.Models.SkillsModels;
using Rosterd.Domain.Models.StaffModels;
using Rosterd.Infrastructure.Extensions;

namespace Rosterd.Services.Mappers
{
    public static class SkillsMapper
    {
        public static SkillModel ToDomainModel(this Data.SqlServer.Models.Skill dataModel)
        {
            var skillModel = new SkillModel {SkillName = dataModel.SkillName, SkillDescription = dataModel.Description, SkillId = dataModel.SkillId};
            return skillModel;
        }

        public static List<SkillModel> ToDomainModels(this PagingHelper<Data.SqlServer.Models.Skill> pagedDataModels)
        {
            var pagedList = pagedDataModels.AlwaysList();
            if (pagedList.IsNullOrEmpty())
                return new List<SkillModel>();

            var skillModels = pagedList.Select(staff => staff.ToDomainModel()).AlwaysList();
            return skillModels;
        }

        public static Data.SqlServer.Models.Skill ToDataModel(this SkillModel domainModel)
        {
            var skillToUpdate = domainModel.ToNew();
            return skillToUpdate;
        }

        public static Data.SqlServer.Models.Skill ToNew(this SkillModel domainModel)
        {
            var skill = new Skill {Description = domainModel.SkillDescription, SkillName = domainModel.SkillName};
            return skill;
        }
    }
}
