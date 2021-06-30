using System.Data;
using FluentValidation;
using Rosterd.Domain.Models.FacilitiesModels;

namespace Rosterd.Admin.Api.Requests.Facility
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

            //This will be not be populated by us, will be taken from the JWT, should not be sent by the client
            RuleFor(s => s.FacilityToAdd.Organization).Null();
        }
    }
}
