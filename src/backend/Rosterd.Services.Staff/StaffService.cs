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
using Rosterd.Domain.Exceptions;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.FacilitiesModels;
using Rosterd.Domain.Models.StaffModels;
using Rosterd.Infrastructure.Extensions;
using Rosterd.Services.Mappers;
using Rosterd.Services.Staff.Interfaces;


namespace Rosterd.Services.Staff
{
    public class StaffService : IStaffService
    {
        private readonly IRosterdDbContext _context;
        private readonly IAzureTableStorage _azureTableStorage;

        public StaffService(IRosterdDbContext context, IAzureTableStorage azureTableStorage)
        {
            _context = context;
            _azureTableStorage = azureTableStorage;
        }

        ///<inheritdoc/>
        public async Task<PagedList<StaffModel>> GetStaffForFacility(PagingQueryStringParameters pagingParameters, long facilityId)
        {
            var query = _context.Staff
                .Where(staff => staff.StaffFacilities.Any(facility => facility.FacilityId == facilityId))
                .Include(s => s.StaffFacilities)
                .Include(s => s.StaffSkills);

            var pagedList = await PagingList<Data.SqlServer.Models.Staff>.ToPagingList(query, pagingParameters.PageNumber, pagingParameters.PageSize);

            var domainModels = pagedList.ToDomainModels();
            return new PagedList<StaffModel>(domainModels, pagedList.TotalCount, pagedList.CurrentPage, pagedList.PageSize, pagedList.TotalPages);
        }

        ///<inheritdoc/>
        public async Task<PagedList<StaffModel>> GetAllStaff(PagingQueryStringParameters pagingParameters)
        {
            var query = _context.Staff;
            var pagedList = await PagingList<Data.SqlServer.Models.Staff>.ToPagingList(query, pagingParameters.PageNumber, pagingParameters.PageSize);

            var domainModels = pagedList.ToDomainModels();
            return new PagedList<StaffModel>(domainModels, pagedList.TotalCount, pagedList.CurrentPage, pagedList.PageSize, pagedList.TotalPages);
        }

        ///<inheritdoc/>
        public async Task<StaffModel> GetStaff(long staffId)
        {
            var staff = await _context.Staff
                .Include(s => s.StaffFacilities)
                .Include(s => s.StaffSkills)
                .FirstOrDefaultAsync(s => s.StaffId == staffId);

            if(staff == null)
                throw new EntityNotFoundException();

            return staff.ToDomainModel();
        }

        ///<inheritdoc/>
        public async Task<StaffModel> CreateStaff(StaffModel staffModel)
        {
            //Populate staff details
            var staffToCreate = staffModel.ToNewStaff();
            var newStaff = await _context.Staff.AddAsync(staffToCreate);

            //Populate the default staff facilities
            if (staffModel.StaffFacilities.IsNotNullOrEmpty())
            {
                var facilityIds = staffModel.StaffFacilities.Select(s => s.FacilityId.Value).AlwaysList();
                var facilitiesFromDb = _context.StaffFacilities.Where(s => facilityIds.Contains(s.FacilityId));

                foreach (var facility in facilitiesFromDb)
                {
                    newStaff.Entity.StaffFacilities.Add(new StaffFacility
                    {
                        FacilityId = facility.FacilityId,
                        FacilityName = facility.FacilityName,
                        StaffId = newStaff.Entity.StaffId
                    });
                }
            }

            //Populate the default staff skills
            if (staffModel.StaffSkills.IsNotNullOrEmpty())
            {
                var skillIds = staffModel.StaffSkills.Select(s => s.SkillId).AlwaysList();
                var skillsFromDb = _context.Skills.Where(s => skillIds.Contains(s.SkillId));

                foreach (var skill in skillsFromDb)
                {
                    newStaff.Entity.StaffSkills.Add(new StaffSkill { SkillId = skill.SkillId, SkillName = skill.SkillName, StaffId = newStaff.Entity.StaffId });
                }
            }

            await _context.SaveChangesAsync();
            return newStaff.Entity.ToDomainModel();
        }

