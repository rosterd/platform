using FluentValidation;
using Rosterd.Domain.Models.FacilitiesModels;
using Rosterd.Domain.Requests.Job;

namespace Rosterd.Domain.Requests.Facility
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

            //This will be populated by us by whats in the JWT, should not be sent by the client
            RuleFor(s => s.FacilityToUpdate.Organization).Null();
        }
    }
}
