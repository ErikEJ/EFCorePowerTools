namespace UnitTests.Models
{
    using System;
    using System.Collections.Generic;
    using EFCorePowerTools.Shared.Models;
    using NUnit.Framework;

    [TestFixture]
    public class TableInformationModelTest
    {
        [Test]
        public void Constructor_ArgumentException_Schema()
        {
            // Arrange
            string table = null;
            var hasPrimaryKey = false;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new TableInformationModel(table, hasPrimaryKey));
        }

        [Test]
        public void Constructor_CorrectCreation()
        {
            // Arrange
            var table = "dbo.Album";
            var hasPrimaryKey = true;

            // Act
            var ti = new TableInformationModel(table, hasPrimaryKey);

            // Assert
            Assert.AreEqual("dbo.Album", ti.Name);
            Assert.IsTrue(ti.HasPrimaryKey);
        }


        [Test]
        public void PropertyChanged_Name_SameValue()
        {
            // Arrange
            var propertyChangedInvoked = false;
            var name = "dbo.Album";
            var hasPrimaryKey = true;
            var ti = new TableInformationModel(name, hasPrimaryKey);
            ti.PropertyChanged += (sender, args) => propertyChangedInvoked = true;

            // Act
            ti.Name = name;

            // Assert
            Assert.IsFalse(propertyChangedInvoked);
        }

        [Test]
        public void PropertyChanged_HasPrimaryKey_SameValue()
        {
            // Arrange
            var propertyChangedInvoked = false;
            var name = "dbo.Album";
            var hasPrimaryKey = true;
            var ti = new TableInformationModel(name, hasPrimaryKey);
            ti.PropertyChanged += (sender, args) => propertyChangedInvoked = true;

            // Act
            ti.HasPrimaryKey = hasPrimaryKey;

            // Assert
            Assert.IsFalse(propertyChangedInvoked);
        }

        [Test]
        public void PropertyChanged_Name_DifferentValue()
        {
            // Arrange
            var changedProperties = new List<string>();
            var name = "Album";
            var hasPrimaryKey = true;
            var ti = new TableInformationModel(name, hasPrimaryKey);
            ti.PropertyChanged += (sender, args) => changedProperties.Add(args.PropertyName);

            // Act
            ti.Name = "bar";

            // Assert
            Assert.AreEqual(1, changedProperties.Count);
            Assert.IsTrue(changedProperties.Contains(nameof(TableInformationModel.Name)));
            Assert.AreEqual("bar", ti.Name);
        }

        [Test]
        public void PropertyChanged_HasPrimaryKey_DifferentValue()
        {
            // Arrange
            var changedProperties = new List<string>();
            var name = "Album";
            var hasPrimaryKey = true;
            var ti = new TableInformationModel(name, hasPrimaryKey);
            ti.PropertyChanged += (sender, args) => changedProperties.Add(args.PropertyName);

            // Act
            ti.HasPrimaryKey = false;

            // Assert
            Assert.AreEqual(1, changedProperties.Count);
            Assert.AreEqual(nameof(TableInformationModel.HasPrimaryKey), changedProperties[0]);
            Assert.IsFalse(ti.HasPrimaryKey);
        }
    }
}