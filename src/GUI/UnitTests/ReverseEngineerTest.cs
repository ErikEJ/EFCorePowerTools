using System.Collections.Generic;
using EFCorePowerTools.Helpers;
using EFCorePowerTools.Shared.Models;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class ReverseEngineerTest
    {
        private readonly ReverseEngineerHelper helper = new ReverseEngineerHelper();

        [Test]
        public void CanParseTables()
        {
            // Arrange
            var tables = new List<TableInformationModel>
            {
                new TableInformationModel("dbo.table", true),
                new TableInformationModel("dbo.table.crazy", false),
                new TableInformationModel("[dbo].[table]", true),
                new TableInformationModel("[dbo].[table.mad]", true),
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
            var tables = new List<TableInformationModel>
            {
                new TableInformationModel("dbo.table", true),
                new TableInformationModel("dbo.table.crazy", false),
                new TableInformationModel("[dbo].[table]", true),
                new TableInformationModel("[dbo].[table.mad]", true),
                new TableInformationModel("table", true),
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
