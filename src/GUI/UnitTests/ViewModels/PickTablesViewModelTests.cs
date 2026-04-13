using Xunit;

namespace UnitTests.ViewModels
{
    using System.Collections.Generic;
    using EFCorePowerTools.Contracts.ViewModels;
    using EFCorePowerTools.ViewModels;
    using Moq;
    using RevEng.Common;
    public class PickTablesViewModelTests
    {
        [Fact]
        public void Constructors_CommandsInitialized()
        {
            // Arrange
            // Act
            var vm = new PickTablesViewModel(CreateObjectTreeViewModelMock().Object);

            // Assert
            Assert.NotNull(vm.OkCommand);
            Assert.NotNull(vm.CancelCommand);
        }

        [Fact]
        public void Constructors_TreeInitialized()
        {
            // Arrange

            // Act
            var vm = new PickTablesViewModel(CreateObjectTreeViewModelMock().Object);

            // Assert
            Assert.NotNull(vm.ObjectTree);
        }

        [Fact]
        public void Constructors_NoSelection()
        {
            // Arrange
            // Act
            var vm = new PickTablesViewModel(CreateObjectTreeViewModelMock().Object);

            // Assert
            Assert.Null(vm.TableSelectionThreeState);
        }

        [Fact]
        public void Constructors_NoSearchText()
        {
            // Arrange
            // Act
            var vm = new PickTablesViewModel(CreateObjectTreeViewModelMock().Object);

            // Assert
            Assert.Equal(string.Empty, vm.SearchText);
        }

        [Fact]
        public void OkCommand_CanExecute_NoObjects()
        {
            // Arrange
            var vm = new PickTablesViewModel(CreateObjectTreeViewModelMock().Object);

            // Act
            var canExecute = vm.OkCommand.CanExecute(null);

            // Assert
            Assert.False(canExecute);
        }

        [Fact]
        public void OkCommand_CanExecute_ObjectsSelected()
        {
            // Arrange
            var vm = new PickTablesViewModel(CreateObjectTreeViewModelMock(true).Object);

            // Act
            var canExecute = vm.OkCommand.CanExecute(null);

            // Assert
            Assert.True(canExecute);
        }

        [Fact]
        public void OkCommand_Executed()
        {
            // Arrange
            var closeRequested = false;
            bool? dialogResult = null;

            var vm = new PickTablesViewModel(CreateObjectTreeViewModelMock(true).Object);
            vm.CloseRequested += (sender, args) =>
            {
                closeRequested = true;
                dialogResult = args.DialogResult;
            };

            // Act
            vm.OkCommand.Execute(null);

            // Assert
            Assert.True(closeRequested);
            Assert.True(dialogResult);
            Assert.NotEmpty(vm.GetSelectedObjects());
        }

        [Fact]
        public void CancelCommand_CanExecute()
        {
            // Arrange
            var vm = new PickTablesViewModel(CreateObjectTreeViewModelMock(true).Object);

            // Act
            var canExecute = vm.CancelCommand.CanExecute(null);

            // Assert
            Assert.True(canExecute);
        }

        [Fact]
        public void CancelCommand_Executed()
        {
            // Arrange
            var closeRequested = false;
            bool? dialogResult = null;

            var vm = new PickTablesViewModel(CreateObjectTreeViewModelMock().Object);
            vm.CloseRequested += (sender, args) =>
            {
                closeRequested = true;
                dialogResult = args.DialogResult;
            };

            // Act
            vm.CancelCommand.Execute(null);

            // Assert
            Assert.True(closeRequested);
            Assert.False(dialogResult);
        }

        [Fact]
        public void AddObjects_Null()
        {
            // Arrange
            var otvm = CreateObjectTreeViewModelMock();
            var vm = new PickTablesViewModel(otvm.Object);

            // Act
            vm.AddObjects(null, null);
        }

        [Fact]
        public void AddObjects_NotNull()
        {
            // Arrange
            var otvm = CreateObjectTreeViewModelMock();
            var vm = new PickTablesViewModel(otvm.Object);
            var databaseObjects = new TableModel[0];
            var replacingSchemas = new Schema[0];

            // Act
            vm.AddObjects(databaseObjects, replacingSchemas);
            otvm.Verify(c => c.AddObjects(databaseObjects, replacingSchemas), Times.Once);
        }

        [Fact]
        public void SelectTables_Null()
        {
            // Arrange
            var otvm = CreateObjectTreeViewModelMock();
            var vm = new PickTablesViewModel(otvm.Object);

            // Act
            vm.SelectObjects(null);
        }

        [Fact]
        public void SelectTables_NotNull()
        {
            // Arrange
            var otvm = CreateObjectTreeViewModelMock();
            var vm = new PickTablesViewModel(otvm.Object);
            var databaseObjects = new SerializationTableModel[0];

            // Act
            vm.SelectObjects(databaseObjects);
            otvm.Verify(c => c.SelectObjects(databaseObjects), Times.Once);
        }

        [Fact]
        public void TableSelectionThreeState_AllSelected()
        {
            // Arrange
            var otvm = CreateObjectTreeViewModelMock();
            IPickTablesViewModel vm = new PickTablesViewModel(otvm.Object);
            vm.SearchText = "foo";

            // Act
            vm.TableSelectionThreeState = true;

            // Assert
            otvm.Verify(s => s.SetSelectionState(true), Times.Once);
            Assert.Equal(string.Empty, vm.SearchText);
        }

        [Fact]
        public void TableSelectionThreeState_NoneSelected()
        {
            // Arrange
            var otvm = CreateObjectTreeViewModelMock();
            var vm = new PickTablesViewModel(otvm.Object);
            vm.SearchText = "foo";

            // Act
            vm.TableSelectionThreeState = false;

            // Assert
            otvm.Verify(s => s.SetSelectionState(false), Times.Once);
            Assert.Equal(string.Empty, vm.SearchText);
        }

        [Fact]
        public void GetSelectedObjects()
        {
            // Arrange
            var otvm = CreateObjectTreeViewModelMock();
            var vm = new PickTablesViewModel(otvm.Object);

            // Act
            vm.GetSelectedObjects();

            // Assert
            otvm.Verify(s => s.GetSelectedObjects(), Times.Once);
        }

        private static Mock<IObjectTreeViewModel> CreateObjectTreeViewModelMock(bool withSelectedObjects = false)
        {
            var mock = new Mock<IObjectTreeViewModel>();
            mock.SetupAllProperties();
            if (withSelectedObjects)
            {
                mock.Setup(c => c.GetSelectedObjects()).Returns(() => new List<SerializationTableModel>()
                {
                    new SerializationTableModel("obj1", ObjectType.Table, new string[0], null),
                });
            }

            return mock;
        }
    }
}
