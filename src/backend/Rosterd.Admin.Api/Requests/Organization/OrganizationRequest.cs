using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Rosterd.Domain.Models.OrganizationModels;

namespace Rosterd.Admin.Api.Requests.Organization
{
    public class GetOrganizationRequest
    {
        [StringLength(1000)]
        public string OrganizationName { get; set; }

        public long? OrganizationId { get; set; }
    }
}
