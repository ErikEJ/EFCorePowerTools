namespace UnitTests.Models
{
    using System;
    using RevEng.Common;
    using Xunit;

    public class TableModelTest
    {
        [Fact]
        public void Constructor_ArgumentException_Schema()
        {
            // Arrange
            string table = null;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new TableModel(table, null, DatabaseType.SQLServer, ObjectType.Table, null));
        }

        [Fact]
        public void Constructor_CorrectCreation()
        {
            // Act
            var ti = new TableModel("Album", "dbo", DatabaseType.Npgsql, ObjectType.Table, null);

            // Assert
            Assert.Equal("dbo.Album", ti.DisplayName);
            Assert.Equal("Album", ti.Name);
            Assert.Equal("dbo", ti.Schema);
            Assert.Equal(ObjectType.Table, ti.ObjectType);
            Assert.Equal(DatabaseType.Npgsql, ti.DatabaseType);
        }

        [Fact]
        public void Constructor_CorrectCreation_2()
        {
            // Act
            var ti = new TableModel("Album", "dbo", DatabaseType.SQLServer, ObjectType.Table, null);

            // Assert
            Assert.Equal("[dbo].[Album]", ti.DisplayName);
            Assert.Equal("Album", ti.Name);
            Assert.Equal("dbo", ti.Schema);
            Assert.Equal(ObjectType.Table, ti.ObjectType);
            Assert.Equal(DatabaseType.SQLServer, ti.DatabaseType);
        }

        [Fact]
        public void Constructor_CorrectCreation_3()
        {
            // Act
            var ti = new TableModel("Album", "dbo", DatabaseType.SQLServerDacpac, ObjectType.Table, null);

            // Assert
            Assert.Equal("[dbo].[Album]", ti.DisplayName);
            Assert.Equal("Album", ti.Name);
            Assert.Equal("dbo", ti.Schema);
            Assert.Equal(ObjectType.Table, ti.ObjectType);
            Assert.Equal(DatabaseType.SQLServerDacpac, ti.DatabaseType);
        }
    }
}