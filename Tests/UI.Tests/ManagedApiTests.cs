using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TNDStudios.DataPortals.UI.Controllers.Api.Helpers;
using Xunit;
using Newtonsoft.Json;

namespace TNDStudios.DataPortals.Tests.UI
{
    /// <summary>
    /// Tests to validate the managed API helper classes and general
    /// functionality
    /// </summary>
    public class ManagedApiTests
    {
        /// <summary>
        /// Test that the Json that is produced for the Api responses
        /// is correctly created from a datatable input
        /// </summary>
        [Fact]
        public void Json_From_DataTable()
        {
            // Arrange
            DataTable data = new DataTable();
            data.Columns.Add(new DataColumn("StringData", typeof(String)));
            data.Columns.Add(new DataColumn("BooleanData", typeof(Boolean)));
            data.Columns.Add(new DataColumn("DateData", typeof(DateTime)));
            data.Columns.Add(new DataColumn("NumericData", typeof(Double)));

            String stringToTest = "String To Test";
            Boolean booleanToTest = true;
            DateTime dateToTest = new DateTime(2018, 12, 1);
            Double numberToTest = (Double)1.5;

            DataRow row = data.NewRow();
            row["StringData"] = stringToTest;
            row["BooleanData"] = booleanToTest;
            row["DateData"] = dateToTest;
            row["NumericData"] = numberToTest;
            data.Rows.Add(row);

            ManagedApiHelpers helpers = new ManagedApiHelpers();

            // Act
            JsonResult jsonResult = helpers.DataTableToJsonFormat(data);
            var result = JsonConvert.SerializeObject(jsonResult.Value, jsonResult.SerializerSettings);

            // Assert 
            Assert.DoesNotContain("\"RowError\":", result); // Standard DataTable Tags Should Be Stripped Out
            Assert.DoesNotContain("\"RowState\":", result); // Standard DataTable Tags Should Be Stripped Out
            Assert.DoesNotContain("\"Table\":", result); // Standard DataTable Tags Should Be Stripped Out
            Assert.DoesNotContain("\"HasErrors\":", result); // Standard DataTable Tags Should Be Stripped Out
            Assert.Contains($"\"StringData\": \"{stringToTest}\"", result); // Element Exists
            Assert.Contains($"\"BooleanData\": true", result); // Element Exists
            Assert.Contains($"\"DateData\": \"{dateToTest.ToString("yyyy")}", result); // Element Exists (We only care about the first part of the date here)
            Assert.Contains($"\"NumericData\": {numberToTest.ToString()}", result); // Element Exists
        }
    }
}
