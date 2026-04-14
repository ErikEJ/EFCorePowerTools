using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ErikEJ.EntityFrameworkCore.SqlServer.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Xunit;
using RevEng.Core.Abstractions;

namespace UnitTests
{
    public class DacpacTest
    {
        private const string SqlServerSparseAnnotation = "SqlServer:Sparse";
        private const string SqlServerIsTemporalAnnotation = "SqlServer:IsTemporal";
        private string dacpac;
        private string dacpacDescending;
        private string dacpacSparse;
        private string dacpacViews;
        private string dacpacQuirk;

        public DacpacTest()
        {
            dacpacQuirk = TestPath("TestDb.dacpac");
            dacpac = TestPath("Chinook.dacpac");
            dacpacDescending = TestPath("DescendingIndex.dacpac");
            dacpacSparse = TestPath("SparseColumn.dacpac");
            dacpacViews = TestPath("ViewColumnTypesSqlProj.dacpac");
            #region Here Is the SQL used to create ViewColumnTypesSqlProj.dacpac
            /*
            CREATE VIEW [dbo].[AllTypes]
AS

SELECT 
 CAST( 0x1  AS bit 				  ) col_bit 						
,CAST( 0x2  AS tinyint			  ) col_tinyint			  
,CAST( 0x3  AS smallint			  ) col_smallint		  
,CAST( 0x4  AS int				  ) col_int				  
,CAST( 0x5  AS bigint			  ) col_bigint			  
,CAST( 6  AS decimal 			  ) col_decimal 		
,CAST( 7  AS numeric 			  ) col_numeric 		
,CAST( 8  AS money				  ) col_money			
,CAST( 9  AS smallmoney		  ) col_smallmoney		  
,CAST( 10 AS float				  ) col_float			  
,CAST( 11  AS real				  ) col_real			  
,CAST( '12/12/12'  AS date				  ) col_date			
,CAST( '13:13'  AS time				  ) col_time			
,CAST( '12/14/2014 14:14' AS datetime2		  ) col_datetime2		
,CAST( '12/15/2015 15:15'  AS datetimeoffset	  ) col_datetimeoffset	  
,CAST( '12/16/2016 16:16'  AS datetime			  ) col_datetime		
,CAST( '12/17/17'  AS smalldatetime		  ) col_smalldatetime
,CAST( 0x18  AS varchar(1)			  ) col_varchar			
,CAST( '19'  AS text				  ) col_text			
,CAST( 0x20  AS nchar				  ) col_nchar			
,CAST( 0x21 AS nvarchar(1)		  ) col_nvarchar		  
,CAST( '22'  AS ntext				  ) col_ntext			
,CAST( 0x23  AS binary			  ) col_binary			  
,CAST( 0x24 AS varbinary(1)			  ) col_varbinary
,CAST( 0x25 AS image				  ) col_image			
,CAST( 0x26  AS rowversion		  ) col_rowversion		  
,CAST( 0x27 AS uniqueidentifier	  ) col_uniqueidentifier  
,CAST( '<root>28</root>'  AS xml				  ) col_xml				  

GO 

CREATE TYPE [dbo].[AllTypes] AS TABLE (
	[col_bit] [bit] NULL,
	[col_tinyint] [tinyint] NULL,
	[col_smallint] [smallint] NULL,
	[col_int] [int] NULL,
	[col_bigint] [bigint] NULL,
	[col_decimal] [decimal](18, 0) NULL,
	[col_numeric] [numeric](18, 0) NULL,
	[col_money] [money] NULL,
	[col_smallmoney] [smallmoney] NULL,
	[col_float] [float] NULL,
	[col_real] [real] NULL,
	[col_date] [date] NULL,
	[col_time] [time](7) NULL,
	[col_datetime2] [datetime2](7) NULL,
	[col_datetimeoffset] [datetimeoffset](7) NULL,
	[col_datetime] [datetime] NULL,
	[col_smalldatetime] [smalldatetime] NULL,
	[col_varchar] [varchar](1) NULL,
	[col_text] [text] NULL,
	[col_nchar] [nchar](30) NULL,
	[col_nvarchar] [nvarchar](1) NULL,
	[col_ntext] [ntext] NULL,
	[col_binary] [binary](30) NULL,
	[col_varbinary] [varbinary](1) NULL,
	[col_image] [image] NULL,
	[col_rowversion] [timestamp] NULL,
	[col_uniqueidentifier] [uniqueidentifier] NULL,
	[col_xml] [xml] NULL
) 
GO
            */
            #endregion
        }

