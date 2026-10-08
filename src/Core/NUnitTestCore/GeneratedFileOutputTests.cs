using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.Extensions.DependencyInjection;
using RevEng.Common;
using RevEng.Core;
using RevEng.Core.Routines.Procedures;
using Xunit;

namespace UnitTests
{
    public sealed class GeneratedFileOutputTests : IDisposable
    {
        private readonly string directory = Path.Combine(Path.GetTempPath(), "EFCorePowerTools.Tests", Guid.NewGuid().ToString("N"));

        public GeneratedFileOutputTests()
        {
            Directory.CreateDirectory(directory);
        }

        [Theory]
        [InlineData("lf", "utf-8", "\n", false)]
        [InlineData("crlf", "utf-8", "\r\n", false)]
        [InlineData("LF", "UTF-8-BOM", "\n", true)]
        [InlineData("crlf", "utf-8-bom", "\r\n", true)]
        [InlineData("native", "utf-8-bom", null, true)]
        [InlineData(null, null, null, true)]
        public void WritersPreserveUnicodeAndApplyFormat(string lineEndings, string encoding, string newLine, bool bom)
        {
            newLine ??= Environment.NewLine;
            var path = Path.Combine(directory, "output.cs");
            // Rewriting a file must remove an existing BOM when requested.
            File.WriteAllText(path, "old", Encoding.UTF8);
            ReverseEngineerRunner.RetryFileWrite(path, "café\r\n東京\nlast\r", lineEndings, encoding);
            AssertBytes(path, $"café{newLine}東京{newLine}last{newLine}", bom);

            ReverseEngineerRunner.RetryFileWrite(path, new List<string> { "café\r\n東京", "last" }, lineEndings, encoding);
            AssertBytes(path, $"café{newLine}東京{newLine}last{newLine}", bom);
        }

        [Fact]
        public void InvalidFormatsFallBackWithWarnings()
        {
            var options = new ReverseEngineerCommandOptions { FileLineEndingStyle = "bad", FileEncoding = "ascii" };
            var warnings = new List<string>();
            GeneratedFileWriter.ValidateOptions(options, warnings);
            Assert.Equal("native", options.FileLineEndingStyle);
            Assert.Equal("utf-8-bom", options.FileEncoding);
            Assert.Equal(2, warnings.Count);
        }

        [Fact]
        public void UnsupportedFormatDoesNotTruncateExistingFile()
        {
            var path = Path.Combine(directory, "output.cs");
            File.WriteAllText(path, "keep");
            Assert.Throws<ArgumentOutOfRangeException>(() => GeneratedFileWriter.WriteAllLines(path, new[] { "replace" }, "bad"));
            Assert.Equal("keep", File.ReadAllText(path));
        }

        [Theory]
        [InlineData("{}", "native", "utf-8-bom")]
        [InlineData("{\"FileLineEndingStyle\":\"lf\",\"FileEncoding\":\"utf-8\"}", "lf", "utf-8")]
        public void ExtensionConfigurationPreservesFormat(string json, string lineEndings, string encoding)
        {
            var serializer = new DataContractJsonSerializer(typeof(ReverseEngineerOptions));
            using var input = new MemoryStream(Encoding.UTF8.GetBytes(json));
            var options = (ReverseEngineerOptions)serializer.ReadObject(input);
            Assert.Equal(lineEndings, options.FileLineEndingStyle);
            Assert.Equal(encoding, options.FileEncoding);
            using var output = new MemoryStream();
            serializer.WriteObject(output, options);
            output.Position = 0;
            var restored = (ReverseEngineerOptions)serializer.ReadObject(output);
            Assert.Equal(lineEndings, restored.FileLineEndingStyle);
            Assert.Equal(encoding, restored.FileEncoding);
        }

        [Theory]
        [InlineData("lf", "utf-8", false)]
        [InlineData("crlf", "utf-8", false)]
        [InlineData("lf", "utf-8", true)]
        [InlineData("crlf", "utf-8-bom", true)]
        [InlineData("native", "utf-8-bom", false)]
        public void ScaffoldingPreservesFormatThroughPostProcessing(string lineEndings, string encoding, bool split)
        {
            var connectionString = new SqliteConnectionStringBuilder { DataSource = Path.Combine(directory, "test.db"), Pooling = false }.ToString();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                using var command = connection.CreateCommand();
                command.CommandText = "CREATE TABLE Widget (Id INTEGER PRIMARY KEY, Name TEXT NOT NULL DEFAULT 'café');";
                command.ExecuteNonQuery();
            }

            var options = new ReverseEngineerCommandOptions
            {
                ConnectionString = connectionString,
                DatabaseType = DatabaseType.SQLite,
                ProjectPath = directory,
                ProjectRootNamespace = "Generated",
                ContextClassName = "TestContext",
                UseNoObjectFilter = true,
                UseFluentApiOnly = true,
                UseNullableReferences = true,
                UseDbContextSplitting = split,
                FileLineEndingStyle = lineEndings,
                FileEncoding = encoding,
            };
            var result = ReverseEngineerRunner.GenerateFiles(options);
            Assert.Empty(result.EntityErrors);
            Assert.NotEmpty(result.EntityTypeFilePaths);
            Assert.False(string.IsNullOrEmpty(result.ContextFilePath));
            if (split)
            {
                Assert.NotEmpty(result.ContextConfigurationFilePaths);
            }

            foreach (var path in result.EntityTypeFilePaths.Concat(result.ContextConfigurationFilePaths).Append(result.ContextFilePath))
            {
                AssertFormat(path, lineEndings, encoding);
            }
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public void RoutineAndExtensionFilesUseRequestedFormat(bool postgres, bool asyncCalls)
        {
            var model = new ScaffoldedModel
            {
                ContextFile = new ScaffoldedFile { Path = "ContextProcedures.cs", Code = "// café\r\n// routines\n" },
            };
            model.AdditionalFiles.Add(new ScaffoldedFile { Path = "Result.cs", Code = "// result\r\n" });
            var options = new ReverseEngineerCommandOptions { DatabaseType = postgres ? DatabaseType.Npgsql : DatabaseType.SQLServer };
            using var services = new ServiceCollection()
                .AddEfpt(options, new List<string>(), new List<string>(), new List<string>())
                .BuildServiceProvider();
            var scaffolder = services.GetRequiredService<IProcedureScaffolder>();
            var result = scaffolder.Save(model, directory, "Generated", asyncCalls, false, true, "lf", "utf-8");
            Assert.Equal(2, result.AdditionalFiles.Count);
            foreach (var path in result.AdditionalFiles.Append(result.ContextFile))
            {
                AssertFormat(path, "lf", "utf-8");
            }
        }

        public void Dispose()
        {
            Directory.Delete(directory, recursive: true);
        }

        private static void AssertFormat(string path, string lineEndings, string encoding)
        {
            var text = File.ReadAllText(path);
            Assert.Contains("\n", text);
            var expected = GeneratedFileWriter.NormalizeLineEndings(text, lineEndings);
            Assert.Equal(expected, text);
            AssertBytes(path, expected, encoding == "utf-8-bom");
        }

        private static void AssertBytes(string path, string text, bool bom)
        {
            var encoding = new UTF8Encoding(bom);
            Assert.Equal(encoding.GetPreamble().Concat(encoding.GetBytes(text)).ToArray(), File.ReadAllBytes(path));
        }
    }
}
