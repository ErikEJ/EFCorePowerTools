using NUnit.Framework;

namespace UnitTests.ViewModels
{
    using EFCorePowerTools.ViewModels;
    using GalaSoft.MvvmLight.Messaging;
    using Moq;
    using NUnit.Framework.Legacy;

    [TestFixture]
    public class ColumnInformationViewModelTests
    {
        [Test]
        public void PropertyChanged_Name_SameValue()
        {
            // Arrange
            var propertyChangedInvoked = false;
            var vm = CreateViewModel();
            vm.Name = "column1";
            vm.PropertyChanged += (sender, args) => propertyChangedInvoked = true;

            // Act
            vm.Name = "column1";

            // Assert
            ClassicAssert.IsFalse(propertyChangedInvoked);
        }

        [Test]
        public void PropertyChanged_IsPrimaryKey_SameValue()
        {
            // Arrange
            var propertyChangedInvoked = false;
            var vm = CreateViewModel();
            vm.Name = "column1";
            vm.IsPrimaryKey = true;
            vm.PropertyChanged += (sender, args) => propertyChangedInvoked = true;

            // Act
            vm.IsPrimaryKey = true;

            // Assert
            ClassicAssert.IsFalse(propertyChangedInvoked);
        }

        [Test]
        public void PropertyChanged_Name_DifferentValue()
        {
            // Arrange
            var propertyChangedInvoked = false;
            var vm = CreateViewModel();
            vm.Name = "column1";
            vm.PropertyChanged += (sender, args) => propertyChangedInvoked = true;

            // Act
            vm.Name = "column2";

            // Assert
            ClassicAssert.IsTrue(propertyChangedInvoked);
        }

        [Test]
        public void PropertyChanged_IsPrimaryKey_DifferentValue()
        {
            // Arrange
            var propertyChangedInvoked = false;
            var vm = CreateViewModel();
            vm.Name = "column1";
            vm.IsPrimaryKey = true;
            vm.PropertyChanged += (sender, args) => propertyChangedInvoked = true;

            // Act
            vm.IsPrimaryKey = false;

            // Assert
            ClassicAssert.IsTrue(propertyChangedInvoked);
        }

        [Test]
        public void PropertyChanged_SetSelectedCommand_PrimaryKeyColumn_TableNotSelected_SetSelected()
        {
            // Arrange
            var vm = CreateViewModel();
            vm.Name = "column1";
            vm.IsPrimaryKey = true;
            vm.IsTableSelected = false;

            // Act
            vm.SetSelectedCommand.Execute(true);

            // Assert
            ClassicAssert.IsFalse(vm.IsSelected);
        }

        [Test]
        public void PropertyChanged_SetSelectedCommand_PrimaryKeyColumn_TableSelected_SetSelected()
        {
            // Arrange
            var vm = CreateViewModel();
            vm.Name = "column1";
            vm.NewName = "column1";
            vm.IsPrimaryKey = true;
            vm.IsTableSelected = true;
            vm.SetSelected(true);

            // Act
            vm.SetSelectedCommand.Execute(false);

            // Assert
            ClassicAssert.IsTrue(vm.IsSelected);
        }

        [Test]
        public void PropertyChanged_SetSelectedCommand_Column_TableSelected_SetSelected()
        {
            // Arrange
            var vm = CreateViewModel();
            vm.Name = "column1";
            vm.IsPrimaryKey = false;
            vm.IsTableSelected = true;

            // Act
            vm.SetSelectedCommand.Execute(true);

            // Assert
            ClassicAssert.IsTrue(vm.IsSelected);
        }

        [Test]
        public void PropertyChanged_DisplayName_NewNameDifferentName()
        {
            // Arrange
            var vm = CreateViewModel();
            vm.Name = "column1";
            vm.IsPrimaryKey = true;

            // Act
            vm.NewName = "newname";

            // Assert
            ClassicAssert.AreEqual("newname - (column1)", vm.DisplayName);
        }

        [Test]
        public void PropertyChanged_DisplayName_NewNameEqualsName()
        {
            // Arrange
            var vm = CreateViewModel();
            vm.Name = "column1";
            vm.IsPrimaryKey = true;

            // Act
            vm.NewName = "column1";

            // Assert
            ClassicAssert.AreEqual("column1", vm.DisplayName);
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
            ClassicAssert.IsTrue(vm.IsEditing);
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
            ClassicAssert.IsFalse(vm.IsEditing);
            ClassicAssert.AreNotSame(vm.Name, vm.NewName);
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
            ClassicAssert.IsFalse(vm.IsEditing);
            ClassicAssert.AreSame(vm.Name, vm.NewName);
        }

        private ColumnInformationViewModel CreateViewModel()
        {
            var messenger = new Mock<IMessenger>();
            messenger.SetupAllProperties();
            return new ColumnInformationViewModel(messenger.Object);
        }
    }
}
