using FluentValidation;
using Rosterd.Domain.Models.OrganizationModels;

namespace Rosterd.Domain.Requests.Organization
{
    public class AddOrganizationRequest
    {
       public OrganizationModel Organization { get; set; }
    }

    public class AddOrganizationRequestValidator : AbstractValidator<AddOrganizationRequest>
    {
        public AddOrganizationRequestValidator()
        {
            RuleFor(s => s.Organization).NotNull();

            //Organization id should be null when creating, this is auto-assigned by us as the primary key
            RuleFor(s => s.Organization.OrganizationId).Null();

            RuleFor(s => s.Organization.TenantId).NotNull();
        }
    }
}
