using Rosterd.Domain.Models.FacilitiesModels;
using FluentValidation;


namespace Rosterd.Domain.Requests.Facility
{
    public class AddFacilityRequest
    {
        public FacilityModel FacilityToAdd { get; set; }
    }

    public class AddFacilityRequestValidator : AbstractValidator<AddFacilityRequest>
    {
        public AddFacilityRequestValidator()
        {
            RuleFor(s => s.FacilityToAdd).NotNull();

            //Facility id should be null when creating a facility, this is auto-assigned by us as the primary key
            RuleFor(s => s.FacilityToAdd.FacilityId).Null();

            //This will be populated by us by whats in the JWT, should not be sent by the client
            RuleFor(s => s.FacilityToAdd.Organization).Null();
        }
    }
}
