using System.Collections.Generic;
using EFCorePowerTools.Helpers;
using Xunit;
using RevEng.Common;

namespace UnitTests
{
    public class ReverseEngineerHelperTest
    {
        [Fact]
        public void CanParseTables()
        {
            // Arrange
            var tables = new List<SerializationTableModel>
            {
                new SerializationTableModel("dbo.table", ObjectType.Table, null, null),
                new SerializationTableModel("dbo.table.crazy", ObjectType.Table, null, null),
                new SerializationTableModel("[dbo].[table]", ObjectType.Table, null, null),
                new SerializationTableModel("[dbo].[table.mad]", ObjectType.Table, null, null),
            };

            // Act
            var result = ReverseEngineerHelper.NormalizeTables(tables, true);

            // Assert
            Assert.Equal(4, result.Count);
            Assert.Equal("[dbo].[table]", result[0].Name);
            Assert.Equal("[dbo].[table.crazy]", result[1].Name);
            Assert.Equal("[dbo].[table]", result[2].Name);
            Assert.Equal("[dbo].[table.mad]", result[3].Name);
        }

        [Fact]
        public void CanSkipTables()
        {
            // Arrange
            var tables = new List<SerializationTableModel>
            {
                new SerializationTableModel("dbo.table", ObjectType.Table, null, null),
                new SerializationTableModel("dbo.table.crazy", ObjectType.Table, null, null),
                new SerializationTableModel("[dbo].[table]", ObjectType.Table, null, null),
                new SerializationTableModel("[dbo].[table.mad]", ObjectType.Table, null, null),
                new SerializationTableModel("table", ObjectType.Table, null, null),
            };

            // Act
            var result = ReverseEngineerHelper.NormalizeTables(tables, false);

            // Assert
            Assert.Equal(5, result.Count);
            for (int i = 0; i < 5; i++)
            {
                Assert.Equal(result[i].Name, tables[i].Name);
            }
        }
    }
}