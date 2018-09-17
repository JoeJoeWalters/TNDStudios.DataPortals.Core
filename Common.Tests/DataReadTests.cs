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
        public void DateRead()
        {
            // Arrange
            DataItemProperty property = new DataItemProperty()
            {
                DataType = typeof(DateTime),
                Name = "DateTimeTest",
                OridinalPosition = -1,
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
        public void StringRead()
        {
            // Arrange

            // Act

            // Assert
        }

        /// <summary>
        /// Check that boolean values get converted from the raw data 
        /// to the boolean format
        /// </summary>
        [Fact]
        public void BooleanRead()
        {
            // Arrange
            DataItemProperty property = new DataItemProperty()
            {
                DataType = typeof(Boolean),
                Name = "BooleanTest",
                OridinalPosition = -1
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
