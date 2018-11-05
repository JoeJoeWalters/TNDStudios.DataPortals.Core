using System;
using System.Data;
using System.IO;
using System.Linq;
using TNDStudios.DataPortals.Data;

namespace TNDStudios.DataPortals.Helpers
{
    public partial class FixedWidthFileHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static String DataTableToString(DataItemDefinition definition, DataTable dataTable)
        {
            String result = ""; // New empty string to populate

            // Get the stream from the file
            using (MemoryStream textStream = new MemoryStream())
            {
            }

            return result; // Send the formatted flat file data back
        }        
    }
}
