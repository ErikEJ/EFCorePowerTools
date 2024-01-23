using System.Collections.Generic;
using System.IO;
using System.Linq;
using ErikEJ.EntityFrameworkCore.SqlServer.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.SqlServer.Metadata.Internal;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace UnitTests
{
    [TestFixture]
    public class DacpacTest
    {
        private string dacpac;

        private string dacpacQuirk;

        [SetUp]
        public void Setup()
        {
            dacpacQuirk = TestPath("TestDb.dacpac");
            dacpac = TestPath("Chinook.dacpac");
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
            ClassicAssert.AreEqual(11, dbModel.Tables.Count());
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
            ClassicAssert.AreEqual(3, dbModel.Tables.Count());
            ClassicAssert.AreEqual("Album", dbModel.Tables[0].Name);
            ClassicAssert.AreEqual(1, dbModel.Tables[0].ForeignKeys.Count);
            ClassicAssert.AreEqual(3, dbModel.Tables[0].Columns.Count);
        }

        [Test]
        public void CanEnumerateSelectedQuirkObjects()
        {
            // Arrange
            var factory = new SqlServerDacpacDatabaseModelFactory(null);
            var tables = new List<string> { "[dbo].[FilteredIndexTable]", "[dbo].[DefaultComputedValues]" };
            var options = new DatabaseModelFactoryOptions(tables, new List<string>());

            // Act
            var dbModel = factory.Create(dacpacQuirk, options);

            // Assert
            ClassicAssert.AreEqual(2, dbModel.Tables.Count());

            ClassicAssert.AreEqual("FilteredIndexTable", dbModel.Tables[1].Name);
            ClassicAssert.AreEqual(0, dbModel.Tables[1].ForeignKeys.Count);
            ClassicAssert.AreEqual(2, dbModel.Tables[1].Columns.Count);

            ClassicAssert.AreEqual("DefaultComputedValues", dbModel.Tables[0].Name);
            ClassicAssert.AreEqual(5, dbModel.Tables[0].Columns.Count);
        }

        [Test]
        public void CanEnumerateSelectedComputed()
        {
            // Arrange
            var factory = new SqlServerDacpacDatabaseModelFactory(null);
            var tables = new List<string> { "[dbo].[DefaultComputedValues]" };
            var options = new DatabaseModelFactoryOptions(tables, new List<string>());

            // Act
            var dbModel = factory.Create(dacpacQuirk, options);

            // Assert
            ClassicAssert.AreEqual(1, dbModel.Tables.Count());

            ClassicAssert.AreEqual("DefaultComputedValues", dbModel.Tables[0].Name);
            ClassicAssert.AreEqual(5, dbModel.Tables[0].Columns.Count);
        }

        [Test]
        public void CanEnumerateTypeAlias()
        {
            // Arrange
            var factory = new SqlServerDacpacDatabaseModelFactory(null);
            var tables = new List<string> { "[dbo].[TypeAlias]" };
            var options = new DatabaseModelFactoryOptions(tables, new List<string>());

            // Act
            var dbModel = factory.Create(dacpacQuirk, options);

            // Assert
            ClassicAssert.AreEqual(1, dbModel.Tables.Count());

            ClassicAssert.AreEqual("TypeAlias", dbModel.Tables[0].Name);
            ClassicAssert.AreEqual(2, dbModel.Tables[0].Columns.Count);

            ClassicAssert.AreEqual("nvarchar(max)", dbModel.Tables[0].Columns[1].StoreType);
        }

        [Test]
        public void CanHandleDefaultValues()
        {
            // Arrange
            var factory = new SqlServerDacpacDatabaseModelFactory(null);
            var tables = new List<string> { "[dbo].[DefaultValues]" };
            var options = new DatabaseModelFactoryOptions(tables, new List<string>());

            // Act
            var dbModel = factory.Create(dacpacQuirk, options);

            // Assert
            ClassicAssert.AreEqual(1, dbModel.Tables.Count());
            ClassicAssert.AreEqual(1, dbModel.Tables
                .Count(t => t.Columns.Any(c => c.DefaultValueSql != null)));
        }

        [Test]
        public void CanBuildAW2014()
        {
            // Arrange
            var factory = new SqlServerDacpacDatabaseModelFactory(null);
            var options = new DatabaseModelFactoryOptions(null, new List<string>());

            // Act
            var dbModel = factory.Create(TestPath("AdventureWorks2014.dacpac"), options);

            // Assert
            ClassicAssert.AreEqual(91, dbModel.Tables.Count());
        }

        [Test]
        public void Issue208ComputedConstraint()
        {
            // Arrange
            var factory = new SqlServerDacpacDatabaseModelFactory(null);
            var options = new DatabaseModelFactoryOptions(null, new List<string>());

            // Act
            var dbModel = factory.Create(TestPath("Issue208.dacpac"), options);

            // Assert
            ClassicAssert.AreEqual(1, dbModel.Tables.Count());
        }

        [Test]
        public void Issue210ComputedConstraintIsFK()
        {
            // Arrange
            var factory = new SqlServerDacpacDatabaseModelFactory(null);
            var options = new DatabaseModelFactoryOptions(null, new List<string>());

            // Act
            var dbModel = factory.Create(TestPath("Issue210.dacpac"), options);

            // Assert
            ClassicAssert.AreEqual(2, dbModel.Tables.Count());
        }

        [Test]
        public void Issue1262_ConsiderSchemaArgument()
        {
            var factory = new SqlServerDacpacDatabaseModelFactory(null);
            var options = new DatabaseModelFactoryOptions(null, new List<string>() { "mat" });

            // Act
            var dbModel = factory.Create(TestPath("Issue1262.dacpac"), options);

            // Assert
            ClassicAssert.AreEqual(1, dbModel.Tables.Count());
            ClassicAssert.AreEqual("mat", dbModel.Tables.Single().Schema);
        }

        [Test]
        public void Issue1262_BehaviourWithoutSchemaArgument()
        {
            var factory = new SqlServerDacpacDatabaseModelFactory(null);
            var options = new DatabaseModelFactoryOptions(null, new List<string>());

            // Act
            var dbModel = factory.Create(TestPath("Issue1262.dacpac"), options);

            // Assert
            ClassicAssert.AreEqual(2, dbModel.Tables.Count());
            ClassicAssert.AreEqual(1, dbModel.Tables.Count(x => x.Schema == "mat"));
            ClassicAssert.AreEqual(1, dbModel.Tables.Count(x => x.Schema == "mat2"));
        }

        [Test]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "EF1001:Internal EF Core API usage.", Justification = "<Pending>")]
        public void Temporal_Support()
        {
            var factory = new SqlServerDacpacDatabaseModelFactory(null);
            var options = new DatabaseModelFactoryOptions(null, new List<string>());

            // Act
            var dbModel = factory.Create(TestPath("Temporal.dacpac"), options);

            // Assert
            ClassicAssert.AreEqual(1, dbModel.Tables.Count());
            ClassicAssert.NotNull(dbModel.Tables.Single().FindAnnotation(SqlServerAnnotationNames.IsTemporal));
        }

        private string TestPath(string file)
        {
            return Path.Combine(TestContext.CurrentContext.TestDirectory, "Dacpac", file);
        }
    }
}
