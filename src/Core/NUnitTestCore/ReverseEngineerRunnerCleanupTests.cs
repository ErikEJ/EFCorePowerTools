using System;
using System.IO;
using System.Reflection;
using System.Text;
using NUnit.Framework;
using RevEng.Common;
using RevEng.Core;

namespace UnitTests
{
    [TestFixture]
    public class ReverseEngineerRunnerCleanupTests
    {
        [Test]
        public void TryRemoveFileDeletesGeneratedFile()
        {
            var codeFile = CreateTestFile(PathHelper.Header + Environment.NewLine + "public class TableTwo {}");

            try
            {
                InvokeTryRemoveFile(codeFile);

                Assert.That(File.Exists(codeFile), Is.False);
            }
            finally
            {
                RemoveIfExists(codeFile);
            }
        }

        [Test]
        public void TryRemoveFileDoesNotDeleteNonGeneratedFile()
        {
            var codeFile = CreateTestFile("public class TableTwo {}");

            try
            {
                InvokeTryRemoveFile(codeFile);

                Assert.That(File.Exists(codeFile), Is.True);
            }
            finally
            {
                RemoveIfExists(codeFile);
            }
        }

        private static string CreateTestFile(string contents)
        {
            var directory = Path.Combine(Path.GetTempPath(), "EFCorePowerTools.Tests", Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(directory);

            var codeFile = Path.Combine(directory, "TableTwo.cs");
            File.WriteAllText(codeFile, contents, Encoding.UTF8);

            return codeFile;
        }

        private static void InvokeTryRemoveFile(string codeFile)
        {
            var method = typeof(ReverseEngineerRunner).GetMethod("TryRemoveFile", BindingFlags.NonPublic | BindingFlags.Static);

            Assert.That(method, Is.Not.Null);

            method!.Invoke(null, new object[] { codeFile });
        }

        private static void RemoveIfExists(string codeFile)
        {
            if (File.Exists(codeFile))
            {
                File.Delete(codeFile);
            }

            var directory = Path.GetDirectoryName(codeFile);
            if (!string.IsNullOrEmpty(directory) && Directory.Exists(directory))
            {
                Directory.Delete(directory, recursive: true);
            }
        }
    }
}
