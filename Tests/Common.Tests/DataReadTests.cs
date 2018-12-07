using System;
using Xunit;
using TNDStudios.DataPortals.Data;
using TNDStudios.DataPortals.Helpers;
using System.Collections.Generic;

namespace TNDStudios.DataPortals.Tests.Common
{
    public class DataReadTests
    {
        /// <summary>
        /// Check that dates get converted from the raw data to 
        /// the DateTime format
        /// </summary>
        [Fact]
        public void Date_Read()
        {
            // Arrange
            DataItemProperty property = new DataItemProperty()
            {
                DataType = typeof(DateTime),
                Name = "DateTimeTest",
                OrdinalPosition = -1,
                Size = 0,
                Pattern = "dd MMM yyyy"
            };

            DataItemDefinition definition = new DataItemDefinition() { };

            // Act
            Object rawValue = DataFormatHelper.ReadData("23 Oct 1984", property, definition);
            DateTime value = (rawValue != DBNull.Value) ? (DateTime)rawValue : DateTime.MinValue;

            // Assert
            Assert.True(value != DateTime.MinValue &&
                        value.Day == 23 &&
                        value.Month == 10 &&
                        value.Year == 1984);
        }

        /// <summary>
        /// Check that strings get converted from the raw data to 
        /// the string format
        /// </summary>
        [Fact]
        public void String_Read()
        {
            // Arrange
            DataItemProperty property = new DataItemProperty()
            {
                DataType = typeof(String),
                Name = "StringTest",
                OrdinalPosition = -1,
                Size = 0
            };

            DataItemDefinition definition = new DataItemDefinition() { };

            // Act
            Object value = DataFormatHelper.ReadData("This Is A String", property, definition);

            // Assert
            Assert.True(value != DBNull.Value &&
                        (String)value == "This Is A String");
        }

        /// <summary>
        /// Check that boolean values get converted from the raw data 
        /// to the boolean format
        /// </summary>
        [Fact]
        public void Boolean_Read()
        {
            // Arrange
            DataItemProperty property = new DataItemProperty()
            {
                DataType = typeof(Boolean),
                Name = "BooleanTest",
                OrdinalPosition = -1,
                Size = 0
            };

            DataItemDefinition definition = new DataItemDefinition() { };

            // Act
            Object rawValue = DataFormatHelper.ReadData("true", property, definition);
            Boolean value = (rawValue != DBNull.Value) ? (Boolean)rawValue : false;

            // Assert
            Assert.True(value);
        }
    }
}
