using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rosterd.Data.SqlServer.Context;
using Rosterd.Data.SqlServer.Helpers;
using Rosterd.Data.SqlServer.Models;
using Rosterd.Domain.Exceptions;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.FacilitiesModels;
using Rosterd.Infrastructure.Security.Interfaces;
using Rosterd.Services.Facilities.Interfaces;
using Rosterd.Services.Mappers;

namespace Rosterd.Services.Facilities
{
    public class FacilitiesService: IFacilitiesService
    {
        private readonly IRosterdDbContext _context;
        private readonly IBelongsToValidator _belongsToValidator;

        public FacilitiesService(IRosterdDbContext context, IBelongsToValidator belongsToValidator)
        {
            _context = context;
            _belongsToValidator = belongsToValidator;
        }

        public async Task<PagedList<FacilityModel>> GetAllFacilities(PagingQueryStringParameters pagingParameters, string auth0OrganizationId)
        {
            var organization = await _belongsToValidator.ValidateOrganizationExistsAndGetIfValid(auth0OrganizationId);

            var query = _context.Facilities.Where(s => s.OrganzationId == organization.OrganizationId && s.IsActive == true);
            var pagedList = await PagingList<Data.SqlServer.Models.Facility>.ToPagingList(query, pagingParameters.PageNumber, pagingParameters.PageSize);

            var domainModels = pagedList.ToDomainModels();
            return new PagedList<FacilityModel>(domainModels, pagedList.TotalCount, pagedList.CurrentPage, pagedList.PageSize, pagedList.TotalPages);
        }

        public async Task<FacilityModel> GetFacility(long facilityId, string auth0OrganizationId)
        {
            var organization = await _belongsToValidator.ValidateOrganizationExistsAndGetIfValid(auth0OrganizationId);

            var facility = await _context.Facilities.Where(s => s.OrganzationId == organization.OrganizationId && s.FacilityId == facilityId).FirstOrDefaultAsync();
            return facility?.ToDomainModel();
        }

        public async Task<FacilityModel> CreateFacility(FacilityModel facilityModel, string auth0OrganizationId)
        {
            var organization = await _belongsToValidator.ValidateOrganizationExistsAndGetIfValid(auth0OrganizationId);

            var facilityToCreate = facilityModel.ToNewFacility();
            facilityToCreate.OrganzationId = organization.OrganizationId;

            await _context.Facilities.AddAsync(facilityToCreate);
            await _context.SaveChangesAsync();

            return facilityToCreate.ToDomainModel();
        }

        public async Task RemoveFacility(long facilityId, string auth0OrganizationId)
        {
            await _belongsToValidator.ValidateFacilityBelongsToOrganization(facilityId, auth0OrganizationId);

            var facility = await _context.Facilities.FindAsync(facilityId);
            if (facility != null)
            {
                //If its already removed then return nothing to do
                if (facility.IsActive == false)
                    return;

                facility.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }

        public async Task ReactivateFacility(long facilityId, string auth0OrganizationId)
        {
            await _belongsToValidator.ValidateFacilityBelongsToOrganization(facilityId, auth0OrganizationId);

            var facility = await _context.Facilities.FindAsync(facilityId);
            if (facility != null)
            {
                //Already active nothing to do
                if (facility.IsActive)
                    return;

                facility.IsActive = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<FacilityModel> UpdateFacility(FacilityModel facilityModel, string auth0OrganizationId)
        {
            var existingFacility = await _context.Facilities.FindAsync(facilityModel.FacilityId);
            if (existingFacility == null)
                throw new EntityNotFoundException($"No existing facility with id {facilityModel.FacilityId} was not found");

            await _belongsToValidator.ValidateFacilityBelongsToOrganization(existingFacility.FacilityId, auth0OrganizationId);

            var facilityModelToUpdate = facilityModel.ToDataModel(existingFacility);

            _context.Facilities.Update(facilityModelToUpdate);
            await _context.SaveChangesAsync();

            return facilityModelToUpdate.ToDomainModel();
        }

        public async Task<bool> DoesFacilityWithSameNameExistForOrganization(FacilityModel facilityModel, string auth0OrganizationId, string allowedName = null)
        {
            var organization = await _belongsToValidator.ValidateOrganizationExistsAndGetIfValid(auth0OrganizationId);

            var matchingFacilities =
                await (from facility in _context.Facilities
                where facility.OrganzationId == organization.OrganizationId &&
                      facility.IsActive == true &&
                      EF.Functions.Like(facility.FacilityName, facilityModel.FacilityName)
                select facility).FirstOrDefaultAsync();

            if (allowedName != null && matchingFacilities != null)
            {
                return matchingFacilities.FacilityName.ToLower() != allowedName.ToLower();
            }

            return matchingFacilities != null;
        }
    }
}
