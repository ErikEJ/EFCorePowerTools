using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using NUnit.Framework;
using RevEng.Common;
using RevEng.Core;

namespace UnitTests.Services
{
    [TestFixture]
    public class ReplacingCandidateNamingServiceTests
    {
        [Test]
        public void GeneratePascalCaseTableNameWithSchemaName()
        {
            // Arrange
            var expected = "MachineAlertUi";
            var exampleOption = new List<Schema>
              {
                  new Schema
                  {
                       SchemaName = "machine",
                       UseSchemaName = true,
                  },
              };

            var sut = new ReplacingCandidateNamingService(false, exampleOption);

            var exampleDbTable = new DatabaseTable
            {
                Name = "alert_ui",
                Schema = "machine",
            };

            // Act
            var result = sut.GenerateCandidateIdentifier(exampleDbTable);

            // Assert
            Assert.That(result, Does.Contain(expected));
        }

        [Test]
        public void GeneratePascalCaseTableNameWithSchemaNameIssue988()
        {
            // Arrange
            var expected = "PetGuineaPig";
            var exampleOption = new List<Schema>
              {
                  new Schema
                  {
                       SchemaName = "pet",
                       UseSchemaName = true,
                       Tables = new List<TableRenamer>
                       {
                            new TableRenamer
                            {
                                Name = "GUINEAPig",
                                NewName = "GuineaPig",
                            },
                       },
                  },
              };

            var sut = new ReplacingCandidateNamingService(false, exampleOption);

            var exampleDbTable = new DatabaseTable
            {
                Name = "GUINEAPig",
                Schema = "pet",
            };

            // Act
            var result = sut.GenerateCandidateIdentifier(exampleDbTable);

            // Assert
            Assert.That(result, Does.Contain(expected));
        }

        [Test]
        public void GeneratePascalCaseTableNameWithSchemaNameWithMoreThanTwoSchemas()
        {
            // Arrange
            var expected = "MachineAlertUi";
            var exampleOption = new List<Schema>
              {
                  new Schema
                  {
                       SchemaName = "machine",
                       UseSchemaName = true,
                  },
                  new Schema
                  {
                       SchemaName = "master",
                       UseSchemaName = true,
                  },
              };

            var sut = new ReplacingCandidateNamingService(false, exampleOption);

            var exampleDbTable = new DatabaseTable
            {
                Name = "alert_ui",
                Schema = "machine",
            };

            // Act
            var result = sut.GenerateCandidateIdentifier(exampleDbTable);

            // Assert
            Assert.That(result, Does.Contain(expected));
        }

        [Test]
        public void GeneratePascalCaseTableNameWithSchemaNameWithMoreThanTwoSchemasForTableCollection()
        {
            // Arrange
            var expected = "MachineAlertUi";
            var expected2 = "MachineMeasure";
            var expected3 = "MasterMeasure";
            var expected4 = "DboWorkType";
            var exampleOption = new List<Schema>
              {
                  new Schema
                  {
                       SchemaName = "machine",
                       UseSchemaName = true,
                  },
                  new Schema
                  {
                       SchemaName = "master",
                       UseSchemaName = true,
                  },
                  new Schema
                  {
                       SchemaName = "dbo",
                       UseSchemaName = true,
                       Tables = new List<TableRenamer> { new TableRenamer { Name = "work_type" } },
                  },
              };

            var sut = new ReplacingCandidateNamingService(false, exampleOption);

            var exampleDbTables = new List<DatabaseTable>
            {
                new DatabaseTable
                {
                    Name = "alert_ui",
                    Schema = "machine",
                },
                new DatabaseTable
                {
                    Name = "measure",
                    Schema = "machine",
                },
                new DatabaseTable
                {
                    Name = "measure",
                    Schema = "master",
                },
                new DatabaseTable
                {
                    Name = "work_type",
                    Schema = "dbo",
                },
            };

            // Act
            List<string> results = new List<string>();
            foreach (var table in exampleDbTables)
            {
                results.Add(sut.GenerateCandidateIdentifier(table));
            }

            // Assert
            Assert.That(results[0], Does.Contain(expected));
            Assert.That(results[1], Does.Contain(expected2));
            Assert.That(results[2], Does.Contain(expected3));
            Assert.That(results[3], Does.Contain(expected4));
        }

