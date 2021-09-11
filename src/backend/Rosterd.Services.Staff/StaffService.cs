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
using Rosterd.Infrastructure.Extensions;
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

        public StaffService(IRosterdDbContext context, IAzureTableStorage azureTableStorage, IBelongsToValidator belongsToValidator)
        {
            _context = context;
            _azureTableStorage = azureTableStorage;
            _belongsToValidator = belongsToValidator;
        }

        ///<inheritdoc/>
        public async Task<PagedList<StaffModel>> GetAllStaff(PagingQueryStringParameters pagingParameters, string auth0OrganizationId)
        {
            var organization = await _belongsToValidator.ValidateOrganizationExistsAndGetIfValid(auth0OrganizationId);

            var query = _context.Staff.Where(s => s.OrganizationId == organization.OrganizationId);
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
                throw new EntityNotFoundException();

            return staff.ToDomainModel();
        }

        ///<inheritdoc/>
        public async Task<StaffModel> CreateStaff(StaffModel staffModel, string auth0OrganizationId)
        {
            if (staffModel.Auth0Id.IsNullOrEmpty())
                throw new Auth0IdNotSetException();

            var organization = await _belongsToValidator.ValidateOrganizationExistsAndGetIfValid(auth0OrganizationId);
            await _belongsToValidator.ValidateSkillsBelongsToOrganization(staffModel.StaffSkills.AlwaysList().Select(s => s.SkillId).AlwaysList(), auth0OrganizationId);

            if (staffModel.StaffFacilities.IsNotNullOrEmpty())
                await _belongsToValidator.ValidateFacilitiesBelongsToOrganization(staffModel.StaffFacilities.Select(s => s.FacilityId).ToList(), auth0OrganizationId);

            //Populate staff details
            var staffToCreate = staffModel.ToNewStaff();
            staffToCreate.OrganizationId = organization.OrganizationId;
            var newStaff = await _context.Staff.AddAsync(staffToCreate);

            //Populate the default staff skills
            var skillIds = staffModel.StaffSkills.Select(s => s.SkillId).AlwaysList();
            var skillsFromDb = _context.Skills.Where(s => skillIds.Contains(s.SkillId));
            foreach (var skill in skillsFromDb.AlwaysList())
            {
                newStaff.Entity.StaffSkills.Add(new StaffSkill { SkillId = skill.SkillId, SkillName = skill.SkillName, StaffId = newStaff.Entity.StaffId });
            }

            //Populate any facilities for the staff (for a facility admin they need to be associated with a facility)
            foreach (var staffFacility in staffModel.StaffFacilities.AlwaysList())
            {
                newStaff.Entity.StaffFacilities.Add(new StaffFacility { FacilityId = staffFacility.FacilityId, StaffId = newStaff.Entity.StaffId });
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
                throw new EntityNotFoundException();

            //Update the db entry with the latest updates from the domain model
            var staffModelToUpdate = staffModel.ToDataModel(staffFromDb);
            _context.Staff.Update(staffModelToUpdate);

            await _context.SaveChangesAsync();
            return staffModelToUpdate.ToDomainModel();
        }

        ///<inheritdoc/>
        public async Task<StaffModel> UpdateStaffToInactive(long staffId, string auth0OrganizationId)
        {
            await _belongsToValidator.ValidateStaffBelongsToOrganization(staffId, auth0OrganizationId);

            var staff = await _context.Staff.FindAsync(staffId);

            if (staff != null)
            {
                staff.IsActive = false;
                staff.Auth0Id = RosterdConstants.Users.UserRemovedFromAuth0;

                await _context.SaveChangesAsync();
                return staff.ToDomainModel();
            }

            return null;
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

        public async Task<StaffAppUserPreferencesModel> GetStaffAppUserPreferences(string userEmail)
        {
            var rosterdAppUser = await _azureTableStorage.GetAsync<RosterdAppUser>(RosterdAppUser.TableName, RosterdAppUser.UsersPartitionKey, userEmail);

            //We don't have the user in our db, so default the preferences which is true for every thing
            if (rosterdAppUser == null)
                return StaffAppUserMapper.ToNew();

            return rosterdAppUser?.ToDomainModel();
        }

        public async Task UpdateStaffAppUserPreferences(StaffAppUserPreferencesModel staffAppUserPreferencesModel)
        {
            var rosterdAppUser = staffAppUserPreferencesModel.ToDataModel();

            await _azureTableStorage.AddOrUpdateAsync(RosterdAppUser.TableName, rosterdAppUser);
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
