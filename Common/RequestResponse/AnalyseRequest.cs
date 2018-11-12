using System;
using System.Collections.Generic;
using System.Text;
using TNDStudios.DataPortals.Data;

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

        // The data connection to be analysed
        public DataConnection Connection { get; set; }

        // How many (max) items to sample to gather the information about
        // the data set
        public Int32 SampleSize { get; set; }

        /// <summary>
        /// Default constructor for the analysis request
        /// </summary>
        public AnalyseRequest()
        {
            SampleSize = 10; // By default, Sample the first X items 
            Connection = null; // No connection by default
        }
    }
}
