using NUnit.Framework;

namespace UnitTests.ViewModels
{
    using EFCorePowerTools.Contracts.ViewModels;
    using EFCorePowerTools.ViewModels;
    using GalaSoft.MvvmLight.Messaging;
    using Moq;
    using RevEng.Shared;
    using System.Linq;

    [TestFixture]
    public class TableInformationViewModelTests
    {
        [Test]
        public void PropertyChanged_Name_SameValue()
        {
            // Arrange
            var propertyChangedInvoked = false;
            var vm = CreateViewModel();
            vm.Name = "album";
            vm.PropertyChanged += (sender, args) => propertyChangedInvoked = true;

            // Act
            vm.Name = "album";

            // Assert
            Assert.IsFalse(propertyChangedInvoked);
        }

        [Test]
        public void PropertyChanged_ObjectType_SameValue()
        {
            // Arrange
            var propertyChangedInvoked = false;
            var vm = CreateViewModel();
            vm.ObjectType = ObjectType.Table;
            vm.PropertyChanged += (sender, args) => propertyChangedInvoked = true;

            // Act
            vm.ObjectType = ObjectType.Table;

            // Assert
            Assert.IsFalse(propertyChangedInvoked);
        }

        [Test]
        public void PropertyChanged_NewName_SameValue()
        {
            // Arrange
            var propertyChangedInvoked = false;
            var vm = CreateViewModel();
            vm.NewName = "album";
            vm.PropertyChanged += (sender, args) => propertyChangedInvoked = true;

            // Act
            vm.NewName = "album";

            // Assert
            Assert.IsFalse(propertyChangedInvoked);
        }

        [Test]
        public void PropertyChanged_Name_DifferentValue()
        {
            // Arrange
            var propertyChangedInvoked = false;
            var vm = CreateViewModel();
            vm.Name = "dbo.album";
            vm.ObjectType = ObjectType.Table;
            vm.PropertyChanged += (sender, args) => propertyChangedInvoked = true;

            // Act
            vm.Name = "dbo.album2";

            // Assert
            Assert.IsTrue(propertyChangedInvoked);
        }

        [Test]
        public void PropertyChanged_ObjectType_DifferentValue()
        {
            // Arrange
            var propertyChangedInvoked = false;
            var vm = CreateViewModel();
            vm.ObjectType = ObjectType.Table;
            vm.PropertyChanged += (sender, args) => propertyChangedInvoked = true;

            // Act
            vm.ObjectType = ObjectType.ScalarFunction;

            // Assert
            Assert.IsTrue(propertyChangedInvoked);
        }

        [Test]
        public void PropertyChanged_NewName_DifferentValue()
        {
            // Arrange
            var propertyChangedInvoked = false;
            var vm = CreateViewModel();
            vm.NewName = "album";
            vm.PropertyChanged += (sender, args) => propertyChangedInvoked = true;

            // Act
            vm.NewName = "album2";

            // Assert
            Assert.IsTrue(propertyChangedInvoked);
        }

        [Test]
        public void PropertyChanged_DisplayName_SameNameNewName()
        {
            // Arrange
            var vm = CreateViewModel();
            vm.Name = "album";
            vm.NewName = "album";

            // Act
            // Assert
            Assert.AreEqual(vm.Name, vm.DisplayName);
        }

        [Test]
        public void PropertyChanged_DisplayName_DifferentNameNewName()
        {
            // Arrange
            var vm = CreateViewModel();
            vm.Name = "album";
            vm.NewName = "album2";

            // Act
            // Assert
            Assert.AreEqual(vm.DisplayName, $"{vm.NewName} - ({vm.Name})");
        }

        [Test]
        public void PropertyChanged_ObjectTypeIcon_Procedure()
        {
            // Arrange
            var vm = CreateViewModel();
            vm.ObjectType = ObjectType.Procedure;

            // Act
            // Assert
            Assert.AreEqual(ObjectTypeIcon.Procedure, vm.ObjectTypeIcon);
        }

