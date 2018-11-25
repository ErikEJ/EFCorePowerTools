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
            string schema = null;
            string table = null;
            var hasPrimaryKey = false;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new TableInformationModel(schema, table, hasPrimaryKey));
        }

        [Test]
        public void Constructor_ArgumentException_Table()
        {
            // Arrange
            var schema = "dbo";
            string table = null;
            var hasPrimaryKey = false;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new TableInformationModel(schema, table, hasPrimaryKey));
        }

        [Test]
        public void Constructor_CorrectCreation()
        {
            // Arrange
            var schema = "dbo";
            var table = "Album";
            var hasPrimaryKey = true;

            // Act
            var ti = new TableInformationModel(schema, table, hasPrimaryKey);

            // Assert
            Assert.AreEqual("dbo", ti.Schema);
            Assert.AreEqual("Album", ti.Name);
            Assert.IsTrue(ti.HasPrimaryKey);
            Assert.AreEqual("dbo.Album", ti.UnsafeFullName);
            Assert.AreEqual("[dbo].[Album]", ti.SafeFullName);
        }

        [Test]
        public void Parse_ArgumentException_Empty()
        {
            // Arrange
            string table = null;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => TableInformationModel.Parse(table));
            Assert.Throws<ArgumentException>(() => TableInformationModel.Parse(table, true));
        }

        [Test]
        public void Parse_ArgumentException_NotTwoPeriods()
        {
            // Arrange
            var table1 = "Album";
            var table2 = "[config.legacy].Album";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => TableInformationModel.Parse(table1));
            Assert.Throws<ArgumentException>(() => TableInformationModel.Parse(table2));
            Assert.Throws<ArgumentException>(() => TableInformationModel.Parse(table1, true));
            Assert.Throws<ArgumentException>(() => TableInformationModel.Parse(table2, true));
        }

        [Test]
        public void Parse_OnlyTable_CorrectCreation()
        {
            // Arrange
            var table = "dbo.Album";

            // Act
            var ti = TableInformationModel.Parse(table);

            // Assert
            Assert.AreEqual("dbo", ti.Schema);
            Assert.AreEqual("Album", ti.Name);
            Assert.IsTrue(ti.HasPrimaryKey);
            Assert.AreEqual("dbo.Album", ti.UnsafeFullName);
            Assert.AreEqual("[dbo].[Album]", ti.SafeFullName);
        }

        [Test]
        public void Parse_TableWithHasPrimaryKey_CorrectCreation()
        {
            // Arrange
            var table = "dbo.Album";

            // Act
            var ti1 = TableInformationModel.Parse(table, false);
            var ti2 = TableInformationModel.Parse(table, true);

            // Assert
            Assert.AreEqual("dbo", ti1.Schema);
            Assert.AreEqual("Album", ti1.Name);
            Assert.IsFalse(ti1.HasPrimaryKey);
            Assert.AreEqual("dbo.Album", ti1.UnsafeFullName);
            Assert.AreEqual("[dbo].[Album]", ti1.SafeFullName);
            Assert.AreEqual("dbo", ti2.Schema);
            Assert.AreEqual("Album", ti2.Name);
            Assert.IsTrue(ti2.HasPrimaryKey);
            Assert.AreEqual("dbo.Album", ti2.UnsafeFullName);
            Assert.AreEqual("[dbo].[Album]", ti2.SafeFullName);
        }

        [Test]
        public void TryParse_Empty()
        {
            // Arrange
            string table = null;

            // Act
            var parsed = TableInformationModel.TryParse(table, out var tableInformation);

            // Assert
            Assert.IsFalse(parsed);
            Assert.IsNull(tableInformation);
        }

        [Test]
        public void TryParse_NotTwoPeriods()
        {
            // Arrange
            var table1 = "Album";
            var table2 = "[config.legacy].Album";

            // Act
            var parsed1 = TableInformationModel.TryParse(table1, out var tableInformation1);
            var parsed2 = TableInformationModel.TryParse(table2, out var tableInformation2);

            // Assert
            Assert.IsFalse(parsed1);
            Assert.IsFalse(parsed2);
            Assert.IsNull(tableInformation1);
            Assert.IsNull(tableInformation2);
        }

        [Test]
        public void TryParse_OnlyTable_CorrectCreation()
        {
            // Arrange
            var table = "dbo.Album";

            // Act
            var parsed = TableInformationModel.TryParse(table, out var ti);

            // Assert
            Assert.IsTrue(parsed);
            Assert.AreEqual("dbo", ti.Schema);
            Assert.AreEqual("Album", ti.Name);
            Assert.IsTrue(ti.HasPrimaryKey);
            Assert.AreEqual("dbo.Album", ti.UnsafeFullName);
            Assert.AreEqual("[dbo].[Album]", ti.SafeFullName);
        }

        [Test]
        public void PropertyChanged_Schema_SameValue()
        {
            // Arrange
            var propertyChangedInvoked = false;
            var schema = "dbo";
            var name = "Album";
            var hasPrimaryKey = true;
            var ti = new TableInformationModel(schema, name, hasPrimaryKey);
            ti.PropertyChanged += (sender, args) => propertyChangedInvoked = true;

            // Act
            ti.Schema = schema;

            // Assert
            Assert.IsFalse(propertyChangedInvoked);
        }

        [Test]
        public void PropertyChanged_Name_SameValue()
        {
            // Arrange
            var propertyChangedInvoked = false;
            var schema = "dbo";
            var name = "Album";
            var hasPrimaryKey = true;
            var ti = new TableInformationModel(schema, name, hasPrimaryKey);
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
            var schema = "dbo";
            var name = "Album";
            var hasPrimaryKey = true;
            var ti = new TableInformationModel(schema, name, hasPrimaryKey);
            ti.PropertyChanged += (sender, args) => propertyChangedInvoked = true;

            // Act
            ti.HasPrimaryKey = hasPrimaryKey;

            // Assert
            Assert.IsFalse(propertyChangedInvoked);
        }

        [Test]
        public void PropertyChanged_Schema_DifferentValue()
        {
            // Arrange
            var changedProperties = new List<string>();
            var schema = "dbo";
            var name = "Album";
            var hasPrimaryKey = true;
            var ti = new TableInformationModel(schema, name, hasPrimaryKey);
            ti.PropertyChanged += (sender, args) => changedProperties.Add(args.PropertyName);

            // Act
            ti.Schema = "foo";

            // Assert
            Assert.AreEqual(3, changedProperties.Count);
            Assert.IsTrue(changedProperties.Contains(nameof(TableInformationModel.Schema)));
            Assert.IsTrue(changedProperties.Contains(nameof(TableInformationModel.UnsafeFullName)));
            Assert.IsTrue(changedProperties.Contains(nameof(TableInformationModel.SafeFullName)));
            Assert.AreEqual("foo", ti.Schema);
            Assert.AreEqual("foo.Album", ti.UnsafeFullName);
            Assert.AreEqual("[foo].[Album]", ti.SafeFullName);
        }

        [Test]
        public void PropertyChanged_Name_DifferentValue()
        {
            // Arrange
            var changedProperties = new List<string>();
            var schema = "dbo";
            var name = "Album";
            var hasPrimaryKey = true;
            var ti = new TableInformationModel(schema, name, hasPrimaryKey);
            ti.PropertyChanged += (sender, args) => changedProperties.Add(args.PropertyName);

            // Act
            ti.Name = "bar";

            // Assert
            Assert.AreEqual(3, changedProperties.Count);
            Assert.IsTrue(changedProperties.Contains(nameof(TableInformationModel.Name)));
            Assert.IsTrue(changedProperties.Contains(nameof(TableInformationModel.UnsafeFullName)));
            Assert.IsTrue(changedProperties.Contains(nameof(TableInformationModel.SafeFullName)));
            Assert.AreEqual("bar", ti.Name);
            Assert.AreEqual("dbo.bar", ti.UnsafeFullName);
            Assert.AreEqual("[dbo].[bar]", ti.SafeFullName);
        }

        [Test]
        public void PropertyChanged_HasPrimaryKey_DifferentValue()
        {
            // Arrange
            var changedProperties = new List<string>();
            var schema = "dbo";
            var name = "Album";
            var hasPrimaryKey = true;
            var ti = new TableInformationModel(schema, name, hasPrimaryKey);
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