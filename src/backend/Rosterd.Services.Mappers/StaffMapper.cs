using System.Collections.Generic;
using System.Linq;
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
            var staffModel = new StaffModel
            {
                Email = dataModel.Email,
                FirstName = dataModel.FirstName,
                IsActive = dataModel.IsActive ?? true,
                JobTitle = dataModel.JobTitle,
                LastName = dataModel.LastName,
                MiddleName = dataModel.MiddleName,
                HomePhoneNumber = dataModel.HomePhoneNumber,
                MobilePhoneNumber = dataModel.MobilePhoneNumber,
                OtherPhoneNumber = dataModel.OtherPhoneNumber,
                Skills = new List<SkillModel>()
            };

            var staffFacility = dataModel.StaffFacilities.FirstOrDefault();
            if (staffFacility != null)
            {
                var facility = new FacilityModel
                {
                    FacilityId = staffFacility.FacilityId, FacilityName = staffFacility.FacilityName

                };
                staffModel.StaffFacility = facility;
            }

            var staffSkills = dataModel.StaffSkills.AlwaysList();
            if (staffSkills.IsNotNullOrEmpty())
            {
                foreach (var staffSkill in staffSkills)
                {
                    var skillModel = new SkillModel {SkillId = staffSkill.SkillId, SkillName = staffSkill.SkillName};
                    staffModel.Skills.Add(skillModel);
                }
            }

            return staffModel;
        }

        public static StaffSearchModel ToSearchModel(this Data.SqlServer.Models.Staff dataModel)
        {
            var staffSearchModel = new StaffSearchModel()
            {
                StaffId = dataModel.StaffId.ToString(),
                Email = dataModel.Email,
                FirstName = dataModel.FirstName,
                IsActive = dataModel.IsActive.ToString() ?? true.ToString(),
                JobTitle = dataModel.JobTitle,
                LastName = dataModel.LastName,
                MiddleName = dataModel.MiddleName,
                HomePhoneNumber = dataModel.HomePhoneNumber,
                MobilePhoneNumber = dataModel.MobilePhoneNumber,
                OtherPhoneNumber = dataModel.OtherPhoneNumber
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

        public static Data.SqlServer.Models.Staff ToDataModel(this StaffModel domainModel)
        {
            var staffToUpdate = domainModel.ToNewStaff();
            staffToUpdate.StaffId = domainModel.StaffId.Value;
            return staffToUpdate;
        }

        public static Data.SqlServer.Models.Staff ToNewStaff(this StaffModel domainModel)
        {
            var staffToSave = new Data.SqlServer.Models.Staff
            {
                IsActive = domainModel.IsActive,
                FirstName = domainModel.FirstName,
                MiddleName = domainModel.MiddleName,
                LastName = domainModel.LastName,
                Email = domainModel.Email,
                HomePhoneNumber = domainModel.HomePhoneNumber,
                MobilePhoneNumber = domainModel.MobilePhoneNumber,
                OtherPhoneNumber = domainModel.OtherPhoneNumber,
                JobTitle = domainModel.JobTitle,

                StaffFacilities = new List<StaffFacility> {new StaffFacility {FacilityId = domainModel.StaffFacility.FacilityId.Value}}
            };

            staffToSave.StaffSkills = new List<StaffSkill>();
            foreach (var domainModelSkill in domainModel.Skills.AlwaysList())
            {
                staffToSave.StaffSkills.Add(new StaffSkill{SkillId = domainModelSkill.SkillId,SkillName = domainModelSkill.SkillName});
            }

            return staffToSave;
        }
    }
}