        [Test]
        public void Issue354()
        {
            // Arrange
            var exampleOption = new List<Schema>
              {
                  new Schema
                  {
                       SchemaName = "stg",
                       UseSchemaName = false,
                       Tables = new List<TableRenamer>
                       {
                           new TableRenamer
                           {
                               Name = "Jobs",
                               NewName = "stg_Jobs",
                               Columns = new List<ColumnNamer>
                               {
                                   new ColumnNamer
                                   {
                                     Name = "JobName",
                                     NewName = "JobRename",
                                   },
                               },
                           },
                           new TableRenamer { Name = "DeliveryAddress", NewName = "stg_DeliveryAddress" },
                       },
                  },
              };

            var sut = new ReplacingCandidateNamingService(false, exampleOption);

            var exampleDbTables = new List<DatabaseTable>
            {
                new DatabaseTable
                {
                    Name = "DeliveryAddress",
                    Schema = "stg",
                },
                new DatabaseTable
                {
                    Name = "Jobs",
                    Schema = "stg",
                },
            };

            // Act
            var results = new List<string>();
            foreach (var table in exampleDbTables)
            {
                results.Add(sut.GenerateCandidateIdentifier(table));
            }

            // Assert
            Assert.That(results[1], Is.EqualTo("stg_Jobs").IgnoreCase);
            Assert.That(results[0], Is.EqualTo("stg_DeliveryAddress").IgnoreCase);
        }

        [Test]
        public void GeneratePascalCaseTableNameWithMoreThanTwoSchemasForTableCollection()
        {
            // Arrange
            var expected = "MachineAlertUi";
            var expected2 = "MachineMeasure";
            var expected3 = "MasterMeasure";
            var expected4 = "WorkCell";
            var expected5 = "DifferentTable";
            var exampleOption = new List<Schema>
              {
                  new Schema
                  {
                       SchemaName = "machine",
                       UseSchemaName = true,
                  },
                  new Schema
                  {
                       SchemaName = "master",
                       UseSchemaName = true,
                  },
                  new Schema
                  {
                       SchemaName = "dbo",
                       UseSchemaName = false,
                  },
              };

            var sut = new ReplacingCandidateNamingService(false, exampleOption);

            var exampleDbTables = new List<DatabaseTable>
            {
                new DatabaseTable
                {
                    Name = "alert_ui",
                    Schema = "machine",
                },
                new DatabaseTable
                {
                    Name = "measure",
                    Schema = "machine",
                },
                new DatabaseTable
                {
                    Name = "measure",
                    Schema = "master",
                },
                new DatabaseTable
                {
                    Name = "work_cell",
                    Schema = "dbo",
                },
                new DatabaseTable
                {
                    Name = "different_table",
                    Schema = "other",
                },
            };

            // Act
            List<string> results = new List<string>();
            foreach (var table in exampleDbTables)
            {
                results.Add(sut.GenerateCandidateIdentifier(table));
            }

            // Assert
            Assert.That(results[0], Does.Contain(expected));
            Assert.That(results[1], Does.Contain(expected2));
            Assert.That(results[2], Does.Contain(expected3));
            Assert.That(results[3], Does.Contain(expected4));
            Assert.That(results[4], Does.Contain(expected5));
        }

        [Test]
        public void GenerateCustomTableNameFromJson()
        {
            // Arrange
            var expected = "new_name_of_the_table";
            var exampleOption = new List<Schema>
              {
                  new Schema
                  {
                     SchemaName = "machine",
                     Tables = new List<TableRenamer>
                     {
                        new TableRenamer
                        {
                            NewName = "new_name_of_the_table",
                            Name = "OldTableName",
                        },
                     },
                  },
              };

            var sut = new ReplacingCandidateNamingService(false, exampleOption);

            var exampleDbTable = new DatabaseTable
            {
                Name = "OldTableName",
                Schema = "machine",
            };

            // Act
            var result = sut.GenerateCandidateIdentifier(exampleDbTable);

            // Assert
            Assert.That(result, Does.Contain(expected));
        }

