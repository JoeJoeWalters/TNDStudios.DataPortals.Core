using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Xunit;
using TNDStudios.DataPortals.Helpers;

namespace TNDStudios.DataPortals.Tests.SQLProvider
{
    public class HelperTests
    {
        // Create a datatable in the format that is needed to create an object reference
        private DataRow CreateDataRow()
        {
            // Create the table
            DataTable result = new DataTable();

            // Add the columns
            result.Columns.Add(new DataColumn("TABLE_CATALOG", typeof(String)));
            result.Columns.Add(new DataColumn("TABLE_SCHEMA", typeof(String)));
            result.Columns.Add(new DataColumn("TABLE_NAME", typeof(String)));

            return result.NewRow(); // Return the table
        }

        [Fact]
        public void ObjectReference_With_Incorrect_Input()
        {
            // Arrange
            String expectedResult = "";
            DataRow row = (new DataTable()).NewRow();

            // Act
            KeyValuePair<String, String> result = 
                SQLProviderHelpers.CreateObjectReference(row);

            // Assert
            Assert.Equal(expectedResult, result.Value);
            Assert.Equal("", result.Key);
        }

        [Fact]
        public void ObjectReference_With_All_3_Components()
        {
            // Arrange
            String expectedResult = "Database.Schema.Table";
            DataRow row = CreateDataRow();
            row["TABLE_CATALOG"] = "Database";
            row["TABLE_SCHEMA"] = "Schema";
            row["TABLE_NAME"] = "Table";

            // Act
            KeyValuePair<String, String> result =
                SQLProviderHelpers.CreateObjectReference(row);

            // Assert
            Assert.Equal(expectedResult, result.Value);
            Assert.Equal(row["TABLE_NAME"], result.Key);
        }

        [Fact]
        public void ObjectReference_With_2_Components()
        {
            // Arrange
            String expectedResult = "Schema.Table";
            DataRow row = CreateDataRow();
            row["TABLE_SCHEMA"] = "Schema";
            row["TABLE_NAME"] = "Table";

            // Act
            KeyValuePair<String, String> result =
                SQLProviderHelpers.CreateObjectReference(row);

            // Assert
            Assert.Equal(expectedResult, result.Value);
            Assert.Equal(row["TABLE_NAME"], result.Key);
        }

        [Fact]
        public void ObjectReference_With_1_Component()
        {
            // Arrange
            String expectedResult = "Table";
            DataRow row = CreateDataRow();
            row["TABLE_NAME"] = "Table";

            // Act
            KeyValuePair<String, String> result =
                SQLProviderHelpers.CreateObjectReference(row);

            // Assert
            Assert.Equal(expectedResult, result.Value);
            Assert.Equal(row["TABLE_NAME"], result.Key);
        }
    }
}
