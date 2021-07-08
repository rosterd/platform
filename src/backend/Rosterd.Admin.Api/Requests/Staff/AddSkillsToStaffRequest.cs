using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Rosterd.Domain.Models.FacilitiesModels;
using Rosterd.Domain.Models.SkillsModels;
using Rosterd.Domain.Models.StaffModels;
using Rosterd.Infrastructure.Extensions;
using Rosterd.Web.Infra.ValidationAttributes;

namespace Rosterd.Admin.Api.Requests.Staff
{
    public class AddSkillsToStaffRequest
    {
        [CollectionIsRequiredAndShouldNotBeEmpty]
        public List<long> SkillsToAdd { get; set; }

        public static List<SkillModel> ToSkillModels(AddSkillsToStaffRequest request) =>
            request.SkillsToAdd.AlwaysList().Select(s => new SkillModel {SkillId = s}).AlwaysList();
    }
}
