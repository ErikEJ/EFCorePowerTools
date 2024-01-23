namespace UnitTests.Models
{
    using System;
    using NUnit.Framework;
    using NUnit.Framework.Legacy;
    using RevEng.Common;

    [TestFixture]
    public class TableModelTest
    {
        [Test]
        public void Constructor_ArgumentException_Schema()
        {
            // Arrange
            string table = null;

            // Act & Assert
            ClassicAssert.Throws<ArgumentException>(() => new TableModel(table, null, DatabaseType.SQLServer, ObjectType.Table, null));
        }

        [Test]
        public void Constructor_CorrectCreation()
        {
            // Act
            var ti = new TableModel("Album", "dbo", DatabaseType.Npgsql, ObjectType.Table, null);

            // Assert
            ClassicAssert.AreEqual("dbo.Album", ti.DisplayName);
            ClassicAssert.AreEqual("Album", ti.Name);
            ClassicAssert.AreEqual("dbo", ti.Schema);
            ClassicAssert.AreEqual(ObjectType.Table, ti.ObjectType);
            ClassicAssert.AreEqual(DatabaseType.Npgsql, ti.DatabaseType);
        }

        [Test]
        public void Constructor_CorrectCreation_2()
        {
            // Act
            var ti = new TableModel("Album", "dbo", DatabaseType.SQLServer, ObjectType.Table, null);

            // Assert
            ClassicAssert.AreEqual("[dbo].[Album]", ti.DisplayName);
            ClassicAssert.AreEqual("Album", ti.Name);
            ClassicAssert.AreEqual("dbo", ti.Schema);
            ClassicAssert.AreEqual(ObjectType.Table, ti.ObjectType);
            ClassicAssert.AreEqual(DatabaseType.SQLServer, ti.DatabaseType);
        }

        [Test]
        public void Constructor_CorrectCreation_3()
        {
            // Act
            var ti = new TableModel("Album", "dbo", DatabaseType.SQLServerDacpac, ObjectType.Table, null);

            // Assert
            ClassicAssert.AreEqual("[dbo].[Album]", ti.DisplayName);
            ClassicAssert.AreEqual("Album", ti.Name);
            ClassicAssert.AreEqual("dbo", ti.Schema);
            ClassicAssert.AreEqual(ObjectType.Table, ti.ObjectType);
            ClassicAssert.AreEqual(DatabaseType.SQLServerDacpac, ti.DatabaseType);
        }
    }
}
