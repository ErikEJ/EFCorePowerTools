using NUnit.Framework;

namespace UnitTests.ViewModels
{
    using EFCorePowerTools.Contracts.ViewModels;
    using EFCorePowerTools.ViewModels;
    using GalaSoft.MvvmLight.Messaging;
    using Moq;
    using System.Linq;

    [TestFixture]
    public class ObjectTreeRootItemViewModelTests
    {
        [Test]
        public void IsSelected_AllSelected()
        {
            // Arrange
            var vm = new ObjectTreeRootItemViewModel();
            AddObjects(vm);

            // Act
            foreach (var item in vm.Schemas)
            {
                item.SetSelectedCommand.Execute(true);
            }

            // Assert
            Assert.IsTrue(vm.IsSelected);
        }

        [Test]
        public void IsSelected_NoneSelected()
        {
            // Arrange
            var vm = new ObjectTreeRootItemViewModel();
            AddObjects(vm);

            // Act
            foreach (var item in vm.Schemas)
            {
                item.SetSelectedCommand.Execute(false);
            }

            // Assert
            Assert.IsFalse(vm.IsSelected);
        }

        [Test]
        public void IsSelected_PartialSelection()
        {
            // Arrange
            var vm = new ObjectTreeRootItemViewModel();
            AddObjects(vm);

            // Act
            foreach (var item in vm.Schemas)
            {
                item.SetSelectedCommand.Execute(vm.Schemas.IndexOf(item) % 2 == 0);
            }

            // Assert
            Assert.IsNull(vm.IsSelected);
        }

        [Test]
        public void IsVisible_NoneVisible()
        {
            // Arrange
            var vm = new ObjectTreeRootItemViewModel();
            AddObjects(vm);

            // Act
            foreach (var item in vm.Schemas.SelectMany(c => c.Objects))
            {
                item.IsVisible = false;
            }

            // Assert
            Assert.IsFalse(vm.IsVisible);
        }

        [Test]
        public void IsVisible_PartialVisible()
        {
            // Arrange
            var vm = new ObjectTreeRootItemViewModel();
            AddObjects(vm);

            // Act
            foreach (var item in vm.Schemas.First().Objects)
            {
                item.IsVisible = false;
            }

            // Assert
            Assert.IsTrue(vm.IsVisible);
        }

        [Test]
        public void Text_SameValue()
        {
            // Arrange
            var propertyChangedInvoked = false;
            var vm = new ObjectTreeRootItemViewModel();
            vm.Text = "Tables";
            vm.PropertyChanged += (sender, args) => propertyChangedInvoked = true;

            // Act
            vm.Text = "Tables";

            // Assert
            Assert.IsFalse(propertyChangedInvoked);
        }

        [Test]
        public void Text_DifferentValue()
        {
            // Arrange
            var propertyChangedInvoked = false;
            var vm = new ObjectTreeRootItemViewModel();
            vm.Text = "Tables";
            vm.PropertyChanged += (sender, args) => propertyChangedInvoked = true;

            // Act
            vm.Text = "Views";

            // Assert
            Assert.IsTrue(propertyChangedInvoked);
        }

        private static void AddObjects(ObjectTreeRootItemViewModel vm)
        {
            foreach (var item in GetDatabaseObjects())
            {
                vm.Schemas.Add(item);
            }
        }

        private static ISchemaInformationViewModel[] GetDatabaseObjects()
        {
            TableInformationViewModel CreateTable(string tableSchema, string name, bool selected)
            {
                var messenger = new Mock<IMessenger>();
                messenger.SetupAllProperties();
                var result = new TableInformationViewModel(messenger.Object);
                result.Name = name;
                result.Schema = tableSchema;
                if  (selected)
                {
                    result.SetSelectedCommand.Execute(true);
                }
                return result;
            }

            ColumnInformationViewModel CreateColumn(string name, bool isPrimaryKey, bool isForeignKey, bool selected)
            {
                var messenger = new Mock<IMessenger>();
                messenger.SetupAllProperties();
                var result = new ColumnInformationViewModel(messenger.Object);
                result.Name = name;
                result.IsPrimaryKey = isPrimaryKey;
                result.IsForeignKey = isForeignKey;
                if (selected)
                {
                    result.SetSelectedCommand.Execute(true);
                }
                return result;
            }

            var schema0 = new SchemaInformationViewModel { Name = "dbo" };

            schema0.Objects.Add(CreateTable("dbo", "Atlas", false));
            schema0.Objects[0].Columns.Add(CreateColumn("id", true, false, false));
            schema0.Objects[0].Columns.Add(CreateColumn("column1", false, false, false));
            schema0.Objects[0].Columns.Add(CreateColumn("column2", false, false, false));

            schema0.Objects.Add(CreateTable("dbo", "__RefactorLog", true));
            schema0.Objects[1].Columns.Add(CreateColumn("id", true, false, false));
            schema0.Objects[1].Columns.Add(CreateColumn("column1", false, false, false));
            schema0.Objects[1].Columns.Add(CreateColumn("column2", false, false, false));

            schema0.Objects.Add(CreateTable("dbo", "sysdiagrams", false));
            schema0.Objects[2].Columns.Add(CreateColumn("id", true, false, false));
            schema0.Objects[2].Columns.Add(CreateColumn("column1", false, false, false));
            schema0.Objects[2].Columns.Add(CreateColumn("column2", false, false, false));

            schema0.Objects.Add(CreateTable("dbo", "sysdiagrams", false));
            schema0.Objects[2].Columns.Add(CreateColumn("id", true, false, false));
            schema0.Objects[2].Columns.Add(CreateColumn("column1", false, false, false));
            schema0.Objects[2].Columns.Add(CreateColumn("column2", false, false, false));

            var schema1 = new SchemaInformationViewModel { Name = "unit" };

            schema1.Objects.Add(CreateTable("unit", "test", false));
            schema1.Objects[0].Columns.Add(CreateColumn("id", true, false, false));
            schema1.Objects[0].Columns.Add(CreateColumn("column1", false, false, false));
            schema1.Objects[0].Columns.Add(CreateColumn("column2", false, false, false));
                  
            schema1.Objects.Add(CreateTable("unit", "foo", true));
            schema1.Objects[1].Columns.Add(CreateColumn("id", true, false, false));
            schema1.Objects[1].Columns.Add(CreateColumn("column1", false, false, false));
            schema1.Objects[1].Columns.Add(CreateColumn("column2", false, false, false));

            return new[] { schema0, schema1 };
        }
    }
}