        [Fact]
        public void CanEnumerateTables()
        {
            // Arrange
            var factory = new SqlServerDacpacDatabaseModelFactory();
            var options = new DatabaseModelFactoryOptions(new List<string>(), new List<string>());

            // Act
            var dbModel = factory.Create(dacpac, options);

            // Assert
            Assert.Equal(11, dbModel.Tables.Count());
        }

        [Fact]
        public void CanEnumerateViewColumns()
        {
            // Arrange
            var factory = new SqlServerDacpacDatabaseModelFactory();
            var options = new DatabaseModelFactoryOptions(new List<string>(), new List<string>());

            // Act
            var dbModel = factory.Create(dacpacViews, options);

            // Assert
            Assert.Equal(28, dbModel.Tables[0].Columns.Count());
        }

        [Fact]
        public void CanEnumerateSelectedTables()
        {
            // Arrange
            var factory = new SqlServerDacpacDatabaseModelFactory();
            var tables = new List<string> { "[dbo].[Album]", "[dbo].[Artist]", "[dbo].[InvoiceLine]" };
            var options = new DatabaseModelFactoryOptions(tables, new List<string>());

            // Act
            var dbModel = factory.Create(dacpac, options);

            // Assert
            Assert.Equal(3, dbModel.Tables.Count());
            Assert.Equal("Album", dbModel.Tables[0].Name);
            Assert.Single(dbModel.Tables[0].ForeignKeys);
            Assert.Equal(3, dbModel.Tables[0].Columns.Count);
        }

        [Fact]
        public void CanEnumerateSelectedQuirkObjects()
        {
            // Arrange
            var factory = new SqlServerDacpacDatabaseModelFactory();
            var tables = new List<string> { "[dbo].[FilteredIndexTable]", "[dbo].[DefaultComputedValues]" };
            var options = new DatabaseModelFactoryOptions(tables, new List<string>());

            // Act
            var dbModel = factory.Create(dacpacQuirk, options);

            // Assert
            Assert.Equal(2, dbModel.Tables.Count());
            Assert.Equal("FilteredIndexTable", dbModel.Tables[1].Name);
            Assert.Empty(dbModel.Tables[1].ForeignKeys);
            Assert.Equal(2, dbModel.Tables[1].Columns.Count);
            Assert.Equal("DefaultComputedValues", dbModel.Tables[0].Name);
            Assert.Equal(6, dbModel.Tables[0].Columns.Count);
        }

        [Fact]
        public void CanEnumerateSelectedComputed()
        {
            // Arrange
            var factory = new SqlServerDacpacDatabaseModelFactory();
            var tables = new List<string> { "[dbo].[DefaultComputedValues]" };
            var options = new DatabaseModelFactoryOptions(tables, new List<string>());

            // Act
            var dbModel = factory.Create(dacpacQuirk, options);

            // Assert
            Assert.Single(dbModel.Tables);
            Assert.Equal("DefaultComputedValues", dbModel.Tables[0].Name);
            Assert.Equal(6, dbModel.Tables[0].Columns.Count);
        }

