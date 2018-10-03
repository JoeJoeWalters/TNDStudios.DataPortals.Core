using System;
using System.Collections.Generic;
using System.Text;

namespace TNDStudios.DataPortals.Helpers
{
    /// <summary>
    /// Request to analyse data from a given source
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AnalyseRequest<T>
    {
        // The raw data of a given type to be analysed
        public T Data { get; set; }

        /// <summary>
        /// Default constructor for the analysis request
        /// </summary>
        public AnalyseRequest()
        {
        }
    }
}
