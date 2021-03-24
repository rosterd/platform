using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rosterd.Domain.Models.SkillsModels;

namespace Rosterd.Services.Staff.Interfaces
{
    public interface IStaffSkillsService
    {
        /// <summary>
        /// Updates all the skills for a Staff (overwrites all the existing skills for a Staff)
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="skillModels"></param>
        /// <returns></returns>
        Task UpdateAllSkillsForStaff(long staffId, List<SkillModel> skillModels);

        /// <summary>
        /// Removes all existing skills for a Staff
        /// </summary>
        /// <param name="staffId"></param>
        /// <returns></returns>
        Task RemoveAllSkillsForStaff(long staffId);
    }
}
