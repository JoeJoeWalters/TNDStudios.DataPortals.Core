using System;
using Xunit;
using TNDStudios.DataPortals.Data;
using TNDStudios.DataPortals.Helpers;
using System.Collections.Generic;
using TNDStudios.DataPortals.Security;

namespace TNDStudios.DataPortals.Tests.Common
{
    /// <summary>
    /// Setup fixture for the tests
    /// </summary>
    public class CredentialTestsFixture : IDisposable
    {
        public Credentials Credentials; // Test credentials to test against

        /// <summary>
        /// Configure the test fixture
        /// </summary>
        public CredentialTestsFixture()
            => Initialise();

        public void Initialise()
            => this.Credentials = new Credentials()
            {
                Properties = new List<Credential>()
                {
                    new Credential(){ Name = "Username", Value = "username" },
                    new Credential(){ Name = "Password", Value = "password" }
                }
            };

        /// <summary>
        /// Dispose of the repository class etc.
        /// </summary>
        public void Dispose()
        {
            this.Credentials = null; // Kill the reference
        }
    }

    /// <summary>
    /// Tests for checking if credential based functionality works
    /// </summary>
    public class CredentialTests : IClassFixture<CredentialTestsFixture>
    {
        private CredentialTestsFixture fixture; // Reference for the test fixture

        /// <summary>
        /// Constructor to inject the fixture
        /// </summary>
        /// <param name="data"></param>
        public CredentialTests(CredentialTestsFixture data)
            => fixture = data;

        /// <summary>
        /// Test that when a template string has a set of credentials applied
        /// to it that the appropriate markers are replaced with the appropriate
        /// credentials
        /// </summary>
        [Fact]
        public void Replace_Placeholders_In_Template_String()
        {
            // Arrange
            fixture.Initialise(); // Reset the fixture
            String template = "username={{Username}};password={{Password}}"; // Template where the markers need replacing
            String expectedOutcome = "username=username;password=password"; // The expected result
            String result = String.Empty; // The result which is empty by default

            // Act
            result = fixture.Credentials.Transform(template); // Transform the template using the credentials

            // Assert
            Assert.Equal(expectedOutcome, result); // Does the expected outcome conform to the result?
        }

        /// <summary>
        /// Set a credential in to a set of credentials
        /// </summary>
        [Fact]
        public void Set_Credential()
        {
            // Arrange
            fixture.Initialise(); // Reset the fixture
            fixture.Credentials.Properties.Add(new Credential() { Name = "Test", Value = "test" }); // Add a new credential

            // Act
            String value = fixture.Credentials.GetValue("Test");

            // Assert
            Assert.Equal("test", value); // With a new credential did the "get" method return the new value            
        }

        /// <summary>
        /// Get a credential in the set of credentials
        /// </summary>
        [Fact]
        public void Get_Credential()
        {
            // Arrange
            fixture.Initialise(); // Reset the fixture

            // Act
            String value = fixture.Credentials.GetValue("Username"); // Get an existing value

            // Assert
            Assert.Equal("username", value); // With a new credential did the "get" method return the existing value 
        }
    }
}
