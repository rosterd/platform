using System.Threading.Tasks;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.OrganizationModels;

namespace Rosterd.Services.Organizations.Interfaces
{
    public interface IOrganizationsService
    {
        Task<PagedList<OrganizationModel>> GetAllOrganizations(PagingQueryStringParameters pagingParameters, bool activeOrganizationsOnly);

        /// <summary>
        /// Gets a specific organization
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        Task<OrganizationModel> GetOrganization(long organizationId);

        /// <summary>
        /// Gets a matching organization with the name
        /// </summary>
        /// <param name="organizationName"></param>
        /// <returns></returns>
        Task<OrganizationModel> GetOrganization(string organizationName);

        /// <summary>
        /// Adds a new organization
        /// </summary>
        /// <param name="organizationModel"></param>
        /// <returns></returns>
        Task<OrganizationModel> CreateOrganization(OrganizationModel organizationModel);

        /// <summary>
        /// Deletes organization
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        Task RemoveOrganization(long organizationId);
    }

}
