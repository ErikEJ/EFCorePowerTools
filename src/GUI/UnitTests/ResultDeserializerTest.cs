using System.IO;
using EFCorePowerTools.Handlers.ReverseEngineer;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace UnitTests
{
    [TestFixture]
    public class ResultDeserializerTest
    {
        private readonly ResultDeserializer parser = new ResultDeserializer();

        [Test]
        [Ignore("Investigate build fail")]
        public void ParseTableResult()
        {
            // Arrange
            var result = ReadAllText("TableResult2.txt");

            // Act
            var parsed = parser.BuildTableResult(result);

            // Assert
            ClassicAssert.IsNotNull(parsed);
            ClassicAssert.AreEqual(29, parsed.Count);
        }

        [Test]
        [Ignore("Investigate build fail")]
        public void ParseTableResultWithWarning()
        {
            // Arrange
            var result = ReadAllText("TableResult1.txt");

            // Act
            var parsed = parser.BuildTableResult(result);

            // Assert
            ClassicAssert.IsNotNull(parsed);
            ClassicAssert.AreEqual(29, parsed.Count);
        }

        [Test]
        [Ignore("Investigate build issue")]
        public void ParseResultWithError()
        {
            // Arrange
            var result = ReadAllText("ErrorResult.txt");

            // Act
            parser.BuildResult(result);

            // Assert
        }

        private string ReadAllText(string file)
        {
            return File.ReadAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, file));
        }
    }
}
