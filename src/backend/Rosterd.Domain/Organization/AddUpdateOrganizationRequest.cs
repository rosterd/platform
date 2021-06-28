using Rosterd.Domain.Models.OrganizationModels;

namespace Rosterd.Domain.Organization
{
    public class AddUpdateOrganizationRequest
    {
       public OrganizationModel OrganizationToAddOrUpdate { get; set; }
    }
}
