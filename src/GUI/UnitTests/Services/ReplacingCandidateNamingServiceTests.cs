using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using NUnit.Framework;
using ReverseEngineer20.ReverseEngineer;
using System.Collections.Generic;
using System.Reflection;

namespace UnitTests.Services
{
    [TestFixture]
    public class ReplacingCandidateNamingServiceTests
    {
        [Test]
        public void GeneratePascalCaseTableNameWithSchemaName()
        {
            //Arrange
            var expected = "MachineAlertUi";
            var exampleOption = new List<Schema>
              {
                  new Schema
                  {
                       SchemaName = "machine",
                       UseSchemaName = true
                  }
              };

            var sut = new ReplacingCandidateNamingService(exampleOption);

            var exampleDbTable = new DatabaseTable
            {
                Name = "alert_ui",
                Schema = "machine"
            };

            // Act
            var result = sut.GenerateCandidateIdentifier(exampleDbTable);

            //Assert
            StringAssert.Contains(expected, result);
        }

        [Test]
        public void GeneratePascalCaseTableNameWithSchemaNameWithMoreThanTwoSchemas()
        {
            //Arrange
            var expected = "MachineAlertUi";
            var exampleOption = new List<Schema>
              {
                  new Schema
                  {
                       SchemaName = "machine",
                       UseSchemaName = true
                  },
                  new Schema
                  {
                       SchemaName = "master",
                       UseSchemaName = true
                  }
              };

            var sut = new ReplacingCandidateNamingService(exampleOption);

            var exampleDbTable = new DatabaseTable
            {
                Name = "alert_ui",
                Schema = "machine"
            };

            // Act
            var result = sut.GenerateCandidateIdentifier(exampleDbTable);


            //Assert
            StringAssert.Contains(expected, result);
        }

        [Test]
        public void GeneratePascalCaseTableNameWithSchemaNameWithMoreThanTwoSchemasForTableCollection()
        {
            //Arrange
            var expected = "MachineAlertUi";
            var expected2 = "MachineMeasure";
            var expected3 = "MasterMeasure";
            var expected4 = "DboWorkType";
            var exampleOption = new List<Schema>
              {
                  new Schema
                  {
                       SchemaName = "machine",
                       UseSchemaName = true
                  },
                  new Schema
                  {
                       SchemaName = "master",
                       UseSchemaName = true
                  },
                  new Schema
                  {
                       SchemaName = "dbo",
                       UseSchemaName = true,
                       Tables = new List<TableRenamer>{ new TableRenamer {  Name = "work_type"} }
                  }
              };

            var sut = new ReplacingCandidateNamingService(exampleOption);

            var exampleDbTables = new List<DatabaseTable>
            {
                new DatabaseTable {
                    Name = "alert_ui",
                    Schema = "machine"
                },
                new DatabaseTable {
                    Name = "measure",
                    Schema = "machine"
                },
                new DatabaseTable {
                    Name = "measure",
                    Schema = "master"
                },
                new DatabaseTable {
                    Name = "work_type",
                    Schema = "dbo"
                }
            };

            // Act

            List<string> results = new List<string>();
            foreach (var table in exampleDbTables)
            {
                results.Add(sut.GenerateCandidateIdentifier(table));
            }

            //Assert
            StringAssert.Contains(expected, results[0]);
            StringAssert.Contains(expected2, results[1]);
            StringAssert.Contains(expected3, results[2]);
            StringAssert.Contains(expected4, results[3]);
        }

        [Test]
        public void GeneratePascalCaseTableNameWithMoreThanTwoSchemasForTableCollection()
        {
            //Arrange
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
                       UseSchemaName = true
                  },
                  new Schema
                  {
                       SchemaName = "master",
                       UseSchemaName = true
                  },
                  new Schema
                  {
                       SchemaName = "dbo",
                       UseSchemaName = false
                  }
              };

            var sut = new ReplacingCandidateNamingService(exampleOption);

            var exampleDbTables = new List<DatabaseTable>
            {
                new DatabaseTable {
                    Name = "alert_ui",
                    Schema = "machine"
                },
                new DatabaseTable {
                    Name = "measure",
                    Schema = "machine"
                },
                new DatabaseTable {
                    Name = "measure",
                    Schema = "master"
                },
                new DatabaseTable {
                    Name = "work_cell",
                    Schema = "dbo"
                },
                new DatabaseTable {
                    Name = "different_table",
                    Schema = "other"
                }
            };

