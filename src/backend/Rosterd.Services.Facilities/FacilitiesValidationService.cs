using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rosterd.Data.SqlServer.Context;
using Rosterd.Data.SqlServer.Extensions;
using Rosterd.Domain.Exceptions;
using Rosterd.Services.Facilities.Interfaces;

namespace Rosterd.Services.Facilities
{
    public class FacilitiesValidationService : IFacilitiesValidationService
    {
        private readonly IRosterdDbContext _context;

        public FacilitiesValidationService(IRosterdDbContext context) => _context = context;

        /// <summary>
        /// Checks to see if the facility belongs to the organization.
        /// Throws an entitynotfound exception if the facility does not belong to the organization
        /// </summary>
        /// <param name="facilityId">the facility id</param>
        /// <param name="auth0OrganizationId">The auth0 organization id</param>
        /// <returns></returns>
        public async Task ValidateFacilityBelongsToOrganization(long facilityId, string auth0OrganizationId)
        {
            var organization = await _context.GetOrganization(auth0OrganizationId);

            var facility = await _context.Facilities.FirstOrDefaultAsync(s => s.FacilityId == facilityId && s.OrganzationId == organization.OrganizationId);
            if (facility == null)
                throw new EntityNotFoundException($"Facility {facilityId} does not belong to organization {organization.OrganizationId}");
        }
    }
}