        [Fact]
        public void CanEnumerateTypeAlias()
        {
            // Arrange
            var factory = new SqlServerDacpacDatabaseModelFactory();
            var tables = new List<string> { "[dbo].[TypeAlias]" };
            var options = new DatabaseModelFactoryOptions(tables, new List<string>());

            // Act
            var dbModel = factory.Create(dacpacQuirk, options);

            // Assert
            Assert.Single(dbModel.Tables);
            Assert.Equal("TypeAlias", dbModel.Tables[0].Name);
            Assert.Equal(2, dbModel.Tables[0].Columns.Count);
            Assert.Equal("nvarchar(max)", dbModel.Tables[0].Columns[1].StoreType);
        }

        [Fact]
        public void PreservesDefaultValueSql()
        {
            // Arrange
            var factory = new SqlServerDacpacDatabaseModelFactory();
            var tables = new List<string> { "[dbo].[DefaultValues]" };
            var options = new DatabaseModelFactoryOptions(tables, new List<string>());

            // Act
            var dbModel = factory.Create(dacpacQuirk, options);

            // Assert
            Assert.Single(dbModel.Tables);
            Assert.Equal(
                new[]
                {
                    "OrderDate",
                    "Freight1",
                    "Freight2",
                    "Freight3",
                    "Freight4",
                    "Freight5",
                    "Freight6",
                    "Freight7",
                    "Freight8",
                },
                dbModel.Tables.Single().Columns
                    .Where(c => c.DefaultValueSql != null)
                    .Select(c => c.Name));
        }

        [Fact]
        public void PreservesNumericClrDefaultConstraintSql()
        {
            var factory = new SqlServerDacpacDatabaseModelFactory();
            var tables = new List<string> { "[dbo].[DefaultValues]" };
            var options = new DatabaseModelFactoryOptions(tables, new List<string>());

            var dbModel = factory.Create(dacpacQuirk, options);
            var table = dbModel.Tables.Single(t => t.Name == "DefaultValues");

            var freight2 = table.Columns.Single(c => c.Name == "Freight2");
            var freight3 = table.Columns.Single(c => c.Name == "Freight3");
            var freight4 = table.Columns.Single(c => c.Name == "Freight4");
            var freight5 = table.Columns.Single(c => c.Name == "Freight5");
            var freight6 = table.Columns.Single(c => c.Name == "Freight6");

            Assert.Equal("0", freight2.DefaultValueSql);
            Assert.Equal("0.0", freight3.DefaultValueSql);
            Assert.Equal("0.0", freight4.DefaultValueSql);
            Assert.Equal("0.0", freight5.DefaultValueSql);
            Assert.Equal("0.0", freight6.DefaultValueSql);
        }

        [Fact]
        public void CanCaptureSparseColumnAnnotation()
        {
            var factory = new SqlServerDacpacDatabaseModelFactory();
            var tables = new List<string> { "[dbo].[Department]" };
            var options = new DatabaseModelFactoryOptions(tables, new List<string>());

            var dbModel = factory.Create(dacpacSparse, options);
            var importToken = dbModel.Tables.Single().Columns.Single(c => c.Name == "ImportToken");

            Assert.Single(dbModel.Tables);
            Assert.NotNull(importToken.FindAnnotation(SqlServerSparseAnnotation));
        }

        [Fact]
        public void CanBuildAW2014()
        {
            // Arrange
            var factory = new SqlServerDacpacDatabaseModelFactory();
            var options = new DatabaseModelFactoryOptions(null, new List<string>());

            // Act
            var dbModel = factory.Create(TestPath("AdventureWorks2014.dacpac"), options);

            // Assert
            Assert.Equal(91, dbModel.Tables.Count());
        }

        [Fact]
        public void Issue208ComputedConstraint()
        {
            // Arrange
            var factory = new SqlServerDacpacDatabaseModelFactory();
            var options = new DatabaseModelFactoryOptions(null, new List<string>());

            // Act
            var dbModel = factory.Create(TestPath("Issue208.dacpac"), options);

            // Assert
            Assert.Single(dbModel.Tables);
        }

