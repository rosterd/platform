using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Rosterd.Domain.Models.FacilitiesModels;
using Rosterd.Web.Infra.ValidationAttributes;

namespace Rosterd.Admin.Api.Requests.Facility
{
    public class UpdateFacilityRequest
    {
        [ValidNumberRequired]
        public long FacilityId { get; set; }

        [Required]
        [StringLength(1000)]
        public string FacilityName { get; set; }

        [Required]
        [StringLength(1000)]
        public string Address { get; set; }

        [StringLength(1000)]
        public string Suburb { get; set; }

        [Required]
        [StringLength(1000)]
        public string City { get; set; }

        [Required]
        [StringLength(1000)]
        public string Country { get; set; }

        [Required]
        public decimal Latitude { get; set; }

        [Required]
        public decimal Longitude { get; set; }

        [Required]
        [StringLength(1000)]
        public string PhoneNumber1 { get; set; }

        [StringLength(1000)]
        public string PhoneNumber2 { get; set; }

        public FacilityModel ToFacilityModel() =>
            new FacilityModel
            {
                FacilityId = FacilityId,
                FacilityName = FacilityName,
                Address = Address,
                Suburb = Suburb,
                City = City,
                Country = Country,
                Latitude = Latitude,
                Longitude = Longitude,
                PhoneNumber1 = PhoneNumber1,
                PhoneNumber2 = PhoneNumber2
            };

    }
}
