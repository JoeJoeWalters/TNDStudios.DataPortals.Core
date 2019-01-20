using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using TNDStudios.DataPortals.Data;
using TNDStudios.DataPortals.PropertyBag;

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
        public const String TestFile_MergeData = "TestFiles.MergeFile.txt";
        public const String TestFile_DataTypes = "TestFiles.DataTypes.txt";

        /// <summary>
        /// Get a test connection for use with the readers
        /// </summary>
        /// <returns>A test connection</returns>
        public DataConnection TestConnection()
        {
            DataConnection connection = new DataConnection()
            {
                ProviderType = DataProviderType.FixedWidthFileProvider,
                LastUpdated = DateTime.Now,
                Id = Guid.NewGuid(),
                PropertyBag =
                        (new FixedWidthFileProvider()).PropertyBagTypes()
                        .Select(type =>
                            new PropertyBagItem()
                            {
                                Value = type.DefaultValue,
                                ItemType = type
                            })
                        .ToList()
            };

            // Create a property bag helper to set some defaults
            PropertyBagHelper propertyBagHelper = new PropertyBagHelper(connection);
            propertyBagHelper.Set<Boolean>(PropertyBagItemTypeEnum.HasHeaderRecord, true);
            propertyBagHelper.Set<Int32>(PropertyBagItemTypeEnum.RowsToSkip, 1);

            return connection;
        }

        /// <summary>
        /// Generate the data set for the testing of different different types
        /// </summary>
        /// <param name="testDefinition">Which test file to load</param>
        /// <returns>The prepared data table</returns>
        public DataTable PopulateDataTable(String testDefinition)
            => PopulateDataTable(testDefinition, TestConnection());
        public DataTable PopulateDataTable(String testDefinition, DataConnection connection)
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
            provider.Connect(definition, connection, resourceStream); // Connect to the location of the data

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
                case TestFile_DataTypes:

                    // Definition for different data types and the data defined by ordinal position
                    definition.Culture = CultureInfo.InvariantCulture;
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "BooleanType", DataType = typeof(Boolean), OrdinalPosition = 0, Size = 16 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "DateType", DataType = typeof(DateTime), OrdinalPosition = 16, Size = 15 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "NumericType", DataType = typeof(Double), OrdinalPosition = 32, Size = 15 });

                    break;

                case TestFile_MergeData:

                    // Definition for different data types and the data defined by ordinal position
                    definition.Culture = new CultureInfo("en-US");
                    definition.ItemProperties.Add(new DataItemProperty() { Key = true, Name = "Entry", DataType = typeof(Int32), OrdinalPosition = 0, Size = 6 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Per", DataType = typeof(String), OrdinalPosition = 8, Size = 4 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "PostDate", DataType = typeof(DateTime), OrdinalPosition = 12, Size = 10 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "GL Account", DataType = typeof(String), OrdinalPosition = 24, Size = 10 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Description", DataType = typeof(String), OrdinalPosition = 37, Size = 26 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Src", DataType = typeof(String), OrdinalPosition = 64, Size = 4 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Flow", DataType = typeof(Boolean), OrdinalPosition = 69, Size = 3 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Ref", DataType = typeof(String), OrdinalPosition = 73, Size = 8 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Post", DataType = typeof(Boolean), OrdinalPosition = 83, Size = 3 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Debit", DataType = typeof(Decimal), OrdinalPosition = 87, Size = 17 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Credit", DataType = typeof(Decimal), OrdinalPosition = 104, Size = 20 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "All", DataType = typeof(Boolean), OrdinalPosition = 130, Size = 3 });

                    break;

                case TestFile_GenericFixedWidth:

                    // Definition for different data types and the data defined by ordinal position
                    definition.Culture = new CultureInfo("en-US");
                    definition.ItemProperties.Add(new DataItemProperty() { Key = true, Name = "Entry", DataType = typeof(Int32), OrdinalPosition = 0, Size = 6 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Per", DataType = typeof(String), OrdinalPosition = 8, Size = 4 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "PostDate", DataType = typeof(DateTime), OrdinalPosition = 12, Size = 10 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "GL Account", DataType = typeof(String), OrdinalPosition = 24, Size = 10 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Description", DataType = typeof(String), OrdinalPosition = 37, Size = 26 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Src", DataType = typeof(String), OrdinalPosition = 64, Size = 4 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Flow", DataType = typeof(Boolean), OrdinalPosition = 69, Size = 3 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Ref", DataType = typeof(String), OrdinalPosition = 73, Size = 8 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Post", DataType = typeof(Boolean), OrdinalPosition = 83, Size = 3 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Debit", DataType = typeof(Decimal), OrdinalPosition = 87, Size = 17 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "Credit", DataType = typeof(Decimal), OrdinalPosition = 104, Size = 20 });
                    definition.ItemProperties.Add(new DataItemProperty() { Name = "All", DataType = typeof(Boolean), OrdinalPosition = 130, Size = 3 });

                    // Calculated Properties
                    definition.ItemProperties.Add(new DataItemProperty()
                    {
                        Name = "Result",
                        DataType = typeof(Double),
                        OrdinalPosition = 0,
                        Size = 0,
                        PropertyType = DataItemPropertyType.Calculated,
                        Calculation = "IIF(Post, Debit, 0)"
                    });

                    break;
            }

            // Return the definition
            return definition;
        }
    }
}
