using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rosterd.Data.SqlServer.Context;
using Rosterd.Data.SqlServer.Extensions;
using Rosterd.Data.SqlServer.Models;
using Rosterd.Domain.Exceptions;
using Rosterd.Domain.Models.SkillsModels;
using Rosterd.Services.Staff.Interfaces;

namespace Rosterd.Services.Staff
{
    public class StaffValidationService : IStaffValidationService
    {
        private readonly IRosterdDbContext _context;

        public StaffValidationService(IRosterdDbContext context) => _context = context;

        ///<inheritdoc/>
        public async Task ValidateStaffBelongsToOrganization(long staffId, string auth0OrganizationId)
        {
            var organization = await _context.GetOrganization(auth0OrganizationId);

            var staff = await _context.Staff.FirstOrDefaultAsync(s => s.StaffId == staffId && s.OrganizationId == organization.OrganizationId);
            if (staff == null)
                throw new EntityNotFoundException($"Staff {staffId} does not belong to organization {organization.OrganizationId}");
        }
    }
}
