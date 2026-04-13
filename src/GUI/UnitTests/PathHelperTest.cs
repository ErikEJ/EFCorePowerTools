using RevEng.Common;
using Xunit;

namespace UnitTests
{
    public class PathHelperTest
    {
        [Fact]
        public void ParsePathType1()
        {
            // Act
            var result = PathHelper.GetAbsPath("..\\..\\folder", "C:\\temp\\data\\");

            // Assert
            Assert.Equal(@"C:\folder", result);
        }

        [Fact]
        public void ParsePathType2()
        {
            // Act
            var result = PathHelper.GetAbsPath("D:\\folder", "C:\\temp\\data\\");

            // Assert
            Assert.Equal(@"D:\folder", result);
        }

        [Fact]
        public void ParsePathType3()
        {
            // Act
            var result = PathHelper.GetAbsPath("\\server\\folder", "C:\\temp\\data\\");

            // Assert
            Assert.Equal(@"\server\folder", result);
        }

        [Fact]
        public void ParsePathType4()
        {
            // Act
            var result = PathHelper.GetAbsPath("folder", "C:\\temp\\data\\");

            // Assert
            Assert.Equal(@"C:\temp\data\folder", result);
        }

        [Fact]
        public void GetRelativePath1()
        {
            // Act
            var result = PathHelper.GetAbsPath("..\\", "C:\\Code\\Github\\EFCorePowerTools\\test\\Ef7Playground\\Ef7Playground");

            // Assert
            Assert.Equal(@"C:\Code\Github\EFCorePowerTools\test\Ef7Playground", result);
        }

        [Fact]
        public void GetRelativePath2()
        {
            // Act
            var result = PathHelper.GetAbsPath("..\\..\\", "C:\\Code\\Github\\EFCorePowerTools\\test\\Ef7Playground\\Ef7Playground");

            // Assert
            Assert.Equal(@"C:\Code\Github\EFCorePowerTools\test", result);
        }
    }
}
