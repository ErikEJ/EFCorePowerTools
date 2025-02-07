using GOEddie.Dacpac.References;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class DacpacConsolidatorTest
    {
        [Test]
        [Ignore("Local only!")]
        public void CanConsolidateIssue274()
        {
            // Act
            var result = DacpacConsolidator.Consolidate(@"C:\Users\Erik\Downloads\CompositeDatabase\CompositeDatabase\CompositeDatabase\bin\Debug\CompositeDatabase.dacpac");

            // Assert
            Assert.IsTrue(result.Contains("\\efpt-"));
        }

        [Test]
        public void CanConsolidateWithoutReferencesIssue274()
        {
            // Arrange
            var dacPath = TestPath("Chinook.dacpac");

            // Act
            var result = DacpacConsolidator.Consolidate(dacPath);

            // Assert
            Assert.AreEqual(result, dacPath);
        }

        private string TestPath(string file)
        {
            return System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, "Dacpac", file);
        }
    }
}
