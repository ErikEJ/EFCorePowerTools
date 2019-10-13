using NUnit.Framework;

namespace UnitTests.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EFCorePowerTools.Contracts.ViewModels;
    using EFCorePowerTools.Shared.DAL;
    using EFCorePowerTools.Shared.Models;
    using EFCorePowerTools.ViewModels;
    using Moq;

    [TestFixture]
    public class PickTablesViewModelTests
    {
        [Test]
        public void Constructors_ArgumentNullException_OperatingSystemAccess()
        {
            // Arrange
            IOperatingSystemAccess osa = null;
            IFileSystemAccess fsa = null;
            Func<ITableInformationViewModel> t = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new PickTablesViewModel(osa, fsa, t));
        }

        [Test]
        public void Constructors_ArgumentNullException_FileSystemAccess()
        {
            // Arrange
            var osa = Mock.Of<IOperatingSystemAccess>();
            IFileSystemAccess fsa = null;
            Func<ITableInformationViewModel> t = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new PickTablesViewModel(osa, fsa, t));
        }

        [Test]
        public void Constructors_ArgumentNullException_TableInformationViewModelFactory()
        {
            // Arrange
            var osa = Mock.Of<IOperatingSystemAccess>();
            var fsa = Mock.Of<IFileSystemAccess>();
            Func<ITableInformationViewModel> t = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new PickTablesViewModel(osa, fsa, t));
        }

        [Test]
        public void Constructors_CommandsInitialized()
        {
            // Arrange
            var osa = Mock.Of<IOperatingSystemAccess>();
            var fsa = Mock.Of<IFileSystemAccess>();

            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            // Act
            var vm = new PickTablesViewModel(osa, fsa, CreateTableInformationViewModelMockObject);

            // Assert
            Assert.IsNotNull(vm.LoadedCommand);
            Assert.IsNotNull(vm.SaveSelectionCommand);
            Assert.IsNotNull(vm.LoadSelectionCommand);
            Assert.IsNotNull(vm.OkCommand);
            Assert.IsNotNull(vm.CancelCommand);
        }

        [Test]
        public void Constructors_CollectionsInitialized()
        {
            // Arrange
            var osa = Mock.Of<IOperatingSystemAccess>();
            var fsa = Mock.Of<IFileSystemAccess>();

            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            // Act
            var vm = new PickTablesViewModel(osa, fsa, CreateTableInformationViewModelMockObject);

            // Assert
            Assert.IsNotNull(vm.Tables);
            Assert.IsNotNull(vm.FilteredTables);
        }

        [Test]
        public void Constructors_NoSelection()
        {
            // Arrange
            var osa = Mock.Of<IOperatingSystemAccess>();
            var fsa = Mock.Of<IFileSystemAccess>();

            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            // Act
            var vm = new PickTablesViewModel(osa, fsa, CreateTableInformationViewModelMockObject);

            // Assert
            Assert.IsNull(vm.TableSelectionThreeState);
        }

        [Test]
        public void Constructors_NoSearchText()
        {
            // Arrange
            var osa = Mock.Of<IOperatingSystemAccess>();
            var fsa = Mock.Of<IFileSystemAccess>();

            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            // Act
            var vm = new PickTablesViewModel(osa, fsa, CreateTableInformationViewModelMockObject);

            // Assert
            Assert.AreEqual(string.Empty, vm.SearchText);
        }

        [Test]
        public void LoadedCommand_CanExecute()
        {
            // Arrange
            var osa = Mock.Of<IOperatingSystemAccess>();
            var fsa = Mock.Of<IFileSystemAccess>();

            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            var vm = new PickTablesViewModel(osa, fsa, CreateTableInformationViewModelMockObject);

            // Act
            var canExecute = vm.LoadedCommand.CanExecute(null);

            // Assert
            Assert.IsTrue(canExecute);
        }

        [Test]
        public void LoadedCommand_Executed()
        {
            // Arrange
            var osa = Mock.Of<IOperatingSystemAccess>();
            var fsa = Mock.Of<IFileSystemAccess>();

            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            var vm = new PickTablesViewModel(osa, fsa, CreateTableInformationViewModelMockObject);
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
        public void SaveSelectionCommand_CanExecute_NoTables()
        {
            // Arrange
            var osa = Mock.Of<IOperatingSystemAccess>();
            var fsa = Mock.Of<IFileSystemAccess>();

            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            var vm = new PickTablesViewModel(osa, fsa, CreateTableInformationViewModelMockObject);

            // Act
            var canExecute = vm.SaveSelectionCommand.CanExecute(null);

            // Assert
            Assert.IsFalse(canExecute);
        }

        [Test]
        public void SaveSelectionCommand_CanExecute_NoTablesSelected()
        {
            // Arrange
            var osa = Mock.Of<IOperatingSystemAccess>();
            var fsa = Mock.Of<IFileSystemAccess>();

            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            var vm = new PickTablesViewModel(osa, fsa, CreateTableInformationViewModelMockObject);
            var tt = GetTestViewModels();
            foreach (var tvm in tt)
            {
                tvm.IsSelected = false;
                vm.Tables.Add(tvm);
            }

            // Act
            var canExecute = vm.SaveSelectionCommand.CanExecute(null);

            // Assert
            Assert.IsFalse(canExecute);
        }

        [Test]
        public void SaveSelectionCommand_CanExecute_NoTablesWithPrimaryKey()
        {
            // Arrange
            var osa = Mock.Of<IOperatingSystemAccess>();
            var fsa = Mock.Of<IFileSystemAccess>();

            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            var vm = new PickTablesViewModel(osa, fsa, CreateTableInformationViewModelMockObject);
            var tt = GetTestViewModels();
            foreach (var tvm in tt)
            {
                tvm.Model.HasPrimaryKey = false;
                vm.Tables.Add(tvm);
            }

            // Act
            var canExecute = vm.SaveSelectionCommand.CanExecute(null);

            // Assert
            Assert.IsFalse(canExecute);
        }

        [Test]
        public void SaveSelectionCommand_CanExecute_TablesWithPrimaryKeyAndSelected()
        {
            // Arrange
            var osa = Mock.Of<IOperatingSystemAccess>();
            var fsa = Mock.Of<IFileSystemAccess>();

            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            var vm = new PickTablesViewModel(osa, fsa, CreateTableInformationViewModelMockObject);
            var tt = GetTestViewModels();
            foreach (var tvm in tt)
            {
                vm.Tables.Add(tvm);
            }

            // Act
            var canExecute = vm.SaveSelectionCommand.CanExecute(null);

            // Assert
            Assert.IsTrue(canExecute);
        }

        [Test]
        public void SaveSelectionCommand_Executed_NoFileSelected()
        {
            // Arrange
            var osaMock = new Mock<IOperatingSystemAccess>();
            osaMock.Setup(m => m.RequestSaveFileName(It.IsNotNull<string>(), It.Is<string>(s => s.Contains("*.txt") && s.Contains("*.*")), true)).Returns<string>(null);
            var fsaMock = new Mock<IFileSystemAccess>();

            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            var vm = new PickTablesViewModel(osaMock.Object, fsaMock.Object, CreateTableInformationViewModelMockObject);
            var tt = GetTestViewModels();
            foreach (var tvm in tt)
            {
                vm.Tables.Add(tvm);
            }

            // Act
            vm.SaveSelectionCommand.Execute(null);

            // Assert
            osaMock.Verify(m => m.RequestSaveFileName(It.IsNotNull<string>(), It.Is<string>(s => s.Contains("*.txt") && s.Contains("*.*")), true), Times.Once);
            fsaMock.Verify(m => m.WriteAllLines(It.IsAny<string>(), It.IsAny<IEnumerable<string>>()), Times.Never);
        }

        [Test]
        public void SaveSelectionCommand_Executed_FileSelected()
        {
            // Arrange
            const string testFilePath = @"C:\Temp\Unit.Test";
            var osaMock = new Mock<IOperatingSystemAccess>();
            osaMock.Setup(m => m.RequestSaveFileName(It.IsNotNull<string>(), It.Is<string>(s => s.Contains("*.txt") && s.Contains("*.*")), true)).Returns(testFilePath);
            var fsaMock = new Mock<IFileSystemAccess>();

            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            var vm = new PickTablesViewModel(osaMock.Object, fsaMock.Object, CreateTableInformationViewModelMockObject);
            var tt = GetTestViewModels();
            foreach (var tvm in tt)
            {
                vm.Tables.Add(tvm);
            }

            // Act
            vm.SaveSelectionCommand.Execute(null);

            // Assert
            osaMock.Verify(m => m.RequestSaveFileName(It.IsNotNull<string>(), It.Is<string>(s => s.Contains("*.txt") && s.Contains("*.*")), true), Times.Once);
            fsaMock.Verify(m => m.WriteAllLines(testFilePath, It.Is<IEnumerable<string>>(c => c.Count() == 3)), Times.Once);
        }

        [Test]
        public void LoadSelectionCommand_CanExecute_NoTables()
        {
            // Arrange
            var osa = Mock.Of<IOperatingSystemAccess>();
            var fsa = Mock.Of<IFileSystemAccess>();

            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            var vm = new PickTablesViewModel(osa, fsa, CreateTableInformationViewModelMockObject);

            // Act
            var canExecute = vm.LoadSelectionCommand.CanExecute(null);

            // Assert
            Assert.IsFalse(canExecute);
        }

        [Test]
        public void LoadSelectionCommand_CanExecute_Tables()
        {
            // Arrange
            var osa = Mock.Of<IOperatingSystemAccess>();
            var fsa = Mock.Of<IFileSystemAccess>();

            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            var vm = new PickTablesViewModel(osa, fsa, CreateTableInformationViewModelMockObject);
            var tt = GetTestViewModels();
            foreach (var tvm in tt)
            {
                vm.Tables.Add(tvm);
            }

            // Act
            var canExecute = vm.LoadSelectionCommand.CanExecute(null);

            // Assert
            Assert.IsTrue(canExecute);
        }

        [Test]
        public void LoadSelectionCommand_Executed_NoFileSelected()
        {
            // Arrange
            var osaMock = new Mock<IOperatingSystemAccess>();
            osaMock.Setup(m => m.RequestLoadFileName(It.IsNotNull<string>(), It.Is<string>(s => s.Contains("*.txt") && s.Contains("*.*")), false, true)).Returns<string>(null);
            var fsaMock = new Mock<IFileSystemAccess>();

            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            var vm = new PickTablesViewModel(osaMock.Object, fsaMock.Object, CreateTableInformationViewModelMockObject);
            var tt = GetTestViewModels();
            foreach (var tvm in tt)
            {
                vm.Tables.Add(tvm);
            }

            // Act
            vm.LoadSelectionCommand.Execute(null);

            // Assert
            osaMock.Verify(m => m.RequestLoadFileName(It.IsNotNull<string>(), It.Is<string>(s => s.Contains("*.txt") && s.Contains("*.*")), false, true), Times.Once);
            fsaMock.Verify(m => m.ReadAllLines(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void LoadSelectionCommand_Executed_FileSelected()
        {
            // Arrange
            const string filePath = @"C:\Temp\unit.test";
            var tl = new[]
            {
                "unit.test",
                "__.RefactorLog",
                "foo.bar",
                "foo",
                "unit.foo"
            };
            var osaMock = new Mock<IOperatingSystemAccess>();
            osaMock.Setup(m => m.RequestLoadFileName(It.IsNotNull<string>(), It.Is<string>(s => s.Contains("*.txt") && s.Contains("*.*")), false, true)).Returns(filePath);
            var fsaMock = new Mock<IFileSystemAccess>();
            fsaMock.Setup(m => m.ReadAllLines(filePath)).Returns(tl);

            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            var vm = new PickTablesViewModel(osaMock.Object, fsaMock.Object, CreateTableInformationViewModelMockObject);
            var tt = GetTestViewModels();
            foreach (var tvm in tt)
            {
                vm.Tables.Add(tvm);
            }

            vm.SearchText = "test";

            // Act
            vm.LoadSelectionCommand.Execute(null);

            // Assert
            osaMock.Verify(m => m.RequestLoadFileName(It.IsNotNull<string>(), It.Is<string>(s => s.Contains("*.txt") && s.Contains("*.*")), false, true), Times.Once);
            fsaMock.Verify(m => m.ReadAllLines(filePath), Times.Once);
            Assert.IsFalse(tt[0].IsSelected);
            Assert.IsFalse(tt[1].IsSelected);
            Assert.IsFalse(tt[2].IsSelected);
            Assert.IsFalse(tt[3].IsSelected);
            Assert.IsTrue(tt[4].IsSelected);
            Assert.IsFalse(tt[5].IsSelected);
            Assert.AreEqual(string.Empty, vm.SearchText);
        }

        [Test]
        public void OkCommand_CanExecute_NoTables()
        {
            // Arrange
            var osa = Mock.Of<IOperatingSystemAccess>();
            var fsa = Mock.Of<IFileSystemAccess>();

            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            var vm = new PickTablesViewModel(osa, fsa, CreateTableInformationViewModelMockObject);

            // Act
            var canExecute = vm.OkCommand.CanExecute(null);

            // Assert
            Assert.IsFalse(canExecute);
        }

        [Test]
        public void OkCommand_CanExecute_NoTablesWithPrimaryKeySelected()
        {
            // Arrange
            var osa = Mock.Of<IOperatingSystemAccess>();
            var fsa = Mock.Of<IFileSystemAccess>();

            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            var vm = new PickTablesViewModel(osa, fsa, CreateTableInformationViewModelMockObject);
            var tt = GetTestViewModels();
            foreach (var tvm in tt)
            {
                tvm.IsSelected = false;
                tvm.Model.HasPrimaryKey = false;
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
            var osa = Mock.Of<IOperatingSystemAccess>();
            var fsa = Mock.Of<IFileSystemAccess>();

            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            var vm = new PickTablesViewModel(osa, fsa, CreateTableInformationViewModelMockObject);
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
            var osa = Mock.Of<IOperatingSystemAccess>();
            var fsa = Mock.Of<IFileSystemAccess>();

            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            var vm = new PickTablesViewModel(osa, fsa, CreateTableInformationViewModelMockObject);
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
            var osa = Mock.Of<IOperatingSystemAccess>();
            var fsa = Mock.Of<IFileSystemAccess>();

            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            var vm = new PickTablesViewModel(osa, fsa, CreateTableInformationViewModelMockObject);

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
            var osa = Mock.Of<IOperatingSystemAccess>();
            var fsa = Mock.Of<IFileSystemAccess>();

            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            var vm = new PickTablesViewModel(osa, fsa, CreateTableInformationViewModelMockObject);
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
            var osa = Mock.Of<IOperatingSystemAccess>();
            var fsa = Mock.Of<IFileSystemAccess>();

            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IPickTablesViewModel vm = new PickTablesViewModel(osa, fsa, CreateTableInformationViewModelMockObject);

            // Act
            vm.AddTables(null);

            // Assert
            Assert.IsEmpty(vm.Tables);
        }

        [Test]
        public void AddTables_EmptyCollection()
        {
            // Arrange
            var osa = Mock.Of<IOperatingSystemAccess>();
            var fsa = Mock.Of<IFileSystemAccess>();

            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IPickTablesViewModel vm = new PickTablesViewModel(osa, fsa, CreateTableInformationViewModelMockObject);
            var c = new TableInformationModel[0];

            // Act
            vm.AddTables(c);

            // Assert
            Assert.IsEmpty(vm.Tables);
        }

        [Test]
        public void AddTables_Collection()
        {
            // Arrange
            var osa = Mock.Of<IOperatingSystemAccess>();
            var fsa = Mock.Of<IFileSystemAccess>();

            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IPickTablesViewModel vm = new PickTablesViewModel(osa, fsa, CreateTableInformationViewModelMockObject);
            var tt = GetTestViewModels();
            var c = tt.Select(m => m.Model).ToArray();

            // Act
            vm.AddTables(c);

            // Assert
            Assert.IsNotEmpty(vm.Tables);
            Assert.AreEqual(tt.Length, vm.Tables.Count);
            Assert.AreSame(tt[0].Model, vm.Tables[0].Model);
            Assert.IsFalse(vm.Tables[0].IsSelected);
            Assert.AreSame(tt[1].Model, vm.Tables[1].Model);
            Assert.IsFalse(vm.Tables[1].IsSelected);
            Assert.AreSame(tt[2].Model, vm.Tables[2].Model);
            Assert.IsFalse(vm.Tables[2].IsSelected);
            Assert.AreSame(tt[3].Model, vm.Tables[3].Model);
            Assert.IsFalse(vm.Tables[3].IsSelected);
            Assert.AreSame(tt[4].Model, vm.Tables[4].Model);
            Assert.IsFalse(vm.Tables[4].IsSelected);
            Assert.AreSame(tt[5].Model, vm.Tables[5].Model);
            Assert.IsFalse(vm.Tables[5].IsSelected);
        }

        [Test]
        public void SelectTables_Null([Values(true, false)] bool hasTables)
        {
            // Arrange
            var osa = Mock.Of<IOperatingSystemAccess>();
            var fsa = Mock.Of<IFileSystemAccess>();

            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IPickTablesViewModel vm = new PickTablesViewModel(osa, fsa, CreateTableInformationViewModelMockObject);
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
            var osa = Mock.Of<IOperatingSystemAccess>();
            var fsa = Mock.Of<IFileSystemAccess>();

            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IPickTablesViewModel vm = new PickTablesViewModel(osa, fsa, CreateTableInformationViewModelMockObject);
            var c = new TableInformationModel[0];
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
            var osa = Mock.Of<IOperatingSystemAccess>();
            var fsa = Mock.Of<IFileSystemAccess>();

            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IPickTablesViewModel vm = new PickTablesViewModel(osa, fsa, CreateTableInformationViewModelMockObject);
            var c = new[]
            {
                new TableInformationModel("unit.test", true)
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
                Assert.IsTrue(vm.Tables.Single(m => m.Model.Name == selectedTableModel.Name).IsSelected);
                Assert.IsTrue(vm.Tables.Where(m => m.Model.Name != selectedTableModel.Name).All(m => m.IsSelected == false));
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
            var osa = Mock.Of<IOperatingSystemAccess>();
            var fsa = Mock.Of<IFileSystemAccess>();

            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IPickTablesViewModel vm = new PickTablesViewModel(osa, fsa, CreateTableInformationViewModelMockObject);
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
            var osa = Mock.Of<IOperatingSystemAccess>();
            var fsa = Mock.Of<IFileSystemAccess>();

            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IPickTablesViewModel vm = new PickTablesViewModel(osa, fsa, CreateTableInformationViewModelMockObject);
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
            var osa = Mock.Of<IOperatingSystemAccess>();
            var fsa = Mock.Of<IFileSystemAccess>();

            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IPickTablesViewModel vm = new PickTablesViewModel(osa, fsa, CreateTableInformationViewModelMockObject);
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
            var osa = Mock.Of<IOperatingSystemAccess>();
            var fsa = Mock.Of<IFileSystemAccess>();

            // Test relies on PropertyChanged, so we rely on the real view model here
            ITableInformationViewModel CreateTableInformationViewModel() => new TableInformationViewModel();

            IPickTablesViewModel vm = new PickTablesViewModel(osa, fsa, CreateTableInformationViewModel);
            var tt = GetTestViewModels();
            vm.AddTables(tt.Select(m => m.Model));

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
            var osa = Mock.Of<IOperatingSystemAccess>();
            var fsa = Mock.Of<IFileSystemAccess>();

            // Test relies on PropertyChanged, so we rely on the real view model here
            ITableInformationViewModel CreateTableInformationViewModel() => new TableInformationViewModel();

            IPickTablesViewModel vm = new PickTablesViewModel(osa, fsa, CreateTableInformationViewModel);
            var tt = GetTestViewModels();
            vm.AddTables(tt.Select(m => m.Model));

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
            var osa = Mock.Of<IOperatingSystemAccess>();
            var fsa = Mock.Of<IFileSystemAccess>();

            // Test relies on PropertyChanged, so we rely on the real view model here
            ITableInformationViewModel CreateTableInformationViewModel() => new TableInformationViewModel();

            IPickTablesViewModel vm = new PickTablesViewModel(osa, fsa, CreateTableInformationViewModel);
            var tt = GetTestViewModels();
            vm.AddTables(tt.Select(m => m.Model));

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
            var osa = Mock.Of<IOperatingSystemAccess>();
            var fsa = Mock.Of<IFileSystemAccess>();

            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IPickTablesViewModel vm = new PickTablesViewModel(osa, fsa, CreateTableInformationViewModelMockObject);
            var tt = GetTestViewModels();
            foreach (var table in tt)
            {
                vm.Tables.Add(table);
            }
            vm.SearchText = "foo";

            // Act
            vm.TableSelectionThreeState = true;

            // Assert
            Assert.IsTrue(vm.Tables.Where(m => m.Model.HasPrimaryKey).All(m => m.IsSelected));
            Assert.IsTrue(vm.Tables.Where(m => !m.Model.HasPrimaryKey).All(m => !m.IsSelected));
            Assert.AreEqual(string.Empty, vm.SearchText);
        }

        [Test]
        public void TableSelectionThreeState_NoneSelected()
        {
            // Arrange
            var osa = Mock.Of<IOperatingSystemAccess>();
            var fsa = Mock.Of<IFileSystemAccess>();

            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IPickTablesViewModel vm = new PickTablesViewModel(osa, fsa, CreateTableInformationViewModelMockObject);
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
            var osa = Mock.Of<IOperatingSystemAccess>();
            var fsa = Mock.Of<IFileSystemAccess>();

            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IPickTablesViewModel vm = new PickTablesViewModel(osa, fsa, CreateTableInformationViewModelMockObject);
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
            var osa = Mock.Of<IOperatingSystemAccess>();
            var fsa = Mock.Of<IFileSystemAccess>();

            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IPickTablesViewModel vm = new PickTablesViewModel(osa, fsa, CreateTableInformationViewModelMockObject);
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
            var osa = Mock.Of<IOperatingSystemAccess>();
            var fsa = Mock.Of<IFileSystemAccess>();

            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IPickTablesViewModel vm = new PickTablesViewModel(osa, fsa, CreateTableInformationViewModelMockObject);
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
                return postFilter.Length < tt.Length && postFilter.All(m => m.Model.Name.Contains("dbo"));
            }, Is.True.After(1500, 200));
        }

        [Test]
        public void GetResults_NoTables()
        {
            // Arrange
            var osa = Mock.Of<IOperatingSystemAccess>();
            var fsa = Mock.Of<IFileSystemAccess>();

            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IPickTablesViewModel vm = new PickTablesViewModel(osa, fsa, CreateTableInformationViewModelMockObject);

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
            var osa = Mock.Of<IOperatingSystemAccess>();
            var fsa = Mock.Of<IFileSystemAccess>();

            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IPickTablesViewModel vm = new PickTablesViewModel(osa, fsa, CreateTableInformationViewModelMockObject);
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
            var osa = Mock.Of<IOperatingSystemAccess>();
            var fsa = Mock.Of<IFileSystemAccess>();

            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IPickTablesViewModel vm = new PickTablesViewModel(osa, fsa, CreateTableInformationViewModelMockObject);
            var tt = GetTestViewModels();
            foreach (var table in tt)
            {
                table.IsSelected = true;
                vm.Tables.Add(table);
            }

            // Act
            var result = vm.GetResult();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Length);
            Assert.AreSame(tt[0].Model, result[0]);
            Assert.AreSame(tt[2].Model, result[1]);
            Assert.AreSame(tt[3].Model, result[2]);
            Assert.AreSame(tt[4].Model, result[3]);
        }

        [Test]
        public void GetResults_WithTables_PartialSelection()
        {
            // Arrange
            var osa = Mock.Of<IOperatingSystemAccess>();
            var fsa = Mock.Of<IFileSystemAccess>();

            ITableInformationViewModel CreateTableInformationViewModelMockObject()
            {
                var mock = new Mock<ITableInformationViewModel>();
                mock.SetupAllProperties();
                return mock.Object;
            }

            IPickTablesViewModel vm = new PickTablesViewModel(osa, fsa, CreateTableInformationViewModelMockObject);
            var tt = GetTestViewModels();
            foreach (var table in tt)
            {
                vm.Tables.Add(table);
            }

            // Act
            var result = vm.GetResult();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Length);
            Assert.AreSame(tt[2].Model, result[0]);
            Assert.AreSame(tt[3].Model, result[1]);
            Assert.AreSame(tt[4].Model, result[2]);
        }

        private static ITableInformationViewModel[] GetTestViewModels()
        {
            var r = new ITableInformationViewModel[6];

            r[0] = new TableInformationViewModel
            {
                IsSelected = false,
                Model = new TableInformationModel("[dbo].[Atlas]", true)
            };
            r[1] = new TableInformationViewModel
            {
                IsSelected = true,
                Model = new TableInformationModel("[__].[RefactorLog]", false)
            };
            r[2] = new TableInformationViewModel
            {
                IsSelected = true,
                Model = new TableInformationModel("[dbo].[__RefactorLog]", true)
            };
            r[3] = new TableInformationViewModel
            {
                IsSelected = true,
                Model = new TableInformationModel("[dbo].[sysdiagrams]", true)
            };
            r[4] = new TableInformationViewModel
            {
                IsSelected = true,
                Model = new TableInformationModel("unit.test", true)
            };
            r[5] = new TableInformationViewModel
            {
                IsSelected = true,
                Model = new TableInformationModel("unit.foo", false)
            };

            return r;
        }
    }
}