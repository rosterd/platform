using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rosterd.Data.SqlServer.Helpers;
using Rosterd.Data.SqlServer.Models;
using Rosterd.Domain.Models.OrganizationModels;
using Rosterd.Infrastructure.Extensions;

namespace Rosterd.Services.Mappers
{
    public static class OrganizationMapper
    {
        public static OrganizationModel ToDomainModel(this Data.SqlServer.Models.Organization dataModel)
        {
            var organizationModel = new OrganizationModel
            {
                OrganizationId = dataModel.OrganizationId,
                OrganizationName = dataModel.OrganizationName,
                Address = dataModel.Address
            };

            return organizationModel;
        }

        public static List<OrganizationModel> ToDomainModels(this PagingList<Data.SqlServer.Models.Organization> pagedDataModels)
        {
            var pagedList = pagedDataModels.AlwaysList();
            if (pagedList.IsNullOrEmpty())
                return new List<OrganizationModel>();

            var organizationModels = pagedList.Select(organization => organization.ToDomainModel()).AlwaysList();
            return organizationModels;
        }

        public static Organization ToDataModel(this OrganizationModel domainModelToUpdate, Organization organizationFromDb)
        {
            organizationFromDb.Address = domainModelToUpdate.Address;
            organizationFromDb.OrganizationName = domainModelToUpdate.OrganizationName;

            return organizationFromDb;
        }

        public static Data.SqlServer.Models.Organization ToNewOrganization(this OrganizationModel domainModel)
        {
            var organizationToSave = new Data.SqlServer.Models.Organization
            {
                OrganizationName = domainModel.OrganizationName,
                Address = domainModel.Address,
                TenantId = domainModel.TenantId.Value
            };

            return organizationToSave;
        }
    }
}
