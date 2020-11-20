using System.Collections.Generic;
using EFCorePowerTools.Helpers;
using EFCorePowerTools.Shared.Models;
using NUnit.Framework;
using RevEng.Shared;

namespace UnitTests
{
    [TestFixture]
    public class ReverseEngineerHelperTest
    {
        private readonly ReverseEngineerHelper helper = new ReverseEngineerHelper();

        [Test]
        public void CanParseTables()
        {
            // Arrange
            var tables = new List<TableModel>
            {
                new TableModel("dbo.table", true, ObjectType.Table, null),
                new TableModel("dbo.table.crazy", false, ObjectType.Table, null),
                new TableModel("[dbo].[table]", true, ObjectType.Table, null),
                new TableModel("[dbo].[table.mad]", true, ObjectType.Table, null),
            };

            // Act
            var result = helper.NormalizeTables(tables, true);

            // Assert
            Assert.AreEqual(4, result.Count);
            Assert.AreEqual("[dbo].[table]", result[0].Name);
            Assert.AreEqual("[dbo].[table.crazy]", result[1].Name);
            Assert.AreEqual("[dbo].[table]", result[2].Name);
            Assert.AreEqual("[dbo].[table.mad]", result[3].Name);
        }

        [Test]
        public void CanSkipTables()
        {
            // Arrange
            var tables = new List<TableModel>
            {
                new TableModel("dbo.table", true, ObjectType.Table, null),
                new TableModel("dbo.table.crazy", false, ObjectType.Table, null),
                new TableModel("[dbo].[table]", true, ObjectType.Table, null),
                new TableModel("[dbo].[table.mad]", true, ObjectType.Table, null),
                new TableModel("table", true, ObjectType.Table, null),
            };

            // Act
            var result = helper.NormalizeTables(tables, false);

            // Assert
            Assert.AreEqual(5, result.Count);
            for (int i = 0; i < 5; i++)
            {
                Assert.AreEqual(result[i].Name, tables[i].Name);
            }
        }
    }
}
