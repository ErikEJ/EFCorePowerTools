using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using Xunit;
using RevEng.Common;
using RevEng.Common.Cli;
using RevEng.Common.Cli.Configuration;

namespace UnitTests
{
    public class CliObjectListTest
    {
        private readonly string cliTestDirectory =
            Path.Combine(AppContext.BaseDirectory, "CliObjectListTests");

        public CliObjectListTest()
        {
            if (!Directory.Exists(cliTestDirectory))
            {
                Directory.CreateDirectory(cliTestDirectory);
            }
        }

        [Fact]
        public void CanGetConfig()
        {
            var config = GetConfig();

            var result = CliConfigMapper.BuildObjectList(config);

            Assert.NotNull(result);
            Assert.Equal(7, result.Count);
            Assert.Contains(result, x => x.Name == "[dbo].[Users]");
            Assert.Contains(result, x => x.Name == "[other].[Accounts]");
            Assert.Contains(result, x => x.Name == "[dbo].[Extras]");
            Assert.Contains(result, x => x.Name == "[dbo].[Actors]");
            Assert.Contains(result, x => x.Name == "[dbo].[Directors]");
            Assert.Contains(result, x => x.Name == "[dbo].[UsersView]");
            Assert.Contains(result, x => x.Name == "TestProcedure");

            var testProcedure = Assert.Single(result, x => x.Name == "TestProcedure");
            Assert.True(testProcedure.UseLegacyResultSetDiscovery);
            Assert.Equal("MyNamespace.MyType", testProcedure.MappedType);
        }

        [Fact]
        public void CanUseGobalFilter()
        {
            var config = GetConfig();

            config.Views.Add(new View { ExclusionWildcard = "*" });
            config.Tables.Add(new Table { ExclusionWildcard = "*" });

            var result = CliConfigMapper.BuildObjectList(config);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            var item = result[0];
            Assert.StartsWith("Dummy", item.Name);
        }
        [Fact]
        public void CanAddStartsWithFilter()
        {
            var config = GetConfig();
            config.Tables.Add(new Table { ExclusionWildcard = "[dbo]*" });
            var result = CliConfigMapper.BuildObjectList(config);
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
        }
        [Fact]
        public void CanAddStartsWithFilter2()
        {
            var config = GetConfig();

            config.Tables.Add(new Table { ExclusionWildcard = "[other]*" });

            var result = CliConfigMapper.BuildObjectList(config);

            Assert.NotNull(result);
            Assert.Equal(6, result.Count);
        }

        [Fact]
        public void CanAddStartsWithFilterAndExplicitInclude()
        {
            var config = GetConfig();

            config.Tables.First().Exclude = false;

            config.Tables.Add(new Table { ExclusionWildcard = "[dbo]*" });

            var result = CliConfigMapper.BuildObjectList(config);

            Assert.NotNull(result);
            Assert.Equal(4, result.Count);
        }

        [Fact]
        public void MultipleExclusionWildcardExcludes()
        {
            var config = GetConfig();

            config.Tables.Add(new Table { ExclusionWildcard = "*Users*" });
            config.Tables.Add(new Table { ExclusionWildcard = "*Accounts*" });
            config.Views.Add(new View { ExclusionWildcard = "*Users*" });

            var result = CliConfigMapper.BuildObjectList(config);

            Assert.NotNull(result);
            Assert.Equal(4, result.Count);
        }

        [Fact]
        public void CanAddEndsWithFilter()
        {
            var config = GetConfig();

            config.Tables.Add(new Table { ExclusionWildcard = "*ors]" });

            var result = CliConfigMapper.BuildObjectList(config);

            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
        }

        [Fact]
        public void CanAddEndsWithFilterAndExplicitInclude()
        {
            var config = GetConfig();

            config.Tables.First().Exclude = false;

            config.Tables.Add(new Table { ExclusionWildcard = "*ors]" });

            var result = CliConfigMapper.BuildObjectList(config);

            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
        }

        [Fact]
        public void CanAddContainsFilter()
        {
            var config = GetConfig();

            config.Tables.Add(new Table { ExclusionWildcard = "*].[Ac*" });

            var result = CliConfigMapper.BuildObjectList(config);

            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
        }

        [Fact]
        public void CanAddContainsFilterAndExplicitInclude()
        {
            var config = GetConfig();

            config.Tables[1].Exclude = false;

            config.Tables.Add(new Table { ExclusionWildcard = "*].[Ac*" });

            var result = CliConfigMapper.BuildObjectList(config);

            Assert.NotNull(result);
            Assert.Equal(6, result.Count);
        }

        [Fact]
        public void TryGetCliConfigWithExistingFileReturnsExpectedConfig()
        {
            var config = GetConfig();
            config.CodeGeneration.UseStoredProcedureResultSetFallback = false;

            var testPath = TestPath("test.efpcli.json");
            try
            {
                WriteConfigFile(config, testPath);

                var fetchedConfigSuccess = CliConfigMapper.TryGetCliConfig(testPath, "fakeConnectionString",
                    DatabaseType.SQLServer,
                    GetDefaultTables(DatabaseType.SQLServer), CodeGenerationMode.EFCore8,
                    out CliConfig resultConfig,
                    out List<string> warnings);

                Assert.True(fetchedConfigSuccess);
                Assert.NotNull(resultConfig);
                Assert.Equal(config.Tables.Count, resultConfig.Tables.Count);
                Assert.False(resultConfig.CodeGeneration.UseStoredProcedureResultSetFallback);

                for (var i = 0; i < config.Tables.Count; i++)
                {
                    Assert.Equal(config.Tables[i].Name, resultConfig.Tables[i].Name);
                    Assert.Equal(config.Tables[i].Exclude, resultConfig.Tables[i].Exclude);
                    Assert.Equal(config.Tables[i].ExclusionWildcard, resultConfig.Tables[i].ExclusionWildcard);
                }
            }
            finally
            {
                RemoveConfigFile(testPath);
            }
        }

        [Fact]
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

                Assert.True(fetchedConfigSuccess);
                Assert.NotNull(resultConfig);
                Assert.Equal(resultConfig.Tables.Count, config.Tables.Count);

                for (var i = 0; i < config.Tables.Count; i++)
                {
                    Assert.Equal(config.Tables[i].Name, resultConfig.Tables[i].Name);
                    Assert.Equal(config.Tables[i].Exclude, resultConfig.Tables[i].Exclude);
                    Assert.Equal(config.Tables[i].ExclusionWildcard, resultConfig.Tables[i].ExclusionWildcard);
                }

                Assert.Equal(4, warnings.Count);
                Assert.Equal(3, resultConfig.Tables[0].ExcludedColumns.Count);
                Assert.Equal("UserId", resultConfig.Tables[0].ExcludedColumns[0]);
                Assert.Equal(3, resultConfig.Views[0].ExcludedColumns.Count);
                Assert.Equal("UserId", resultConfig.Views[0].ExcludedColumns[0]);
                Assert.Null(resultConfig.Functions[0].ExcludedColumns);
                Assert.Null(resultConfig.StoredProcedures[0].ExcludedColumns);
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