        [Test]
        public void GenerateCustomTableNameFromJsonWithTableCollection()
        {
            // Arrange
            var expected = "new_name_of_the_table";
            var expected1 = "AlertUi";
            var expected2 = "WorkType";

            var exampleOption = new List<Schema>
            {
                new Schema
                {
                    SchemaName = "machine",
                    Tables = new List<TableRenamer>
                    {
                        new TableRenamer
                        {
                            NewName = "new_name_of_the_table",
                            Name = "OldTableName",
                        },
                    },
                },
            };

            var sut = new ReplacingCandidateNamingService(false, exampleOption);

            var exampleDbTables = new List<DatabaseTable>
            {
                new DatabaseTable
                {
                    Name = "OldTableName",
                    Schema = "machine",
                },
                new DatabaseTable
                {
                    Name = "alert_ui",
                    Schema = "machine",
                },
                new DatabaseTable
                {
                    Name = "WorkType",
                    Schema = "dbo",
                },
            };

            var result = new List<string>();

            // Act
            foreach (var exmapleTable in exampleDbTables)
            {
                result.Add(sut.GenerateCandidateIdentifier(exmapleTable));
            }

            // Assert
            Assert.That(result[0], Does.Contain(expected));
            Assert.That(result[1], Does.Contain(expected1));
            Assert.That(result[2], Does.Contain(expected2));
        }

        [Test]
        public void GenerateColumnNameFromJson()
        {
            // Arrange
            var exampleColumn = new DatabaseColumn
            {
                Name = "OldDummyColumnName",
                Table = new DatabaseTable
                {
                    Name = "table_name",
                    Schema = "machine",
                },
            };

            var expected = "NewDummyColumnName";

            var exampleOption = new List<Schema>
              {
                  new Schema
                  {
                       SchemaName = "machine",
                       UseSchemaName = true,
                       Tables = new List<TableRenamer>
                       {
                          new TableRenamer
                          {
                               Name = "table_name",
                               Columns = new List<ColumnNamer>
                               {
                                   new ColumnNamer
                                   {
                                        Name = "OldDummyColumnName",
                                        NewName = "NewDummyColumnName",
                                   },
                               },
                          },
                       },
                  },
              };

            var sut = new ReplacingCandidateNamingService(false, exampleOption);

            // Act
            var actResult = sut.GenerateCandidateIdentifier(exampleColumn);

            // Assert
            Assert.That(actResult, Does.Contain(expected));
        }

        [Test]
        public void GenerateColumnNameInStandarNamingConvention()
        {
            // Arrange
            var exampleColumn = new DatabaseColumn
            {
                Name = "column_name_to_rename",
                Table = new DatabaseTable
                {
                    Name = "table_name",
                    Schema = "dbo",
                },
            };

            var expected = "ColumnNameToRename";

            var exampleOption = new List<Schema>
              {
                  new Schema
                  {
                       SchemaName = "machine",
                       UseSchemaName = true,
                       Tables = new List<TableRenamer>
                       {
                          new TableRenamer
                          {
                               Name = "table_name",
                               Columns = new List<ColumnNamer>
                               {
                                   new ColumnNamer
                                   {
                                        Name = "OldDummyColumnName",
                                        NewName = "NewDummyColumnName",
                                   },
                               },
                          },
                       },
                  },
              };

            var sut = new ReplacingCandidateNamingService(false, exampleOption);

            // Act
            var actResult = sut.GenerateCandidateIdentifier(exampleColumn);

            // Assert
            Assert.That(actResult, Does.Contain(expected));
        }

        [Test]
        public void ChangeColumnNameToCustom()
        {
            // Arrange
            var exampleColumns = new List<DatabaseColumn>
            {
                new DatabaseColumn
                {
                    Name = "column_name_to_rename",
                    Table = new DatabaseTable
                    {
                        Name = "table_name",
                        Schema = "dbo",
                    },
                },
                new DatabaseColumn
                {
                    Name = "measure",
                    Table = new DatabaseTable
                    {
                        Name = "table_name",
                        Schema = "machine",
                    },
                },
            };

            var expected = "SomethingCustom";
            var expected1 = "Measure";

            var exampleOption = new List<Schema>
              {
                  new Schema
                  {
                       SchemaName = "machine",
                       UseSchemaName = true,
                       Tables = new List<TableRenamer>
                       {
                          new TableRenamer
                          {
                               Name = "table_name",
                               Columns = new List<ColumnNamer>
                               {
                                   new ColumnNamer
                                   {
                                        Name = "OldDummyColumnName",
                                        NewName = "NewDummyColumnName",
                                   },
                               },
                          },
                       },
                  },
                  new Schema
                  {
                       SchemaName = "dbo",
                       UseSchemaName = false,
                       Tables = new List<TableRenamer>
                       {
                          new TableRenamer
                          {
                               Name = "table_name",
                               Columns = new List<ColumnNamer>
                               {
                                   new ColumnNamer
                                   {
                                        Name = "column_name_to_rename",
                                        NewName = "SomethingCustom",
                                   },
                               },
                          },
                       },
                  },
              };

            var sut = new ReplacingCandidateNamingService(false, exampleOption);

            // Act
            var actResult = new List<string>();

            foreach (var column in exampleColumns)
            {
                actResult.Add(sut.GenerateCandidateIdentifier(column));
            }

            // Assert
            Assert.That(actResult[0], Does.Contain(expected));
            Assert.That(actResult[1], Does.Contain(expected1));
        }

