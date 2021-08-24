using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rosterd.Domain.Models.SkillsModels;

namespace Rosterd.Services.Staff.Interfaces
{
    public interface IStaffValidationService
    {
        /// <summary>
        /// Checks to see if the staff belongs to the organization.
        /// Throws an entitynotfound exception if the staff does not belong to the organization
        /// </summary>
        /// <param name="staffId">the staff id</param>
        /// <param name="auth0OrganizationId">The auth0 organization id</param>
        /// <returns></returns>
        Task ValidateStaffBelongsToOrganization(long staffId, string auth0OrganizationId);
    }
}
