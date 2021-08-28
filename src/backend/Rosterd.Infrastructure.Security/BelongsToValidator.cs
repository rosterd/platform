using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Rosterd.Data.SqlServer.Context;
using Rosterd.Data.SqlServer.Models;
using Rosterd.Domain.Exceptions;
using Rosterd.Infrastructure.Security.Interfaces;

namespace Rosterd.Infrastructure.Security
{
    public class BelongsToValidator : IBelongsToValidator
    {
        private readonly IRosterdDbContext _context;
        private readonly IMemoryCache _cache;

        public BelongsToValidator(IRosterdDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

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
        public async Task ValidateSkillBelongsToOrganization(long skillId, string auth0OrganizationId)
        {
            var organization = await ValidateOrganizationExistsAndGetIfValid(auth0OrganizationId);

            var skill = await _context.Skills.FirstOrDefaultAsync(s => s.SkillId == skillId && s.OrganizationId == organization.OrganizationId);
            if (skill == null)
                throw new EntityNotFoundException($"Skill {skillId} does not belong to organization {organization.OrganizationId}");
        }

        ///<inheritdoc/>
        public async Task<Organization> ValidateOrganizationExistsAndGetIfValid(string auth0OrganizationId)
        {
            if (!_cache.TryGetValue(auth0OrganizationId, out var cacheEntry))
            {
                var organization = await _context.Organizations.FirstOrDefaultAsync(s => s.Auth0OrganizationId == auth0OrganizationId);

                //Save data in cache.
                cacheEntry = organization ?? throw new EntityNotFoundException($"The given organization was not found, we don't have a matching organization with auth0 organization id {auth0OrganizationId}");
                _cache.Set(auth0OrganizationId, organization);
            }

            return cacheEntry as Organization;
        }
    }
}
