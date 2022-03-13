using System;
using Rosterd.Domain.Models.StaffModels;

namespace Rosterd.UnitTests.Rosterd.Client.Api.Utilities
{
    public class PreferencesModelDataHelper
    {
        public static StaffAppUserPreferencesModel createStaffAppUserPreferencesModel()
        {
            var random = new Random().Next();
            var staffAppUserPreferencesModel = new StaffAppUserPreferencesModel
            {
                FirstName = "Number " + random,
                LastName = "Test",
                MiddleName = "Middle",
                DeviceId = "xiomi" + random,
                City = "Auckland",
                AvailableDays = new AvailableDays {Saturday = true, Sunday = true},
                Shift = new Shift {NightShift = true}
            };
            return staffAppUserPreferencesModel;
        }
    }
}
