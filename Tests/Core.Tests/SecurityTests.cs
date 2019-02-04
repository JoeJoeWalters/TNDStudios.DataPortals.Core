using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using TNDStudios.DataPortals.Security;

namespace TNDStudios.DataPortals.Tests.Core
{
    /// <summary>
    /// Security based tests 
    /// </summary>
    public class SecurityTests
    {
        /// <summary>
        /// Test to check that basic authentication tokens are generated
        /// in the proper format
        /// </summary>
        [Fact]
        public void Generate_Valid_Basic_Authentication_Token()
        {
            // Arrange
            String expectedResult = "VXNlcm5hbWU6UGFzc3dvcmQ=";
            String result = String.Empty;
            String username = "Username";
            String password = "Password";

            // Act
            result = WebAuthHelper.GenerateBasicAuthString(username, password);

            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
