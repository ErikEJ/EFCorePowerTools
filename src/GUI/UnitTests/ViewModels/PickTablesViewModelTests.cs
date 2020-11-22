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
        public void Constructors_ArgumentNullException_TableInformationViewModelFactory()
        {
            // Arrange
            Func<ITableInformationViewModel> t = null;
            Func<IColumnInformationViewModel> c = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new PickTablesViewModel(t, c));
        }

        [Test]
        public void Constructors_ArgumentNullException_ColumnInformationViewModelFactory()
        {
            // Arrange
            Func<ITableInformationViewModel> t = null;
            Func<IColumnInformationViewModel> c = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new PickTablesViewModel(t, c));
        }

        [Test]
        public void Constructors_CommandsInitialized()
        {
            // Arrange
            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IColumnInformationViewModel CreateColumnInformationViewModelMockObject()
            {
                var mock = new Mock<IColumnInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            // Act
            var vm = new PickTablesViewModel(CreateTableInformationViewModelMockObject, CreateColumnInformationViewModelMockObject);

            // Assert
            Assert.IsNotNull(vm.LoadedCommand);
            Assert.IsNotNull(vm.OkCommand);
            Assert.IsNotNull(vm.CancelCommand);
        }

        [Test]
        public void Constructors_CollectionsInitialized()
        {
            // Arrange
            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IColumnInformationViewModel CreateColumnInformationViewModelMockObject()
            {
                var mock = new Mock<IColumnInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            // Act
            var vm = new PickTablesViewModel(CreateTableInformationViewModelMockObject, CreateColumnInformationViewModelMockObject);

            // Assert
            Assert.IsNotNull(vm.Tables);
            Assert.IsNotNull(vm.FilteredTables);
        }

        [Test]
        public void Constructors_NoSelection()
        {
            // Arrange
            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IColumnInformationViewModel CreateColumnInformationViewModelMockObject()
            {
                var mock = new Mock<IColumnInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            // Act
            var vm = new PickTablesViewModel(CreateTableInformationViewModelMockObject, CreateColumnInformationViewModelMockObject);

            // Assert
            Assert.IsNull(vm.TableSelectionThreeState);
        }

        [Test]
        public void Constructors_NoSearchText()
        {
            // Arrange
            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IColumnInformationViewModel CreateColumnInformationViewModelMockObject()
            {
                var mock = new Mock<IColumnInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            // Act
            var vm = new PickTablesViewModel(CreateTableInformationViewModelMockObject, CreateColumnInformationViewModelMockObject);

            // Assert
            Assert.AreEqual(string.Empty, vm.SearchText);
        }

        [Test]
        public void LoadedCommand_CanExecute()
        {
            // Arrange
            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IColumnInformationViewModel CreateColumnInformationViewModelMockObject()
            {
                var mock = new Mock<IColumnInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            var vm = new PickTablesViewModel(CreateTableInformationViewModelMockObject, CreateColumnInformationViewModelMockObject);

            // Act
            var canExecute = vm.LoadedCommand.CanExecute(null);

            // Assert
            Assert.IsTrue(canExecute);
        }

        [Test]
        public void LoadedCommand_Executed()
        {
            // Arrange
            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IColumnInformationViewModel CreateColumnInformationViewModelMockObject()
            {
                var mock = new Mock<IColumnInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            var vm = new PickTablesViewModel(CreateTableInformationViewModelMockObject, CreateColumnInformationViewModelMockObject);
            var tt = GetTestViewModels();
            foreach (var tvm in tt)
                vm.Tables.Add(tvm);

            // Act
            vm.LoadedCommand.Execute(null);

            // Assert
            Assert.IsFalse(tt[0].IsSelected);
            Assert.IsFalse(tt[1].IsSelected);
            Assert.IsFalse(tt[2].IsSelected);
            Assert.IsFalse(tt[3].IsSelected);
            Assert.IsTrue(tt[4].IsSelected);
            Assert.IsTrue(tt[5].IsSelected);
        }

        [Test]
        public void OkCommand_CanExecute_NoTables()
        {
            // Arrange

            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IColumnInformationViewModel CreateColumnInformationViewModelMockObject()
            {
                var mock = new Mock<IColumnInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            var vm = new PickTablesViewModel(CreateTableInformationViewModelMockObject, CreateColumnInformationViewModelMockObject);

            // Act
            var canExecute = vm.OkCommand.CanExecute(null);

            // Assert
            Assert.IsFalse(canExecute);
        }

        [Test]
        public void OkCommand_CanExecute_NoTablesWithPrimaryKeySelected()
        {
            // Arrange
            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IColumnInformationViewModel CreateColumnInformationViewModelMockObject()
            {
                var mock = new Mock<IColumnInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            var vm = new PickTablesViewModel(CreateTableInformationViewModelMockObject, CreateColumnInformationViewModelMockObject);
            var tt = GetTestViewModels();
            foreach (var tvm in tt)
            {
                tvm.IsSelected = false;
                tvm.HasPrimaryKey = false;
                vm.Tables.Add(tvm);
            }

            // Act
            var canExecute = vm.OkCommand.CanExecute(null);

            // Assert
            Assert.IsFalse(canExecute);
        }

        [Test]
        public void OkCommand_CanExecute_TablesWithPrimaryKeySelected()
        {
            // Arrange
            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IColumnInformationViewModel CreateColumnInformationViewModelMockObject()
            {
                var mock = new Mock<IColumnInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }
            
            var vm = new PickTablesViewModel(CreateTableInformationViewModelMockObject, CreateColumnInformationViewModelMockObject);
            var tt = GetTestViewModels();
            foreach (var tvm in tt)
            {
                vm.Tables.Add(tvm);
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

            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IColumnInformationViewModel CreateColumnInformationViewModelMockObject()
            {
                var mock = new Mock<IColumnInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }
            
            var vm = new PickTablesViewModel(CreateTableInformationViewModelMockObject, CreateColumnInformationViewModelMockObject);
            vm.CloseRequested += (sender,
                                  args) =>
            {
                closeRequested = true;
                dialogResult = args.DialogResult;
            };
            var tt = GetTestViewModels();
            foreach (var tvm in tt)
            {
                vm.Tables.Add(tvm);
            }

            // Act
            vm.OkCommand.Execute(null);

            // Assert
            Assert.IsTrue(closeRequested);
            Assert.IsTrue(dialogResult);
            Assert.IsNotEmpty(vm.Tables);
        }

        [Test]
        public void CancelCommand_CanExecute()
        {
            // Arrange
            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IColumnInformationViewModel CreateColumnInformationViewModelMockObject()
            {
                var mock = new Mock<IColumnInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            var vm = new PickTablesViewModel(CreateTableInformationViewModelMockObject, CreateColumnInformationViewModelMockObject);

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

            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IColumnInformationViewModel CreateColumnInformationViewModelMockObject()
            {
                var mock = new Mock<IColumnInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            var vm = new PickTablesViewModel(CreateTableInformationViewModelMockObject, CreateColumnInformationViewModelMockObject);
            vm.CloseRequested += (sender,
                                  args) =>
            {
                closeRequested = true;
                dialogResult = args.DialogResult;
            };
            var tt = GetTestViewModels();
            foreach (var tvm in tt)
            {
                vm.Tables.Add(tvm);
            }

            // Act
            vm.CancelCommand.Execute(null);

            // Assert
            Assert.IsTrue(closeRequested);
            Assert.IsFalse(dialogResult);
            Assert.IsEmpty(vm.Tables);
        }

        [Test]
        public void AddTables_Null()
        {
            // Arrange

            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IColumnInformationViewModel CreateColumnInformationViewModelMockObject()
            {
                var mock = new Mock<IColumnInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IPickTablesViewModel vm = new PickTablesViewModel(CreateTableInformationViewModelMockObject, CreateColumnInformationViewModelMockObject);

            // Act
            vm.AddTables(null);

            // Assert
            Assert.IsEmpty(vm.Tables);
        }

        [Test]
        public void AddTables_EmptyCollection()
        {
            // Arrange
            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IColumnInformationViewModel CreateColumnInformationViewModelMockObject()
            {
                var mock = new Mock<IColumnInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IPickTablesViewModel vm = new PickTablesViewModel(CreateTableInformationViewModelMockObject, CreateColumnInformationViewModelMockObject);
            var c = new TableModel[0];

            // Act
            vm.AddTables(c);

            // Assert
            Assert.IsEmpty(vm.Tables);
        }

        [Test]
        public void AddTables_Collection()
        {
            // Arrange
            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>(MockBehavior.Default);
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

            IPickTablesViewModel vm = new PickTablesViewModel(CreateTableInformationViewModelMockObject, CreateColumnInformationViewModelMockObject);
            var tt = GetTestViewModels();
            var c = tt.Select(m => new TableModel(m.Name, m.HasPrimaryKey, m.ObjectType, m.Columns.Select(co => new ColumnModel(co.Name, true)))).ToArray();

            // Act
            vm.AddTables(c);
            var vmt = vm.Tables.Select(m => new TableModel(m.Name, m.HasPrimaryKey, m.ObjectType, m.Columns.Select(co => new ColumnModel(co.Name, true)))).ToArray();

            // Assert
            Assert.IsNotEmpty(vm.Tables);
            Assert.AreEqual(tt.Length, vm.Tables.Count);
            AreTableEqual(c[0], vmt[0]);
            Assert.IsFalse(vm.Tables[0].IsSelected);
            AreTableEqual(c[1], vmt[1]);
            Assert.IsFalse(vm.Tables[1].IsSelected);
            AreTableEqual(c[2], vmt[2]);
            Assert.IsFalse(vm.Tables[2].IsSelected);
            AreTableEqual(c[3], vmt[3]);
            Assert.IsFalse(vm.Tables[3].IsSelected);
            AreTableEqual(c[4], vmt[4]);
            Assert.IsFalse(vm.Tables[4].IsSelected);
            AreTableEqual(c[5], vmt[5]);
            Assert.IsFalse(vm.Tables[5].IsSelected);
        }

        [Test]
        public void SelectTables_Null([Values(true, false)] bool hasTables)
        {
            // Arrange
            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IColumnInformationViewModel CreateColumnInformationViewModelMockObject()
            {
                var mock = new Mock<IColumnInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IPickTablesViewModel vm = new PickTablesViewModel(CreateTableInformationViewModelMockObject, CreateColumnInformationViewModelMockObject);
            ITableInformationViewModel[] tt = null;
            if (hasTables)
            {
                tt = GetTestViewModels();
                foreach (var table in tt)
                {
                    table.IsSelected = false;
                    vm.Tables.Add(table);
                }
            }

            // Act
            vm.SelectTables(null);

            // Assert
            if (hasTables)
            {
                CollectionAssert.AreEqual(tt, vm.Tables);
                Assert.IsTrue(vm.Tables.All(m => m.IsSelected == false));
            }
            else
            {
                Assert.IsEmpty(vm.Tables);
            }
        }

        [Test]
        public void SelectTables_EmptyCollection([Values(true, false)] bool hasTables)
        {
            // Arrange
            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IColumnInformationViewModel CreateColumnInformationViewModelMockObject()
            {
                var mock = new Mock<IColumnInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IPickTablesViewModel vm = new PickTablesViewModel(CreateTableInformationViewModelMockObject, CreateColumnInformationViewModelMockObject);
            var c = new SerializationTableModel[0];
            ITableInformationViewModel[] tt = null;
            if (hasTables)
            {
                tt = GetTestViewModels();
                foreach (var table in tt)
                {
                    table.IsSelected = false;
                    vm.Tables.Add(table);
                }
            }

            // Act
            vm.SelectTables(c);

            // Assert
            if (hasTables)
            {
                CollectionAssert.AreEqual(tt, vm.Tables);
                Assert.IsTrue(vm.Tables.All(m => m.IsSelected == false));
            }
            else
            {
                Assert.IsEmpty(vm.Tables);
            }
        }

        [Test]
        public void SelectTables_Collection([Values(true, false)] bool hasTables)
        {
            // Arrange
            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IColumnInformationViewModel CreateColumnInformationViewModelMockObject()
            {
                var mock = new Mock<IColumnInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IPickTablesViewModel vm = new PickTablesViewModel(CreateTableInformationViewModelMockObject, CreateColumnInformationViewModelMockObject);
            var c = new[]
            {
                new SerializationTableModel("unit.test", ObjectType.Table, null)
            };
            ITableInformationViewModel[] tt = null;
            if (hasTables)
            {
                tt = GetTestViewModels();
                foreach (var table in tt)
                {
                    table.IsSelected = false;
                    vm.Tables.Add(table);
                }
            }

            // Act
            vm.SelectTables(c);

            // Assert
            if (hasTables)
            {
                CollectionAssert.AreEqual(tt, vm.Tables);
                var selectedTableModel = c.Single();
                Assert.IsTrue(vm.Tables.Single(m => m.Name == selectedTableModel.Name).IsSelected);
                Assert.IsTrue(vm.Tables.Where(m => m.Name != selectedTableModel.Name).All(m => m.IsSelected == false));
            }
            else
            {
                Assert.IsEmpty(vm.Tables);
            }
        }

        [Test]
        public void Update_TableSelectionThreeState_OnCollectionChanged_SelectedTable()
        {
            // Arrange
            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IColumnInformationViewModel CreateColumnInformationViewModelMockObject()
            {
                var mock = new Mock<IColumnInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IPickTablesViewModel vm = new PickTablesViewModel(CreateTableInformationViewModelMockObject, CreateColumnInformationViewModelMockObject);
            var tt = GetTestViewModels();

            // Act
            vm.Tables.Add(tt[1]);

            // Assert
            Assert.IsTrue(vm.TableSelectionThreeState);
        }

        [Test]
        public void Update_TableSelectionThreeState_OnCollectionChanged_UnselectedTable()
        {
            // Arrange
            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IColumnInformationViewModel CreateColumnInformationViewModelMockObject()
            {
                var mock = new Mock<IColumnInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IPickTablesViewModel vm = new PickTablesViewModel(CreateTableInformationViewModelMockObject, CreateColumnInformationViewModelMockObject);
            var tt = GetTestViewModels();

            // Act
            vm.Tables.Add(tt[0]);

            // Assert
            Assert.IsFalse(vm.TableSelectionThreeState);
        }

        [Test]
        public void Update_TableSelectionThreeState_OnCollectionChanged_MixedTableSelection()
        {
            // Arrange
            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IColumnInformationViewModel CreateColumnInformationViewModelMockObject()
            {
                var mock = new Mock<IColumnInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IPickTablesViewModel vm = new PickTablesViewModel(CreateTableInformationViewModelMockObject, CreateColumnInformationViewModelMockObject);
            var tt = GetTestViewModels();

            // Act
            vm.Tables.Add(tt[0]);
            vm.Tables.Add(tt[1]);

            // Assert
            Assert.IsNull(vm.TableSelectionThreeState);
        }

        [Test]
        public void Update_TableSelectionThreeState_OnTableSelectionChanged_AllTablesSelected()
        {
            // Arrange
            
            // Test relies on PropertyChanged, so we rely on the real view model here
            ITableInformationViewModel CreateTableInformationViewModel() => new TableInformationViewModel();
            IColumnInformationViewModel CreateColumnInformationViewModel() => new ColumnInformationViewModel();


            IPickTablesViewModel vm = new PickTablesViewModel(CreateTableInformationViewModel, CreateColumnInformationViewModel);
            var tt = GetTestViewModels();
            vm.AddTables(tt.Select(m => new TableModel(m.Name, m.HasPrimaryKey, m.ObjectType, m.Columns.Select(co => new ColumnModel(co.Name, true)))));

            // Act
            foreach (var table in vm.Tables)
            {
                table.IsSelected = true;
            }

            // Assert
            Assert.IsTrue(vm.TableSelectionThreeState);
        }

        [Test]
        public void Update_TableSelectionThreeState_OnTableSelectionChanged_NoTablesSelected()
        {
            // Arrange
            
            // Test relies on PropertyChanged, so we rely on the real view model here
            ITableInformationViewModel CreateTableInformationViewModel() => new TableInformationViewModel();
            IColumnInformationViewModel CreateColumnInformationViewModel() => new ColumnInformationViewModel();

            IPickTablesViewModel vm = new PickTablesViewModel(CreateTableInformationViewModel, CreateColumnInformationViewModel);
            var tt = GetTestViewModels();
            vm.AddTables(tt.Select(m => new TableModel(m.Name, m.HasPrimaryKey, m.ObjectType, new List<ColumnModel>())));

            // Act
            foreach (var table in vm.Tables)
            {
                table.IsSelected = false;
            }

            // Assert
            Assert.IsFalse(vm.TableSelectionThreeState);
        }

        [Test]
        public void Update_TableSelectionThreeState_OnTableSelectionChanged_MixedTableSelection()
        {
            // Arrange

            // Test relies on PropertyChanged, so we rely on the real view model here
            ITableInformationViewModel CreateTableInformationViewModel() => new TableInformationViewModel();
            IColumnInformationViewModel CreateColumnInformationViewModel() => new ColumnInformationViewModel();

            IPickTablesViewModel vm = new PickTablesViewModel(CreateTableInformationViewModel, CreateColumnInformationViewModel);
            var tt = GetTestViewModels();
            vm.AddTables(tt.Select(m => new TableModel(m.Name, m.HasPrimaryKey, m.ObjectType, m.Columns.Select(co => new ColumnModel(co.Name, true)))));

            // Act
            for (var i = 0; i < vm.Tables.Count; i++)
            {
                var table = vm.Tables[i];
                table.IsSelected = i % 2 == 0;
            }

            // Assert
            Assert.IsNull(vm.TableSelectionThreeState);
        }

        [Test]
        public void TableSelectionThreeState_AllSelected()
        {
            // Arrange
            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IColumnInformationViewModel CreateColumnInformationViewModelMockObject()
            {
                var mock = new Mock<IColumnInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IPickTablesViewModel vm = new PickTablesViewModel(CreateTableInformationViewModelMockObject, CreateColumnInformationViewModelMockObject);
            var tt = GetTestViewModels();
            foreach (var table in tt)
            {
                vm.Tables.Add(table);
            }
            vm.SearchText = "foo";

            // Act
            vm.TableSelectionThreeState = true;

            // Assert
            Assert.IsTrue(vm.Tables.All(m => m.IsSelected));
            Assert.AreEqual(string.Empty, vm.SearchText);
        }

        [Test]
        public void TableSelectionThreeState_NoneSelected()
        {
            // Arrange
            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IColumnInformationViewModel CreateColumnInformationViewModelMockObject()
            {
                var mock = new Mock<IColumnInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IPickTablesViewModel vm = new PickTablesViewModel(CreateTableInformationViewModelMockObject, CreateColumnInformationViewModelMockObject);
            var tt = GetTestViewModels();
            foreach (var table in tt)
            {
                vm.Tables.Add(table);
            }
            vm.SearchText = "foo";

            // Act
            vm.TableSelectionThreeState = false;

            // Assert
            Assert.IsTrue(vm.Tables.All(m => !m.IsSelected));
            Assert.AreEqual(string.Empty, vm.SearchText);
        }

        [Test]
        public void TableSelectionThreeState_NullSelected()
        {
            // Arrange
            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IColumnInformationViewModel CreateColumnInformationViewModelMockObject()
            {
                var mock = new Mock<IColumnInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IPickTablesViewModel vm = new PickTablesViewModel(CreateTableInformationViewModelMockObject, CreateColumnInformationViewModelMockObject);
            var tt = GetTestViewModels();
            foreach (var table in tt)
            {
                vm.Tables.Add(table);
            }
            vm.SearchText = "foo";

            // Act
            vm.TableSelectionThreeState = null;

            // Assert
            Assert.IsFalse(vm.Tables.All(m => m.IsSelected));
            Assert.IsFalse(vm.Tables.All(m => !m.IsSelected));
            Assert.AreEqual("foo", vm.SearchText);
        }

        [Test]
        public void SearchText_NoDirectFilter()
        {
            // Arrange
            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IColumnInformationViewModel CreateColumnInformationViewModelMockObject()
            {
                var mock = new Mock<IColumnInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IPickTablesViewModel vm = new PickTablesViewModel(CreateTableInformationViewModelMockObject, CreateColumnInformationViewModelMockObject);
            var tt = GetTestViewModels();
            foreach (var table in tt)
            {
                vm.Tables.Add(table);
            }

            var preFilter = vm.FilteredTables.OfType<ITableInformationViewModel>().ToArray();

            // Act
            vm.SearchText = "dbo";

            // Assert
            Assert.AreEqual(tt.Length, preFilter.Length);
            var postFilter = vm.FilteredTables.OfType<ITableInformationViewModel>().ToArray();
            Assert.AreEqual(tt.Length, postFilter.Length);
        }

        [Test]
        public void SearchText_FilterAfterDelay()
        {
            // Arrange
            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IColumnInformationViewModel CreateColumnInformationViewModelMockObject()
            {
                var mock = new Mock<IColumnInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IPickTablesViewModel vm = new PickTablesViewModel(CreateTableInformationViewModelMockObject, CreateColumnInformationViewModelMockObject);
            var tt = GetTestViewModels();
            foreach (var table in tt)
            {
                vm.Tables.Add(table);
            }

            var preFilter = vm.FilteredTables.OfType<ITableInformationViewModel>().ToArray();

            // Act
            vm.SearchText = "dbo";

            // Assert
            Assert.AreEqual(tt.Length, preFilter.Length);
            Assert.That(() =>
            {
                var postFilter = vm.FilteredTables.OfType<ITableInformationViewModel>().ToArray();
                return postFilter.Length < tt.Length && postFilter.All(m => m.Name.Contains("dbo"));
            }, Is.True.After(1500, 200));
        }

        [Test]
        public void GetResults_NoTables()
        {
            // Arrange
            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IColumnInformationViewModel CreateColumnInformationViewModelMockObject()
            {
                var mock = new Mock<IColumnInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IPickTablesViewModel vm = new PickTablesViewModel(CreateTableInformationViewModelMockObject, CreateColumnInformationViewModelMockObject);

            // Act
            var result = vm.GetResult();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public void GetResults_WithTables_NoneSelected()
        {
            // Arrange
            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IColumnInformationViewModel CreateColumnInformationViewModelMockObject()
            {
                var mock = new Mock<IColumnInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IPickTablesViewModel vm = new PickTablesViewModel(CreateTableInformationViewModelMockObject, CreateColumnInformationViewModelMockObject);
            var tt = GetTestViewModels();
            foreach (var table in tt)
            {
                table.IsSelected = false;
                vm.Tables.Add(table);
            }

            // Act
            var result = vm.GetResult();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public void GetResults_WithTables_AllSelected()
        {
            // Arrange
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

            IPickTablesViewModel vm = new PickTablesViewModel(CreateTableInformationViewModelMockObject, CreateColumnInformationViewModelMockObject);
            var tt = GetTestViewModels();
            var c = tt.Select(m => new SerializationTableModel(m.Name, m.ObjectType, m.Columns.Select(co => co.Name))).ToArray();
            foreach (var table in tt)
            {
                table.IsSelected = true;
                vm.Tables.Add(table);
            }

            // Act
            var result = vm.GetResult();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(6, result.Length);
            AreTableEqual(c[0], result[0], false);
            AreTableEqual(c[1], result[1], false);
            AreTableEqual(c[2], result[2], false);
            AreTableEqual(c[3], result[3], false);
            AreTableEqual(c[4], result[4], false);
            AreTableEqual(c[5], result[5], false);
        }

        [Test]
        public void GetResults_WithTables_PartialSelection()
        {
            // Arrange
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

            IPickTablesViewModel vm = new PickTablesViewModel(CreateTableInformationViewModelMockObject, CreateColumnInformationViewModelMockObject);
            var tt = GetTestViewModels();
            var c = tt.Select(m => new SerializationTableModel(m.Name, m.ObjectType, m.Columns.Select(co => co.Name))).ToArray();
            foreach (var table in tt)
            {
                vm.Tables.Add(table);
            }

            // Act
            var result = vm.GetResult();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.Length);
            AreTableEqual(c[1], result[0], false);
            AreTableEqual(c[2], result[1], false);
            AreTableEqual(c[3], result[2], false);
            AreTableEqual(c[4], result[3], false);
            AreTableEqual(c[5], result[4], false);
        }

        [Test]
        public void GetResults_WithTables_Modified()
        {
            // Arrange
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

            IPickTablesViewModel vm = new PickTablesViewModel(CreateTableInformationViewModelMockObject, CreateColumnInformationViewModelMockObject);
            var tt = GetTestViewModels();
            var c = tt.Select(m => new SerializationTableModel(m.Name, m.ObjectType, m.Columns.Select(co => co.Name))).ToArray();
            foreach (var table in tt)
            {
                vm.Tables.Add(table);
            }

            // Act
            vm.Tables[1].IsSelected = false;
            vm.Tables[2].Columns.First().IsSelected = false;

            var result = vm.GetResult();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Length);
            AreTableEqual(c[2], result[0], false);
            AreTableEqual(c[3], result[1], false);
            Assert.IsFalse(result[1].ExcludedColumns.Any(tc => tc == result[2].ExcludedColumns.First()));
            AreTableEqual(c[4], result[2], false);
            AreTableEqual(c[5], result[3], false);
        }

        private static ITableInformationViewModel[] GetTestViewModels()
        {
            var r = new ITableInformationViewModel[6];

            r[0] = new TableInformationViewModel
            {
                IsSelected = false,
                HasPrimaryKey = true,
                Name = "[dbo].[Atlas]",
            };
            r[0].Columns.Add(new ColumnInformationViewModel
            {
                IsSelected = true,
                Name = "Id"
            });
            r[0].Columns.Add(new ColumnInformationViewModel
            {
                IsSelected = true,
                Name = "column1"
            });
            r[0].Columns.Add(new ColumnInformationViewModel
            {
                IsSelected = true,
                Name = "column2"
            });

            r[1] = new TableInformationViewModel
            {
                IsSelected = true,
                HasPrimaryKey = true,
                Name = "[__].[RefactorLog]",
            };
            r[1].Columns.Add(new ColumnInformationViewModel
            {
                IsSelected = true,
                Name = "Id"
            });
            r[1].Columns.Add(new ColumnInformationViewModel
            {
                IsSelected = true,
                Name = "column1"
            });
            r[1].Columns.Add(new ColumnInformationViewModel
            {
                IsSelected = true,
                Name = "column2"
            });

            r[2] = new TableInformationViewModel
            {
                IsSelected = true,
                HasPrimaryKey = true,
                Name = "[dbo].[__RefactorLog]",
            };
            r[2].Columns.Add(new ColumnInformationViewModel
            {
                IsSelected = true,
                Name = "Id"
            });
            r[2].Columns.Add(new ColumnInformationViewModel
            {
                IsSelected = true,
                Name = "column1"
            });
            r[2].Columns.Add(new ColumnInformationViewModel
            {
                IsSelected = true,
                Name = "column2"
            });

            r[3] = new TableInformationViewModel
            {
                IsSelected = true,
                HasPrimaryKey = true,
                Name = "[dbo].[sysdiagrams]",
            };
            r[3].Columns.Add(new ColumnInformationViewModel
            {
                IsSelected = true,
                Name = "Id"
            });
            r[3].Columns.Add(new ColumnInformationViewModel
            {
                IsSelected = true,
                Name = "column1"
            });
            r[3].Columns.Add(new ColumnInformationViewModel
            {
                IsSelected = true,
                Name = "column2"
            });

            r[4] = new TableInformationViewModel
            {
                IsSelected = true,
                HasPrimaryKey = true,
                Name = "unit.test",
            };
            r[4].Columns.Add(new ColumnInformationViewModel
            {
                IsSelected = true,
                Name = "Id"
            });
            r[4].Columns.Add(new ColumnInformationViewModel
            {
                IsSelected = true,
                Name = "column1"
            });
            r[4].Columns.Add(new ColumnInformationViewModel
            {
                IsSelected = true,
                Name = "column2"
            });

            r[5] = new TableInformationViewModel
            {
                IsSelected = true,
                HasPrimaryKey = true,
                Name = "unit.foo",
            };
            r[5].Columns.Add(new ColumnInformationViewModel
            {
                IsSelected = true,
                Name = "Id"
            });
            r[5].Columns.Add(new ColumnInformationViewModel
            {
                IsSelected = true,
                Name = "column1"
            });
            r[5].Columns.Add(new ColumnInformationViewModel
            {
                IsSelected = true,
                Name = "column2"
            });

            return r;
        }

        private static void AreTableEqual(TableModel a, TableModel b, bool compareColumns = true)
        {
            Assert.AreEqual(a.HasPrimaryKey, b.HasPrimaryKey);
            Assert.AreEqual(a.Name, b.Name);
            Assert.AreEqual(a.ObjectType, b.ObjectType);
            if (compareColumns)
            {
                Assert.AreEqual(a.Columns.ElementAt(0).Name, b.Columns.ElementAt(0).Name);
                Assert.AreEqual(a.Columns.ElementAt(1).Name, b.Columns.ElementAt(1).Name);
                Assert.AreEqual(a.Columns.ElementAt(2).Name, b.Columns.ElementAt(2).Name);
            }
        }

        private static void AreTableEqual(SerializationTableModel a, SerializationTableModel b, bool compareColumns = true)
        {
            Assert.AreEqual(a.Name, b.Name);
            Assert.AreEqual(a.ObjectType, b.ObjectType);
            if (compareColumns)
            {
                Assert.AreEqual(a.ExcludedColumns.ElementAt(0), b.ExcludedColumns.ElementAt(0));
                Assert.AreEqual(a.ExcludedColumns.ElementAt(1), b.ExcludedColumns.ElementAt(1));
                Assert.AreEqual(a.ExcludedColumns.ElementAt(2), b.ExcludedColumns.ElementAt(2));
            }
        }
    }
}