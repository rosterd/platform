using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rosterd.Data.SqlServer.Context;
using Rosterd.Data.SqlServer.Models;
using Rosterd.Domain.Exceptions;

namespace Rosterd.Data.SqlServer.Extensions
{
    public static class OrganizationContextHelperExtensions
    {
        /// <summary>
        /// Gets a matching organization from our Rosterd database, if no organization is found then throws an exception
        /// </summary>
        /// <param name="context">The db context</param>
        /// <param name="auth0OrganizationId">The auth0 id for the organization</param>
        /// <returns>The matching organization if present otherwise an exception is thrown</returns>
        public static async Task<Organization> GetOrganization(this IRosterdDbContext context, string auth0OrganizationId)
        {
            var organization = await context.Organizations.FirstOrDefaultAsync(s => s.Auth0OrganizationId == auth0OrganizationId);
            if (organization == null)
                throw new EntityNotFoundException($"The given organization was not found, we don't have a matching organization with auth0 organization id {auth0OrganizationId}");

            return organization;
        }
    }
}
