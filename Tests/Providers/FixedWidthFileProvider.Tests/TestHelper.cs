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
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Title", DataType = typeof(String), OridinalPosition = 0 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "CreatedDate", DataType = typeof(DateTime), OridinalPosition = 1 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Size", DataType = typeof(Double), OridinalPosition = 2 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Description", DataType = typeof(String), OridinalPosition = 3 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Active", DataType = typeof(Boolean), OridinalPosition = 4 });
                    definition.PropertyBag[DataItemPropertyBagItem.HasHeaderRecord.ToString()] = false;

                    break;
            }

            // Return the definition
            return definition;
        }
    }
}
