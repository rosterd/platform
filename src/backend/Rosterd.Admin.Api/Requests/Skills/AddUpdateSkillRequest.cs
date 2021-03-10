using Rosterd.Domain.Models.SkillsModels;

namespace Rosterd.Admin.Api.Requests.Skills
{
    public class AddUpdateSkillRequest
    {
        public SkillModel SkillToAddOrUpdate { get; set; }
    }
    
}
