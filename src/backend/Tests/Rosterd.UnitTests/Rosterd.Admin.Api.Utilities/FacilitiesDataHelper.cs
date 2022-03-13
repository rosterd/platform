using Rosterd.Data.SqlServer.Context;
using Rosterd.Data.SqlServer.Models;

namespace Rosterd.UnitTests.Rosterd.Admin.Api.Utilities
{
    public class FacilitiesDataHelper
    {
        public static void ArrangeFacilitiesTestData(IRosterdDbContext context)
        {
            var facility1 = new Facility
            {
                FacilityId = 1,
                OrganzationId = 1,
                FacilityName = "Everyl Orr",
                Address = "63 Allendale Rd",
                Suburb = "Mt Albert",
                City = "Auckland",
                PhoneNumber1 = "09-1234567",
                IsActive = true
            };
            var facility2 = new Facility
            {
                FacilityId = 2,
                OrganzationId = 1,
                FacilityName = "The Sands",
                Address = "9 Bayview Road",
                Suburb = "Browns Bay",
                City = "Auckland",
                PhoneNumber1 = "09-2345678",
                IsActive = true
            };
            var facility3 = new Facility
            {
                FacilityId = 3,
                OrganzationId = 1,
                FacilityName = "Wesley",
                Address = "227 Mount Eden Rd",
                Suburb = "Mt Eden",
                City = "Auckland",
                PhoneNumber1 = "09-33456789",
                IsActive = true
            };
            var facility4 = new Facility
            {
                FacilityId = 4,
                OrganzationId = 1,
                FacilityName = "Amberwood Care Centre",
                Address = "499 Don Buck Rd",
                Suburb = "Massey",
                City = "Auckland",
                PhoneNumber1 = "09-43456789",
                IsActive = true
            };
            var facility5 = new Facility {
                FacilityId = 5,
                OrganzationId = 1,
                FacilityName = "Meadowbank Care Centre",
                Address = "148 Meadowbank Rd",
                Suburb = "Meadowbank",
                City = "Auckland",
                PhoneNumber1 = "09-53456789",
                IsActive = true
            };
            var facility6 = new Facility {
                FacilityId = 6,
                OrganzationId = 1,
                FacilityName = "Everyl Orr Old",
                Address = "63 Allendale Rd",
                Suburb = "Mt Albert",
                City = "Auckland",
                PhoneNumber1 = "09-1234567",
                IsActive = false
            };

            context.Facilities.Add(facility1);
            context.Facilities.Add(facility2);
            context.Facilities.Add(facility3);
            context.Facilities.Add(facility4);
            context.Facilities.Add(facility5);
            context.Facilities.Add(facility6);

            context.SaveChanges();
        }
    }
}