        [Test]
        public void PropertyChanged_ObjectTypeIcon_Table([Values(true, false)] bool hasPrimaryKey)
        {
            // Arrange
            var vm = CreateViewModel();
            vm.ObjectType = ObjectType.Table;
            if (hasPrimaryKey)
            {
                vm.Columns.Add(CreateColumnViewModel("id", true));
            }

            // Act
            // Assert
            if (hasPrimaryKey)
                Assert.AreEqual(ObjectTypeIcon.Table, vm.ObjectTypeIcon);
            else
                Assert.AreEqual(ObjectTypeIcon.TableWithoutKey, vm.ObjectTypeIcon);
        }

        [Test]
        public void PropertyChanged_ObjectTypeIcon_View()
        {
            // Arrange
            var vm = CreateViewModel();
            vm.ObjectType = ObjectType.View;

            // Act
            // Assert
            Assert.AreEqual(ObjectTypeIcon.View, vm.ObjectTypeIcon);
        }

        [Test]
        public void PropertyChanged_IsSelected_ColumnsSelection([Values(true, false)] bool isSelected)
        {
            // Arrange
            var vm = CreateViewModel();
            vm.ObjectType = ObjectType.Table;
            vm.Columns.Add(CreateColumnViewModel("column1"));
            vm.Columns.Add(CreateColumnViewModel("column2"));
            vm.Columns.Add(CreateColumnViewModel("column3"));
            // Act
            vm.SetSelectedCommand.Execute(isSelected);

            // Assert
            Assert.IsTrue(vm.Columns.All(c => c.IsSelected == isSelected));
        }

        [Test]
        public void PropertyChanged_DisplayName_NewNameDifferentName()
        {
            // Arrange
            var vm = CreateViewModel();
            vm.Name = "table1";

            // Act
            vm.NewName = "newname";

            // Assert
            Assert.AreEqual("newname - (table1)", vm.DisplayName);
        }

        [Test]
        public void PropertyChanged_DisplayName_NewNameEqualsName()
        {
            // Arrange
            var vm = CreateViewModel();
            vm.Name = "table1";

            // Act
            vm.NewName = "table1";

            // Assert
            Assert.AreEqual("table1", vm.DisplayName);
        }

        [Test]
        public void StartEditing_Execute()
        {
            // Arrange
            var vm = CreateViewModel();
            vm.Name = "table1";

            // Act
            vm.StartEditCommand.Execute(null);

            // Assert
            Assert.IsTrue(vm.IsEditing);
        }

        [Test]
        public void ConfirmEditing_Execute()
        {
            // Arrange
            var vm = CreateViewModel();
            vm.Name = "table1";
            vm.StartEditCommand.Execute(null);

            // Act
            vm.NewName = "newtable1";
            vm.ConfirmEditCommand.Execute(null);

            // Assert
            Assert.IsFalse(vm.IsEditing);
            Assert.AreNotSame(vm.Name, vm.NewName);
        }

        [Test]
        public void CancelEditing_Execute()
        {
            // Arrange
            var vm = CreateViewModel();
            vm.Name = "table1";
            vm.StartEditCommand.Execute(null);

            // Act
            vm.NewName = "newtable1";
            vm.CancelEditCommand.Execute(null);

            // Assert
            Assert.IsFalse(vm.IsEditing);
            Assert.AreSame(vm.Name, vm.NewName);
        }

        private TableInformationViewModel CreateViewModel()
        {

            var messenger = new Mock<IMessenger>();
            messenger.SetupAllProperties();
            return new TableInformationViewModel(messenger.Object);
        }

        private ColumnInformationViewModel CreateColumnViewModel(string name, bool isPrimaryKey = false, bool isForeignKey = false)
        {
            var messenger = new Mock<IMessenger>();
            messenger.SetupAllProperties();
            var vm = new ColumnInformationViewModel(messenger.Object);
            vm.Name = name;
            vm.IsPrimaryKey = isPrimaryKey;
            vm.IsForeignKey = isForeignKey;
            return vm;
        }
    }
}