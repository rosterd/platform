using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Rosterd.Data.SqlServer.Models;

namespace Rosterd.Infrastructure.Security.Interfaces
{
    public interface IBelongsToValidator
    {
        // <summary>
        /// Checks to see if the facility belongs to the organization.
        /// Throws an entitynotfound exception if the facility does not belong to the organization
        /// </summary>
        /// <param name="facilityId">the facility id</param>
        /// <param name="auth0OrganizationId">The auth0 organization id</param>
        /// <returns></returns>
        Task<Organization> ValidateFacilityBelongsToOrganization(long facilityId, string auth0OrganizationId);

        // <summary>
        /// Checks to see if the facilities belongs to the organization.
        /// Throws an entitynotfound exception if the facility does not belong to the organization
        /// </summary>
        /// <param name="facilityIds"></param>
        /// <param name="auth0OrganizationId">The auth0 organization id</param>
        /// <returns></returns>
        Task ValidateFacilitiesBelongsToOrganization(List<long> facilityIds, string auth0OrganizationId);

        /// <summary>
        /// Checks to see if the staff belongs to the organization.
        /// Throws an entitynotfound exception if the staff does not belong to the organization
        /// </summary>
        /// <param name="staffId">the staff id</param>
        /// <param name="auth0OrganizationId">The auth0 organization id</param>
        /// <returns></returns>
        Task ValidateStaffBelongsToOrganization(long staffId, string auth0OrganizationId);

        Task ValidateSkillBelongsToOrganization(long skillId, string auth0OrganizationId);

        Task ValidateSkillsBelongsToOrganization(List<long> skillIds, string auth0OrganizationId);

        Task<Organization> ValidateOrganizationExistsAndGetIfValid(string auth0OrganizationId);

        Task<bool> DoesFacilityAdminHaveAccessToFacility(long facilityId, string auth0IdForStaff);
    }
}
