using System.IO;
using EFCorePowerTools.Helpers;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class ModelSerializeTest
    {
        [Test]
        public void ParseTableResult()
        {
            // Arrange, Act
            var result = RenamingRulesSerializer.TryRead(Path.Combine(TestContext.CurrentContext.TestDirectory, "renaming.json"));

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
