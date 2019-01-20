using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using TNDStudios.DataPortals.Data;
using Xunit;

namespace TNDStudios.DataPortals.Tests.Common
{
    /// <summary>
    /// Setup fixture for the tests
    /// </summary>
    public class DataDefinitionTestsFixture : IDisposable
    {
        public DataItemDefinition Definition; // The definition to test
        public DataTable Data; // The data table to test

        public const String PrimaryKeyName = "PkId";
        public const String CalculatedColumnName = "CalculatedColumn";
        public const String CalculatedExpression = "[PkId] * 100";

        /// <summary>
        /// Configure the test fixture
        /// </summary>
        public DataDefinitionTestsFixture()
            => Initialise();

        /// <summary>
        /// Initialise the data
        /// </summary>
        public void Initialise()
        {
            // Set up common properties
            CultureInfo cultureInfo = new CultureInfo("en-US"); // Set a specific culture to test the transformation
            Encoding encodingFormat = Encoding.UTF8; // Default encoding format

            // Create a new datatable to test converting from and checking
            // as a reference once converted in to
            Data = new DataTable() { Locale = cultureInfo };
            DataColumn primaryKeyColumn = new DataColumn(PrimaryKeyName, typeof(Int32));
            DataColumn calculatedColumn = new DataColumn(CalculatedColumnName, typeof(Int32), CalculatedExpression);
            Data.Columns.Add(primaryKeyColumn); // Add the primary key
            Data.Columns.Add(new DataColumn("StringColumn", typeof(String)));
            Data.Columns.Add(new DataColumn("DateColumn", typeof(DateTime)));
            Data.Columns.Add(new DataColumn("BooleanColumn", typeof(Boolean)));
            Data.Columns.Add(new DataColumn("IntegerColumn", typeof(Int32)));
            Data.Columns.Add(new DataColumn("FloatColumn", typeof(Double)));
            Data.Columns.Add(calculatedColumn);
            Data.PrimaryKey = new DataColumn[1] { primaryKeyColumn }; // Assign the primary key column

            // Create a new data item definition to test converting from and
            // checking as a reference once converted in to
            Definition = new DataItemDefinition()
            {
                Culture = cultureInfo,
                Name = "",
                Description = "",
                EncodingFormat = encodingFormat,
                Id = Guid.NewGuid(),
                ItemProperties = new List<DataItemProperty>()
                {
                    new DataItemProperty()
                    {
                        Key = true,
                        Name = PrimaryKeyName,
                        Description = PrimaryKeyName,
                        DataType = typeof(Int32),
                        OrdinalPosition = 0,
                        Path = PrimaryKeyName,
                        PropertyType = DataItemPropertyType.Property
                    },
                    new DataItemProperty()
                    {
                        Key = false,
                        Name = "StringColumn",
                        Description = "StringColumn",
                        DataType = typeof(String),
                        OrdinalPosition = 1,
                        Path = "StringColumn",
                        PropertyType = DataItemPropertyType.Property
                    },
                    new DataItemProperty()
                    {
                        Key = false,
                        Name = "DateColumn",
                        Description = "DateColumn",
                        DataType = typeof(DateTime),
                        OrdinalPosition = 2,
                        Path = "DateColumn",
                        PropertyType = DataItemPropertyType.Property
                    },
                    new DataItemProperty()
                    {
                        Key = false,
                        Name = "BooleanColumn",
                        Description = "BooleanColumn",
                        DataType = typeof(Boolean),
                        OrdinalPosition = 3,
                        Path = "BooleanColumn",
                        PropertyType = DataItemPropertyType.Property
                    },
                    new DataItemProperty()
                    {
                        Key = false,
                        Name = "IntegerColumn",
                        Description = "IntegerColumn",
                        DataType = typeof(Int32),
                        OrdinalPosition = 4,
                        Path = "IntegerColumn",
                        PropertyType = DataItemPropertyType.Property
                    },
                    new DataItemProperty()
                    {
                        Key = false,
                        Name = "FloatColumn",
                        Description = "FloatColumn",
                        DataType = typeof(Double),
                        OrdinalPosition = 5,
                        Path = "FloatColumn",
                        PropertyType = DataItemPropertyType.Property
                    },
                    new DataItemProperty()
                    {
                        Key = false,
                        Name = CalculatedColumnName,
                        Description = CalculatedColumnName,
                        DataType = typeof(Int32),
                        OrdinalPosition = 6,
                        Path = CalculatedColumnName,
                        PropertyType = DataItemPropertyType.Calculated,
                        Calculation = CalculatedExpression
                    },
                }
            };
        }

        /// <summary>
        /// Dispose of the repository class etc.
        /// </summary>
        public void Dispose()
        {
            // Kill the references
            Definition = null;
            Data = null;
        }
    }

    public class DataDefinitionTests : IClassFixture<DataDefinitionTestsFixture>
    {
        private DataDefinitionTestsFixture fixture; // Reference for the test fixture

