using NUnit.Framework;
using RevEng.Shared;

namespace UnitTests
{
    [TestFixture]
    public class PathHelperTest
    {

        [Test]
        public void ParsePathType1()
        {
            // Act
            var result = PathHelper.GetAbsPath("..\\..\\folder", "C:\\temp\\data\\");

            // Assert
            Assert.AreEqual(@"C:\folder", result);
        }

        [Test]
        public void ParsePathType2()
        {
            // Act
            var result = PathHelper.GetAbsPath("D:\\folder", "C:\\temp\\data\\");

            // Assert
            Assert.AreEqual(@"D:\folder", result);
        }

        [Test]
        public void ParsePathType3()
        {
            // Act
            var result = PathHelper.GetAbsPath("\\server\\folder", "C:\\temp\\data\\");

            // Assert
            Assert.AreEqual(@"\server\folder", result);
        }

        [Test]
        public void ParsePathType4()
        {
            // Act
            var result = PathHelper.GetAbsPath("folder", "C:\\temp\\data\\");

            // Assert
            Assert.AreEqual(@"C:\temp\data\folder", result);
        }
    }
}
