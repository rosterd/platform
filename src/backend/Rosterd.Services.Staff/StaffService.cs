using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rosterd.Data.SqlServer.Context;
using Rosterd.Data.SqlServer.Helpers;
using Rosterd.Data.SqlServer.Models;
using Rosterd.Data.TableStorage.Context;
using Rosterd.Data.TableStorage.Models;
using Rosterd.Domain;
using Rosterd.Domain.Exceptions;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.FacilitiesModels;
using Rosterd.Domain.Models.StaffModels;
using Rosterd.Domain.Search;
using Rosterd.Infrastructure.Extensions;
using Rosterd.Infrastructure.Search.Interfaces;
using Rosterd.Infrastructure.Security.Interfaces;
using Rosterd.Services.Mappers;
using Rosterd.Services.Staff.Interfaces;


namespace Rosterd.Services.Staff
{
    public class StaffService : IStaffService
    {
        private readonly IRosterdDbContext _context;
        private readonly IAzureTableStorage _azureTableStorage;
        private readonly IBelongsToValidator _belongsToValidator;
        private readonly ISearchIndexProvider _searchIndexProvider;

        public StaffService(IRosterdDbContext context, IAzureTableStorage azureTableStorage, IBelongsToValidator belongsToValidator, ISearchIndexProvider searchIndexProvider)
        {
            _context = context;
            _azureTableStorage = azureTableStorage;
            _belongsToValidator = belongsToValidator;
            _searchIndexProvider = searchIndexProvider;
        }

        ///<inheritdoc/>
        public async Task<PagedList<StaffModel>> GetAllStaff(PagingQueryStringParameters pagingParameters, string auth0OrganizationId)
        {
            var organization = await _belongsToValidator.ValidateOrganizationExistsAndGetIfValid(auth0OrganizationId);

            var query = _context.Staff.Include(s => s.StaffSkills).Where(s => s.OrganizationId == organization.OrganizationId && s.IsActive == true);
            var pagedList = await PagingList<Data.SqlServer.Models.Staff>.ToPagingList(query, pagingParameters.PageNumber, pagingParameters.PageSize);

            var domainModels = pagedList.ToDomainModels();
            return new PagedList<StaffModel>(domainModels, pagedList.TotalCount, pagedList.CurrentPage, pagedList.PageSize, pagedList.TotalPages);
        }

        ///<inheritdoc/>
        public async Task<StaffModel> GetStaff(long staffId, string auth0OrganizationId)
        {
            await _belongsToValidator.ValidateStaffBelongsToOrganization(staffId, auth0OrganizationId);

            var staff = await _context.Staff
                .Include(s => s.StaffSkills)
                .FirstOrDefaultAsync(s => s.StaffId == staffId);

            if(staff == null)
                throw new EntityNotFoundException("The staff member does not exist");

            //We need the skills names
            var staffSkillIds = staff.StaffSkills.Select(s => s.SkillId).ToList();
            var skills = await _context.Skills.Where(s => staffSkillIds.Contains(s.SkillId)).ToListAsync();

            return staff.ToDomainModel(skills);
        }

        ///<inheritdoc/>
        public async Task<(long StaffId, long OrganizationId, string Auth0OrganizationId)> GetStaff(string staffAuth0Id)
        {
            var staff = await _context.Staff.Include(s => s.Organization).FirstOrDefaultAsync(s => s.Auth0Id == staffAuth0Id);
            if(staff == null)
                throw new EntityNotFoundException("The staff member does not exist");

            return (staff.StaffId, staff.OrganizationId, staff.Organization.Auth0OrganizationId);
        }

        public async Task<StaffModel> GetStaffFromAuth0Id(string staffAuth0Id)
        {
            var staff = await _context.Staff.Include(s => s.Organization).FirstOrDefaultAsync(s => s.Auth0Id == staffAuth0Id);
            if (staff == null)
                throw new EntityNotFoundException("The staff member does not exist");

            return staff.ToDomainModel();
        }

        ///<inheritdoc/>
        public async Task<StaffModel> CreateStaff(StaffModel staffModel, string auth0OrganizationId)
        {
            if (staffModel.Auth0Id.IsNullOrEmpty())
                throw new Auth0IdNotSetException("Staff member is not created with the identity provider");

            var organization = await _belongsToValidator.ValidateOrganizationExistsAndGetIfValid(auth0OrganizationId);
            await _belongsToValidator.ValidateSkillsBelongsToOrganization(staffModel.StaffSkills.AlwaysList().Select(s => s.SkillId).AlwaysList(), auth0OrganizationId);

            if (staffModel.StaffFacilities.IsNotNullOrEmpty())
                await _belongsToValidator.ValidateFacilitiesBelongsToOrganization(staffModel.StaffFacilities.Select(s => s.FacilityId).ToList(), auth0OrganizationId);

            //Populate staff details
            var staffToCreate = staffModel.ToNewStaff();
            staffToCreate.OrganizationId = organization.OrganizationId;
            var newStaff = await _context.Staff.AddAsync(staffToCreate);

            //Populate the staff skills
            var staffSkills = staffModel.StaffSkills.Select(s => new StaffSkill {SkillId = s.SkillId, StaffId = newStaff.Entity.StaffId}).AlwaysList();
            newStaff.Entity.StaffSkills.AddRange(staffSkills.ToArray());

            //Populate any facilities for the staff (for a facility admin they need to be associated with a facility)
            if (staffModel.StaffFacilities.IsNotNullOrEmpty())
            {
                var staffFacilities = staffModel.StaffFacilities.Select(s => new StaffFacility { FacilityId = s.FacilityId, StaffId = newStaff.Entity.StaffId }).AlwaysList();
                newStaff.Entity.StaffFacilities.AddRange(staffFacilities.ToArray());
            }

            await _context.SaveChangesAsync();
            return newStaff.Entity.ToDomainModel();
        }

