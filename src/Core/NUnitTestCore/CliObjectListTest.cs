using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using NUnit.Framework;
using RevEng.Common;
using RevEng.Common.Cli;
using RevEng.Common.Cli.Configuration;

namespace UnitTests
{
    [TestFixture]
    public class CliObjectListTest
    {

        private readonly string cliTestDirectory =
            Path.Combine(TestContext.CurrentContext.TestDirectory, "CliObjectListTests");

        [SetUp]
        public void Setup()
        {

            if (!Directory.Exists(cliTestDirectory))
                Directory.CreateDirectory(cliTestDirectory);
        }

        [Test]
        public void CanGetConfig()
        {
            var config = GetConfig();

            var result = CliConfigMapper.BuildObjectList(config);

            Assert.NotNull(result);

            Assert.AreEqual(5, result.Count);
        }

        [Test]
        public void CanUseGobalFilter()
        {
            var config = GetConfig();

            config.Tables.Add(new Table { ExclusionWildcard = "*" });

            var result = CliConfigMapper.BuildObjectList(config);

            Assert.NotNull(result);

            Assert.IsEmpty(result);
        }

        [Test]
        public void CanAddStartsWithFilter()
        {
            var config = GetConfig();

            config.Tables.Add(new Table { ExclusionWildcard = "[dbo]*" });

            var result = CliConfigMapper.BuildObjectList(config);

            Assert.NotNull(result);

            Assert.AreEqual(1, result.Count);
        }

        [Test]
        public void CanAddStartsWithFilter_2()
        {
            var config = GetConfig();

            config.Tables.Add(new Table { ExclusionWildcard = "[other]*" });

            var result = CliConfigMapper.BuildObjectList(config);

            Assert.NotNull(result);

            Assert.AreEqual(4, result.Count);
        }

        [Test]
        public void CanAddStartsWithFilterAndExplicitInclude()
        {
            var config = GetConfig();

            config.Tables.First().Exclude = false;

            config.Tables.Add(new Table { ExclusionWildcard = "[dbo]*" });

            var result = CliConfigMapper.BuildObjectList(config);

            Assert.NotNull(result);

            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public void CanAddEndsWithFilter()
        {
            var config = GetConfig();

            config.Tables.Add(new Table { ExclusionWildcard = "*ors]" });

            var result = CliConfigMapper.BuildObjectList(config);

            Assert.NotNull(result);

            Assert.AreEqual(3, result.Count);
        }

        [Test]
        public void CanAddEndsWithFilterAndExplicitInclude()
        {
            var config = GetConfig();

            config.Tables.First().Exclude = false;

            config.Tables.Add(new Table { ExclusionWildcard = "*ors]" });

            var result = CliConfigMapper.BuildObjectList(config);

            Assert.NotNull(result);

            Assert.AreEqual(3, result.Count);
        }

        [Test]
        public void CanAddContainsFilter()
        {
            var config = GetConfig();

            config.Tables.Add(new Table { ExclusionWildcard = "*].[Ac*" });

            var result = CliConfigMapper.BuildObjectList(config);

            Assert.NotNull(result);

            Assert.AreEqual(3, result.Count);
        }

        [Test]
        public void CanAddContainsFilterAndExplicitInclude()
        {
            var config = GetConfig();

            config.Tables[1].Exclude = false;

            config.Tables.Add(new Table { ExclusionWildcard = "*].[Ac*" });

            var result = CliConfigMapper.BuildObjectList(config);

            Assert.NotNull(result);

            Assert.AreEqual(4, result.Count);
        }

        [Test]
        public void TryGetCliConfigWithExistingFileReturnsExpectedConfig()
        {
            var config = GetConfig();
            var testPath = TestPath("test.efpcli.json");
            try
            {
                WriteConfigFile(config, testPath);

                var fetchedConfigSuccess = CliConfigMapper.TryGetCliConfig(testPath, "fakeConnectionString",
                    DatabaseType.SQLServer,
                    GetDefaultTables(DatabaseType.SQLServer), CodeGenerationMode.EFCore7,
                    out CliConfig resultConfig);

                Assert.True(fetchedConfigSuccess);

                Assert.NotNull(resultConfig);

                Assert.True(config.Tables.Count == resultConfig.Tables.Count);

                for (var i = 0; i < config.Tables.Count; i++)
                {
                    Assert.AreEqual(config.Tables[i].Name, resultConfig.Tables[i].Name);
                    Assert.AreEqual(config.Tables[i].Exclude, resultConfig.Tables[i].Exclude);
                    Assert.AreEqual(config.Tables[i].ExclusionWildcard, resultConfig.Tables[i].ExclusionWildcard);
                }
            }
            finally
            {
                RemoveConfigFile(testPath);
            }
        }

        private static List<TableModel> GetDefaultTables(DatabaseType databaseType)
        {
            return new List<TableModel>()
            {
                new TableModel("Users", "dbo", databaseType, ObjectType.Table, new List<ColumnModel>()),
                new TableModel("Accounts", "other", databaseType, ObjectType.Table, new List<ColumnModel>()),
                new TableModel("Extras", "dbo", databaseType, ObjectType.Table, new List<ColumnModel>()),
                new TableModel("Actors", "dbo", databaseType, ObjectType.Table, new List<ColumnModel>()),
                new TableModel("Directors", "dbo", databaseType, ObjectType.Table, new List<ColumnModel>()),
            };
        }

        private void WriteConfigFile(CliConfig configToWrite, string fullPath)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(fullPath, JsonSerializer.Serialize(configToWrite, options), Encoding.UTF8);
        }

        private void RemoveConfigFile(string fullPath)
        {
            File.Delete(fullPath);
        }

        private string TestPath(string file)
        {
            return System.IO.Path.Combine(cliTestDirectory, file);
        }

        private CliConfig GetConfig()
        {
            var config = new CliConfig
            {
                Tables = new System.Collections.Generic.List<Table>
                {
                    new Table
                    {
                        Name = "[dbo].[Users]",
                    },
                    new Table
                    {
                        Name = "[other].[Accounts]",
                    },
                    new Table
                    {
                        Name = "[dbo].[Extras]",
                    },
                    new Table
                    {
                        Name = "[dbo].[Actors]",
                    },
                    new Table
                    {
                        Name = "[dbo].[Directors]",
                    },
                }
            };

            return config;
        }
    }
}
