using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.Resources;
using Rosterd.Services.Resources.Interfaces;

namespace Rosterd.Admin.Api.Controllers
{
    [ApiVersion("1.0")]
    public class SkillsController: BaseApiController
    {
        private readonly ILogger<SkillsController> _logger;
        private readonly ISkillService _skillService;

        public SkillsController(ILogger<SkillsController> logger, ISkillService skillService) : base()
        {
            _logger = logger;
            _skillService = skillService;
        }

        /// <summary>
        /// Gets all the resources 
        /// </summary>
        /// <param name="id">the resource id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PagedList<SkillModel>> Get() =>
            (await _skillService.GetSkills(new PagingQueryStringParameters()));

        [HttpGet("{skillId:long}")]
        public async Task<SkillModel> GetSkillById(long skillId) =>
     (await _skillService.GetSkillById(skillId));

        [HttpDelete("{skillId:long}")]
        public async Task<IActionResult> DeleteSkill(long skillId) =>
            (await _skillService.DeleteSkill(skillId));


        [HttpPost]
        public async Task<IActionResult> CreateSkill(SkillModel skillModel) =>
            (await _skillService.PostSkill(skillModel));
    }
}
