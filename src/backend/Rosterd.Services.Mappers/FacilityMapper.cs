using System;
using System.Collections.Generic;
using System.Linq;
using Rosterd.Data.SqlServer.Helpers;
using Rosterd.Domain.Models.FacilitiesModels;
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
                FacilityName = dataModel.FacilityName
            };

            return facilityModel;
        }

        public static List<FacilityModel> ToDomainModels(this PagingHelper<Data.SqlServer.Models.Facility> pagedDataModels)
        {
            var pagedList = pagedDataModels.AlwaysList();
            if (pagedList.IsNullOrEmpty())
                return new List<FacilityModel>();

            var facilityModels = pagedList.Select(facility => facility.ToDomainModel()).AlwaysList();
            return facilityModels;
        }

        public static Data.SqlServer.Models.Facility ToDataModel(this FacilityModel domainModel)
        {
            var facilityToUpdate = domainModel.ToNewFacility();
            facilityToUpdate.FacilityId = domainModel.FacilityId;
            return facilityToUpdate;
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
                Suburb = domainModel.Suburb
            };

            return facilityToSave;
        }
    }
}
