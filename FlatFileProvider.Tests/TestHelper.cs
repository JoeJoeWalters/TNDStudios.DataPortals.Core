using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using TNDStudios.DataPortals.Data;

namespace TNDStudios.DataPortals.Tests.FlatFile
{
    /// <summary>
    /// Support methods to load data from streams etc.
    /// </summary>
    public class TestHelper
    {
        // Constants for the test files (used so we can get the resouce stream
        // but also so we can abstract the creation of the data definition)
        public const String TestFile_DataTypes = "TestFiles.DataTypesTest.txt";
        public const String TestFile_Headers = "TestFiles.HeadersTest.txt";
        public const String TestFile_ISODates = "TestFiles.Dates.ISODates.txt";
        public const String TestFile_CustomDates = "TestFiles.Dates.CustomDates.txt";
        public const String TestFile_WriteTests = "TestFiles.WriteTest.txt";
        public const String TestFile_PKMergeFrom = "TestFiles.PrimaryKey.MergeFrom.txt";
        public const String TestFile_PKMergeTo = "TestFiles.PrimaryKey.MergeTo.txt";
        public const String TestFile_ExpressionTests = "TestFiles.ExpressionTest.txt";
        public const String TestFile_BigFileSalesRecords = "TestFiles.BigFiles.SalesRecords5000.csv";

        /// <summary>
        /// Generate the data set for the testing of different different types
        /// </summary>
        /// <param name="testDefinition">Which test file to load</param>
        /// <returns>The prepared data table</returns>
        public static DataTable PopulateDataTable(String testDefinition)
        {
            // Get the test data from the resource in the manifest
            Stream resourceStream = GetResourceStream(testDefinition);

            // Get the test definition (The columns, data types etc. for this file)
            DataItemDefinition definition = TestDefinition(testDefinition);

            // Create a new flat file provider
            IDataProvider provider = new FlatFileProvider()
            {
                TestMode = true // The provider should be marked as being in test mode
            };
            provider.Connect(definition, resourceStream); // Connect to the location of the data

            // Read the data from the provider
            DataTable data = provider.Read(""); // Get the data

            // Return the data table
            return data;
        }

        /// <summary>
        /// Generate a test definition that is common between the different test files
        /// </summary>
        /// <returns>The common data definition</returns>
        public static DataItemDefinition TestDefinition(String testDefinition)
        {
            // Arrage: Provide a definition of what wants to be retrieved from the flat file
            DataItemDefinition definition = new DataItemDefinition();

            switch (testDefinition)
            {
                case TestFile_DataTypes:

                    // Definition for different data types and the data defined by ordinal position
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Title", DataType = typeof(String), OridinalPosition = 0 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "CreatedDate", DataType = typeof(DateTime), OridinalPosition = 1 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Size", DataType = typeof(Double), OridinalPosition = 2 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Description", DataType = typeof(String), OridinalPosition = 3 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Active", DataType = typeof(Boolean), OridinalPosition = 4 });
                    definition.PropertyBag[DataItemPropertyBagItem.HasHeaderRecord.ToString()] = false;

                    break;

                case TestFile_Headers:

                    // Definition for getting the data by the name of the header
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Title", DataType = typeof(String), OridinalPosition = -1 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Description Header", DataType = typeof(String), OridinalPosition = -1 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Value", DataType = typeof(String), OridinalPosition = -1 });
                    definition.PropertyBag[DataItemPropertyBagItem.HasHeaderRecord.ToString()] = true;

                    break;

                case TestFile_ISODates:

                    // Definition for supplying a list of ISO (and bad) dates to test
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Date", DataType = typeof(DateTime), OridinalPosition = 0 });
                    definition.PropertyBag[DataItemPropertyBagItem.HasHeaderRecord.ToString()] = true;
                    definition.Culture = CultureInfo.InvariantCulture;

                    break;

                case TestFile_CustomDates:

                    // Definition for supplying a list of custom (and bad) dates to test
                    // where the format is defined as dd MMM yyyy
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Date", DataType = typeof(DateTime), OridinalPosition = 0, Pattern = "dd MMM yyyy" });
                    definition.PropertyBag[DataItemPropertyBagItem.HasHeaderRecord.ToString()] = true;
                    definition.Culture = CultureInfo.CurrentCulture;

                    break;

                case TestFile_WriteTests:

                    // Define lots of different data types to write to a file
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "StringValue", DataType = typeof(String), OridinalPosition = 0 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "DateValue", DataType = typeof(DateTime), OridinalPosition = 1, Pattern = "dd MMM yyyy" });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "BooleanValue", DataType = typeof(Boolean), OridinalPosition = 2 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "NumericValue", DataType = typeof(Double), OridinalPosition = 3 });
                    definition.PropertyBag[DataItemPropertyBagItem.HasHeaderRecord.ToString()] = true;
                    definition.Culture = CultureInfo.CurrentCulture;

                    break;

                case TestFile_PKMergeFrom:
                case TestFile_PKMergeTo:

                    // Define lots of different data types to write to a file
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Primary Key Part 1", DataType = typeof(String), OridinalPosition = 0, Key = true });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Primary Key Part 2", DataType = typeof(String), OridinalPosition = 1, Key = true });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Data", DataType = typeof(String), OridinalPosition = 2 });
                    definition.PropertyBag[DataItemPropertyBagItem.HasHeaderRecord.ToString()] = true;
                    definition.Culture = CultureInfo.CurrentCulture;

                    break;

                case TestFile_ExpressionTests:

                    // Define lots of different data types to write to a file
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Title", DataType = typeof(String), OridinalPosition = 0, Key = true });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Value", DataType = typeof(int), OridinalPosition = 1 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Multiplier", DataType = typeof(Double), OridinalPosition = 2 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Result", DataType = typeof(Double), OridinalPosition = 3, Calculation = "(Value * Multiplier)", PropertyType = DataItemPropertyType.Calculated });
                    definition.PropertyBag[DataItemPropertyBagItem.HasHeaderRecord.ToString()] = true;
                    definition.Culture = CultureInfo.CurrentCulture;

                    break;

                case TestFile_BigFileSalesRecords:

                    break;
            }

            // Return the definition
            return definition;
        }

        /// <summary>
        /// Build a memory stream from the embedded resource to feed to the test scenarios
        /// </summary>
        /// <param name="embeddedResourceName">The name of the resource to read</param>
        /// <returns>A memory stream with the data contained within</returns>
        public static Stream GetResourceStream(String embeddedResourceName)
        {
            String name = FormatResourceName(Assembly.GetExecutingAssembly(), embeddedResourceName);
            return Assembly.GetExecutingAssembly().GetManifestResourceStream(
                    name
                    );
        }

        /// <summary>
        /// Cast the stream of a given resource to a string to be passed to other methods
        /// </summary>
        /// <param name="embeddedResourceName">The name of the resource to read</param>
        /// <returns>A string representing the resource data</returns>
        public static String GetResourceString(String embeddedResourceName)
        {
            using (StreamReader reader = new StreamReader(GetResourceStream(embeddedResourceName)))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Get the resource name by deriving it from the assembly
        /// </summary>
        /// <param name="assembly">The assembly to check</param>
        /// <param name="resourceName">The name of the resource</param>
        /// <returns></returns>
        public static String FormatResourceName(Assembly assembly, string resourceName)
            => assembly.GetName().Name + "." + resourceName.Replace(" ", "_")
                                                            .Replace("\\", ".")
                                                            .Replace("/", ".");
    }
}
