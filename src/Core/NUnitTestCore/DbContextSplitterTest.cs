using NUnit.Framework;
using NUnit.Framework.Legacy;
using RevEng.Core;

namespace UnitTests
{
    [TestFixture]
    public class DbContextSplitterTest
    {
        [Test, Ignore("run manually")]
        public void CanSplit()
        {
            var result = DbContextSplitter.Split("C:\\Code\\Github\\EFCorePowerTools\\test\\Ef7Playground\\Ef7Playground\\Models\\NorthwindContext.cs", "Test", false, "NorthwindContext");

            ClassicAssert.NotNull(result);
        }
    }
}
