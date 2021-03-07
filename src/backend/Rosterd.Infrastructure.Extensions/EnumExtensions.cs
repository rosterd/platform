using System;

namespace Rosterd.Infrastructure.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Coverts a given string to a enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this string value) => (T)Enum.Parse(typeof(T), value, true);
    }
}
