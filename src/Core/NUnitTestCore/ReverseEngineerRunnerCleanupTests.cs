using Xunit;
using RevEng.Common;
using RevEng.Core;
using System;
using System.IO;
using System.Text;

namespace NUnitTestCore
{
    public class ReverseEngineerRunnerCleanupTests
    {
        [Fact]
        public void TryRemoveFileDeletesGeneratedFile()
        {
            var codeFile = CreateTestFile(PathHelper.Header + Environment.NewLine + "public class TableTwo {}");

            try
            {
                InvokeTryRemoveFile(codeFile);

                Assert.False(File.Exists(codeFile));
            }
            finally
            {
                RemoveIfExists(codeFile);
            }
        }

        [Fact]
        public void TryRemoveFileDoesNotDeleteNonGeneratedFile()
        {
            var codeFile = CreateTestFile("public class TableTwo {}");

            try
            {
                InvokeTryRemoveFile(codeFile);

                Assert.True(File.Exists(codeFile));
            }
            finally
            {
                RemoveIfExists(codeFile);
            }
        }

        [Fact]
        public void RetryFileWriteUsesLfLineEndings()
        {
            var codeFile = CreateTestFile(string.Empty);

            try
            {
                ReverseEngineerRunner.RetryFileWrite(codeFile, new System.Collections.Generic.List<string> { "line1", "line2" }, "lf");

                var contents = File.ReadAllText(codeFile, Encoding.UTF8);

                Assert.Equal("line1\nline2\n", contents);
            }
            finally
            {
                RemoveIfExists(codeFile);
            }
        }

        [Fact]
        public void RetryFileWriteUsesCrLfLineEndings()
        {
            var codeFile = CreateTestFile(string.Empty);

            try
            {
                ReverseEngineerRunner.RetryFileWrite(codeFile, "line1\nline2\n", "crlf");

                var contents = File.ReadAllText(codeFile, Encoding.UTF8);

                Assert.Equal("line1\r\nline2\r\n", contents);
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
            ReverseEngineerRunner.TryRemoveFile(codeFile);
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
