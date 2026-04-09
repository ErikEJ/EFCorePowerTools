using Xunit;
using RevEng.Core;

namespace UnitTests
{
    public class DbContextSplitterTest
    {
        [Fact(Skip = "run manually")]
        public void CanSplit()
        {
            var result = DbContextSplitter.Split("C:\\Code\\Github\\EFCorePowerTools\\test\\Ef7Playground\\Ef7Playground\\Models\\NorthwindContext.cs", "Test", false, "NorthwindContext");

            Assert.NotNull(result);
        }
    }
}