        /// <summary>
        /// Testing the table renaming method using Regex.
        /// </summary>
        [Test]
        public void GenerateCustomTableNameFromJsonUsingRegexRenaming()
        {
            // Arrange
            var expected = "NewTableName";

            var exampleOption = new List<Schema>
              {
                  new Schema
                  {
                      SchemaName = "dbo",
                      TableRegexPattern = "^Old",
                      TablePatternReplaceWith = "New",
                  },
              };

            var sut = new ReplacingCandidateNamingService(false, exampleOption);

            var exampleDbTable = new DatabaseTable
            {
                Name = "OldTableName",
                Schema = "dbo",
            };

            // Act
            var result = sut.GenerateCandidateIdentifier(exampleDbTable);

            // Assert
            Assert.That(result, Does.Contain(expected));
        }

        /// <summary>
        /// Testing the table renaming method using Regex.
        /// </summary>
        [Test]
        public void GenerateCustomTableNameFromJsonUsingRegexRenamingIssue1440()
        {
            // Arrange
            var expected = "PurposeSummary";

            var exampleOption = new List<Schema>
              {
                  new Schema
                  {
                      SchemaName = "dbo",
                      TableRegexPattern = "^PREFIX_",
                      TablePatternReplaceWith = "",
                  },
              };

            var sut = new ReplacingCandidateNamingService(false, exampleOption);

            var exampleDbTable = new DatabaseTable
            {
                Name = "PREFIX_PURPOSE_SUMMARY",
                Schema = "dbo",
            };

            // Act
            var result = sut.GenerateCandidateIdentifier(exampleDbTable);

            // Assert
            Assert.That(result, Does.Contain(expected));
        }

        /// Testing the table renaming method using Regex.
        /// </summary>
        [Test]
        public void GenerateCustomTableNameFromJsonUsingRegexRenamingIssue1503()
        {
            // Arrange
            var expected = "GGD";

            var exampleOption = new List<Schema>
              {
                  new Schema
                  {
                      SchemaName = "dbo",
                      TableRegexPattern = "^tbl",
                      TablePatternReplaceWith = "",
                  },
              };

            var sut = new ReplacingCandidateNamingService(false, exampleOption, true);

            var exampleDbTable = new DatabaseTable
            {
                Name = "tblGGD",
                Schema = "dbo",
            };

            // Act
            var result = sut.GenerateCandidateIdentifier(exampleDbTable);

            // Assert
            Assert.That(result, Does.Contain(expected));
        }

        /// Testing the table renaming method using Regex.
        /// </summary>
        [Test]
        public void GenerateCustomTableNameFromJsonUsingRegexRenamingIssue1503WithCasing()
        {
            // Arrange
            var expected = "Ggd";

            var exampleOption = new List<Schema>
              {
                  new Schema
                  {
                      SchemaName = "dbo",
                      TableRegexPattern = "^tbl",
                      TablePatternReplaceWith = "",
                  },
              };

            var sut = new ReplacingCandidateNamingService(false, exampleOption, false);

            var exampleDbTable = new DatabaseTable
            {
                Name = "tblGGD",
                Schema = "dbo",
            };

            // Act
            var result = sut.GenerateCandidateIdentifier(exampleDbTable);

            // Assert
            Assert.That(result, Does.Contain(expected));
        }

