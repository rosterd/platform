using FluentValidation;
using Rosterd.Domain.Models.FacilitiesModels;

namespace Rosterd.Admin.Api.Requests.Facility
{
    public class UpdateFacilityRequest
    {
        public FacilityModel FacilityToUpdate { get; set; }
    }

    public class UpdateFacilityRequestValidator : AbstractValidator<UpdateFacilityRequest>
    {
        public UpdateFacilityRequestValidator()
        {
            RuleFor(s => s.FacilityToUpdate).NotNull();

            //Facility id should be present when updating a facility
            RuleFor(s => s.FacilityToUpdate.FacilityId).NotNull();
            RuleFor(s => s.FacilityToUpdate.FacilityId).GreaterThan(0);

            //This will be not be populated by us, will be taken from the JWT, should not be sent by the client
            RuleFor(s => s.FacilityToUpdate.Organization).Null();
        }
    }
}
