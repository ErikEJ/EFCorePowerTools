using EFCorePowerTools.Handlers.ReverseEngineer;
using NUnit.Framework;
using System.IO;

namespace UnitTests
{
    [TestFixture]
    public class ResultDeserializerTest
    {
        private readonly ResultDeserializer _parser = new ResultDeserializer();

        [Test, Ignore("Investigate build fail")]
        public void ParseTableResult()
        {
            // Arrange
            var result = ReadAllText("TableResult2.txt");

            // Act
            var parsed = _parser.BuildTableResult(result);

            // Assert
            Assert.IsNotNull(parsed);
            Assert.AreEqual(29, parsed.Count);
        }

        [Test, Ignore("Investigate build fail")]
        public void ParseTableResultWithWarning()
        {
            // Arrange
            var result = ReadAllText("TableResult1.txt");

            // Act
            var parsed = _parser.BuildTableResult(result);

            // Assert
            Assert.IsNotNull(parsed);
            Assert.AreEqual(29, parsed.Count);
        }

        [Test, Ignore("Investigate build issue")]
        public void ParseResultWithError()
        {
            // Arrange
            var result = ReadAllText("ErrorResult.txt");

            // Act
            var parsed = _parser.BuildResult(result);

            // Assert
        }

        private string ReadAllText(string file)
        {
            return File.ReadAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, file));
        }
    }
}
