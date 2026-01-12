using System.Collections.Generic;
using System.IO;
using System.Linq;
using ErikEJ.EntityFrameworkCore.SqlServer.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.SqlServer.Metadata.Internal;
using NUnit.Framework;
using RevEng.Core.Abstractions;

namespace UnitTests
{
    [TestFixture]
    public class DacpacTest
    {
        private string dacpac;
        private string dacpacViews;
        private string dacpacQuirk;

        [SetUp]
        public void Setup()
        {
            dacpacQuirk = TestPath("TestDb.dacpac");
            dacpac = TestPath("Chinook.dacpac");
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

        [Test]
        public void CanEnumerateTables()
        {
            // Arrange
            var factory = new SqlServerDacpacDatabaseModelFactory();
            var options = new DatabaseModelFactoryOptions(new List<string>(), new List<string>());

            // Act
            var dbModel = factory.Create(dacpac, options);

            // Assert
            Assert.AreEqual(11, dbModel.Tables.Count());
        }

        [Test]
        public void CanEnumerateViewColumns()
        {
            // Arrange
            var factory = new SqlServerDacpacDatabaseModelFactory();
            var options = new DatabaseModelFactoryOptions(new List<string>(), new List<string>());

            // Act
            var dbModel = factory.Create(dacpacViews, options);

            // Assert
            Assert.AreEqual(28, dbModel.Tables[0].Columns.Count());
        }

        [Test]
        public void CanEnumerateSelectedTables()
        {
            // Arrange
            var factory = new SqlServerDacpacDatabaseModelFactory();
            var tables = new List<string> { "[dbo].[Album]", "[dbo].[Artist]", "[dbo].[InvoiceLine]" };
            var options = new DatabaseModelFactoryOptions(tables, new List<string>());

            // Act
            var dbModel = factory.Create(dacpac, options);

            // Assert
            Assert.AreEqual(3, dbModel.Tables.Count());
            Assert.AreEqual("Album", dbModel.Tables[0].Name);
            Assert.AreEqual(1, dbModel.Tables[0].ForeignKeys.Count);
            Assert.AreEqual(3, dbModel.Tables[0].Columns.Count);
        }

        [Test]
        public void CanEnumerateSelectedQuirkObjects()
        {
            // Arrange
            var factory = new SqlServerDacpacDatabaseModelFactory();
            var tables = new List<string> { "[dbo].[FilteredIndexTable]", "[dbo].[DefaultComputedValues]" };
            var options = new DatabaseModelFactoryOptions(tables, new List<string>());

            // Act
            var dbModel = factory.Create(dacpacQuirk, options);

            // Assert
            Assert.AreEqual(2, dbModel.Tables.Count());

            Assert.AreEqual("FilteredIndexTable", dbModel.Tables[1].Name);
            Assert.AreEqual(0, dbModel.Tables[1].ForeignKeys.Count);
            Assert.AreEqual(2, dbModel.Tables[1].Columns.Count);

            Assert.AreEqual("DefaultComputedValues", dbModel.Tables[0].Name);
            Assert.AreEqual(5, dbModel.Tables[0].Columns.Count);
        }

        [Test]
        public void CanEnumerateSelectedComputed()
        {
            // Arrange
            var factory = new SqlServerDacpacDatabaseModelFactory();
            var tables = new List<string> { "[dbo].[DefaultComputedValues]" };
            var options = new DatabaseModelFactoryOptions(tables, new List<string>());

            // Act
            var dbModel = factory.Create(dacpacQuirk, options);

            // Assert
            Assert.AreEqual(1, dbModel.Tables.Count());

            Assert.AreEqual("DefaultComputedValues", dbModel.Tables[0].Name);
            Assert.AreEqual(5, dbModel.Tables[0].Columns.Count);
        }

        [Test]
        public void CanEnumerateTypeAlias()
        {
            // Arrange
            var factory = new SqlServerDacpacDatabaseModelFactory();
            var tables = new List<string> { "[dbo].[TypeAlias]" };
            var options = new DatabaseModelFactoryOptions(tables, new List<string>());

            // Act
            var dbModel = factory.Create(dacpacQuirk, options);

            // Assert
            Assert.AreEqual(1, dbModel.Tables.Count());

            Assert.AreEqual("TypeAlias", dbModel.Tables[0].Name);
            Assert.AreEqual(2, dbModel.Tables[0].Columns.Count);

            Assert.AreEqual("nvarchar(max)", dbModel.Tables[0].Columns[1].StoreType);
        }

        [Test]
        public void CanHandleDefaultValues()
        {
            // Arrange
            var factory = new SqlServerDacpacDatabaseModelFactory();
            var tables = new List<string> { "[dbo].[DefaultValues]" };
            var options = new DatabaseModelFactoryOptions(tables, new List<string>());

            // Act
            var dbModel = factory.Create(dacpacQuirk, options);

            // Assert
            Assert.AreEqual(1, dbModel.Tables.Count());
            Assert.AreEqual(1, dbModel.Tables
                .Count(t => t.Columns.Any(c => c.DefaultValueSql != null)));
        }

        [Test]
        public void CanBuildAW2014()
        {
            // Arrange
            var factory = new SqlServerDacpacDatabaseModelFactory();
            var options = new DatabaseModelFactoryOptions(null, new List<string>());

            // Act
            var dbModel = factory.Create(TestPath("AdventureWorks2014.dacpac"), options);

            // Assert
            Assert.AreEqual(91, dbModel.Tables.Count());
        }

        [Test]
        public void Issue208ComputedConstraint()
        {
            // Arrange
            var factory = new SqlServerDacpacDatabaseModelFactory();
            var options = new DatabaseModelFactoryOptions(null, new List<string>());

            // Act
            var dbModel = factory.Create(TestPath("Issue208.dacpac"), options);

            // Assert
            Assert.AreEqual(1, dbModel.Tables.Count());
        }

        [Test]
        public void Issue210ComputedConstraintIsFK()
        {
            // Arrange
            var factory = new SqlServerDacpacDatabaseModelFactory();
            var options = new DatabaseModelFactoryOptions(null, new List<string>());

            // Act
            var dbModel = factory.Create(TestPath("Issue210.dacpac"), options);

            // Assert
            Assert.AreEqual(2, dbModel.Tables.Count());
        }

        [Test]
        public void Issue1262ConsiderSchemaArgument()
        {
            var factory = new SqlServerDacpacDatabaseModelFactory();
            var options = new DatabaseModelFactoryOptions(null, new List<string>() { "mat" });

            // Act
            var dbModel = factory.Create(TestPath("Issue1262.dacpac"), options);

            // Assert
            Assert.AreEqual(1, dbModel.Tables.Count());
            Assert.AreEqual("mat", dbModel.Tables.Single().Schema);
        }

        [Test]
        public void Issue1262BehaviourWithoutSchemaArgument()
        {
            var factory = new SqlServerDacpacDatabaseModelFactory();
            var options = new DatabaseModelFactoryOptions(null, new List<string>());

            // Act
            var dbModel = factory.Create(TestPath("Issue1262.dacpac"), options);

            // Assert
            Assert.AreEqual(2, dbModel.Tables.Count());
            Assert.AreEqual(1, dbModel.Tables.Count(x => x.Schema == "mat"));
            Assert.AreEqual(1, dbModel.Tables.Count(x => x.Schema == "mat2"));
        }

        
        [Test]
        [Ignore("TBD - need to investigate")]
        public void Issue2263SprocWithCte()
        {
            var factory = new SqlServerDacpacStoredProcedureModelFactory(
                new SqlServerDacpacDatabaseModelFactoryOptions{ MergeDacpacs = false });
            var options = new ModuleModelFactoryOptions { FullModel = true, Modules = new List<string>() };

            // Act
            var dbModel = factory.Create(TestPath("abc.dacpac"), options);

            // Assert
            Assert.AreEqual(1, dbModel.Routines.Count());
        }

        [Test]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "EF1001:Internal EF Core API usage.", Justification = "Test")]
        public void TemporalSupport()
        {
            var factory = new SqlServerDacpacDatabaseModelFactory();
            var options = new DatabaseModelFactoryOptions(null, new List<string>());

            // Act
            var dbModel = factory.Create(TestPath("Temporal.dacpac"), options);

            // Assert
            Assert.AreEqual(1, dbModel.Tables.Count());
            Assert.NotNull(dbModel.Tables.Single().FindAnnotation(SqlServerAnnotationNames.IsTemporal));
        }

        [Test]
        public void Issue2322TvpSprocParameters()
        {
            var factory = new SqlServerDacpacStoredProcedureModelFactory(
                new SqlServerDacpacDatabaseModelFactoryOptions{ MergeDacpacs = false });
            var options = new ModuleModelFactoryOptions { FullModel = true, Modules = new List<string>() };

            // Act
            var dbModel = factory.Create(TestPath("TvpParams.dacpac"), options);

            // Assert
            Assert.AreEqual(1, dbModel.Routines.Count);
            Assert.AreEqual(2, dbModel.Routines[0].Parameters.Count);
            Assert.AreEqual("[Constant].[NumberIDList]", dbModel.Routines[0].Parameters[0].TypeName);
            Assert.AreEqual("structured", dbModel.Routines[0].Parameters[0].StoreType);
            Assert.IsNotNull(dbModel.Routines[0].Parameters[0].TvpColumns);
            Assert.Greater(dbModel.Routines[0].Parameters[0].TvpColumns.Count, 0);
        }

        private string TestPath(string file)
        {
            return Path.Combine(TestContext.CurrentContext.TestDirectory, "Dacpac", file);
        }
    }
}
