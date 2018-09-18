using System;
using Xunit;
using TNDStudios.DataPortals.Helpers;
using System.Collections.Generic;

namespace TNDStudios.DataPortals.Tests.Common
{
    public class FormattingTests
    {

        [Fact]
        public void Can_Remove_Ends_Leave_Other_Characters()
        {
            // Arrange
            Char quoteCharacter = '\"';
            String expectedResult = "Result \"Expected\"";
            String stringToClean = $"{quoteCharacter}{expectedResult}{quoteCharacter}";

            // Act
            String result = DataFormatHelper.CleanString(stringToClean, quoteCharacter);

            // Assert
            Assert.Equal(expectedResult, result);

        }
    }
}
