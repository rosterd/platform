using FluentValidation;
using Rosterd.Domain.Models.OrganizationModels;

namespace Rosterd.Domain.Requests.Organization
{
    public class UpdateOrganizationRequest
    {
       public OrganizationModel Organization { get; set; }
    }

    public class UpdateOrganizationRequestValidator : AbstractValidator<UpdateOrganizationRequest>
    {
        public UpdateOrganizationRequestValidator()
        {
            RuleFor(s => s.Organization).NotNull();

            //Organization id should be present when updating, so we know which one to update
            RuleFor(s => s.Organization.OrganizationId).NotNull();
        }
    }
}
