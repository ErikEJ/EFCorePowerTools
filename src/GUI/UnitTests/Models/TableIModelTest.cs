namespace UnitTests.Models
{
    using NUnit.Framework;
    using RevEng.Shared;
    using System;

    [TestFixture]
    public class TableModelTest
    {
        [Test]
        public void Constructor_ArgumentException_Schema()
        {
            // Arrange
            string table = null;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new TableModel(table, null, DatabaseType.SQLServer, ObjectType.Table, null));
        }

        [Test]
        public void Constructor_CorrectCreation()
        {
            // Act
            var ti = new TableModel("Album", "dbo", DatabaseType.Npgsql, ObjectType.Table, null);

            // Assert
            Assert.AreEqual("dbo.Album", ti.DisplayName);
            Assert.AreEqual("Album", ti.Name);
            Assert.AreEqual("dbo", ti.Schema);
            Assert.AreEqual(ObjectType.Table, ti.ObjectType);
            Assert.AreEqual(DatabaseType.Npgsql, ti.DatabaseType);
        }

        [Test]
        public void Constructor_CorrectCreation_2()
        {
            // Act
            var ti = new TableModel("Album", "dbo", DatabaseType.SQLServer, ObjectType.Table, null);

            // Assert
            Assert.AreEqual("[dbo].[Album]", ti.DisplayName);
            Assert.AreEqual("Album", ti.Name);
            Assert.AreEqual("dbo", ti.Schema);
            Assert.AreEqual(ObjectType.Table, ti.ObjectType);
            Assert.AreEqual(DatabaseType.SQLServer, ti.DatabaseType);
        }

        [Test]
        public void Constructor_CorrectCreation_3()
        {
            // Act
            var ti = new TableModel("Album", "dbo", DatabaseType.SQLServerDacpac, ObjectType.Table, null);

            // Assert
            Assert.AreEqual("[dbo].[Album]", ti.DisplayName);
            Assert.AreEqual("Album", ti.Name);
            Assert.AreEqual("dbo", ti.Schema);
            Assert.AreEqual(ObjectType.Table, ti.ObjectType);
            Assert.AreEqual(DatabaseType.SQLServerDacpac, ti.DatabaseType);
        }
    }
}