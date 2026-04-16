using System;
using GOEddie.Dacpac.References;
using Xunit;

namespace UnitTests
{
    public class DacpacConsolidatorTest
    {
        [Fact]
        public void CanConsolidateWithoutReferencesIssue274()
        {
            // Arrange
            var dacPath = TestPath("Chinook.dacpac");

            // Act
            var result = DacpacConsolidator.Consolidate(dacPath);

            // Assert
            Assert.Equal(dacPath, result);
        }

        private string TestPath(string file)
        {
            return System.IO.Path.Combine(AppContext.BaseDirectory, "Dacpac", file);
        }
    }
}
