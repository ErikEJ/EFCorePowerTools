namespace UnitTests.Models
{
    using System;
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

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new TableModel(table, null, null, ObjectType.Table, null));
        }

        [Test]
        public void Constructor_CorrectCreation()
        {
            // Arrange
            var table = "dbo.Album";

            // Act
            var ti = new TableModel(table, "Album", "dbo", ObjectType.Table, null);

            // Assert
            Assert.AreEqual("dbo.Album", ti.DisplayName);
            Assert.AreEqual("Album", ti.Name);
            Assert.AreEqual("dbo", ti.Schema);
            Assert.AreEqual(ObjectType.Table, ti.ObjectType);
        }
    }
}