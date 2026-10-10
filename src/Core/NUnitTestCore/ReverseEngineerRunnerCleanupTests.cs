using Xunit;
using RevEng.Common;
using RevEng.Core;
using System;
using System.IO;
using System.Reflection;
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
                ReverseEngineerRunner.RetryFileWrite(codeFile, new System.Collections.Generic.List<string> { "line1", "line2" }, new GeneratedFileFormat("lf", null));

                var contents = File.ReadAllText(codeFile, Encoding.UTF8);

                Assert.Equal("line1\nline2\n", contents);
            }
            finally
            {
                RemoveIfExists(codeFile);
            }
        }

        [Fact]
        public void RetryFileWritePreservesExistingFileWhenLineEndingStyleIsUnsupported()
        {
            var codeFile = CreateTestFile("existing contents\r\n");

            try
            {
                var originalBytes = File.ReadAllBytes(codeFile);

                Assert.Throws<ArgumentOutOfRangeException>(() =>
                    ReverseEngineerRunner.RetryFileWrite(codeFile, new System.Collections.Generic.List<string> { "replacement" }, new GeneratedFileFormat("unsupported", null)));

                Assert.Equal(originalBytes, File.ReadAllBytes(codeFile));
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
                ReverseEngineerRunner.RetryFileWrite(codeFile, "line1\nline2\n", new GeneratedFileFormat("crlf", null));

                var contents = File.ReadAllText(codeFile, Encoding.UTF8);

                Assert.Equal("line1\r\nline2\r\n", contents);
            }
            finally
            {
                RemoveIfExists(codeFile);
            }
        }

        [Theory]
        [InlineData("lf", true)]
        [InlineData("lf", false)]
        [InlineData("crlf", true)]
        [InlineData("crlf", false)]
        [InlineData("native", true)]
        [InlineData("native", false)]
        [InlineData(null, true)]
        [InlineData(null, false)]
        public void PostProcessUsesConfiguredLineEndings(string lineEndingStyle, bool useNullable)
        {
            var codeFile = CreateTestFile("public class TableTwo\r\n{\n}\r\n");

            try
            {
                var method = typeof(ReverseEngineerRunner).GetMethod("PostProcess", BindingFlags.NonPublic | BindingFlags.Static);
                Assert.NotNull(method);
                method.Invoke(null, new object[] { codeFile, useNullable, new GeneratedFileFormat(lineEndingStyle, null) });

                var lineEnding = lineEndingStyle switch
                {
                    "lf" => "\n",
                    "crlf" => "\r\n",
                    _ => Environment.NewLine,
                };
                var nullableDirective = useNullable ? "#nullable enable" : "#nullable disable";
                var expected = string.Join(lineEnding, PathHelper.Header, nullableDirective, "public class TableTwo", "{", "}");

                Assert.Equal(expected, File.ReadAllText(codeFile, Encoding.UTF8));
            }
            finally
            {
                RemoveIfExists(codeFile);
            }
        }

        [Theory]
        [InlineData("lf", "utf-8")]
        [InlineData("crlf", "utf-8")]
        [InlineData("native", "utf-8")]
        [InlineData("lf", "utf-8-bom")]
        [InlineData("crlf", "utf-8-bom")]
        [InlineData("native", "utf-8-bom")]
        public void RetryFileWriteNormalizesMixedMarkdownLineEndings(string lineEndingStyle, string charset)
        {
            var codeFile = CreateTestFile(string.Empty);

            try
            {
                ReverseEngineerRunner.RetryFileWrite(
                    codeFile,
                    "# caf\u00e9\r\n\r\n```xml\n<PackageReference />\r```\n",
                    new GeneratedFileFormat(lineEndingStyle, charset));

                var lineEnding = lineEndingStyle switch
                {
                    "lf" => "\n",
                    "crlf" => "\r\n",
                    _ => Environment.NewLine,
                };
                var expectedText = string.Join(lineEnding, "# caf\u00e9", string.Empty, "```xml", "<PackageReference />", "```", string.Empty);
                var expectedBytes = Encoding.UTF8.GetBytes((charset == "utf-8-bom" ? "\uFEFF" : string.Empty) + expectedText);

                Assert.Equal(expectedBytes, File.ReadAllBytes(codeFile));
            }
            finally
            {
                RemoveIfExists(codeFile);
            }
        }

        [Theory]
        [InlineData(null, true)]
        [InlineData(null, false)]
        [InlineData("utf-8-bom", true)]
        [InlineData("utf-8-bom", false)]
        [InlineData("UTF-8-BOM", true)]
        [InlineData("UTF-8-BOM", false)]
        public void RetryFileWriteWritesBomForDefaultCharset(string charset, bool writeLines)
        {
            var codeFile = CreateTestFile(string.Empty);

            try
            {
                WriteWithCharset(codeFile, charset, writeLines);

                Assert.Equal(new byte[] { 0xEF, 0xBB, 0xBF }, File.ReadAllBytes(codeFile)[..3]);
                Assert.Equal("caf\u00e9\n\u4e2d\u6587\n", File.ReadAllText(codeFile, Encoding.UTF8));
            }
            finally
            {
                RemoveIfExists(codeFile);
            }
        }

        [Theory]
        [InlineData("utf-8", true)]
        [InlineData("utf-8", false)]
        [InlineData("UTF-8", true)]
        [InlineData("UTF-8", false)]
        public void RetryFileWriteWritesNoBomForUtf8Charset(string charset, bool writeLines)
        {
            var codeFile = CreateTestFile(string.Empty);

            try
            {
                WriteWithCharset(codeFile, charset, writeLines);

                Assert.Equal(Encoding.UTF8.GetBytes("caf\u00e9\n\u4e2d\u6587\n"), File.ReadAllBytes(codeFile));
            }
            finally
            {
                RemoveIfExists(codeFile);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void RetryFileWritePreservesExistingFileWhenCharsetIsUnsupported(bool writeLines)
        {
            var codeFile = CreateTestFile("existing contents\r\n");

            try
            {
                var originalBytes = File.ReadAllBytes(codeFile);

                Assert.Throws<ArgumentOutOfRangeException>(() => WriteWithCharset(codeFile, "latin1", writeLines));

                Assert.Equal(originalBytes, File.ReadAllBytes(codeFile));
            }
            finally
            {
                RemoveIfExists(codeFile);
            }
        }

        private static void WriteWithCharset(string codeFile, string charset, bool writeLines)
        {
            var fileFormat = new GeneratedFileFormat("lf", charset);

            if (writeLines)
            {
                ReverseEngineerRunner.RetryFileWrite(codeFile, new System.Collections.Generic.List<string> { "caf\u00e9", "\u4e2d\u6587" }, fileFormat);
            }
            else
            {
                ReverseEngineerRunner.RetryFileWrite(codeFile, "caf\u00e9\n\u4e2d\u6587\n", fileFormat);
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
