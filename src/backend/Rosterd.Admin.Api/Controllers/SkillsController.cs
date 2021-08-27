using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rosterd.Admin.Api.Requests.Skills;
using Rosterd.Admin.Api.Services;
using Rosterd.Domain;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.SkillsModels;
using Rosterd.Domain.Settings;
using Rosterd.Services.Skills.Interfaces;
using Rosterd.Web.Infra.Filters.Swagger;
using Rosterd.Web.Infra.Security;
using Rosterd.Web.Infra.ValidationAttributes;
using PagingQueryStringParameters = Rosterd.Domain.Models.PagingQueryStringParameters;

namespace Rosterd.Admin.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "Skills")]
    [AuthorizeByRole(RosterdConstants.RosterdRoleNames.FacilityAdmin, RosterdConstants.RosterdRoleNames.OrganizationAdmin, RosterdConstants.RosterdRoleNames.RosterdAdmin)]
    public class SkillsController : BaseApiController
    {
        private readonly ILogger<SkillsController> _logger;
        private readonly ISkillsService _skillService;
        private readonly IUserContext _userContext;

        public SkillsController(ILogger<SkillsController> logger, ISkillsService skillService, IOptions<AppSettings> appSettings, IUserContext userContext) : base(appSettings)
        {
            _logger = logger;
            _skillService = skillService;
            _userContext = userContext;
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
            var pagedList = await _skillService.GetAllSkills(pagingParameters, _userContext.UsersAuth0OrganizationId);

            return pagedList;
        }

        /// <summary>
        /// Get Skill by Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{skillId}")]
        [OperationOrderAttribute(2)]
        public async Task<ActionResult<SkillModel>> GetSkillById([ValidNumberRequired] long? skillId)
        {
            var skillModel = await _skillService.GetSkill(skillId.Value, _userContext.UsersAuth0OrganizationId);
            return skillModel == null ? NotFound() : skillModel;
        }

        /// <summary>
        /// Adds a new Skill
        /// </summary>
        /// <param name="request">The Skill to add</param>
        /// <returns></returns>
        [HttpPost]
        [OperationOrderAttribute(3)]
        public async Task<ActionResult> AddNewSkill([FromBody] AddSkillRequest request)
        {
            await _skillService.CreateSkill(request.ToSkillModel(), _userContext.UsersAuth0OrganizationId);
            return Ok();
        }

        /// <summary>
        /// Update a Skill
        /// </summary>
        /// <param name="request">The Skill to update</param>
        /// <returns></returns>
        [HttpPut]
        [OperationOrderAttribute(4)]
        public async Task<ActionResult> UpdateSkill([FromBody] UpdateSkillRequest request)
        {
            await _skillService.UpdateSkill(request.ToSkillModel(), _userContext.UsersAuth0OrganizationId);
            return Ok();
        }


        /// <summary>
        /// Deletes Skill
        /// </summary>
        /// <param name="skillId">The Skill to be deleted</param>
        /// <returns></returns>
        [HttpDelete("{skillId}")]
        [OperationOrderAttribute(5)]
        public async Task<ActionResult> RemoveSkill([ValidNumberRequired] long? skillId)
        {
            await _skillService.RemoveSkill(skillId.Value, _userContext.UsersAuth0OrganizationId);
            return Ok();
        }
    }
}
