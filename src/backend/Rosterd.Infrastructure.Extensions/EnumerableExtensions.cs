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
        /// When given an enumerable, will always return a properly instantiated List object.
        /// </summary>
        /// <typeparam name="T">The generic type of the list.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns>
        /// A list containing the objects in the array, or an empty list if the array was null.
        /// </returns>
        public static List<T> AlwaysList<T>(this IEnumerable<T> enumerable) => (enumerable ?? new List<T>()) as List<T> ?? new List<T>(enumerable ?? Array.Empty<T>());

        /// <summary>
        /// Converts a list to string to a delimited string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="itemList"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string ToDelimitedString<T>(this List<T> itemList, string separator = ",") => itemList.IsNullOrEmpty() ? string.Empty :  string.Join(separator, itemList);
    }
}
