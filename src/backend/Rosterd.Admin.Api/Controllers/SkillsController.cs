using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rosterd.Admin.Api.Infrastructure.Filters.Swagger;
using Rosterd.Admin.Api.Requests.Skills;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.SkillsModels;
using Rosterd.Services.Skills.Interfaces;

namespace Rosterd.Admin.Api.Controllers
{
    [ApiVersion("1.0")]
    public class SkillsController : BaseApiController
    {
        private readonly ILogger<SkillsController> _logger;
        private readonly ISkillsService _skillService;

        public SkillsController(ILogger<SkillsController> logger, ISkillsService skillService) : base()
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
        [OperationOrderAttribute(1)]
        public async Task<ActionResult<PagedList<SkillModel>>> GetAllSkills([FromQuery] PagingQueryStringParameters pagingParameters)
        {
            pagingParameters ??= new PagingQueryStringParameters();
            PagedList<SkillModel> pagedList;

            pagedList = await _skillService.GetAllSkills(pagingParameters);

            return pagedList;
        }

        /// Get Skill by Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        [OperationOrderAttribute(2)]
        public async Task<ActionResult<SkillModel>> GetSkillById(string? id)
        {
            var skillModel = await _skillService.GetSkill(long.Parse(id));
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
