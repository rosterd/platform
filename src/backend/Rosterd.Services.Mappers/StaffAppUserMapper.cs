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
        public static StaffAppUserPreferencesModel ToDomainModel(this RosterdAppUser dataModel) =>
            new StaffAppUserPreferencesModel
            {
                Email = dataModel.Email,
                DeviceId = dataModel.DeviceId,
                FirstName = dataModel.FirstName,
                MiddleName = dataModel.MiddleName,
                IdmUserName = dataModel.IdmUserName,
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
                Shift = new Shift {DayShift = dataModel.DayShiftOk, NightShift = dataModel.NightShiftOk}
            };

        public static RosterdAppUser ToDataModel(this StaffAppUserPreferencesModel domainModel) =>
            new RosterdAppUser(domainModel.Email)
            {
                Email = domainModel.Email,
                DeviceId = domainModel.DeviceId,
                FirstName = domainModel.FirstName,
                MiddleName = domainModel.MiddleName,
                IdmUserName = domainModel.IdmUserName,
                TurnAllNotificationsOff = domainModel.TurnAllNotificationsOff,
                MondayAvailable = domainModel.AvailableDays.Monday,
                TuesdayAvailable = domainModel.AvailableDays.Tuesday,
                WednesdayAvailable = domainModel.AvailableDays.Wednesday,
                ThursdayAvailable = domainModel.AvailableDays.Thursday,
                FridayAvailable = domainModel.AvailableDays.Friday,
                SaturdayAvailable = domainModel.AvailableDays.Saturday,
                SundayAvailable = domainModel.AvailableDays.Sunday,
                DayShiftOk = domainModel.Shift.DayShift,
                NightShiftOk = domainModel.Shift.NightShift
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
                Shift = new Shift {DayShift = true, NightShift = true},
                TurnAllNotificationsOff = false
            };
    }
}
