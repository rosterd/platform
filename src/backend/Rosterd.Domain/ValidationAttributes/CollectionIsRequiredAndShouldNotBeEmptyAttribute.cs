using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Rosterd.Domain.ValidationAttributes
{
    /// <summary>
    ///     Validation attribute to indicate that a property field or parameter is required, it should be a number and should be greater than zero
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class CollectionIsRequiredAndShouldNotBeEmptyAttribute : ValidationAttribute
    {
        /// <summary>
        ///     Validation attribute to indicate that a property field or parameter is required, it should be a number and should be greater than zero
        /// </summary>
        /// <param name="value">The integer value of the selection</param>
        /// <returns>True if value is greater than zero</returns>
        public override bool IsValid(object value) =>
            // return true if value is a non-null number > 0, otherwise return false
            value != null && (value as ICollection).IsNotNull() && ((ICollection) value).Count > 0;
    }
}