        /// <summary>
        /// Constructor to inject the fixture
        /// </summary>
        /// <param name="data"></param>
        public DataDefinitionTests(DataDefinitionTestsFixture data)
            => fixture = data;

        /// <summary>
        /// Test converting a native data table to a data definition 
        /// object (mainly used for analysing data sources when discovering
        /// by the user)
        /// </summary>
        [Fact]
        public void Convert_DataTable_To_DataDefinition()
        {
            // Arrange
            DataItemDefinition result = new DataItemDefinition(); // Empty by default

            // Act
            result.FromDataTable(fixture.Data);

            // Assert
            Assert.True(fixture.Data.Columns.Count == result.ItemProperties.Count); // Correct amount of columns?
            result.ItemProperties.ForEach(itemProperty =>
            {
                // Get the source column and check it actually exists
                Assert.True(fixture.Data.Columns.Contains(itemProperty.Name)); // Column Exists Test
                DataColumn sourceColumn = fixture.Data.Columns[itemProperty.Name]; // Get the column for further tests

                // Data in the converted item are what we expect them to be?
                Assert.Equal(sourceColumn.DataType, itemProperty.DataType);
                Assert.Equal(sourceColumn.ColumnName, itemProperty.Description);
                Assert.Equal(sourceColumn.ColumnName, itemProperty.Path);

                // In the right order?
                Assert.Equal(sourceColumn.Ordinal, itemProperty.OrdinalPosition);

                // If the source column is an expression, did it cast over?
                if ((sourceColumn.Expression ?? String.Empty) != String.Empty)
                {
                    Assert.Equal(DataItemPropertyType.Calculated, itemProperty.PropertyType); // Made the data item definition property a "calculated" column?
                    Assert.Equal(sourceColumn.Expression, itemProperty.Calculation); // Calculation the same as the source expression?
                }
            });

            // Any primary keys? Check to see if they were copied over
            (new List<DataColumn>(fixture.Data.PrimaryKey))
                .ForEach(column =>
                    {
                        // Do the checks to see if primary keys were identified correctly
                        DataItemProperty property = result.ItemProperties
                                .Where(prop => prop.Name == column.ColumnName)
                                .FirstOrDefault();

                        Assert.NotNull(property); // We actually have a column converted with the right name
                        Assert.True(property.Key); // The property is marked as being a key
                    });
        }

        /// <summary>
        /// Test the merging of column names (the actual column names (path) not the name)
        /// together so they can be passed to queries
        /// </summary>
        [Fact]
        public void Convert_DataDefinition_ToCSVString()
        {
            // Arrange
            String result = String.Empty; // Empty by default
            String expectedResult = $"{DataDefinitionTestsFixture.PrimaryKeyName},StringColumn,DateColumn,BooleanColumn,IntegerColumn,FloatColumn,CalculatedColumn"; // What we expect to get

            // Act
            result = fixture.Definition.ToItemCSV(true);

            // Assert
            Assert.Equal(expectedResult, result);

        }

        /// <summary>
        /// Test converting a data definition object to a native datatable object
        /// and that all of the columns, primary keys etc are all transformed
        /// </summary>
        [Fact]
        public void Convert_DataDefinition_ToDataTable()
        {
            // Arrange
            DataTable result = new DataTable(); // Empty by default

            // Act
            result = fixture.Definition.ToDataTable();

            // Assert
            Assert.True(fixture.Definition.ItemProperties.Count == result.Columns.Count); // Correct amount of columns?

            fixture.Definition.ItemProperties.ForEach(itemProperty =>
            {
                // Get the source column and check it actually exists
                Assert.True(result.Columns.Contains(itemProperty.Name)); // Column Exists Test
                DataColumn generatedColumn = result.Columns[itemProperty.Name]; // Get the column for further tests

                // Data in the converted item are what we expect them to be?
                Assert.Equal(generatedColumn.DataType, itemProperty.DataType);
                Assert.Equal(generatedColumn.ColumnName, itemProperty.Description);
                Assert.Equal(generatedColumn.ColumnName, itemProperty.Path);

                // In the right order?
                Assert.Equal(generatedColumn.Ordinal, itemProperty.OrdinalPosition);

                // If the source column is an expression, did it cast over?
                if ((generatedColumn.Expression ?? String.Empty) != String.Empty)
                {
                    Assert.Equal(generatedColumn.Expression, itemProperty.Calculation); // Calculation the same as the source expression?
                }
            });

            // Any primary keys? Check to see if they were copied over
            Assert.Equal(
                fixture.Definition.ItemProperties.Where(prop => prop.Key).Count(), 
                result.PrimaryKey.Length);

            (new List<DataColumn>(result.PrimaryKey))
                .ForEach(column =>
                {
                    // Do the checks to see if primary keys were identified correctly
                    DataItemProperty property = fixture.Definition.ItemProperties
                            .Where(prop => prop.Name == column.ColumnName)
                            .FirstOrDefault();

                    Assert.NotNull(property); // We actually have a column converted with the right name
                    Assert.True(property.Key); // The property is marked as being a key
                });
        }
    }
}
