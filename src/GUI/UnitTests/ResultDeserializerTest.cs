using System;
using System.IO;
using EFCorePowerTools.Handlers.ReverseEngineer;
using Xunit;

namespace UnitTests
{
    public class ResultDeserializerTest
    {
        [Fact(Skip = "Investigate build fail")]
        public void ParseTableResult()
        {
            // Arrange
            var result = ReadAllText("TableResult2.txt");

            // Act
            var parsed = ResultDeserializer.BuildTableResult(result);

            // Assert
            Assert.NotNull(parsed);
            Assert.Equal(29, parsed.Count);
        }

        [Fact(Skip = "Investigate build fail")]
        public void ParseTableResultWithWarning()
        {
            // Arrange
            var result = ReadAllText("TableResult1.txt");

            // Act
            var parsed = ResultDeserializer.BuildTableResult(result);

            // Assert
            Assert.NotNull(parsed);
            Assert.Equal(29, parsed.Count);
        }

        [Fact(Skip = "Investigate build issue")]
        public void ParseResultWithError()
        {
            // Arrange
            var result = ReadAllText("ErrorResult.txt");

            // Act
            ResultDeserializer.BuildResult(result);

            // Assert
        }

        private static string ReadAllText(string file)
        {
            return File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file));
        }
    }
}
