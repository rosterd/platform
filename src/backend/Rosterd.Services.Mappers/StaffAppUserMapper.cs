using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rosterd.Data.TableStorage.Models;
using Rosterd.Domain.Models.StaffModels;

namespace Rosterd.Services.Mappers
{
    public static class StaffAppUserMapper
    {
        public static StaffAppUserPreferencesModel ToDomainModel(this RosterdAppUserPreferences dataModel) =>
            new StaffAppUserPreferencesModel
            {
                DeviceId = dataModel.DeviceId,
                FirstName = dataModel.FirstName,
                MiddleName = dataModel.MiddleName,
                TurnAllNotificationsOff = dataModel.TurnAllNotificationsOff,
                AvailableDays = new AvailableDays
                {
                    Monday = dataModel.MondayAvailable,
                    Tuesday = dataModel.TuesdayAvailable,
                    Wednesday = dataModel.WednesdayAvailable,
                    Thursday = dataModel.ThursdayAvailable,
                    Friday = dataModel.FridayAvailable,
                    Saturday = dataModel.SaturdayAvailable,
                    Sunday = dataModel.SundayAvailable
                },
                Shift = new Shift {NightShift = dataModel.NightShiftOk},
            };

        public static RosterdAppUserPreferences ToDataModel(this StaffAppUserPreferencesModel domainModel, string userAuth0Id) =>
            new RosterdAppUserPreferences(userAuth0Id)
            {
                DeviceId = domainModel.DeviceId,
                FirstName = domainModel.FirstName,
                MiddleName = domainModel.MiddleName,
                TurnAllNotificationsOff = domainModel.TurnAllNotificationsOff,
                MondayAvailable = domainModel.AvailableDays.Monday,
                TuesdayAvailable = domainModel.AvailableDays.Tuesday,
                WednesdayAvailable = domainModel.AvailableDays.Wednesday,
                ThursdayAvailable = domainModel.AvailableDays.Thursday,
                FridayAvailable = domainModel.AvailableDays.Friday,
                SaturdayAvailable = domainModel.AvailableDays.Saturday,
                SundayAvailable = domainModel.AvailableDays.Sunday,
                NightShiftOk = domainModel.Shift.NightShift,
                City = domainModel.City
            };

        public static StaffAppUserPreferencesModel ToNew() =>
            new StaffAppUserPreferencesModel
            {
                AvailableDays = new AvailableDays
                {
                    Monday = true,
                    Tuesday = true,
                    Wednesday = true,
                    Thursday = true,
                    Friday = true,
                    Saturday = true,
                    Sunday = true
                },
                Shift = new Shift {NightShift = true},
                TurnAllNotificationsOff = false
            };
    }
}
