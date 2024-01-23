using GOEddie.Dacpac.References;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace UnitTests
{
    [TestFixture]
    public class DacpacConsolidatorTest
    {
        [Test]
        [Ignore("Local only!")]
        public void CanConsolidate_Issue_274()
        {
            // Act
            var result = DacpacConsolidator.Consolidate(@"C:\Users\Erik\Downloads\CompositeDatabase\CompositeDatabase\CompositeDatabase\bin\Debug\CompositeDatabase.dacpac");

            // Assert
            ClassicAssert.IsTrue(result.Contains("\\efpt-"));
        }

        [Test]
        public void CanConsolidate_Without_References_Issue_274()
        {
            // Arrange
            var dacPath = TestPath("Chinook.dacpac");

            // Act
            var result = DacpacConsolidator.Consolidate(dacPath);

            // Assert
            ClassicAssert.AreEqual(result, dacPath);
        }

        private string TestPath(string file)
        {
            return System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, "Dacpac", file);
        }
    }
}
