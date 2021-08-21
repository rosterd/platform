using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Rosterd.Domain.Models.OrganizationModels;

namespace Rosterd.Admin.Api.Requests.Organization
{
    public class AddOrganizationRequest
    {
        [Required]
        [StringLength(1000)]
        public string OrganizationName { get; set; }

        [StringLength(1000)]
        public string Phone { get; set; }

        [StringLength(1000)]
        public string Address { get; set; }

        [StringLength(1000)]
        public string Comments { get; set; }

        public OrganizationModel ToOrganizationModel() =>
            new OrganizationModel()
            {
                OrganizationName = OrganizationName,
                Phone = Phone,
                Address = Address,
                Comments = Comments,
                IsActive = true //default is true when creating
            };
    }
}
