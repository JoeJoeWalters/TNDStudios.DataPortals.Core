using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using TNDStudios.DataPortals.Data;

namespace TNDStudios.DataPortals.Tests.FixedWidthFile
{
    /// <summary>
    /// Support methods to load data from streams etc.
    /// </summary>
    public class TestHelper : EmbeddedTestHelperBase
    {
        // Constants for the test files (used so we can get the resouce stream
        // but also so we can abstract the creation of the data definition)
        public const String TestFile_GenericFixedWidth = "TestFiles.GenericFixedWidthFile.txt";

        /// <summary>
        /// Generate the data set for the testing of different different types
        /// </summary>
        /// <param name="testDefinition">Which test file to load</param>
        /// <returns>The prepared data table</returns>
        public DataTable PopulateDataTable(String testDefinition)
        {
            // Get the test data from the resource in the manifest
            Stream resourceStream = GetResourceStream(testDefinition);

            // Get the test definition (The columns, data types etc. for this file)
            DataItemDefinition definition = TestDefinition(testDefinition);

            // Create a new flat file provider
            IDataProvider provider = new FixedWidthFileProvider()
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
        public DataItemDefinition TestDefinition(String testDefinition)
        {
            // Arrage: Provide a definition of what wants to be retrieved from the flat file
            DataItemDefinition definition = new DataItemDefinition();

            switch (testDefinition)
            {
                case TestFile_GenericFixedWidth:

                    // Definition for different data types and the data defined by ordinal position
                    definition.Culture = new CultureInfo("en-US");
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Entry", DataType = typeof(Int32), OridinalPosition = 0, Size = 6 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Per", DataType = typeof(String), OridinalPosition = 8, Size = 4 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "PostDate", DataType = typeof(DateTime), OridinalPosition = 12, Size = 10 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "GL Account", DataType = typeof(String), OridinalPosition = 24, Size = 10 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Description", DataType = typeof(String), OridinalPosition = 37, Size = 26 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Src", DataType = typeof(String), OridinalPosition = 64, Size = 4 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Flow", DataType = typeof(Boolean), OridinalPosition = 69, Size = 3 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Ref", DataType = typeof(String), OridinalPosition = 73, Size = 8 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Post", DataType = typeof(Boolean), OridinalPosition = 83, Size = 3 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Debit", DataType = typeof(Decimal), OridinalPosition = 87, Size = 17 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Credit", DataType = typeof(Decimal), OridinalPosition = 104, Size = 20 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "All", DataType = typeof(Boolean), OridinalPosition = 130, Size = 3 });

                    // Calculated Properties
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Result", DataType = typeof(Double), OridinalPosition = 0, Size = 0,
                        PropertyType = DataItemPropertyType.Calculated, Calculation = "IIF(Post, Debit, 0)"
                    });

                    // Property bag items to define how the provider should handle custom settings
                    definition.PropertyBag[DataItemPropertyBagItem.HasHeaderRecord.ToString()] = true; // There is a header record
                    definition.PropertyBag[DataItemPropertyBagItem.RowsToSkip.ToString()] = 1; // .. but we also want to skip a record as it is a spacer

                    break;
            }

            // Return the definition
            return definition;
        }
    }
}
