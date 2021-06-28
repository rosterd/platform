using System;
using System.Threading.Tasks;
using Rosterd.Data.SqlServer.Context;
using Rosterd.Data.SqlServer.Helpers;
using Rosterd.Data.SqlServer.Models;
using Rosterd.Domain.Exceptions;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.OrganizationModels;
using Rosterd.Services.Organizations.Interfaces;
using Rosterd.Services.Mappers;

namespace Rosterd.Services.Organizations
{
    public class OrganizationsService : IOrganizationsService
    {
        private readonly IRosterdDbContext _context;

        public OrganizationsService(IRosterdDbContext context) => _context = context;


        public async Task<PagedList<OrganizationModel>> GetAllOrganizations(PagingQueryStringParameters pagingParameters)
        {
            var query = _context.Organizations;
            var pagedList = await PagingList<Data.SqlServer.Models.Organization>.ToPagingList(query, pagingParameters.PageNumber, pagingParameters.PageSize);

            var domainModels = pagedList.ToDomainModels();
            return new PagedList<OrganizationModel>(domainModels, pagedList.TotalCount, pagedList.CurrentPage, pagedList.PageSize, pagedList.TotalPages);
        }


        public async Task<OrganizationModel> GetOrganization(long organizationId)
        {
            var organization = await _context.Organizations.FindAsync(organizationId);
            return organization?.ToDomainModel();
        }

        public async Task<OrganizationModel> CreateOrganization(OrganizationModel organizationModel)
        {
            var organizationToCreate = organizationModel.ToNewOrganization();

            await _context.Organizations.AddAsync(organizationToCreate);
            await _context.SaveChangesAsync();

            return organizationToCreate.ToDomainModel();
        }

        public async Task RemoveOrganization(long organizationId)
        {
            var organization = await _context.Organizations.FindAsync(organizationId);
            if (organization != null)
            {
                _context.Organizations.Remove(organization);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<OrganizationModel> UpdateOrganization(OrganizationModel organizationModel)
        {
            if (organizationModel.OrganizationId == null)
                throw new ArgumentNullException();

            var organizationFromDb = await _context.Organizations.FindAsync(organizationModel.OrganizationId);
            if (organizationFromDb == null)
                throw new EntityNotFoundException();

            var organizationModelToUpdate = organizationModel.ToDataModel(organizationFromDb);

            _context.Organizations.Update(organizationModelToUpdate);
            await _context.SaveChangesAsync();

            return organizationModelToUpdate.ToDomainModel();
        }
    }
}
