using System;
using System.Collections.Generic;
using System.Text;

namespace TNDStudios.DataPortals.Helpers
{
    /// <summary>
    /// Extensions to base classes to perform common actions
    /// </summary>
    public static class BaseClassExtensions
    {
        /// <summary>
        /// Returns a shortened string value suitable for comparison to data types
        /// </summary>
        /// <param name="value">The type value we want to convert to a short form</param>
        /// <returns>The string value representing the short form</returns>
        public static String ToShortName(this Type value)
            => value.ToString().ToLower()
            .Replace("system.", "")
            .Replace("tndstudios.dataportals.data.", "");
    }
}
