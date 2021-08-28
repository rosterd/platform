using System.Threading.Tasks;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.SkillsModels;

namespace Rosterd.Services.Skills.Interfaces
{
    public interface ISkillsService
    {
        /// <summary>
        /// Gets all the skills
        /// </summary>
        /// <param name="pagingParameters"></param>
        /// <param name="auth0OrganizationId"></param>
        /// <returns></returns>
        Task<PagedList<SkillModel>> GetAllSkills(PagingQueryStringParameters pagingParameters, string auth0OrganizationId);

        /// <summary>
        /// Gets a specific skill
        /// </summary>
        /// <param name="skillId"></param>
        /// <param name="auth0OrganizationId"></param>
        /// <returns></returns>
        Task<SkillModel> GetSkill(long skillId, string auth0OrganizationId);

        /// <summary>
        /// Adds a new skill
        /// </summary>
        /// <param name="skillModel"></param>
        /// <param name="auth0OrganizationId"></param>
        /// <returns></returns>
        Task<SkillModel> CreateSkill(SkillModel skillModel, string auth0OrganizationId);

        /// <summary>
        /// Updates an existing skill
        /// </summary>
        /// <param name="skillModel"></param>
        /// <param name="auth0OrganizationId"></param>
        /// <returns></returns>
        Task<SkillModel> UpdateSkill(SkillModel skillModel, string auth0OrganizationId);

        /// <summary>
        /// Deletes skill
        /// </summary>
        /// <param name="skillId"></param>
        /// <param name="auth0OrganizationId"></param>
        /// <returns></returns>
        Task RemoveSkill(long skillId, string auth0OrganizationId);
    }
}
