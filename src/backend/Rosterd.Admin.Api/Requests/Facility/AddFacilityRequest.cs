using System.Data;
using FluentValidation;
using Rosterd.Domain.Models.FacilitiesModels;
using Rosterd.Services.Facilities.Interfaces;

namespace Rosterd.Admin.Api.Requests.Facility
{
    public class AddFacilityRequest
    {
        public FacilityModel FacilityToAdd { get; set; }
    }

    public class AddFacilityRequestValidator : AbstractValidator<AddFacilityRequest>
    {

        public AddFacilityRequestValidator(IFacilitiesService facilitiesService)
        {
            RuleFor(s => s.FacilityToAdd).NotNull();

            //Facility id should be null when creating a facility, this is auto-assigned by us as the primary key
            RuleFor(s => s.FacilityToAdd.FacilityId).Null();

            //This will be populated by us by whats in the JWT, should not be sent by the client
            RuleFor(s => s.FacilityToAdd.Organization).Null();
        }
    }
}
