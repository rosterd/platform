using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.SkillsModels;

namespace Rosterd.Services.Resources.Interfaces
{
    public interface ISkillService
    {
        Task<PagedList<SkillModel>> GetSkills(PagingQueryStringParameters pagingParameters);
        Task<SkillModel> GetSkillById(long skillId);
        Task<IActionResult> PostSkill(SkillModel skillModel);
        Task<IActionResult> DeleteSkill(long skillId);
    }
}
