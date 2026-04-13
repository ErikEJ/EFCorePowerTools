using Xunit;

namespace UnitTests.ViewModels
{
    using EFCorePowerTools.ViewModels;
    using GalaSoft.MvvmLight.Messaging;
    using Moq;
    public class ColumnInformationViewModelTests
    {
        [Fact]
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
            Assert.False(propertyChangedInvoked);
        }

        [Fact]
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
            Assert.False(propertyChangedInvoked);
        }

        [Fact]
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
            Assert.True(propertyChangedInvoked);
        }

        [Fact]
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
            Assert.True(propertyChangedInvoked);
        }

        [Fact]
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
            Assert.False(vm.IsSelected);
        }

        [Fact]
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
            Assert.True(vm.IsSelected);
        }

        [Fact]
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
            Assert.True(vm.IsSelected);
        }

        [Fact]
        public void PropertyChanged_DisplayName_NewNameDifferentName()
        {
            // Arrange
            var vm = CreateViewModel();
            vm.Name = "column1";
            vm.IsPrimaryKey = true;

            // Act
            vm.NewName = "newname";

            // Assert
            Assert.Equal("newname - (column1)", vm.DisplayName);
        }

        [Fact]
        public void PropertyChanged_DisplayName_NewNameEqualsName()
        {
            // Arrange
            var vm = CreateViewModel();
            vm.Name = "column1";
            vm.IsPrimaryKey = true;

            // Act
            vm.NewName = "column1";

            // Assert
            Assert.Equal("column1", vm.DisplayName);
        }

        [Fact]
        public void StartEditing_Execute()
        {
            // Arrange
            var vm = CreateViewModel();
            vm.Name = "table1";

            // Act
            vm.StartEditCommand.Execute(null);

            // Assert
            Assert.True(vm.IsEditing);
        }

        [Fact]
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
            Assert.False(vm.IsEditing);
            Assert.NotEqual(vm.Name, vm.NewName);
        }

        [Fact]
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
            Assert.False(vm.IsEditing);
            Assert.Equal(vm.Name, vm.NewName);
        }

        private static ColumnInformationViewModel CreateViewModel()
        {
            var messenger = new Mock<IMessenger>();
            messenger.SetupAllProperties();
            return new ColumnInformationViewModel(messenger.Object);
        }
    }
}
