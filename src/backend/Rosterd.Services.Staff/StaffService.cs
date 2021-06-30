using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rosterd.Data.SqlServer.Context;
using Rosterd.Data.SqlServer.Helpers;
using Rosterd.Data.SqlServer.Models;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.StaffModels;
using Rosterd.Services.Mappers;
using Rosterd.Services.Staff.Interfaces;


namespace Rosterd.Services.Staff
{
    public class StaffService : IStaffService
    {
        private readonly IRosterdDbContext _context;

        public StaffService(IRosterdDbContext context) => _context = context;

        ///<inheritdoc/>
        public async Task<PagedList<StaffModel>> GetStaffForFacility(PagingQueryStringParameters pagingParameters, long facilityId)
        {
            var query = _context.Staff
                .Where(s => s.IsActive == true)
                .Where(s => s.StaffFacilities.Any(s => s.FacilityId == facilityId))
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
            var staff = await _context.Staff.FindAsync(staffId);
            return staff?.ToDomainModel();
        }

        ///<inheritdoc/>
        public async Task<long> CreateStaffMember(StaffModel staffModel)
        {
            var staffToCreate = staffModel.ToNewStaff();

            var newStaff = await _context.Staff.AddAsync(staffToCreate);
            await _context.SaveChangesAsync();

            return newStaff.Entity.StaffId;
        }

        ///<inheritdoc/>
        public async Task UpdateStaffMember(StaffModel staffModel)
        {
            var staffModelToUpdate = staffModel.ToDataModel();

            _context.Staff.Update(staffModelToUpdate);
            await _context.SaveChangesAsync();
        }

        ///<inheritdoc/>
        public async Task RemoveStaffMember(long staffId)
        {
            var staff = await _context.Staff.FindAsync(staffId);
            if (staff != null)
            {
                staff.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }

        ///<inheritdoc/>
        public async Task MoveStaffMemberToAnotherFacility(long staffId, long facilityId)
        {
            var staff = await _context.Staff.FindAsync(staffId);
            var facility = await _context.Facilities.FindAsync(facilityId);
            if (staff != null && facility != null)
            {
                //Remove the existing facility connection to Staff
                var toDelete = _context.StaffFacilities.Where(s => s.StaffId == staffId);
                _context.StaffFacilities.RemoveRange(toDelete);

                //Add the new facility connection
                await _context.StaffFacilities.AddAsync(new StaffFacility {FacilityId = facilityId, FacilityName = facility.FacilityName, StaffId = staffId});

                //Finally commit to db
                await _context.SaveChangesAsync();
            }
        }
    }
}
