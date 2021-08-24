using System.Threading.Tasks;

namespace Rosterd.Services.Facilities.Interfaces
{
    public interface IFacilitiesValidationService
    {
        /// <summary>
        /// Checks to see if the facility belongs to the organization.
        /// Throws an entitynotfound exception if the facility does not belong to the organization
        /// </summary>
        /// <param name="facilityId">the facility id</param>
        /// <param name="auth0OrganizationId">The auth0 organization id</param>
        /// <returns></returns>
        Task ValidateFacilityBelongsToOrganization(long facilityId, string auth0OrganizationId);
    }
}
