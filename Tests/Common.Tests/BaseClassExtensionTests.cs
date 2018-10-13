using System;
using Xunit;
using TNDStudios.DataPortals.Data;
using TNDStudios.DataPortals.Helpers;
using System.Collections.Generic;

namespace TNDStudios.DataPortals.Tests.Common
{
    /// <summary>
    /// Tests to check that base class extensions are functioning correctly
    /// </summary>
    public class BaseClassExtensionTests
    {
        [Fact]
        public void Type_To_Short_Name()
        {
            // Arrange
            Dictionary<Type, String> typesToCheck =
                new Dictionary<Type, String>()
                {
                    { typeof(System.String), "string" },
                    { typeof(System.Int64 ), "int64" },
                    { typeof(System.Int32 ), "int32" },
                    { typeof(System.Int16 ), "int16" },
                    { typeof(System.Single), "single" },
                    { typeof(System.Double), "double" }
                };

            Dictionary<String, Boolean> results = 
                new Dictionary<String, Boolean>() { };
            
            // Act
            foreach(KeyValuePair<Type, String> typeToCheck in typesToCheck)
            {
                // Do the check and record the result
                String shortValue = typeToCheck.Key.ToShortName();
                results.Add($"{typeToCheck.Value} => {shortValue}",
                    (shortValue == typeToCheck.Value));
            }

            // Assert
            foreach(KeyValuePair<String, Boolean> result in results)
            {
                Assert.True(result.Value);
            }
        }
    }
}
