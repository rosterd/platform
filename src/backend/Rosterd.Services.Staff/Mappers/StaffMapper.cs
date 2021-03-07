using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rosterd.Data.SqlServer.Helpers;
using Rosterd.Data.SqlServer.Models;
using Rosterd.Domain.Models.FacilitiesModels;
using Rosterd.Domain.Models.SkillsModels;
using Rosterd.Domain.Models.StaffModels;
using Rosterd.Infrastructure.Extensions;

namespace Rosterd.Services.Staff.Mappers
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
                var facility = new FacilityModel {FacilityId = staffFacility.FacilityId, FacilityName = staffFacility.FacilityName};
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

        public static List<StaffModel> ToDomainModels(this PagingHelper<Data.SqlServer.Models.Staff> pagedDataModels)
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
                JobTitle = domainModel.JobTitle
            };

            return staffToSave;
        }
    }
}
