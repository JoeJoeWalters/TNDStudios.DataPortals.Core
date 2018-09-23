using System;
using Xunit;
using TNDStudios.DataPortals.Data;
using TNDStudios.DataPortals.Helpers;
using System.Collections.Generic;

namespace TNDStudios.DataPortals.Tests.Common
{
    public class DataAnalysisTests
    {
        /// <summary>
        /// Check that different date formats show as the correct type
        /// </summary>
        [Fact]
        public void Date_Analyse()
        {
            // Arrange
            List<String> testItems =
                new List<String>()
                {
                    "21/01/1987",
                    "21 Jan 1987",
                    "21-01-1987"
                };
            List<Type> testResults = new List<Type>();

            // Act
            testItems.ForEach(item =>
            {
                testResults.Add(
                    DataFormatHelper.CalculateType(item)
                    );
            });

            // Assert
            testResults.ForEach(result =>
            {
                Assert.Equal(typeof(DateTime), result);
            });
        }

        /// <summary>
        /// Check that boolean positive data types are picked up
        /// </summary>
        [Fact]
        public void Boolean_Analyse_Positive()
        {
            // Arrange
            List<String> testItems = 
                new List<String>() { "true", "yes", "y" };
            List<Type> testResults = new List<Type>();

            // Act
            testItems.ForEach(item => 
            {
                testResults.Add(
                    DataFormatHelper.CalculateType(item)
                    );
            });

            // Assert
            testResults.ForEach(result => 
            {
                Assert.Equal(typeof(Boolean), result);
            });
        }

        /// <summary>
        /// Check that boolean negative data types are picked up
        /// </summary>
        [Fact]
        public void Boolean_Analyse_Negative()
        {
            // Arrange
            List<String> testItems =
                new List<String>() { "false", "no", "n" };
            List<Type> testResults = new List<Type>();

            // Act
            testItems.ForEach(item =>
            {
                testResults.Add(
                    DataFormatHelper.CalculateType(item)
                    );
            });

            // Assert
            testResults.ForEach(result =>
            {
                Assert.Equal(typeof(Boolean), result);
            });
        }

        /// <summary>
        /// Check that numeric data types are picked up from raw data
        /// </summary>
        [Fact]
        public void Numeric_Analyse()
        {
            // Arrange
            List<String> testItems =
                new List<String>()
                {
                    "0",
                    "0.112",
                    "-1.234",
                    "1,2345.00"
                };
            List<Type> acceptableResults =
                new List<Type>()
                {
                    typeof(Double),
                    typeof(Int32),
                    typeof(Int64),
                    typeof(Boolean)
                };
            List<Type> testResults = new List<Type>();

            // Act
            testItems.ForEach(item =>
            {
                testResults.Add(
                    DataFormatHelper.CalculateType(item)
                    );
            });

            // Assert
            testResults.ForEach(result =>
            {
                Assert.Contains(result, acceptableResults);
            });
        }
    }
}
