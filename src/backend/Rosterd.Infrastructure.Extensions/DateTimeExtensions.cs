using System;
using TimeZoneConverter;

namespace Rosterd.Infrastructure.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Converts UTC time to NZ Time
        /// </summary>
        /// <param name="value">This MUST be the UTC Datetime</param>
        /// <returns></returns>
        public static DateTime ToNzstFromUtc(this DateTime value) => TimeZoneInfo.ConvertTimeFromUtc(value, TZConvert.GetTimeZoneInfo("New Zealand Standard Time"));

        /// <summary>
        /// Converts any given datetime with timezone info to nz time
        /// </summary>
        /// <param name="value">This can be any datetime, it will be converted to utc datetime</param>
        /// <returns></returns>
        public static DateTime ToNzst(this DateTime value)
        {
            var utcDateTime = value.ToUniversalTime();
            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, TZConvert.GetTimeZoneInfo("New Zealand Standard Time"));
        }

        /// <summary>
        /// Converts any given datetime with NZ timezone to UTC time
        /// </summary>
        /// <param name="value">NZ datetime</param>
        /// <returns></returns>
        public static DateTime ToUtcFromNz(this DateTime value) => TimeZoneInfo.ConvertTimeToUtc(value, TZConvert.GetTimeZoneInfo("New Zealand Standard Time"));

        /// <summary>
        /// Checks if the given datetime falls in the given time rage (start time and end time)
        /// </summary>
        /// <param name="now">The datetime to compare against</param>
        /// <param name="start">The start time</param>
        /// <param name="end">The end time</param>
        /// <returns></returns>
        public static bool IsBetween(this DateTime now, TimeSpan start, TimeSpan end)
        {
            var nowTime = now.TimeOfDay;

            if (start <= end)
            {
                // start and stop times are in the same day
                if (nowTime >= start && nowTime <= end)
                {
                    // current time is between start and stop
                    return true;
                }
            }
            else
            {
                // start and stop times are in different days
                if (nowTime >= start || nowTime <= end)
                {
                    // current time is between start and stop
                    return true;
                }
            }

            return false;
        }
    }
}
