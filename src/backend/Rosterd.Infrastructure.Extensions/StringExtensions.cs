using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;

namespace Rosterd.Infrastructure.Extensions
{
    /// <summary>
    /// All helpful/extension methods for strings
    /// </summary>
    public static class StringExtensions
    {
        public static string GetTheFirstXCharsOrEmpty(this string str, int countOfCharsToGet)
        {
            var firstX = str.IsNotNullOrEmpty() && str.Length > 5 ?
                str[..5] :
                str;

            return firstX;
        }
    }
}
