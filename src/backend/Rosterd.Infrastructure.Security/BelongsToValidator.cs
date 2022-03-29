using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Caching.Memory;
using Rosterd.Data.SqlServer.Context;
using Rosterd.Data.SqlServer.Models;
using Rosterd.Domain.Exceptions;
using Rosterd.Infrastructure.Extensions;
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
        public async Task<Organization> ValidateFacilityBelongsToOrganization(long facilityId, string auth0OrganizationId)
        {
            var organization = await ValidateOrganizationExistsAndGetIfValid(auth0OrganizationId);

            if (!_cache.TryGetValue(facilityId, out var cacheEntry))
            {
                var facility = await _context.Facilities.FirstOrDefaultAsync(s => s.FacilityId == facilityId && s.OrganzationId == organization.OrganizationId);
                if (facility == null)
                    throw new EntityNotFoundException($"Facility {facilityId} does not belong to organization {organization.OrganizationId}");

                cacheEntry = auth0OrganizationId;
                _cache.Set(facilityId, auth0OrganizationId);
            }

            //This means facility is there but does not belong to the organization
            if (cacheEntry.ToString() != auth0OrganizationId)
                throw new EntityNotFoundException($"Facility {facilityId} does not belong to organization {organization.OrganizationId}");

            return organization;
        }

        ///<inheritdoc/>
        public async Task ValidateFacilitiesBelongsToOrganization(List<long> facilityIds, string auth0OrganizationId)
        {
            foreach (var facilityId in facilityIds.AlwaysList())
                await ValidateFacilityBelongsToOrganization(facilityId, auth0OrganizationId);
        }

        ///<inheritdoc/>
        public async Task ValidateStaffBelongsToOrganization(long staffId, string auth0OrganizationId)
        {
            var organization = await ValidateOrganizationExistsAndGetIfValid(auth0OrganizationId);

            if (!_cache.TryGetValue(staffId, out var cacheEntry))
            {
                var staff = await _context.Staff.FirstOrDefaultAsync(s => s.StaffId == staffId && s.OrganizationId == organization.OrganizationId);
                if (staff == null)
                    throw new EntityNotFoundException($"Staff {staffId} does not belong to organization {organization.OrganizationId}");

                cacheEntry = auth0OrganizationId;
                _cache.Set(staffId, auth0OrganizationId);
            }

            //This means staff is there but does not belong to the organization
            if (cacheEntry.ToString() != auth0OrganizationId)
                throw new EntityNotFoundException($"Staff {staffId} does not belong to organization {organization.OrganizationId}");
        }

        ///<inheritdoc/>
        public async Task ValidateSkillBelongsToOrganization(long skillId, string auth0OrganizationId)
        {
            var organization = await ValidateOrganizationExistsAndGetIfValid(auth0OrganizationId);

            if (!_cache.TryGetValue(skillId, out var cacheEntry))
            {
                var skill = await _context.Skills.FirstOrDefaultAsync(s => s.SkillId == skillId && s.OrganizationId == organization.OrganizationId);
                if (skill == null)
                    throw new EntityNotFoundException($"Skill {skillId} does not belong to organization {organization.OrganizationId}");

                cacheEntry = auth0OrganizationId;
                _cache.Set(skillId, auth0OrganizationId);
            }

            //This means skill is there but does not belong to the organization
            if(cacheEntry.ToString() != auth0OrganizationId)
                throw new EntityNotFoundException($"Skill {skillId} does not belong to organization {organization.OrganizationId}");
        }

        ///<inheritdoc/>
        public async Task ValidateSkillsBelongsToOrganization(List<long> skillIds, string auth0OrganizationId)
        {
            foreach (var skillId in skillIds.AlwaysList())
                await ValidateSkillBelongsToOrganization(skillId, auth0OrganizationId);
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

        public async Task<bool> DoesFacilityAdminHaveAccessToFacility(long facilityId, string auth0IdForStaff)
        {
            var staff = await _context.Staff.FirstOrDefaultAsync(s => s.Auth0Id == auth0IdForStaff);
            if (staff == null)
                throw new EntityNotFoundException($"Staff with auth0 id {auth0IdForStaff} not found");

            var staffFacility = await _context.StaffFacilities.FirstOrDefaultAsync(s => s.StaffId == staff.StaffId && s.FacilityId == facilityId);
            return staffFacility != null;
        }
    }
}
