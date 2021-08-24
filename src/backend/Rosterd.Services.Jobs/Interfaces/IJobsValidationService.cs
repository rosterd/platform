using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;

namespace Rosterd.Services.Jobs.Interfaces
{
    public interface IJobsValidationService
    {
        /// <summary>
        /// Runs all the validation checks required before accepting a job
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        Task<(bool isJobValid, IEnumerable<string> errorMessages)> IsJobStillValidToAccept(long jobId);

        /// <summary>
        /// Runs all the validation checks required before a staff member can cancel a job
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="staff"></param>
        /// <returns></returns>
        Task<(bool isJobValid, IEnumerable<string> errorMessages)> IsJobStillValidToCancelForStaff(long jobId, long staff);

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
