using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rosterd.Data.SqlServer.Context;
using Rosterd.Data.SqlServer.Helpers;
using Rosterd.Data.SqlServer.Models;
using Rosterd.Domain.Exceptions;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.FacilitiesModels;
using Rosterd.Services.Facilities.Interfaces;
using Rosterd.Services.Mappers;

namespace Rosterd.Services.Facilities
{
    public class FacilitiesService: IFacilitiesService
    {
        private readonly IRosterdDbContext _context;

        public FacilitiesService(IRosterdDbContext context) => _context = context;


        public async Task<PagedList<FacilityModel>> GetAllFacilities(PagingQueryStringParameters pagingParameters)
        {
            var query = _context.Facilities;
            var pagedList = await PagingList<Data.SqlServer.Models.Facility>.ToPagingList(query, pagingParameters.PageNumber, pagingParameters.PageSize);

            var domainModels = pagedList.ToDomainModels();
            return new PagedList<FacilityModel>(domainModels, pagedList.TotalCount, pagedList.CurrentPage, pagedList.PageSize, pagedList.TotalPages);
        }
        
        public async Task<FacilityModel> GetFacility(long facilityId)
        {
            var facility = await _context.Facilities.FindAsync(facilityId);
            return facility?.ToDomainModel();
        }

        public async Task<FacilityModel> CreateFacility(FacilityModel facilityModel)
        {
            var facilityToCreate = facilityModel.ToNewFacility();

            await _context.Facilities.AddAsync(facilityToCreate);
            await _context.SaveChangesAsync(); 

            return facilityToCreate.ToDomainModel();
        }

        public async Task RemoveFacility(long facilityId)
        {
            var facility = await _context.Facilities.FindAsync(facilityId);
            if (facility != null)
            {
                _context.Facilities.Remove(facility);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<FacilityModel> UpdateFacility(FacilityModel facilityModel)
        {
            if (facilityModel.FacilityId == null)
                throw new ArgumentNullException();

            var existingFacility = await _context.Facilities.FindAsync(facilityModel.FacilityId);
            if (existingFacility == null)
                throw new EntityNotFoundException();

            var facilityModelToUpdate = facilityModel.ToDataModel(existingFacility);

            _context.Facilities.Update(facilityModelToUpdate);
            await _context.SaveChangesAsync();

            return facilityModelToUpdate.ToDomainModel();
        }

        public async Task<bool> DoesFacilityWithSameNameExistForOrganization(FacilityModel facilityModel)
        {
            var matchingFacilities =
                await (from facility in _context.Facilities
                where facility.OrganzationId == facilityModel.Organization.OrganizationId && EF.Functions.Like(facility.FacilityName, facilityModel.FacilityName)
                select facility).FirstOrDefaultAsync();

            return matchingFacilities != null;
        }
    }
}