        [Fact]
        public void Issue210ComputedConstraintIsFK()
        {
            // Arrange
            var factory = new SqlServerDacpacDatabaseModelFactory();
            var options = new DatabaseModelFactoryOptions(null, new List<string>());

            // Act
            var dbModel = factory.Create(TestPath("Issue210.dacpac"), options);

            // Assert
            Assert.Equal(2, dbModel.Tables.Count());
        }

        [Fact]
        public void Issue1262ConsiderSchemaArgument()
        {
            var factory = new SqlServerDacpacDatabaseModelFactory();
            var options = new DatabaseModelFactoryOptions(null, new List<string>() { "mat" });

            // Act
            var dbModel = factory.Create(TestPath("Issue1262.dacpac"), options);

            // Assert
            Assert.Single(dbModel.Tables);
            Assert.Equal("mat", dbModel.Tables.Single().Schema);
        }

        [Fact]
        public void Issue1262BehaviourWithoutSchemaArgument()
        {
            var factory = new SqlServerDacpacDatabaseModelFactory();
            var options = new DatabaseModelFactoryOptions(null, new List<string>());

            // Act
            var dbModel = factory.Create(TestPath("Issue1262.dacpac"), options);

            // Assert
            Assert.Equal(2, dbModel.Tables.Count());
            Assert.Equal(1, dbModel.Tables.Count(x => x.Schema == "mat"));
            Assert.Equal(1, dbModel.Tables.Count(x => x.Schema == "mat2"));
        }

        [Fact]
        public void Issue3341PersistedComputedForeignKeys()
        {
            var factory = new SqlServerDacpacDatabaseModelFactory();
            var options = new DatabaseModelFactoryOptions(null, new List<string>());

            var dbModel = factory.Create(TestPath("Issue3341.dacpac"), options);

            Assert.Equal(3, dbModel.Tables.Count());

            var bin = dbModel.Tables.Single(t => t.Schema == "dbo" && t.Name == "Bin");
            var inventoryNodeTypeCode = bin.Columns.Single(c => c.Name == "InventoryNodeTypeCode");

            Assert.Equal("tinyint", inventoryNodeTypeCode.StoreType);
            Assert.Contains("CONVERT", inventoryNodeTypeCode.ComputedColumnSql);
            Assert.Contains("8", inventoryNodeTypeCode.ComputedColumnSql);
            Assert.True(inventoryNodeTypeCode.IsStored);
            Assert.False(inventoryNodeTypeCode.IsNullable);
            Assert.Equal(2, bin.ForeignKeys.Count);
            Assert.Contains(bin.ForeignKeys, fk => fk.Columns.Any(c => c.Name == "InventoryNodeTypeCode"));
            Assert.Contains(bin.ForeignKeys, fk => fk.Columns.Count == 2);
        }

