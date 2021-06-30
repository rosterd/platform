using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rosterd.Admin.Api.Requests.Skills;
using Rosterd.Domain;
using Rosterd.Domain.Models.SkillsModels;
using Rosterd.Services.Skills.Interfaces;
using Rosterd.Web.Infra.Filters.Swagger;
using PagingQueryStringParameters = Rosterd.Domain.Models.PagingQueryStringParameters;

namespace Rosterd.Admin.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "Skills")]
    public class SkillsController : BaseApiController
    {
        private readonly ILogger<SkillsController> _logger;
        private readonly ISkillsService _skillService;

        public SkillsController(ILogger<SkillsController> logger, ISkillsService skillService, IOptions<AppSettings> appSettings) : base(appSettings)
        {
            _logger = logger;
            _skillService = skillService;
        }

        /// <summary>
        /// Gets all the resources 
        /// </summary>
        /// <param name="pagingParameters"></param>
        /// <returns></returns>
        [HttpGet]
        [OperationOrder(1)]
        public async Task<ActionResult<Domain.Models.PagedList<SkillModel>>> GetAllSkills([FromQuery] PagingQueryStringParameters pagingParameters)
        {
            pagingParameters ??= new PagingQueryStringParameters();
            Domain.Models.PagedList<SkillModel> pagedList;

            pagedList = await _skillService.GetAllSkills(pagingParameters);

            return pagedList;
        }

        /// <summary>
        /// Get Skill by Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        [OperationOrderAttribute(2)]
        public async Task<ActionResult<SkillModel>> GetSkillById([Required] long? id)
        {
            var skillModel = await _skillService.GetSkill(id.Value);
            return skillModel;
        }

        /// <summary>
        /// Adds a new Skill
        /// </summary>
        /// <param name="request">The Skill to add</param>
        /// <returns></returns>
        [HttpPost]
        [OperationOrderAttribute(3)]
        public async Task<ActionResult> AddNewSkill([FromBody] AddUpdateSkillRequest request)
        {
            await _skillService.CreateSkill(request.SkillToAddOrUpdate);
            return Ok();
        }

        /// <summary>
        /// Update a Skill
        /// </summary>
        /// <param name="request">The Skill to update</param>
        /// <returns></returns>
        [HttpPut]
        [OperationOrderAttribute(4)]
        public async Task<ActionResult> UpdateSkill([FromBody] AddUpdateSkillRequest request)
        {
            await _skillService.UpdateSkill(request.SkillToAddOrUpdate);
            return Ok();
        }


        /// <summary>
        /// Deletes Skill
        /// </summary>
        /// <param name="skillId">The Skill to be deleted</param>
        /// <returns></returns>
        [HttpDelete]
        [OperationOrderAttribute(5)]
        public async Task<ActionResult> RemoveSkill([FromQuery][Required] long? skillId)
        {
            await _skillService.RemoveSkill(skillId.Value);
            return Ok();
        }

    }
}
