using System.IO;
using EFCorePowerTools.Helpers;
using NUnit.Framework;
using NUnit.Framework.Legacy;

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
            ClassicAssert.IsNotNull(result);
        }
    }
}
