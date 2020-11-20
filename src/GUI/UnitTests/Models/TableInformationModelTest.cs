namespace UnitTests.Models
{
    using System;
    using System.Collections.Generic;
    using EFCorePowerTools.Shared.Models;
    using NUnit.Framework;
    using RevEng.Shared;

    [TestFixture]
    public class TableModelTest
    {
        [Test]
        public void Constructor_ArgumentException_Schema()
        {
            // Arrange
            string table = null;
            var hasPrimaryKey = false;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new TableModel(table, hasPrimaryKey, ObjectType.Table, null));
        }

        [Test]
        public void Constructor_CorrectCreation()
        {
            // Arrange
            var table = "dbo.Album";
            var hasPrimaryKey = true;

            // Act
            var ti = new TableModel(table, hasPrimaryKey, ObjectType.Table, null);

            // Assert
            Assert.AreEqual("dbo.Album", ti.Name);
            Assert.IsTrue(ti.HasPrimaryKey);
        }
    }
}