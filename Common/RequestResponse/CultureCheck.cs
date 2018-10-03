using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace TNDStudios.DataPortals.Helpers
{
    /// <summary>
    /// Response from a culture check request
    /// giving the culture information but also some additional info
    /// </summary>
    public class CultureCheck
    {
        /// <summary>
        /// The culture info for the data that was analysed
        /// </summary>
        public CultureInfo Culture { get; set; }

        /// <summary>
        /// If the result is open to interpretation such as date checking
        /// where the day and the month are both 12 or under, so may indicate
        /// a futher test may be required
        /// </summary>
        public Boolean AmbigiousResult { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public CultureCheck()
        {
            Culture = CultureInfo.InvariantCulture; // Default to "no" defined culture
            AmbigiousResult = false; // Not an ambiguous result by default
        }

    }
}
