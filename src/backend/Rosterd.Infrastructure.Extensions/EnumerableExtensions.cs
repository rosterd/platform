using System;
using System.Collections.Generic;
using System.Linq;

namespace Rosterd.Infrastructure.Extensions
{
    /// <summary>
    /// All helpful/extension methods to do with enumerable(s), lists and collections
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// When given an enumberable, will always return a properly instantiated List object.
        /// </summary>
        /// <typeparam name="T">The generic type of the list.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns>
        /// A list containing the objects in the array, or an empty list if the array was null.
        /// </returns>
        public static List<T> AlwaysList<T>(this IEnumerable<T> enumerable) => (enumerable ?? new List<T>()) as List<T> ?? new List<T>(enumerable ?? Array.Empty<T>());

        /// <summary>
        /// Converts a csv string into a list (if string is null or empty then an empty list is returned)
        /// </summary>
        /// <param name="csvString"></param>
        /// <returns></returns>
        public static List<long> ConvertCsvStringToList(this string csvString)
        {
            if (csvString.IsNullOrEmpty())
                return new List<long>();

            var splitEntries = csvString.Split(",");
            if (splitEntries.IsNullOrEmpty())
                return new List<long>();

            var list = splitEntries.Select(splitEntry => splitEntry.ToInt64()).AlwaysList();
            return list;
        }

        /// <summary>
        /// Converts a list to string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="itemList"></param>
        /// <returns></returns>
        public static string ToCsvString<T>(this List<T> itemList) => itemList.IsNullOrEmpty() ? string.Empty :  string.Join(",", itemList);
    }
}
