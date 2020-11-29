using NUnit.Framework;

namespace UnitTests.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using EFCorePowerTools.Contracts.ViewModels;
    using EFCorePowerTools.Shared.Models;
    using EFCorePowerTools.ViewModels;
    using Moq;
    using RevEng.Shared;

    [TestFixture]
    public class PickTablesViewModelTests
    {
        [Test]
        public void Constructors_CommandsInitialized()
        {
            // Arrange
            // Act
            var vm = new PickTablesViewModel(CreateObjectTreeViewModel());

            // Assert
            Assert.IsNotNull(vm.OkCommand);
            Assert.IsNotNull(vm.CancelCommand);
        }

        [Test]
        public void Constructors_TreeInitialized()
        {
            // Arrange

            // Act
            var vm = new PickTablesViewModel(CreateObjectTreeViewModel());

            // Assert
            Assert.IsNotNull(vm.ObjectTree);
        }

        [Test]
        public void Constructors_NoSelection()
        {
            // Arrange
            // Act
            var vm = new PickTablesViewModel(CreateObjectTreeViewModel());

            // Assert
            Assert.IsNull(vm.TableSelectionThreeState);
        }

        [Test]
        public void Constructors_NoSearchText()
        {
            // Arrange
            // Act
            var vm = new PickTablesViewModel(CreateObjectTreeViewModel());

            // Assert
            Assert.AreEqual(string.Empty, vm.SearchText);
        }

        [Test]
        public void OkCommand_CanExecute_NoObjects()
        {
            // Arrange
            var vm = new PickTablesViewModel(CreateObjectTreeViewModel());

            // Act
            var canExecute = vm.OkCommand.CanExecute(null);

            // Assert
            Assert.IsFalse(canExecute);
        }

        [Test]
        public void OkCommand_CanExecute_ObjectsSelected()
        {
            // Arrange
            IPickTablesViewModel vm = new PickTablesViewModel(CreateObjectTreeViewModel());
            vm.AddObjects(GetDatabaseObjects());
            foreach(var item in vm.ObjectTree.Types.SelectMany(c => c.Objects))
            {
                item.IsSelected = true;
            }

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

            IPickTablesViewModel vm = new PickTablesViewModel(CreateObjectTreeViewModel());
            vm.CloseRequested += (sender,
                                  args) =>
            {
                closeRequested = true;
                dialogResult = args.DialogResult;
            };
            vm.AddObjects(GetDatabaseObjects());
            vm.ObjectTree.Types[0].Objects[0].IsSelected = true;

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
            var vm = new PickTablesViewModel(CreateObjectTreeViewModel());

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

            var vm = new PickTablesViewModel(CreateObjectTreeViewModel());
            vm.CloseRequested += (sender,
                                  args) =>
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
            IPickTablesViewModel vm = new PickTablesViewModel(CreateObjectTreeViewModel());

            // Act
            // Assert
            Assert.DoesNotThrow(() => vm.AddObjects(null));
        }

        [Test]
        public void AddTables_NotNull()
        {
            // Arrange
            IPickTablesViewModel vm = new PickTablesViewModel(CreateObjectTreeViewModel());
            var databaseObjects = new TableModel[0];

            // Act
            // Assert
            Assert.DoesNotThrow(() => vm.AddObjects(databaseObjects));
        }

        [Test]
        public void SelectTables_Null()
        {
            // Arrange
            IPickTablesViewModel vm = new PickTablesViewModel(CreateObjectTreeViewModel());

            // Act
            // Assert
            Assert.DoesNotThrow(() => vm.SelectObjects(null));
        }

        [Test]
        public void SelectTables_NotNull()
        {
            // Arrange
            IPickTablesViewModel vm = new PickTablesViewModel(CreateObjectTreeViewModel());

            // Act
            // Assert
            Assert.DoesNotThrow(() => vm.SelectObjects(null));
        }

        [Test]
        public void TableSelectionThreeState_AllSelected()
        {
            // Arrange
            IPickTablesViewModel vm = new PickTablesViewModel(CreateObjectTreeViewModel());
            var databaseObjects = GetDatabaseObjects();
            vm.AddObjects(databaseObjects);
            vm.SearchText = "foo";

            // Act
            vm.TableSelectionThreeState = true;

            // Assert
            Assert.AreEqual(vm.GetSelectedObjects().Count(), databaseObjects.Count());
            Assert.AreEqual(string.Empty, vm.SearchText);
        }

        [Test]
        public void TableSelectionThreeState_NoneSelected()
        {
            // Arrange
            IPickTablesViewModel vm = new PickTablesViewModel(CreateObjectTreeViewModel());
            var databaseObjects = GetDatabaseObjects();
            vm.AddObjects(databaseObjects);
            vm.SearchText = "foo";

            // Act
            vm.TableSelectionThreeState = false;

            // Assert
            Assert.AreEqual(vm.GetSelectedObjects().Count(), 0);
            Assert.AreEqual(string.Empty, vm.SearchText);
        }

        [Test]
        public void GetSelectedObjects()
        {
            // Arrange
            IPickTablesViewModel vm = new PickTablesViewModel(CreateObjectTreeViewModel());

            // Act
            // Assert
            Assert.DoesNotThrow(() => vm.GetSelectedObjects());
        }

        private static TableModel[] GetDatabaseObjects()
        {
            IEnumerable<ColumnModel> CreateColumnsWithId()
            {
                return new[]
                {
                    new ColumnModel("Id", true),
                    new ColumnModel("column1", false),
                    new ColumnModel("column2", false)
                };
            }

            IEnumerable<ColumnModel> CreateColumnsWithoutId()
            {
                return new[]
                {
                    new ColumnModel("Id", false),
                    new ColumnModel("column1", false),
                    new ColumnModel("column2", false)
                };
            }

            var r = new TableModel[10];

            r[0] = new TableModel("[dbo].[Atlas]", "Atlas", "dbo", ObjectType.Table, CreateColumnsWithId());
            r[1] = new TableModel("[__].[RefactorLog]", "RefactorLog", "", ObjectType.Table, CreateColumnsWithId());
            r[2] = new TableModel("[dbo].[__RefactorLog]", "__RefactorLog", "", ObjectType.Table, CreateColumnsWithId());
            r[3] = new TableModel("[dbo].[sysdiagrams]", "sysdiagrams", "dbo", ObjectType.Table, CreateColumnsWithId());
            r[4] = new TableModel("[unit].[test]", "test", "unit", ObjectType.Table, CreateColumnsWithId());
            r[5] = new TableModel("[unit].[foo]", "foo", "unit", ObjectType.Table, CreateColumnsWithId());
            r[6] = new TableModel("[views].[view1]", "view1", "views", ObjectType.View, CreateColumnsWithoutId());
            r[7] = new TableModel("[views].[view2]", "view2", "views", ObjectType.View, CreateColumnsWithoutId());
            r[8] = new TableModel("[stored].[procedure1]", "procedure1", "stored", ObjectType.Procedure, new ColumnModel[0]);
            r[9] = new TableModel("[stored].[procedure2]", "procedure2", "stored", ObjectType.Procedure, new ColumnModel[0]);
            return r;
        }

        private static IObjectTreeViewModel CreateObjectTreeViewModel()
        {
            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                mock.SetupGet(g => g.Columns).Returns(new ObservableCollection<IColumnInformationViewModel>());
                return mock.Object;
            }

            IColumnInformationViewModel CreateColumnInformationViewModelMockObject()
            {
                var mock = new Mock<IColumnInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            return new ObjectTreeViewModel(CreateTableInformationViewModelMockObject, CreateColumnInformationViewModelMockObject);
        }
    }
}