            // Act
            List<string> results = new List<string>();
            foreach (var table in exampleDbTables)
            {
                results.Add(sut.GenerateCandidateIdentifier(table));
            }

            //Assert
            StringAssert.Contains(expected, results[0]);
            StringAssert.Contains(expected2, results[1]);
            StringAssert.Contains(expected3, results[2]);
            StringAssert.Contains(expected4, results[3]);
            StringAssert.Contains(expected5, results[4]);
        }

        [Test]
        public void GenerateCustomTableNameFromJson()
        {
            //Arrange
            var expected = "new_name_of_the_table";
            var exampleOption = new List<Schema>
              {
                  new Schema
                  {
                       SchemaName = "machine",
                       Tables = new List<TableRenamer>
                        {
                         new TableRenamer{
                             NewName = "new_name_of_the_table",
                             Name = "OldTableName"
                         }
                        }

                  }
              };

            var sut = new ReplacingCandidateNamingService(exampleOption);

            var exampleDbTable = new DatabaseTable
            {
                Name = "OldTableName",
                Schema = "machine"
            };

            // Act
            var result = sut.GenerateCandidateIdentifier(exampleDbTable);

            //Assert
            StringAssert.Contains(expected, result);
        }

        [Test]
        public void GenerateCustomTableNameFromJsonWithTableCollection()
        {
            //Arrange
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
                         new TableRenamer{
                             NewName = "new_name_of_the_table",
                             Name = "OldTableName"
                         }
                        }

                  }
              };

            var sut = new ReplacingCandidateNamingService(exampleOption);

            var exampleDbTables = new List<DatabaseTable>
            {
                new DatabaseTable
                {
                    Name = "OldTableName",
                    Schema = "machine"
                },
                new DatabaseTable{
                    Name = "alert_ui",
                    Schema = "machine"
                },
                new DatabaseTable{
                    Name = "WorkType",
                    Schema = "dbo"
                }

            };

            var result = new List<string>();

            // Act
            foreach (var exmapleTable in exampleDbTables)
            {
                result.Add(sut.GenerateCandidateIdentifier(exmapleTable));
            }

            //Assert
            StringAssert.Contains(expected, result[0]);
            StringAssert.Contains(expected1, result[1]);
            StringAssert.Contains(expected2, result[2]);
        }

        [Test]
        public void GenerateColumnNameFromJson()
        {
            //Arrange
            var exampleColumn = new DatabaseColumn
            {
                Name = "OldDummyColumnName",
                Table = new DatabaseTable
                {
                    Name = "table_name",
                    Schema = "machine"

                }
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
                                        NewName = "NewDummyColumnName"
                                   }
                               }
                          }
                       }
                  }
              };

            var sut = new ReplacingCandidateNamingService(exampleOption);

            //Act
            var actResult = sut.GenerateCandidateIdentifier(exampleColumn);

            //Assert
            StringAssert.Contains(expected, actResult);
        }

        [Test]
        public void GenerateColumnNameInStandarNamingConvention()
        {
            //Arrange
            var exampleColumn = new DatabaseColumn
            {
                Name = "column_name_to_rename",
                Table = new DatabaseTable
                {
                    Name = "table_name",
                    Schema = "dbo"

                }
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
                                        NewName = "NewDummyColumnName"
                                   }
                               }
                          }
                       }
                  }
              };

            var sut = new ReplacingCandidateNamingService(exampleOption);

            //Act
            var actResult = sut.GenerateCandidateIdentifier(exampleColumn);

            //Assert
            StringAssert.Contains(expected, actResult);
        }

        [Test]
        public void ChangeColumnNameToCustom()
        {
            //Arrange
            var exampleColumns = new List<DatabaseColumn>
            {
                new DatabaseColumn{
                    Name = "column_name_to_rename",
                    Table = new DatabaseTable
                    {
                        Name = "table_name",
                        Schema = "dbo"
                    }
                },
                new DatabaseColumn{
                    Name = "measure",
                    Table = new DatabaseTable
                    {
                        Name = "table_name",
                        Schema = "machine"
                    }
                }
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
                                        NewName = "NewDummyColumnName"
                                   }
                               }
                          }
                       }
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
                                        NewName = "SomethingCustom"
                                   }
                               }
                          }
                       }
                  }
              };

            var sut = new ReplacingCandidateNamingService(exampleOption);

            //Act
            var actResult = new List<string>();

            foreach (var column in exampleColumns)
            {
                actResult.Add(sut.GenerateCandidateIdentifier(column));
            }

            //Assert
            StringAssert.Contains(expected, actResult[0]);
            StringAssert.Contains(expected1, actResult[1]);
        }

        [Test]
        public void GenerateIForeignKeyForSelfReferenceTable()
        {
            //Arrange
            var expected = "SelfRef";
            var exampleOption = new List<Schema>
            {
            };

            var sut = new ReplacingCandidateNamingService(exampleOption);

            var fk = CreateSelfRefFK();

            // Act
            var result = sut.GetDependentEndCandidateNavigationPropertyName(fk);


            //Assert
            StringAssert.Contains(expected, result);
            //Assert.AreSame(fk.PrincipalEntityType, fk.ResolveOtherEntityType(fk.DeclaringEntityType));
        }

        [Test]
        public void GenerateIForeignKeyNameWithSchemaName()
        {
            //Arrange
            var expected = "SchemaOneToManyPrincipal";
            var exampleOption = new List<Schema>
            {
                new Schema{ SchemaName = "schema", UseSchemaName = true}
            };

            var sut = new ReplacingCandidateNamingService(exampleOption);

            var fk = CreateOneToManyFK();

            // Act
            var result = sut.GetDependentEndCandidateNavigationPropertyName(fk);


            //Assert
            StringAssert.Contains(expected, result);
        }

        [Test]
        public void GenerateIForeignKeyNameWithoutSchemaName()
        {
            //Arrange
            var expected = "OneToManyPrincipal";
            var exampleOption = new List<Schema>
            {
                new Schema{ SchemaName = "schema", UseSchemaName = false}
            };

            var sut = new ReplacingCandidateNamingService(exampleOption);

            var fk = CreateOneToManyFK();

            // Act
            var result = sut.GetDependentEndCandidateNavigationPropertyName(fk);


            //Assert
            StringAssert.Contains(expected, result);
        }

        private ForeignKey CreateSelfRefFK(bool useAltKey = false)
        {
            var entityType = new Model().AddEntityType(typeof(SelfRef));
            entityType.Scaffolding().Schema = "SchemaName";
            var pk = entityType.GetOrSetPrimaryKey(entityType.AddProperty(SelfRef.IdProperty));
            var fkProp = entityType.AddProperty(SelfRef.SelfRefIdProperty);

            var property = entityType.AddProperty("AltId", typeof(int));
            var principalKey = useAltKey
                ? entityType.GetOrAddKey(property)
                : pk;

            var fk = entityType.AddForeignKey(new[] { fkProp }, principalKey, entityType);
            fk.IsUnique = true;
            fk.HasDependentToPrincipal(SelfRef.SelfRefPrincipalProperty);
            fk.HasPrincipalToDependent(SelfRef.SelfRefDependentProperty);
            return fk;
        }

        private class SelfRef
        {
            public static readonly PropertyInfo IdProperty = typeof(SelfRef).GetProperty(nameof(Id));
            public static readonly PropertyInfo SelfRefIdProperty = typeof(SelfRef).GetProperty(nameof(SelfRefId));
            public static readonly PropertyInfo SelfRefPrincipalProperty = typeof(SelfRef).GetProperty(nameof(SelfRefPrincipal));
            public static readonly PropertyInfo SelfRefDependentProperty = typeof(SelfRef).GetProperty(nameof(SelfRefDependent));

            public int Id { get; set; }
            public SelfRef SelfRefPrincipal { get; set; }
            public SelfRef SelfRefDependent { get; set; }
            public int? SelfRefId { get; set; }
        }

        private ForeignKey CreateOneToManyFK()
        {
            var model = new Model();
            var principalEntityType = model.AddEntityType(typeof(OneToManyPrincipal));

            var property = principalEntityType.AddProperty(NavigationBase.IdProperty);
            var pk = principalEntityType.GetOrSetPrimaryKey(property);

            var dependentEntityType = model.AddEntityType(typeof(OneToManyDependent));
            var fkProp = dependentEntityType.AddProperty(OneToManyDependent.ForeignKeyProperty);
            var fk = dependentEntityType.AddForeignKey(new[] { fkProp }, pk, principalEntityType);
            principalEntityType.Scaffolding().Schema = "schema";
            fk.HasPrincipalToDependent(NavigationBase.OneToManyDependentsProperty);
            fk.HasDependentToPrincipal(NavigationBase.OneToManyPrincipalProperty);
            return fk;
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
