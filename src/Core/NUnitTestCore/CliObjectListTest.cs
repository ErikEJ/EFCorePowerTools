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

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(7));

            Assert.That(result.Any(x => x.Name == "[dbo].[Users]"), Is.True);

            Assert.That(result.Any(x => x.Name == "[other].[Accounts]"), Is.True);

            Assert.That(result.Any(x => x.Name == "[dbo].[Extras]"), Is.True);

            Assert.That(result.Any(x => x.Name == "[dbo].[Actors]"), Is.True);

            Assert.That(result.Any(x => x.Name == "[dbo].[Directors]"), Is.True);

            Assert.That(result.Any(x => x.Name == "[dbo].[UsersView]"), Is.True);

            Assert.That(result.Any(x => x.Name == "TestProcedure"), Is.True);

            Assert.That(result[6].UseLegacyResultSetDiscovery, Is.True);

            Assert.That(result[6].MappedType, Is.EqualTo("MyNamespace.MyType"));
        }

        [Test]
        public void CanUseGobalFilter()
        {
            var config = GetConfig();

            config.Views.Add(new View { ExclusionWildcard = "*" });
            config.Tables.Add(new Table { ExclusionWildcard = "*" });

            var result = CliConfigMapper.BuildObjectList(config);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Name.StartsWith("Dummy"), Is.True);
        }

        [Test]
        public void CanAddStartsWithFilter()
        {
            var config = GetConfig();

            config.Tables.Add(new Table { ExclusionWildcard = "[dbo]*" });

            var result = CliConfigMapper.BuildObjectList(config);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(3));
        }

        [Test]
        public void CanAddStartsWithFilter2()
        {
            var config = GetConfig();

            config.Tables.Add(new Table { ExclusionWildcard = "[other]*" });

            var result = CliConfigMapper.BuildObjectList(config);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(6));
        }

        [Test]
        public void CanAddStartsWithFilterAndExplicitInclude()
        {
            var config = GetConfig();

            config.Tables.First().Exclude = false;

            config.Tables.Add(new Table { ExclusionWildcard = "[dbo]*" });

            var result = CliConfigMapper.BuildObjectList(config);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(4));
        }

        [Test]
        public void MultipleExclusionWildcardExcludes()
        {
            var config = GetConfig();

            config.Tables.Add(new Table { ExclusionWildcard = "*Users*" });
            config.Tables.Add(new Table { ExclusionWildcard = "*Accounts*" });
            config.Views.Add(new View { ExclusionWildcard = "*Users*" });

            var result = CliConfigMapper.BuildObjectList(config);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(4));
        }

        [Test]
        public void CanAddEndsWithFilter()
        {
            var config = GetConfig();

            config.Tables.Add(new Table { ExclusionWildcard = "*ors]" });

            var result = CliConfigMapper.BuildObjectList(config);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(5));
        }

        [Test]
        public void CanAddEndsWithFilterAndExplicitInclude()
        {
            var config = GetConfig();

            config.Tables.First().Exclude = false;

            config.Tables.Add(new Table { ExclusionWildcard = "*ors]" });

            var result = CliConfigMapper.BuildObjectList(config);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(5));
        }

        [Test]
        public void CanAddContainsFilter()
        {
            var config = GetConfig();

            config.Tables.Add(new Table { ExclusionWildcard = "*].[Ac*" });

            var result = CliConfigMapper.BuildObjectList(config);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(5));
        }

        [Test]
        public void CanAddContainsFilterAndExplicitInclude()
        {
            var config = GetConfig();

            config.Tables[1].Exclude = false;

            config.Tables.Add(new Table { ExclusionWildcard = "*].[Ac*" });

            var result = CliConfigMapper.BuildObjectList(config);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(6));
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
                    GetDefaultTables(DatabaseType.SQLServer), CodeGenerationMode.EFCore8,
                    out CliConfig resultConfig,
                    out List<string> warnings);

                Assert.That(fetchedConfigSuccess, Is.True);
                Assert.That(resultConfig, Is.Not.Null);
                Assert.That(config.Tables.Count, Is.EqualTo(resultConfig.Tables.Count));

                for (var i = 0; i < config.Tables.Count; i++)
                {
                    Assert.That(resultConfig.Tables[i].Name, Is.EqualTo(config.Tables[i].Name));
                    Assert.That(resultConfig.Tables[i].Exclude, Is.EqualTo(config.Tables[i].Exclude));
                    Assert.That(resultConfig.Tables[i].ExclusionWildcard, Is.EqualTo(config.Tables[i].ExclusionWildcard));
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
                    GetDefaultTables(DatabaseType.SQLServer), CodeGenerationMode.EFCore8,
                    out CliConfig resultConfig,
                    out List<string> warnings);

                Assert.That(fetchedConfigSuccess, Is.True);
                Assert.That(resultConfig, Is.Not.Null);
                Assert.That(config.Tables.Count, Is.EqualTo(resultConfig.Tables.Count));

                for (var i = 0; i < config.Tables.Count; i++)
                {
                    Assert.That(resultConfig.Tables[i].Name, Is.EqualTo(config.Tables[i].Name));
                    Assert.That(resultConfig.Tables[i].Exclude, Is.EqualTo(config.Tables[i].Exclude));
                    Assert.That(resultConfig.Tables[i].ExclusionWildcard, Is.EqualTo(config.Tables[i].ExclusionWildcard));
                }

                Assert.That(warnings.Count, Is.EqualTo(4));
                Assert.That(resultConfig.Tables[0].ExcludedColumns.Count, Is.EqualTo(3));
                Assert.That(resultConfig.Tables[0].ExcludedColumns[0], Is.EqualTo("UserId"));
                Assert.That(resultConfig.Views[0].ExcludedColumns.Count, Is.EqualTo(3));
                Assert.That(resultConfig.Views[0].ExcludedColumns[0], Is.EqualTo("UserId"));
                Assert.That(resultConfig.Functions[0].ExcludedColumns, Is.Null);
                Assert.That(resultConfig.StoredProcedures[0].ExcludedColumns, Is.Null);
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
                },
                StoredProcedures = new List<StoredProcedure>()
                {
                    new StoredProcedure()
                    {
                        Name = "TestProcedure",
                        UseLegacyResultsetDiscovery = true,
                        MappedType = "MyNamespace.MyType",
                    }
                }
            };

            return config;
        }
    }
}
