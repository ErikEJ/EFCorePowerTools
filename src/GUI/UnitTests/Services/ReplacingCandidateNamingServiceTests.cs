using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Newtonsoft.Json;
using NUnit.Framework;
using ReverseEngineer20.ReverseEngineer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                       DatabaseCustomNameOptions = new List<TableRenamer>{ new TableRenamer {  OldTableName = "work_type"} }
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
                       DatabaseCustomNameOptions = new List<TableRenamer>
                        {
                         new TableRenamer{
                             NewTableName = "new_name_of_the_table",
                             OldTableName = "OldTableName"
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
            var expected1 = "alert_ui";
            var expected2 = "WorkType";

            var exampleOption = new List<Schema>
              {
                  new Schema
                  {
                       SchemaName = "machine",
                       DatabaseCustomNameOptions = new List<TableRenamer>
                        {
                         new TableRenamer{
                             NewTableName = "new_name_of_the_table",
                             OldTableName = "OldTableName"
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
                       DatabaseCustomNameOptions = new List<TableRenamer>
                       {
                          new TableRenamer
                          {
                               OldTableName = "table_name",
                               Columns = new List<ColumnNamer>
                               {
                                   new ColumnNamer
                                   {
                                        OldColumnName = "OldDummyColumnName",
                                        NewColumnName = "NewDummyColumnName"
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
                       DatabaseCustomNameOptions = new List<TableRenamer>
                       {
                          new TableRenamer
                          {
                               OldTableName = "table_name",
                               Columns = new List<ColumnNamer>
                               {
                                   new ColumnNamer
                                   {
                                        OldColumnName = "OldDummyColumnName",
                                        NewColumnName = "NewDummyColumnName"
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

    }
}
