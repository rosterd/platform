using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.SkillsModels;
using Rosterd.Services.Skills.Interfaces;

namespace Rosterd.Admin.Api.Controllers
{
    [ApiVersion("1.0")]
    public class SkillsController
    {
        private readonly ILogger<SkillsController> _logger;
        private readonly ISkillsService _skillService;

        public SkillsController(ILogger<SkillsController> logger, ISkillsService skillService) : base()
        {
            _logger = logger;
            _skillService = skillService;
        }
    }
}
