using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rosterd.Data.SqlServer.Helpers;
using Rosterd.Data.SqlServer.Models;
using Rosterd.Domain.Models.FacilitiesModels;
using Rosterd.Domain.Models.SkillsModels;
using Rosterd.Domain.Models.StaffModels;
using Rosterd.Domain.Search;
using Rosterd.Infrastructure.Extensions;

namespace Rosterd.Services.Mappers
{
    public static class StaffMapper
    {
        public static StaffModel ToDomainModel(this Data.SqlServer.Models.Staff dataModel, List<Skill> skills = null)
        {
            // ReSharper disable once UseObjectOrCollectionInitializer
            var staffModel = new StaffModel
            {
                StaffId = dataModel.StaffId,
                Email = dataModel.Email,
                FirstName = dataModel.FirstName,
                IsActive = dataModel.IsActive ?? true,
                JobTitle = dataModel.JobTitle,
                LastName = dataModel.LastName,
                MobilePhoneNumber = dataModel.MobilePhoneNumber,
                Comments = dataModel.Comments,
                Auth0Id = dataModel.Auth0Id
            };

            staffModel.StaffSkills = dataModel.StaffSkills.AlwaysList().Select(s => new SkillModel
            {
                SkillId = s.SkillId,
                SkillName = skills.AlwaysList().FirstOrDefault(t => s.SkillId == t.SkillId)?.SkillName ?? string.Empty
            }).AlwaysList();

            return staffModel;
        }

        public static StaffSearchModel ToSearchModel(this Data.SqlServer.Models.Staff dataModel, List<Skill> skillsForStaff)
        {
            var staffSearchModel = new StaffSearchModel()
            {
                StaffId = dataModel.StaffId.ToString(),
                Auth0IdForStaff = dataModel.Auth0Id,
                Auth0OrganizationId = dataModel.Organization.Auth0OrganizationId,
                Email = dataModel.Email,
                FirstName = dataModel.FirstName,
                IsActive = dataModel.IsActive.ToBooleanOrDefault(),
                JobTitle = dataModel.JobTitle,
                LastName = dataModel.LastName,
                MobilePhoneNumber = dataModel.MobilePhoneNumber,
            };

            if (skillsForStaff.IsNotNullOrEmpty())
            {
                staffSearchModel.SkillsIds = skillsForStaff.Select(s => s.SkillId.ToString()).ToArray();
                staffSearchModel.SkillNames = skillsForStaff.Select(s => s.SkillName).ToArray();
            }

            return staffSearchModel;
        }

        public static List<StaffModel> ToDomainModels(this PagingList<Data.SqlServer.Models.Staff> pagedDataModels)
        {
            var pagedList = pagedDataModels.AlwaysList();
            if (pagedList.IsNullOrEmpty())
                return new List<StaffModel>();

            var staffModels = pagedList.Select(staff => staff.ToDomainModel()).AlwaysList();
            return staffModels;
        }

        public static Staff ToDataModel(this StaffModel domainModel, Staff staffFromDb)
        {
            staffFromDb.FirstName = domainModel.FirstName;
            staffFromDb.LastName = domainModel.LastName;
            staffFromDb.MobilePhoneNumber = domainModel.MobilePhoneNumber;
            staffFromDb.JobTitle = domainModel.JobTitle;
            staffFromDb.Comments = domainModel.Comments;
            staffFromDb.StaffRole = domainModel.StaffRole;

            return staffFromDb;
        }

        public static Data.SqlServer.Models.Staff ToNewStaff(this StaffModel domainModel)
        {
            var staffToSave = new Data.SqlServer.Models.Staff
            {
                IsActive = domainModel.IsActive,
                FirstName = domainModel.FirstName,
                LastName = domainModel.LastName,
                Email = domainModel.Email,
                MobilePhoneNumber = domainModel.MobilePhoneNumber,
                JobTitle = domainModel.JobTitle,
                Comments = domainModel.Comments,
                Auth0Id = domainModel.Auth0Id,
                StaffRole = domainModel.StaffRole
            };

            return staffToSave;
        }
    }
}
