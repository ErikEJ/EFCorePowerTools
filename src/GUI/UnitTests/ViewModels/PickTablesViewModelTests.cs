using NUnit.Framework;

namespace UnitTests.ViewModels
{
    using EFCorePowerTools.Contracts.ViewModels;
    using EFCorePowerTools.ViewModels;
    using Moq;
    using RevEng.Shared;
    using System.Collections.Generic;

    [TestFixture]
    public class PickTablesViewModelTests
    {
        [Test]
        public void Constructors_CommandsInitialized()
        {
            // Arrange
            // Act
            var vm = new PickTablesViewModel(CreateObjectTreeViewModelMock().Object);

            // Assert
            Assert.IsNotNull(vm.OkCommand);
            Assert.IsNotNull(vm.CancelCommand);
        }

        [Test]
        public void Constructors_TreeInitialized()
        {
            // Arrange

            // Act
            var vm = new PickTablesViewModel(CreateObjectTreeViewModelMock().Object);

            // Assert
            Assert.IsNotNull(vm.ObjectTree);
        }

        [Test]
        public void Constructors_NoSelection()
        {
            // Arrange
            // Act
            var vm = new PickTablesViewModel(CreateObjectTreeViewModelMock().Object);

            // Assert
            Assert.IsNull(vm.TableSelectionThreeState);
        }

        [Test]
        public void Constructors_NoSearchText()
        {
            // Arrange
            // Act
            var vm = new PickTablesViewModel(CreateObjectTreeViewModelMock().Object);

            // Assert
            Assert.AreEqual(string.Empty, vm.SearchText);
        }

        [Test]
        public void OkCommand_CanExecute_NoObjects()
        {
            // Arrange
            var vm = new PickTablesViewModel(CreateObjectTreeViewModelMock().Object);

            // Act
            var canExecute = vm.OkCommand.CanExecute(null);

            // Assert
            Assert.IsFalse(canExecute);
        }

        [Test]
        public void OkCommand_CanExecute_ObjectsSelected()
        {
            // Arrange
            var vm = new PickTablesViewModel(CreateObjectTreeViewModelMock(true).Object);

            // Act
            var canExecute = vm.OkCommand.CanExecute(null);

            // Assert
            Assert.IsTrue(canExecute);
        }

        [Test]
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
            Assert.IsTrue(closeRequested);
            Assert.IsTrue(dialogResult);
            Assert.IsNotEmpty(vm.GetSelectedObjects());
        }

        [Test]
        public void CancelCommand_CanExecute()
        {
            // Arrange
            var vm = new PickTablesViewModel(CreateObjectTreeViewModelMock(true).Object);

            // Act
            var canExecute = vm.CancelCommand.CanExecute(null);

            // Assert
            Assert.IsTrue(canExecute);
        }

        [Test]
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
            Assert.IsTrue(closeRequested);
            Assert.IsFalse(dialogResult);
        }

        [Test]
        public void AddObjects_Null()
        {
            // Arrange
            var otvm = CreateObjectTreeViewModelMock();
            var vm = new PickTablesViewModel(otvm.Object);

            // Act
            // Assert
            Assert.DoesNotThrow(() => vm.AddObjects(null, null));
        }

        [Test]
        public void AddObjects_NotNull()
        {
            // Arrange
            var otvm = CreateObjectTreeViewModelMock();
            var vm = new PickTablesViewModel(otvm.Object);
            var databaseObjects = new TableModel[0];
            var replacingSchemas = new Schema[0];

            // Act
            // Assert
            Assert.DoesNotThrow(() => vm.AddObjects(databaseObjects, replacingSchemas));
            otvm.Verify(c => c.AddObjects(databaseObjects, replacingSchemas), Times.Once);
        }

        [Test]
        public void SelectTables_Null()
        {
            // Arrange
            var otvm = CreateObjectTreeViewModelMock();
            var vm = new PickTablesViewModel(otvm.Object);

            // Act
            // Assert
            Assert.DoesNotThrow(() => vm.SelectObjects(null));
        }

        [Test]
        public void SelectTables_NotNull()
        {
            // Arrange
            var otvm = CreateObjectTreeViewModelMock();
            var vm = new PickTablesViewModel(otvm.Object);
            var databaseObjects = new SerializationTableModel[0];

            // Act
            // Assert
            Assert.DoesNotThrow(() => vm.SelectObjects(databaseObjects));
            otvm.Verify(c => c.SelectObjects(databaseObjects), Times.Once);
        }

        [Test]
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
            Assert.AreEqual(string.Empty, vm.SearchText);
        }

        [Test]
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
            Assert.AreEqual(string.Empty, vm.SearchText);
        }

        [Test]
        public void GetSelectedObjects()
        {
            // Arrange
            var otvm = CreateObjectTreeViewModelMock();
            var vm = new PickTablesViewModel(otvm.Object);

            // Act
            var selectedObjects = vm.GetSelectedObjects();

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
                    new SerializationTableModel("obj1", ObjectType.Table, new string[0])
                });
            }
            return mock;
        }
    }
}