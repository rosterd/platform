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
        /// <returns></returns>
        Task<PagedList<SkillModel>> GetAllSkills(PagingQueryStringParameters pagingParameters);

        /// <summary>
        /// Gets a specific skill
        /// </summary>
        /// <param name="skillId"></param>
        /// <returns></returns>
        Task<SkillModel> GetSkill(long skillId);

        /// <summary>
        /// Adds a new skill
        /// </summary>
        /// <param name="skillModel"></param>
        /// <returns></returns>
        Task CreateSkill(SkillModel skillModel);

        /// <summary>
        /// Updates an existing skill
        /// </summary>
        /// <param name="skillModel"></param>
        /// <returns></returns>
        Task UpdateSkill(SkillModel skillModel);

        /// <summary>
        /// Deletes skill
        /// </summary>
        /// <param name="skillId"></param>
        /// <returns></returns>
        Task RemoveSkill(long skillId);
    }
}
