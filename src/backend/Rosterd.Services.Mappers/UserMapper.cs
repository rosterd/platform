using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rosterd.Data.TableStorage.Models;
using Rosterd.Domain.Models.Users;

namespace Rosterd.Services.Mappers
{
    public static class UserMapper
    {
        public static UserPreferencesModel ToDomainModel(this RosterdAppUser dataModel) =>
            new UserPreferencesModel
            {
                Email = dataModel.Email,
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

        public static RosterdAppUser ToDataModel(this UserPreferencesModel domainModel) =>
            new RosterdAppUser(domainModel.Email)
            {
                Email = domainModel.Email,
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

        public static UserPreferencesModel ToNew() =>
            new UserPreferencesModel
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
