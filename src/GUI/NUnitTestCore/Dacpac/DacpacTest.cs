using ErikEJ.EntityFrameworkCore.SqlServer.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            var factory = new SqlServerDacpacDatabaseModelFactory(null);
            var options = new DatabaseModelFactoryOptions(new List<string>(), new List<string>());

            // Act
            var dbModel = factory.Create(dacpac, options);

            // Assert
            Assert.AreEqual(11, dbModel.Tables.Count());
        }

        [Test]
        public void CanEnumerateSelectedTables()
        {
            // Arrange
            var factory = new SqlServerDacpacDatabaseModelFactory(null);
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
            var factory = new SqlServerDacpacDatabaseModelFactory(null);
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
            var factory = new SqlServerDacpacDatabaseModelFactory(null);
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
            var factory = new SqlServerDacpacDatabaseModelFactory(null);
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
            var factory = new SqlServerDacpacDatabaseModelFactory(null);
            var tables = new List<string> { "[dbo].[DefaultValues]" };
            var options = new DatabaseModelFactoryOptions(tables, new List<string>());

            // Act
            var dbModel = factory.Create(dacpacQuirk, options);

            // Assert
            Assert.AreEqual(1, dbModel.Tables.Count());
            Assert.AreEqual(1, dbModel.Tables
                .Where(t => t.Columns.Any(c => c.DefaultValueSql != null))
                .Count());
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
            Assert.AreEqual(91, dbModel.Tables.Count());
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
            Assert.AreEqual(1, dbModel.Tables.Count());
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
            Assert.AreEqual(2, dbModel.Tables.Count());
        }

        private string TestPath(string file)
        {
            return Path.Combine(TestContext.CurrentContext.TestDirectory, "Dacpac", file);
        }
    }
}
