using NUnit.Framework;
using ReverseEngineer20.DacpacConsolidate;

namespace UnitTests
{
    [TestFixture]
    public class DacpacConsolidatorTest
    {
        [Test, Ignore("Local only!")]
        public void CanConsolidate_Issue_274()
        {
            // Arrange
            var consolidator = new DacpacConsolidator();

            // Act
            var result = consolidator.Consolidate(@"C:\Users\Erik\Downloads\CompositeDatabase\CompositeDatabase\CompositeDatabase\bin\Debug\CompositeDatabase.dacpac");

            // Assert
            Assert.IsTrue(result.Contains("\\efpt-"));
        }

        [Test]
        public void CanConsolidate_Without_References_Issue_274()
        {
            // Arrange
            var consolidator = new DacpacConsolidator();
            var dacPath = TestPath("Chinook.dacpac");

            // Act
            var result = consolidator.Consolidate(dacPath);

            // Assert
            Assert.AreEqual(result, dacPath);
        }

        private string TestPath(string file)
        {
            return System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, file);
        }
    }
}
