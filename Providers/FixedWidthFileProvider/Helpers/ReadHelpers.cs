using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using TNDStudios.DataPortals.Data;
using TNDStudios.DataPortals.PropertyBag;

namespace TNDStudios.DataPortals.Helpers
{
    public partial class FixedWidthFileHelper
    {
        /// <summary>
        /// Read the raw data file and populate the in-memory data table with it
        /// </summary>
        /// <param name="rawData">The raw flat file data from wherever it came from</param>
        /// <returns>If the translation was successful</returns>
        public static DataTable TextToDataTable(DataItemDefinition definition, DataConnection connection, String rawData)
        {
            // Create a list of data items to return
            DataTable dataItems = definition.ToDataTable();

            // Create a helper to read the property bag items
            PropertyBagHelper propertyBagHelper = new PropertyBagHelper(connection);

            // Raw data has something to convert?
            if ((rawData ?? "") != "")
            {
                // Open up a text reader to stream the data to the CSV Reader
                using (TextReader textReader = new StringReader(rawData))
                {
                    // Get properties needed to process the file (total lines to skip etc.)
                    Int32 lineNo = 0;
                    Int32 linesToSkip =
                        propertyBagHelper.Get<Int32>(PropertyBagItemTypeEnum.RowsToSkip, 0) +
                        (propertyBagHelper.Get<Boolean>(PropertyBagItemTypeEnum.HasHeaderRecord, false) ? 1 : 0);
                    
                    // Loop each line of the file (ignoring lines that do not need to be processed)
                    String line = "";
                    while ((line = textReader.ReadLine()) != null)
                    {
                        // Is this a line we should be processing
                        if (lineNo >= linesToSkip)
                        {
                            DataRow row = dataItems.NewRow(); // The new row to populate based on the defintion

                            // Process the row
                            if (ProcessRow(line, row, definition))
                            {
                                dataItems.Rows.Add(row); // Add the row if processing was successful
                            }
                        }

                        lineNo++; // Increment the line number counter
                    }
                }
            }

            return dataItems; // Send the datatable back
        }

        /// <summary>
        /// Process a line using a defintion to get a data row
        /// </summary>
        /// <param name="line"></param>
        /// <param name="definition"></param>
        /// <returns></returns>
        public static Boolean ProcessRow(String line, DataRow row, DataItemDefinition definition)
        {
            Boolean result = true; // Define the default result as a success

            // Loop the properties that are not calculated items etc.
            definition.ItemProperties
                .Where(item => item.PropertyType == DataItemPropertyType.Property)
                .ToList()
                .ForEach(item => 
            {
                // Get the raw value from the line by position and length
                String rawValue = CutString(line, item.OrdinalPosition, item.Size);
                if (rawValue != null)
                {
                    // Cast the data to the appropriate type using the common rule set
                    Object value = DataFormatHelper.ReadData(rawValue, item, definition);
                    row[item.Name] = value;
                }
            });

            // Return the result
            return result;
        }

        /// <summary>
        /// Cut a segment out of the string if it can to then process by another method
        /// </summary>
        /// <returns></returns>
        public static String CutString(String origional, Int32 start, Int32 length)
        {
            String result = null; // By default nothing is found, so make it null

            // Make sure the cut is in bounds
            if (origional.Length >= (start + length))
                result = origional.Substring(start, length);

            // Return the result
            return result;
        }
    }
}