        [Test]
        public void GenerateCustomColumnNameUsingRegexRenamingIssue1478()
        {
            // Arrange
            var expected = "AppleName";

            var exampleOption = new List<Schema>
              {
                  new Schema
                  {
                      SchemaName = "dbo",
                      TableRegexPattern = "^PREFIX_",
                      TablePatternReplaceWith = "",
                      ColumnPatternReplaceWith = "",
                      ColumnRegexPattern = "PREFIX_",
                      Tables = new List<TableRenamer>
                      {
                          new TableRenamer
                          {
                            Name = "whatever",
                            Columns = new List<ColumnNamer>
                            {
                                new ColumnNamer
                                {
                                    Name = "PREFIX_APPLE_NAME",
                                    NewName = "AppleName",
                                },
                            },
                          },
                      },
                  },
              };

            var sut = new ReplacingCandidateNamingService(false, exampleOption);

            var exampleDbColumn = new DatabaseColumn
            {
                Name = "PREFIX_APPLE_NAME",
                Table = new DatabaseTable
                { 
                    Schema = "dbo",
                    Name = "whatever",
                },
            };

            // Act
            var result = sut.GenerateCandidateIdentifier(exampleDbColumn);

            // Assert
            Assert.That(result, Does.Contain(expected));
        }

        [Test]
        public void GenerateCustomColumnNameUsingRegexRenamingIssue1478Preserve()
        {
            // Arrange
            var expected = "APPLE_NAME";

            var exampleOption = new List<Schema>
              {
                  new Schema
                  {
                      SchemaName = "dbo",
                      TableRegexPattern = "^PREFIX_",
                      TablePatternReplaceWith = "",
                      ColumnPatternReplaceWith = "",
                      ColumnRegexPattern = "PREFIX_",
                  },
              };

            var sut = new ReplacingCandidateNamingService(false, exampleOption, true);

            var exampleDbColumn = new DatabaseColumn
            {
                Name = "PREFIX_APPLE_NAME",
                Table = new DatabaseTable
                {
                    Schema = "dbo",
                    Name = "whatever",
                },
            };

            // Act
            var result = sut.GenerateCandidateIdentifier(exampleDbColumn);

            // Assert
            Assert.That(result, Does.Contain(expected));
        }

        [Test]
        public void GenerateCustomColumnNameUsingRegexRenamingIssue1503()
        {
            // Arrange
            var expected = "ThisIsUPPERCASE";

            var exampleOption = new List<Schema>
              {
                  new Schema
                  {
                      SchemaName = "dbo",
                      TableRegexPattern = "^tbl",
                      TablePatternReplaceWith = "",
                      ColumnPatternReplaceWith = "",
                      ColumnRegexPattern = "^(vch|bit|dtm|int|dec|nvc|sin|chr|nch)",
                      Tables = new List<TableRenamer>(),
                  },
              };

            var sut = new ReplacingCandidateNamingService(false, exampleOption, true);

            var exampleDbColumn = new DatabaseColumn
            {
                Name = "vchThisIsUPPERCASE",
                Table = new DatabaseTable
                {
                    Schema = "dbo",
                    Name = "whatever",
                },
            };

            // Act
            var result = sut.GenerateCandidateIdentifier(exampleDbColumn);

            // Assert
            Assert.That(result, Does.Contain(expected));
        }

        /// <summary>
        /// Testing the table renaming method using Regex.
        /// </summary>
        [Test]
        public void GenerateCustomColumnNameUsingRegexRenamingIssue1486()
        {
            // Arrange
            var expected = "UserID";

            var exampleOption = new List<Schema>
              {
                  new Schema
                  {
                      SchemaName = "dbo",
                      ColumnPatternReplaceWith = "$0",
                      ColumnRegexPattern = "^.+$",
                      Tables = new List<TableRenamer>
                      {
                          new TableRenamer
                          { 
                            Name = "Users",
                            Columns = new List<ColumnNamer>
                            { 
                                new ColumnNamer
                                { 
                                    Name = "UserID",
                                    NewName = "UserID",
                                },
                            },
                          },
                      },                       
                  },
              };

            var sut = new ReplacingCandidateNamingService(false, exampleOption);

            var exampleDbColumn = new DatabaseColumn
            {
                Name = "UserID",
                Table = new DatabaseTable
                {
                    Schema = "dbo",
                    Name = "Users",
                },
            };

            // Act
            var result = sut.GenerateCandidateIdentifier(exampleDbColumn);

            // Assert
            Assert.That(result, Does.Contain(expected));
        }

