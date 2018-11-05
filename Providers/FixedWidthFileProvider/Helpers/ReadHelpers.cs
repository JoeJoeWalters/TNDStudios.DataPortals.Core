using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using TNDStudios.DataPortals.Data;

namespace TNDStudios.DataPortals.Helpers
{
    public partial class FixedWidthFileHelper
    {
        /// <summary>
        /// Analyse some raw data and work out how 
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        public static DataItemDefinition AnalyseText(AnalyseRequest<String> request)
        {
            // Start with a blank definition
            DataItemDefinition result = new DataItemDefinition()
            {
                Culture = CultureInfo.InvariantCulture
            };
            Boolean ambigiousCulture = true;

            // Raw data has something to convert?
            if ((request.Data ?? "") != "")
            {
                // Open up a text reader to stream the data to the CSV Reader
                using (TextReader textReader = new StringReader(request.Data))
                {
                }
            }

            return result; // Send the definition back
        }

        /// <summary>
        /// Read the raw data file and populate the in-memory data table with it
        /// </summary>
        /// <param name="rawData">The raw flat file data from wherever it came from</param>
        /// <returns>If the translation was successful</returns>
        public static DataTable TextToDataTable(DataItemDefinition definition, String rawData)
        {
            // Create a list of data items to return
            DataTable dataItems = definition.ToDataTable();

            // Raw data has something to convert?
            if ((rawData ?? "") != "")
            {
                // Open up a text reader to stream the data to the CSV Reader
                using (TextReader textReader = new StringReader(rawData))
                {
                }
            }

            return dataItems; // Send the datatable back
        }
    }
}
