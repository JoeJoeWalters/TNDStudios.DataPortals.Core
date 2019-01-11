using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using Xunit;
using Newtonsoft.Json;
using TNDStudios.DataPortals.Api;

namespace TNDStudios.DataPortals.Tests.Core
{
    /// <summary>
    /// Setup fixture for the tests
    /// </summary>
    public class ManagedApiTestsFixture : IDisposable
    {
        // The table to test
        public DataTable Data; 

        // Static reference data for the data table
        public String StringToTest = "String To Test";
        public Boolean BooleanToTest = true;
        public DateTime DateToTest = new DateTime(2018, 12, 1);
        public Double NumberToTest = (Double)1.5;

        // Amount of rows expected
        public readonly Int32 ExpectedRowCount = 3;

        /// <summary>
        /// Configure the test fixture
        /// </summary>
        public ManagedApiTestsFixture() => Initialise();

        /// <summary>
        /// Initialise the test fixture
        /// </summary>
        public void Initialise()
        {
            // Create a new reference data table
            Data = new DataTable();
            Data.Columns.Add(new DataColumn("StringData", typeof(String)));
            Data.Columns.Add(new DataColumn("BooleanData", typeof(Boolean)));
            Data.Columns.Add(new DataColumn("DateData", typeof(DateTime)));
            Data.Columns.Add(new DataColumn("NumericData", typeof(Double)));

            // Add a number of test rows
            for (Int32 rowId = 0; rowId < ExpectedRowCount; rowId++)
            {
                DataRow row = Data.NewRow();
                row["StringData"] = StringToTest;
                row["BooleanData"] = BooleanToTest;
                row["DateData"] = DateToTest;
                row["NumericData"] = NumberToTest;
                Data.Rows.Add(row);
            }
        }

        /// <summary>
        /// Dispose of the factory class etc.
        /// </summary>
        public void Dispose()
        {
            Data = null; // Kill the reference
        }
    }

    /// <summary>
    /// Tests to validate the managed API helper classes and general
    /// functionality
    /// </summary>
    public class ManagedApiTests : EmbeddedTestHelperBase, IClassFixture<ManagedApiTestsFixture>
    {
        // Test file references
        public const String TestFile_SingleRecord = "TestFiles.ManagedApi.SingleRecord.json";
        public const String TestFile_MultipleRecords = "TestFiles.ManagedApi.MultipleRecords.json";

        /// <summary>
        /// The injected fixture
        /// </summary>
        private ManagedApiTestsFixture fixture;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fixture">The fixture to test</param>
        public ManagedApiTests(ManagedApiTestsFixture fixture)
        {
            this.fixture = fixture;
        }

        /// <summary>
        /// Test that aliases for the data definition tables are injected
        /// in to the column headers correctly
        /// </summary>
        [Fact]
        public void Aliases_Injected_To_DataTable()
        {
            // Arrange
            fixture.Initialise();
            String aliasName = "Alias Name";
            List<KeyValuePair<String, String>> aliases = 
                new List<KeyValuePair<String, String>>()
                {
                    new KeyValuePair<String, String>("StringData", aliasName)
                };

            // Act
            ManagedApiHelper.HandleAliases(fixture.Data, aliases);
            DataColumn dataColumn = fixture.Data.Columns["StringData"];

            // Assert
            Assert.True(dataColumn.ExtendedProperties.Contains("Alias"));
            Assert.Equal(aliasName, dataColumn.ExtendedProperties["Alias"]);
        }

        /// <summary>
        /// Test that the Json that is produced for the Api responses
        /// is correctly created from a datatable input
        /// </summary>
        [Fact]
        public void Json_From_DataTable()
        {
            // Arrange
            fixture.Initialise();

            // Act
            JsonResult jsonResult = ManagedApiHelper.ToJson(fixture.Data);
            var result = JsonConvert.SerializeObject(jsonResult.Value, jsonResult.SerializerSettings);

            // Assert 
            Assert.DoesNotContain("\"RowError\":", result); // Standard DataTable Tags Should Be Stripped Out
            Assert.DoesNotContain("\"RowState\":", result); // Standard DataTable Tags Should Be Stripped Out
            Assert.DoesNotContain("\"Table\":", result); // Standard DataTable Tags Should Be Stripped Out
            Assert.DoesNotContain("\"HasErrors\":", result); // Standard DataTable Tags Should Be Stripped Out
            Assert.Contains($"\"StringData\": \"{fixture.StringToTest}\"", result); // Element Exists
            Assert.Contains($"\"BooleanData\": true", result); // Element Exists
            Assert.Contains($"\"DateData\": \"{fixture.DateToTest.ToString("yyyy")}", result); // Element Exists (We only care about the first part of the date here)
            Assert.Contains($"\"NumericData\": {fixture.NumberToTest.ToString()}", result); // Element Exists
        }

        /// <summary>
        /// Convert an incoming data set that for example needs updating etc.
        /// from Json to a Datatable for the provider to handle where there is only
        /// one record in the json (it is not an array)
        /// </summary>
        [Fact]
        public void DataTable_From_SingleRecord_Json()
        {
            // Arrange
            String json = GetResourceString(TestFile_SingleRecord);

            // Act
            DataTable dataTable = ManagedApiHelper.ToDataTable("application/json", json);

            // Assert
            Assert.Equal(1, dataTable.Rows.Count);
        }

        /// <summary>
        /// Convert an incoming data set that for example needs updating etc.
        /// from Json to a Datatable for the provider to handle where there are
        /// multiple records in the json (it's an array)
        /// </summary>
        [Fact]
        public void DataTable_From_MultipleRecords_Json()
        {
            // Arrange
            String json = GetResourceString(TestFile_MultipleRecords);

            // Act
            DataTable dataTable = ManagedApiHelper.ToDataTable("application/json", json);

            // Assert
            Assert.Equal(fixture.ExpectedRowCount, dataTable.Rows.Count);
        }
    }
}
