using System;
using System.Collections.Generic;
using System.Linq;
using Rosterd.Data.SqlServer.Helpers;
using Rosterd.Data.SqlServer.Models;
using Rosterd.Domain.Models.FacilitiesModels;
using Rosterd.Domain.Models.OrganizationModels;
using Rosterd.Infrastructure.Extensions;

namespace Rosterd.Services.Mappers
{
    public static class FacilityMapper
    {
        public static FacilityModel ToDomainModel(this Data.SqlServer.Models.Facility dataModel)
        {
            var facilityModel = new FacilityModel
            {
                FacilityId = dataModel.FacilityId,
                FacilityName = dataModel.FacilityName,
                Address = dataModel.Address,
                City = dataModel.City,
                Country = dataModel.Country,
                PhoneNumber1 = dataModel.PhoneNumber1,
                PhoneNumber2 = dataModel.PhoneNumber2,
                Organization = new OrganizationModel { OrganizationId = dataModel.OrganzationId},
                IsActive = dataModel.IsActive
            };

            return facilityModel;
        }

        public static List<FacilityModel> ToDomainModels(this PagingList<Data.SqlServer.Models.Facility> pagedDataModels)
        {
            var pagedList = pagedDataModels.AlwaysList();
            if (pagedList.IsNullOrEmpty())
                return new List<FacilityModel>();

            var facilityModels = pagedList.Select(facility => facility.ToDomainModel()).AlwaysList();
            return facilityModels;
        }

        public static Facility ToDataModel(this FacilityModel domainModelToUpdate, Facility existingDataModelFromDb)
        {
            existingDataModelFromDb.FacilityName = domainModelToUpdate.FacilityName;
            existingDataModelFromDb.Address = domainModelToUpdate.Address;
            existingDataModelFromDb.City = domainModelToUpdate.City;
            existingDataModelFromDb.Country = domainModelToUpdate.Country;
            existingDataModelFromDb.PhoneNumber1 = domainModelToUpdate.PhoneNumber1;
            existingDataModelFromDb.PhoneNumber2 = domainModelToUpdate.PhoneNumber2;
            existingDataModelFromDb.Suburb = domainModelToUpdate.Suburb;
            existingDataModelFromDb.IsActive = domainModelToUpdate.IsActive.Value;

            return existingDataModelFromDb;
        }

        public static Data.SqlServer.Models.Facility ToNewFacility(this FacilityModel domainModel)
        {
            var facilityToSave = new Data.SqlServer.Models.Facility
            {
                FacilityName = domainModel.FacilityName,
                Address = domainModel.Address,
                City = domainModel.City,
                Country = domainModel.Country,
                PhoneNumber1 = domainModel.PhoneNumber1,
                PhoneNumber2 = domainModel.PhoneNumber2,
                Suburb = domainModel.Suburb,
                IsActive = domainModel.IsActive.Value
            };

            return facilityToSave;
        }
    }
}
