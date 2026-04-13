using Xunit;

namespace UnitTests.ViewModels
{
    using System.Linq;
    using EFCorePowerTools.Contracts.ViewModels;
    using EFCorePowerTools.ViewModels;
    using GalaSoft.MvvmLight.Messaging;
    using Moq;
    using RevEng.Common;
    public class TableInformationViewModelTests
    {
        [Fact]
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
            Assert.False(propertyChangedInvoked);
        }

        [Fact]
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
            Assert.False(propertyChangedInvoked);
        }

        [Fact]
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
            Assert.False(propertyChangedInvoked);
        }

        [Fact]
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
            Assert.True(propertyChangedInvoked);
        }

        [Fact]
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
            Assert.True(propertyChangedInvoked);
        }

        [Fact]
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
            Assert.True(propertyChangedInvoked);
        }

        [Fact]
        public void PropertyChanged_DisplayName_SameNameNewName()
        {
            // Arrange
            var vm = CreateViewModel();
            vm.Name = "album";
            vm.NewName = "album";

            // Act
            // Assert
            Assert.Equal(vm.Name, vm.DisplayName);
        }

        [Fact]
        public void PropertyChanged_DisplayName_DifferentNameNewName()
        {
            // Arrange
            var vm = CreateViewModel();
            vm.Name = "album";
            vm.NewName = "album2";

            // Act
            // Assert
            Assert.Equal(vm.DisplayName, $"{vm.NewName} - ({vm.Name})");
        }

        [Fact]
        public void PropertyChanged_ObjectTypeIcon_Procedure()
        {
            // Arrange
            var vm = CreateViewModel();
            vm.ObjectType = ObjectType.Procedure;

            // Act
            // Assert
            Assert.Equal(ObjectTypeIcon.Procedure, vm.ObjectTypeIcon);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void PropertyChanged_ObjectTypeIcon_Table(bool hasPrimaryKey)
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
            {
                Assert.Equal(ObjectTypeIcon.Table, vm.ObjectTypeIcon);
            }
            else
            {
                Assert.Equal(ObjectTypeIcon.TableWithoutKey, vm.ObjectTypeIcon);
            }
        }

        [Fact]
        public void PropertyChanged_ObjectTypeIcon_View()
        {
            // Arrange
            var vm = CreateViewModel();
            vm.ObjectType = ObjectType.View;

            // Act
            // Assert
            Assert.Equal(ObjectTypeIcon.View, vm.ObjectTypeIcon);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void PropertyChanged_IsSelected_ColumnsSelection(bool isSelected)
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
            Assert.True(vm.Columns.All(c => c.IsSelected == isSelected));
        }

        [Fact]
        public void PropertyChanged_DisplayName_NewNameDifferentName()
        {
            // Arrange
            var vm = CreateViewModel();
            vm.Name = "table1";

            // Act
            vm.NewName = "newname";

            // Assert
            Assert.Equal("newname - (table1)", vm.DisplayName);
        }

        [Fact]
        public void PropertyChanged_DisplayName_NewNameEqualsName()
        {
            // Arrange
            var vm = CreateViewModel();
            vm.Name = "table1";

            // Act
            vm.NewName = "table1";

            // Assert
            Assert.Equal("table1", vm.DisplayName);
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

        private static TableInformationViewModel CreateViewModel()
        {
            var messenger = new Mock<IMessenger>();
            messenger.SetupAllProperties();
            return new TableInformationViewModel(messenger.Object);
        }

        private static ColumnInformationViewModel CreateColumnViewModel(string name, bool isPrimaryKey = false, bool isForeignKey = false)
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