        public async Task<long> GetFacilityForStaffWhoIsFacilityAdmin(string auth0IdForStaff)
        {
            var staff = await _context.Staff.FirstOrDefaultAsync(s => s.Auth0Id == auth0IdForStaff);
            if (staff == null)
                throw new EntityNotFoundException($"Staff with auth0id {auth0IdForStaff} not found");

            var staffFacility = await _context.StaffFacilities.FirstOrDefaultAsync(s => s.StaffId == staff.StaffId);
            if (staffFacility == null)
                throw new EntityNotFoundException($"No staff facilities found for staff-id {staff.StaffId}, auth0id {staff.Auth0Id}");

            return staffFacility.FacilityId;
        }

        ///<inheritdoc/>
        public async Task<StaffModel> UpdateStaff(StaffModel staffModel, string auth0OrganizationId)
        {
            await _belongsToValidator.ValidateStaffBelongsToOrganization(staffModel.StaffId.Value, auth0OrganizationId);

            //Get the existing staff
            var staffFromDb = await _context.Staff.FindAsync(staffModel.StaffId.Value);
            if (staffFromDb == null)
                throw new EntityNotFoundException("The staff member does not exist");

            //Update the db entry with the latest updates from the domain model
            var staffModelToUpdate = staffModel.ToDataModel(staffFromDb);
            _context.Staff.Update(staffModelToUpdate);

            //Reset the staff skills to new updated skills
            if (staffModel.StaffSkills.IsNotNullOrEmpty())
            {
                var existingSkills = await _context.StaffSkills.Where(s => s.StaffId == staffFromDb.StaffId).ToListAsync();
                _context.StaffSkills.RemoveRange(existingSkills);

                foreach (var staffSkillToUpdate in staffModel.StaffSkills)
                {
                    await _context.StaffSkills.AddAsync(new StaffSkill { SkillId = staffSkillToUpdate.SkillId, StaffId = staffFromDb.StaffId });
                }
            }

            await _context.SaveChangesAsync();
            return staffModelToUpdate.ToDomainModel();
        }

        ///<inheritdoc/>
        public async Task<string> UpdateStaffToInactive(long staffId, string auth0OrganizationId)
        {
            await _belongsToValidator.ValidateStaffBelongsToOrganization(staffId, auth0OrganizationId);
            var staff = await _context.Staff.FindAsync(staffId);
            var oldAuth0Id = staff.Auth0Id;

            if (staff == null)
                throw new EntityNotFoundException($"staff with staff id {staffId} for auth0-organizationId {auth0OrganizationId} not found");

            staff.IsActive = false;
            staff.Auth0Id = $"{RosterdConstants.Users.UserRemovedFromAuth0Text}_{Guid.NewGuid()}";

            await _context.SaveChangesAsync();
            return oldAuth0Id;
        }

        ///<inheritdoc/>
        public async Task<StaffModel> UpdateStaffToInactive(string auth0Userid, string auth0OrganizationId)
        {
            var staff = await _context.Staff.FirstOrDefaultAsync(s => s.Auth0Id == auth0Userid);
            if(staff == null)
                throw new EntityNotFoundException($"staff with auth0userid {auth0Userid} for auth0-organizationId {auth0OrganizationId}");

            //Its ok to call this method because we already fetched this record in memory and when this method calls it will get it from memory
            await UpdateStaffToInactive(staff.StaffId, auth0OrganizationId);

            return staff.ToDomainModel();
        }

        ///<inheritdoc/>
        public async Task UpdateStaffToActive(long staffId, string userAuth0Id, string auth0OrganizationId)
        {
            await _belongsToValidator.ValidateStaffBelongsToOrganization(staffId, auth0OrganizationId);

            var staff = await _context.Staff.FindAsync(staffId);

            if (staff != null)
            {
                staff.IsActive = true;
                staff.Auth0Id = userAuth0Id;

                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<FacilityLiteModel>> GetFacilitiesForStaff(string auth0IdForStaff, string auth0OrganizationId)
        {
            var staff = await _context.Staff.Include(s => s.StaffFacilities).FirstOrDefaultAsync(s => s.Auth0Id == auth0IdForStaff);
            if (staff == null)
                throw new EntityNotFoundException($"Staff member with auth0 Id {auth0IdForStaff} was not found");

            await _belongsToValidator.ValidateStaffBelongsToOrganization(staff.StaffId, auth0OrganizationId);

            if (staff.StaffFacilities.IsNullOrEmpty())
                return new List<FacilityLiteModel>();

            var facilityIds = staff.StaffFacilities.Select(s => s.FacilityId).ToList();
            var facilities = _context.Facilities.Where(s => facilityIds.Contains(s.FacilityId));

            return (await facilities.Select(s => new FacilityLiteModel { FacilityId = s.FacilityId, FacilityName = s.FacilityName }).ToListAsync());
        }
    }
}
