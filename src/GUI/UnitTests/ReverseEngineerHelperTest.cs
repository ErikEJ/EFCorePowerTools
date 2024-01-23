using EFCorePowerTools.Helpers;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using RevEng.Common;
using System.Collections.Generic;

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
            var tables = new List<SerializationTableModel>
            {
                new SerializationTableModel("dbo.table", ObjectType.Table, null),
                new SerializationTableModel("dbo.table.crazy", ObjectType.Table, null),
                new SerializationTableModel("[dbo].[table]", ObjectType.Table, null),
                new SerializationTableModel("[dbo].[table.mad]", ObjectType.Table, null),
            };

            // Act
            var result = helper.NormalizeTables(tables, true);

            // Assert
            ClassicAssert.AreEqual(4, result.Count);
            ClassicAssert.AreEqual("[dbo].[table]", result[0].Name);
            ClassicAssert.AreEqual("[dbo].[table.crazy]", result[1].Name);
            ClassicAssert.AreEqual("[dbo].[table]", result[2].Name);
            ClassicAssert.AreEqual("[dbo].[table.mad]", result[3].Name);
        }

        [Test]
        public void CanSkipTables()
        {
            // Arrange
            var tables = new List<SerializationTableModel>
            {
                new SerializationTableModel("dbo.table", ObjectType.Table, null),
                new SerializationTableModel("dbo.table.crazy", ObjectType.Table, null),
                new SerializationTableModel("[dbo].[table]", ObjectType.Table, null),
                new SerializationTableModel("[dbo].[table.mad]", ObjectType.Table, null),
                new SerializationTableModel("table", ObjectType.Table, null),
            };

            // Act
            var result = helper.NormalizeTables(tables, false);

            // Assert
            ClassicAssert.AreEqual(5, result.Count);
            for (int i = 0; i < 5; i++)
            {
                ClassicAssert.AreEqual(result[i].Name, tables[i].Name);
            }
        }
    }
}