        ///<inheritdoc/>
        public async Task<StaffModel> UpdateStaff(StaffModel staffModel)
        {
            //Get the existing staff
            var staffFromDb = await _context.Staff
                .Include(s => s.StaffSkills)
                .Include(s => s.StaffFacilities)
                .FirstOrDefaultAsync(s => s.StaffId == staffModel.StaffId.Value);

            if (staffFromDb == null)
                throw new EntityNotFoundException();

            //Update the db entry with the latest updates from the domain model
            var staffModelToUpdate = staffModel.ToDataModel(staffFromDb);
            _context.Staff.Update(staffModelToUpdate);

            await _context.SaveChangesAsync();
            return staffModelToUpdate.ToDomainModel();
        }

        ///<inheritdoc/>
        public async Task UpdateStaffToInactive(long staffId)
        {
            var staff = await _context.Staff.FindAsync(staffId);

            if (staff != null)
            {
                staff.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }

        ///<inheritdoc/>
        public async Task UpdateStaffToActive(long staffId)
        {
            var staff = await _context.Staff.FindAsync(staffId);

            if (staff != null)
            {
                staff.IsActive = true;
                await _context.SaveChangesAsync();
            }
        }

        ///<inheritdoc/>
        public async Task DeleteAllExistingFacilitiesForStaffAndAddNew(long staffId, List<FacilityModel> facilityModels)
        {
            //Remove all existing
            var existingStaffFacilities = _context.StaffFacilities.Where(s => s.StaffId == staffId);
            _context.StaffFacilities.RemoveRange(existingStaffFacilities);

            //Add the new ones
            var newFacilityIds = facilityModels.AlwaysList().Select(s => s.FacilityId.Value);
            var newFacilities = _context.StaffFacilities.Where(s => newFacilityIds.Contains(s.FacilityId));
            foreach (var facility in newFacilities.AlwaysList())
            {
                await _context.StaffFacilities.AddAsync(new StaffFacility
                {
                    FacilityId = facility.FacilityId, FacilityName = facility.FacilityName, StaffId = staffId
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task AddFacilityToStaff(long staffId, long facilityId)
        {
            var facility = await _context.Facilities.FindAsync(facilityId);
            if (facility != null)
            {
                var existingStaffFacility = await _context.StaffFacilities.FirstOrDefaultAsync(s => s.StaffId == staffId && s.FacilityId == facilityId);
                if (existingStaffFacility != null) //There is already this facility added for the staff no need to do anything
                    return;

                await _context.StaffFacilities.AddAsync(new StaffFacility
                {
                    FacilityId = facility.FacilityId, FacilityName = facility.FacilityName, StaffId = staffId
                });

                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveFacilityFromStaff(long staffId, long facilityId)
        {
            var staffFacility = await _context.StaffFacilities.FirstOrDefaultAsync(s => s.FacilityId == facilityId && s.StaffId == staffId);
            if (staffFacility != null)
            {
                _context.StaffFacilities.Remove(staffFacility);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<StaffPreferencesModel> GetStaffPreferences(string userEmail)
        {
            var rosterdAppUser = await _azureTableStorage.GetAsync<RosterdAppUser>(RosterdAppUser.TableName, RosterdAppUser.UsersPartitionKey, userEmail);

            //We don't have the user in our db, so default the preferences which is true for every thing
            if (rosterdAppUser == null)
                return UserMapper.ToNew();

            return rosterdAppUser?.ToDomainModel();
        }

        public async Task UpdateStaffPreferences(StaffPreferencesModel staffPreferencesModel)
        {
            var rosterdAppUser = staffPreferencesModel.ToDataModel();

            await _azureTableStorage.AddOrUpdateAsync(RosterdAppUser.TableName, rosterdAppUser);
        }
    }
}