        /// <summary>
        /// This is to guarantee that the renaming using regex does not overwrite the current table renaming method.
        /// </summary>
        [Test]
        public void GenerateCustomTableNameFromJsonUsingRegexRenamingOverwritten()
        {
            // Arrange
            var expected = "TableNameNotOverwritten";

            var exampleOption = new List<Schema>
              {
                  new Schema
                  {
                      SchemaName = "dbo",
                      TableRegexPattern = "^Old",
                      TablePatternReplaceWith = "New",
                      Tables = new List<TableRenamer>
                       {
                           new TableRenamer
                           {
                               NewName = "TableNameNotOverwritten",
                               Name = "OldTableName",
                           },
                       },
                  },
              };

            var sut = new ReplacingCandidateNamingService(false, exampleOption);

            var exampleDbTable = new DatabaseTable
            {
                Name = "OldTableName",
                Schema = "dbo",
            };

            // Act
            var result = sut.GenerateCandidateIdentifier(exampleDbTable);

            // Assert
            Assert.That(result, Does.Contain(expected));
        }

        /// <summary>
        /// Testing the column renaming method using Regex.
        /// </summary>
        [Test]
        public void GenerateCustomColumnNameFromJsonUsingRegexRenaming()
        {
            // Arrange
            var exampleColumn = new DatabaseColumn
            {
                Name = "OldColumnName",
                Table = new DatabaseTable
                {
                    Name = "table_name",
                    Schema = "dbo",
                },
            };

            var expected = "NewColumnName";

            var exampleOption = new List<Schema>
              {
                  new Schema
                  {
                       SchemaName = "dbo",
                       UseSchemaName = true,
                       ColumnRegexPattern = "^Old",
                       ColumnPatternReplaceWith = "New",
                  },
              };

            var sut = new ReplacingCandidateNamingService(false, exampleOption);

            // Act
            var actResult = sut.GenerateCandidateIdentifier(exampleColumn);

            // Assert
            Assert.That(actResult, Does.Contain(expected));
        }

        /// <summary>
        /// This is to guarantee that the renaming using regex does not overwrite the current column renaming method.
        /// </summary>
        [Test]
        public void GenerateCustomColumnNameFromJsonUsingRegexRenamingOverwritten()
        {
            // Arrange
            var exampleColumn = new DatabaseColumn
            {
                Name = "OldColumnName",
                Table = new DatabaseTable
                {
                    Name = "table_name",
                    Schema = "dbo",
                },
            };

            var expected = "ColumnNameNotOverwritten";

            var exampleOption = new List<Schema>
              {
                  new Schema
                  {
                       SchemaName = "dbo",
                       UseSchemaName = true,
                       ColumnRegexPattern = "^Old",
                       ColumnPatternReplaceWith = "New",
                       Tables = new List<TableRenamer>
                       {
                          new TableRenamer
                          {
                               Name = "table_name",
                               Columns = new List<ColumnNamer>
                               {
                                   new ColumnNamer
                                   {
                                        Name = "OldColumnName",
                                        NewName = "ColumnNameNotOverwritten",
                                   },
                               },
                          },
                       },
                  },
              };

            var sut = new ReplacingCandidateNamingService(false, exampleOption);

            // Act
            var actResult = sut.GenerateCandidateIdentifier(exampleColumn);

            // Assert
            Assert.That(actResult, Does.Contain(expected));
        }

        public abstract class NavigationBase
        {
            public static readonly PropertyInfo IdProperty = typeof(NavigationBase).GetProperty(nameof(Id));
            public static readonly PropertyInfo OneToManyDependentsProperty = typeof(NavigationBase).GetProperty(nameof(OneToManyDependents));
            public static readonly PropertyInfo OneToManyPrincipalProperty = typeof(NavigationBase).GetProperty(nameof(OneToManyPrincipal));

            public int Id { get; set; }
            public IEnumerable<OneToManyDependent> OneToManyDependents { get; set; }
            public OneToManyPrincipal OneToManyPrincipal { get; set; }
        }

        public class OneToManyPrincipal : NavigationBase
        {
            public IEnumerable<OneToManyDependent> OneToManyDependent { get; set; }
        }

        public class DerivedOneToManyPrincipal : OneToManyPrincipal
        {
        }

        public class OneToManyDependent : NavigationBase
        {
            public static readonly PropertyInfo DeceptionProperty = typeof(OneToManyDependent).GetProperty(nameof(Deception));
            public static readonly PropertyInfo ForeignKeyProperty = typeof(OneToManyDependent).GetProperty(nameof(OneToManyPrincipalId));
            public int OneToManyPrincipalId { get; set; }
            public OneToManyPrincipal Deception { get; set; }
        }
    }
}
