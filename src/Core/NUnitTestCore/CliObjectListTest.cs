using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using NUnit.Framework;
using NUnit.Framework.Legacy;
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

            ClassicAssert.NotNull(result);
            
            ClassicAssert.AreEqual(6, result.Count);
        }

        [Test]
        public void CanUseGobalFilter()
        {
            var config = GetConfig();

            config.Views.Add(new View { ExclusionWildcard = "*" });
            config.Tables.Add(new Table { ExclusionWildcard = "*" });

            var result = CliConfigMapper.BuildObjectList(config);

            ClassicAssert.NotNull(result);

            ClassicAssert.IsEmpty(result);
        }

        [Test]
        public void CanAddStartsWithFilter()
        {
            var config = GetConfig();

            config.Tables.Add(new Table { ExclusionWildcard = "[dbo]*" });

            var result = CliConfigMapper.BuildObjectList(config);

            ClassicAssert.NotNull(result);

            ClassicAssert.AreEqual(2, result.Count);
        }

        [Test]
        public void CanAddStartsWithFilter_2()
        {
            var config = GetConfig();

            config.Tables.Add(new Table { ExclusionWildcard = "[other]*" });

            var result = CliConfigMapper.BuildObjectList(config);

            ClassicAssert.NotNull(result);

            ClassicAssert.AreEqual(5, result.Count);
        }

        [Test]
        public void CanAddStartsWithFilterAndExplicitInclude()
        {
            var config = GetConfig();

            config.Tables.First().Exclude = false;

            config.Tables.Add(new Table { ExclusionWildcard = "[dbo]*" });

            var result = CliConfigMapper.BuildObjectList(config);

            ClassicAssert.NotNull(result);

            ClassicAssert.AreEqual(3, result.Count);
        }

        [Test]
        public void CanAddEndsWithFilter()
        {
            var config = GetConfig();

            config.Tables.Add(new Table { ExclusionWildcard = "*ors]" });

            var result = CliConfigMapper.BuildObjectList(config);

            ClassicAssert.NotNull(result);

            ClassicAssert.AreEqual(4, result.Count);
        }

        [Test]
        public void CanAddEndsWithFilterAndExplicitInclude()
        {
            var config = GetConfig();

            config.Tables.First().Exclude = false;

            config.Tables.Add(new Table { ExclusionWildcard = "*ors]" });

            var result = CliConfigMapper.BuildObjectList(config);

            ClassicAssert.NotNull(result);

            ClassicAssert.AreEqual(4, result.Count);
        }

        [Test]
        public void CanAddContainsFilter()
        {
            var config = GetConfig();

            config.Tables.Add(new Table { ExclusionWildcard = "*].[Ac*" });

            var result = CliConfigMapper.BuildObjectList(config);

            ClassicAssert.NotNull(result);

            ClassicAssert.AreEqual(4, result.Count);
        }

        [Test]
        public void CanAddContainsFilterAndExplicitInclude()
        {
            var config = GetConfig();

            config.Tables[1].Exclude = false;

            config.Tables.Add(new Table { ExclusionWildcard = "*].[Ac*" });

            var result = CliConfigMapper.BuildObjectList(config);

            ClassicAssert.NotNull(result);

            ClassicAssert.AreEqual(5, result.Count);
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
                    out CliConfig resultConfig,
                    out List<string> warnings);

                ClassicAssert.True(fetchedConfigSuccess);

                ClassicAssert.NotNull(resultConfig);

                ClassicAssert.True(config.Tables.Count == resultConfig.Tables.Count);

                for (var i = 0; i < config.Tables.Count; i++)
                {
                    ClassicAssert.AreEqual(config.Tables[i].Name, resultConfig.Tables[i].Name);
                    ClassicAssert.AreEqual(config.Tables[i].Exclude, resultConfig.Tables[i].Exclude);
                    ClassicAssert.AreEqual(config.Tables[i].ExclusionWildcard, resultConfig.Tables[i].ExclusionWildcard);
                }
            }
            finally
            {
                RemoveConfigFile(testPath);
            }
        }

        [Test]
        public void TryGetCliConfigWithExcludedColumnsReturns()
        {
            var config = GetConfig();
            var userTable = config.Tables.Single(x => x.Name == "[dbo].[Users]");
            userTable.ExcludedColumns = new List<string> { "UserId", "AccountId", "Name" };

            var userView = config.Views.Single(x => x.Name == "[dbo].[UsersView]");
            userView.ExcludedColumns = new List<string> { "UserId", "AccountId", "Name" };

            config.Functions = new List<Function>()
            {
                new()
                {
                    Name = "TestFunction",
                    ExcludedColumns = new List<string> { "ColumnA", "ColumnB"}
                }
            };

            config.StoredProcedures = new List<StoredProcedure>()
            {
                new()
                {
                    Name = "TestProcedure",
                    ExcludedColumns = new List<string> { "ColumnA", "ColumnB"}
                }
            };

            var testPath = TestPath("test.efpcli.json");
            try
            {
                WriteConfigFile(config, testPath);

                var fetchedConfigSuccess = CliConfigMapper.TryGetCliConfig(testPath, "fakeConnectionString",
                    DatabaseType.SQLServer,
                    GetDefaultTables(DatabaseType.SQLServer), CodeGenerationMode.EFCore7,
                    out CliConfig resultConfig,
                    out List<string> warnings);

                ClassicAssert.True(fetchedConfigSuccess);

                ClassicAssert.NotNull(resultConfig);

                ClassicAssert.True(config.Tables.Count == resultConfig.Tables.Count);

                for (var i = 0; i < config.Tables.Count; i++)
                {
                    ClassicAssert.AreEqual(config.Tables[i].Name, resultConfig.Tables[i].Name);
                    ClassicAssert.AreEqual(config.Tables[i].Exclude, resultConfig.Tables[i].Exclude);
                    ClassicAssert.AreEqual(config.Tables[i].ExclusionWildcard, resultConfig.Tables[i].ExclusionWildcard);
                }

                ClassicAssert.True(warnings.Count == 4);
                ClassicAssert.True(resultConfig.Tables[0].ExcludedColumns.Count == 3);
                ClassicAssert.True(resultConfig.Tables[0].ExcludedColumns[0] == "UserId");
                ClassicAssert.True(resultConfig.Views[0].ExcludedColumns.Count == 3);
                ClassicAssert.True(resultConfig.Views[0].ExcludedColumns[0] == "UserId");
                ClassicAssert.IsNull(resultConfig.Functions[0].ExcludedColumns);
                ClassicAssert.IsNull(resultConfig.StoredProcedures[0].ExcludedColumns);
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
                new TableModel("Users", "dbo", databaseType, ObjectType.Table, new List<ColumnModel>()
                {
                    new ColumnModel("UserId", "", true, false),
                    new ColumnModel("AccountId", "", false, true),
                    new ColumnModel("Name", "", false, false),
                    new ColumnModel("Email", "", false, false),

                }),
                new TableModel("Accounts", "other", databaseType, ObjectType.Table, new List<ColumnModel>()),
                new TableModel("Extras", "dbo", databaseType, ObjectType.Table, new List<ColumnModel>()),
                new TableModel("Actors", "dbo", databaseType, ObjectType.Table, new List<ColumnModel>()),
                new TableModel("Directors", "dbo", databaseType, ObjectType.Table, new List<ColumnModel>()),
                new TableModel("TestFunction", "dbo", databaseType, ObjectType.ScalarFunction, new List<ColumnModel>()
                {
                    new ColumnModel("ColumnA", "", false, false),
                    new ColumnModel("ColumnB", "", false, false)
                }),
                new TableModel("TestProcedure", "dbo", databaseType, ObjectType.Procedure, new List<ColumnModel>()
                {
                    new ColumnModel("ColumnA", "", false, false),
                    new ColumnModel("ColumnB", "", false, false)
                }),
                new TableModel("UsersView", "dbo", databaseType, ObjectType.View, new List<ColumnModel>()
                {
                    new ColumnModel("UserId", "", true, false),
                    new ColumnModel("AccountId", "", false, true),
                    new ColumnModel("Name", "", false, false)
                })
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
                },
                Views = new List<View>()
                {
                    new View()
                    {
                        Name = "[dbo].[UsersView]"
                    }
                }
            };

            return config;
        }
    }
}
