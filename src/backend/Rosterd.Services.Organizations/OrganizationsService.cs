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
using System.Text.Json;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Rosterd.Services.Auth0;

namespace Rosterd.Services.Organizations
{ public class OrganizationsService : IOrganizationsService
    {
        private readonly IRosterdDbContext _context;
        private readonly IAuth0AuthenticationService _auth0AuthenticationService;

        public OrganizationsService(IRosterdDbContext context, IAuth0AuthenticationService auth0AuthenticationService)
        {
            _context = context;
            _auth0AuthenticationService = auth0AuthenticationService;
        }

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
            //TODO: remove white space from organization name
            var organizationToCreate = organizationModel.ToNewOrganization();
            var organizationCreateRequest = new OrganizationCreateRequest
            {
                Name = $"organization-{organizationModel.OrganizationName.ToLower()}",
                DisplayName = organizationModel.OrganizationName
            };

            var managementClient = await _auth0AuthenticationService.GetAuth0ApiManagementClient();
            var organizationFromAuth0 = await managementClient.Organizations.CreateAsync(organizationCreateRequest);

            organizationToCreate.Auth0OrganizationId = organizationFromAuth0.Id;
            await _context.Organizations.AddAsync(organizationToCreate);
            await _context.SaveChangesAsync();

            return organizationToCreate.ToDomainModel();
        }

        public async Task RemoveOrganization(long organizationId)
        {
            var organization = await _context.Organizations.FindAsync(organizationId);
            if (organization != null)
            {
                var managementClient = await _auth0AuthenticationService.GetAuth0ApiManagementClient();
                await managementClient.Organizations.DeleteAsync(organization.Auth0OrganizationId);

                organization.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }
    }
}