        [Fact]
        public void MultipleTriggersFromDacpacAreCaptured()
        {
            var factory = new SqlServerDacpacDatabaseModelFactory();
            var options = new DatabaseModelFactoryOptions(null, new List<string>());

            var dbModel = factory.Create(TestPath("AdventureWorks2014.dacpac"), options);
            var table = dbModel.Tables.Single(t => t.Schema == "Production" && t.Name == "WorkOrder");

            Assert.Equal(
                new[] { "iWorkOrder", "uWorkOrder" }.OrderBy(x => x),
                table.Triggers.Select(t => t.Name).OrderBy(x => x));
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("N'The location''s address'", "The location's address")]
        [InlineData("'The location''s address'", "The location's address")]
        [InlineData("N'Plain text'", "Plain text")]
        [InlineData("'Plain text'", "Plain text")]
        [InlineData("Plain text", "Plain text")]
        [InlineData("N''''", "'")]
        public void FixExtendedPropertyValueNormalizesSqlStringLiterals(string input, string expected)
        {
            var result = SqlServerDacpacDatabaseModelFactory.FixExtendedPropertyValue(input);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void DescendingIndexFromDacpacIsCaptured()
        {
            var factory = new SqlServerDacpacDatabaseModelFactory();
            var options = new DatabaseModelFactoryOptions(null, new List<string>());

            var dbModel = factory.Create(dacpacDescending, options);
            var table = dbModel.Tables.Single(t => t.Schema == "support" && t.Name == "MessageFault");
            var index = table.Indexes.Single(i => i.Name == "IX_support_MessageFault_MessageFaultStatusCode_FaultedTimestamp");

            Assert.Equal(new[] { "MessageFaultStatusCode", "FaultedTimestamp" }, index.Columns.Select(c => c.Name));
            Assert.Equal(new[] { false, true }, index.IsDescending);
        }

        [Fact]
        public void ExplicitDefaultConstraintNameFromDacpacIsCaptured()
        {
            var factory = new SqlServerDacpacDatabaseModelFactory();
            var options = new DatabaseModelFactoryOptions(null, new List<string>());

            var dbModel = factory.Create(TestPath("AdventureWorks2014.dacpac"), options);
            var table = dbModel.Tables.Single(t => t.Schema == "dbo" && t.Name == "AWBuildVersion");
            var column = table.Columns.Single(c => c.Name == "ModifiedDate");

            Assert.NotNull(column.DefaultValueSql);
            Assert.Equal("DF_AWBuildVersion_ModifiedDate", column["Relational:DefaultConstraintName"]);
        }

        [Fact(Skip = "TBD - need to investigate")]
        public void Issue2263SprocWithCte()
        {
            var factory = new SqlServerDacpacStoredProcedureModelFactory(
                new SqlServerDacpacDatabaseModelFactoryOptions{ MergeDacpacs = false });
            var options = new ModuleModelFactoryOptions { FullModel = true, Modules = new List<string>() };

            // Act
            var dbModel = factory.Create(TestPath("abc.dacpac"), options);

            // Assert
            Assert.Single(dbModel.Routines);
        }

        [Fact]
        public void TemporalSupport()
        {
            var factory = new SqlServerDacpacDatabaseModelFactory();
            var options = new DatabaseModelFactoryOptions(null, new List<string>());

            // Act
            var dbModel = factory.Create(TestPath("Temporal.dacpac"), options);

            // Assert
            Assert.Single(dbModel.Tables);
            Assert.NotNull(dbModel.Tables.Single().FindAnnotation(SqlServerIsTemporalAnnotation));
        }

        [Fact]
        public void Issue2322TvpSprocParameters()
        {
            var factory = new SqlServerDacpacStoredProcedureModelFactory(
                new SqlServerDacpacDatabaseModelFactoryOptions{ MergeDacpacs = false });
            var options = new ModuleModelFactoryOptions { FullModel = true, Modules = new List<string>() };

            // Act
            var dbModel = factory.Create(TestPath("TvpParams.dacpac"), options);

            // Assert
            Assert.Single(dbModel.Routines);
            Assert.Equal(2, dbModel.Routines[0].Parameters.Count);
            Assert.Equal("NumberIDList", dbModel.Routines[0].Parameters[0].TypeName);
            Assert.Equal("Constant", dbModel.Routines[0].Parameters[0].TypeSchemaName);
            Assert.Equal("structured", dbModel.Routines[0].Parameters[0].StoreType);
            Assert.NotNull(dbModel.Routines[0].Parameters[0].TvpColumns);
            Assert.True(dbModel.Routines[0].Parameters[0].TvpColumns.Count > 0);
            
            // Debug - print TVP columns
            System.Console.WriteLine($"TVP Columns count: {dbModel.Routines[0].Parameters[0].TvpColumns.Count}");
            foreach (var col in dbModel.Routines[0].Parameters[0].TvpColumns)
            {
                System.Console.WriteLine($"  Column: {col.Name}, Type: {col.StoreType}");
            }
        }

        private string TestPath(string file)
        {
            return Path.Combine(AppContext.BaseDirectory, "Dacpac", file);
        }
    }
}
