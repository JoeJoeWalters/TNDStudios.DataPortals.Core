using CsvHelper;
using System;
using System.Data;
using System.IO;
using System.Linq;
using TNDStudios.DataPortals.Data;
using TNDStudios.DataPortals.PropertyBag;

namespace TNDStudios.DataPortals.Helpers
{
    public partial class DelimitedFileHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static String DataTableToString(DataItemDefinition definition, DataTable dataTable)
        {
            String result = ""; // New empty string to populate
            
            // Create a helper to read the property bag items
            PropertyBagHelper propertyBagHelper = new PropertyBagHelper(definition.PropertyBag);

            // Get the stream from the file
            using (MemoryStream textStream = new MemoryStream())
            {
                // Set up the writer
                StreamWriter streamWriter = new StreamWriter(textStream);
                using (CsvWriter writer = SetupWriter(definition, streamWriter))
                {
                    // Do we need to write a header?
                    if (propertyBagHelper.GetPropertyBagItem<Boolean>(PropertyBagItemTypeEnum.HasHeaderRecord, false))
                    {
                        // Loop the header records and output the header record line manually
                        definition.ItemProperties
                            .Where(prop => prop.PropertyType == DataItemPropertyType.Property)
                            .ToList()
                            .ForEach(
                                header =>
                                {
                                    writer.WriteField(header.Name);
                                });

                        // Move to the next line and flush the data
                        writer.NextRecord();
                        streamWriter.Flush();
                    }

                    // Loop through the actual records and add them to the csv
                    foreach (DataRow row in dataTable.Rows)
                    {
                        // Loop the header records and output the header record line manually
                        definition.ItemProperties
                            .Where(prop => prop.PropertyType == DataItemPropertyType.Property)
                            .ToList()
                            .ForEach(
                                property =>
                                {
                                    writer.WriteField(DataFormatHelper.WriteData(row[property.Name], property, definition), property.Quoted);
                                });

                        // Move to the next line and flush the data
                        writer.NextRecord();
                        streamWriter.Flush();
                    }

                    // Put the data back in the buffer
                    textStream.Position = 0;
                    result = ((new StreamReader(textStream)).ReadToEnd() ?? "");
                }
            }

            return result; // Send the formatted flat file data back
        }

        /// <summary>
        /// Set up a new csv writer based on the definition given
        /// </summary>
        /// <param name="definition">The data item definition</param>
        /// <returns></returns>
        private static CsvWriter SetupWriter(DataItemDefinition definition, TextWriter textWriter)
        {
            // Create the new writer
            CsvWriter writer = new CsvWriter(textWriter);

            // Create a helper to read the property bag items
            PropertyBagHelper propertyBagHelper = new PropertyBagHelper(definition.PropertyBag);

            // Force all fields to be quoted or not
            writer.Configuration.QuoteAllFields =
                propertyBagHelper.GetPropertyBagItem<Boolean>(PropertyBagItemTypeEnum.QuoteAllFields, false);

            return writer;
        }
    }
}
