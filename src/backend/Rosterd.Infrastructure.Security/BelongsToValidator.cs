using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rosterd.Data.SqlServer.Context;
using Rosterd.Data.SqlServer.Models;
using Rosterd.Domain.Exceptions;
using Rosterd.Infrastructure.Security.Interfaces;

namespace Rosterd.Infrastructure.Security
{
    public class BelongsToValidator : IBelongsToValidator
    {
        private readonly IRosterdDbContext _context;

        public BelongsToValidator(IRosterdDbContext context) => _context = context;

        ///<inheritdoc/>
        public async Task ValidateFacilityBelongsToOrganization(long facilityId, string auth0OrganizationId)
        {
            var organization = await ValidateOrganizationExistsAndGetIfValid(auth0OrganizationId);

            var facility = await _context.Facilities.FirstOrDefaultAsync(s => s.FacilityId == facilityId && s.OrganzationId == organization.OrganizationId);
            if (facility == null)
                throw new EntityNotFoundException($"Facility {facilityId} does not belong to organization {organization.OrganizationId}");
        }

        ///<inheritdoc/>
        public async Task ValidateStaffBelongsToOrganization(long staffId, string auth0OrganizationId)
        {
            var organization = await ValidateOrganizationExistsAndGetIfValid(auth0OrganizationId);

            var staff = await _context.Staff.FirstOrDefaultAsync(s => s.StaffId == staffId && s.OrganizationId == organization.OrganizationId);
            if (staff == null)
                throw new EntityNotFoundException($"Staff {staffId} does not belong to organization {organization.OrganizationId}");
        }

        ///<inheritdoc/>
        public async Task<Organization> ValidateOrganizationExistsAndGetIfValid(string auth0OrganizationId)
        {
            //TODO: Cache this in memory, as we wont have that many organizations and we can do a restart of the app if we need at a new one for now.
            var organization = await _context.Organizations.FirstOrDefaultAsync(s => s.Auth0OrganizationId == auth0OrganizationId);
            if (organization == null)
                throw new EntityNotFoundException($"The given organization was not found, we don't have a matching organization with auth0 organization id {auth0OrganizationId}");

            return organization;
        }
    }
}
