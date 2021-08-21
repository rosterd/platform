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
        public static StaffModel ToDomainModel(this Data.SqlServer.Models.Staff dataModel)
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

            staffModel.StaffFacilities = dataModel.StaffFacilities.AlwaysList().Select(s => new FacilityModel
            {
                FacilityId = s.FacilityId,
                FacilityName = s.FacilityName,

            }).AlwaysList();

            staffModel.StaffSkills = dataModel.StaffSkills.AlwaysList().Select(s => new SkillModel
            {
                SkillId = s.SkillId,
                SkillName = s.SkillName
            }).AlwaysList();

            return staffModel;
        }

        public static StaffSearchModel ToSearchModel(this Data.SqlServer.Models.Staff dataModel)
        {
            var staffSearchModel = new StaffSearchModel()
            {
                StaffId = dataModel.StaffId.ToString(),
                Email = dataModel.Email,
                FirstName = dataModel.FirstName,
                IsActive = dataModel.IsActive?.ToString() ?? true.ToString(),
                JobTitle = dataModel.JobTitle,
                LastName = dataModel.LastName,
                MobilePhoneNumber = dataModel.MobilePhoneNumber,
            };

            var staffFacility = dataModel.StaffFacilities.FirstOrDefault();
            if (staffFacility != null)
            {
                staffSearchModel.FacilityId = staffFacility.FacilityId.ToString();
                staffSearchModel.FacilityName = staffFacility.FacilityName;
            }

            var staffSkills = dataModel.StaffSkills.AlwaysList();
            if (staffSkills.IsNotNullOrEmpty())
                staffSearchModel.Skills = staffSkills.Select(s => s.SkillName).ToArray();

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
            staffFromDb.Email = domainModel.Email;
            staffFromDb.MobilePhoneNumber = domainModel.MobilePhoneNumber;
            staffFromDb.JobTitle = domainModel.JobTitle;
            staffFromDb.Comments = domainModel.Comments;

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
                Auth0Id = domainModel.Auth0Id
            };

            return staffToSave;
        }
    }
}
