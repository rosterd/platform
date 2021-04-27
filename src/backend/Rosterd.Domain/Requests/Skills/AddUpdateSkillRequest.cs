using Rosterd.Domain.Models.SkillsModels;

namespace Rosterd.Domain.Requests.Skills
{
    public class AddUpdateSkillRequest
    {
        public SkillModel SkillToAddOrUpdate { get; set; }
    }
    
}
