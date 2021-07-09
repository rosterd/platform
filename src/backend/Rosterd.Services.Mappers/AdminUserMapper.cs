using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rosterd.Data.TableStorage.Models;
using Rosterd.Domain.Models.StaffModels;
using Rosterd.Domain.Models.Users;
using Rosterd.Infrastructure.Extensions;

namespace Rosterd.Services.Mappers
{
    public static class AdminUserMapper
    {
        public static AdminUserModel ToDomainModel(this RosterdAdminUser dataModel) =>
            new AdminUserModel
            {
                OrganizationId = dataModel.OrganizationId,
                Auth0Id = dataModel.Auth0Id,
                FacilityIds = dataModel.FacilityIdsCsvString.ConvertCsvStringToList(),
                CreatedDateTimeUtc = dataModel.CreatedDateTimeUtc,
                LastUpdatedDateTimeUtc = dataModel.LastUpdatedDateTimeUtc
            };

        public static RosterdAdminUser ToDataModel(this AdminUserModel domainModel) =>
            new RosterdAdminUser(domainModel.Auth0Id)
            {
                OrganizationId = domainModel.OrganizationId,
                Auth0Id = domainModel.Auth0Id,
                FacilityIdsCsvString = domainModel.FacilityIds.AlwaysList().ToCsvString()
            };
    }
}
