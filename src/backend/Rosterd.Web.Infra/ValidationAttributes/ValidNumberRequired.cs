using System;
using System.ComponentModel.DataAnnotations;

namespace Rosterd.Web.Infra.ValidationAttributes
{
    /// <summary>
    ///     Validation attribute to indicate that a property field or parameter is required, it should be a number and should be greater than zero
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class ValidNumberRequired : ValidationAttribute
    {
        /// <summary>
        ///     Validation attribute to indicate that a property field or parameter is required, it should be a number and should be greater than zero
        /// </summary>
        /// <param name="value">The integer value of the selection</param>
        /// <returns>True if value is greater than zero</returns>
        public override bool IsValid(object value) =>
            // return true if value is a non-null number > 0, otherwise return false
            value != null && long.TryParse(value.ToString(), out var i) && i > 0;
    }
}
