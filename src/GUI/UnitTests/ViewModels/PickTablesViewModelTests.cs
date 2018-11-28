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
            Func<ITableInformationViewModel> t = Mock.Of<ITableInformationViewModel>;

            // Act
            var vm = new PickTablesViewModel(osa, fsa, t);

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
            Func<ITableInformationViewModel> t = Mock.Of<ITableInformationViewModel>;

            // Act
            var vm = new PickTablesViewModel(osa, fsa, t);

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
            Func<ITableInformationViewModel> t = Mock.Of<ITableInformationViewModel>;

            // Act
            var vm = new PickTablesViewModel(osa, fsa, t);

            // Assert
            Assert.IsNull(vm.TableSelectionThreeState);
        }

        [Test]
        public void Constructors_NoSearchText()
        {
            // Arrange
            var osa = Mock.Of<IOperatingSystemAccess>();
            var fsa = Mock.Of<IFileSystemAccess>();
            Func<ITableInformationViewModel> t = Mock.Of<ITableInformationViewModel>;

            // Act
            var vm = new PickTablesViewModel(osa, fsa, t);

            // Assert
            Assert.AreEqual(string.Empty, vm.SearchText);
        }

        [Test]
        public void LoadedCommand_CanExecute()
        {
            // Arrange
            var osa = Mock.Of<IOperatingSystemAccess>();
            var fsa = Mock.Of<IFileSystemAccess>();
            Func<ITableInformationViewModel> t = Mock.Of<ITableInformationViewModel>;
            var vm = new PickTablesViewModel(osa, fsa, t);

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
            Func<ITableInformationViewModel> t = Mock.Of<ITableInformationViewModel>;
            var vm = new PickTablesViewModel(osa, fsa, t);
            var tt = GetTestViewModels();
            foreach (var tvm in tt)
                vm.Tables.Add(tvm);

            // Act
            vm.LoadedCommand.Execute(null);

            // Assert
            Assert.IsTrue(tt[0].IsSelected);
            Assert.IsFalse(tt[1].IsSelected);
            Assert.IsFalse(tt[2].IsSelected);
            Assert.IsFalse(tt[3].IsSelected);
            Assert.IsTrue(tt[4].IsSelected);
        }

        [Test]
        public void SaveSelectionCommand_CanExecute_NoTables()
        {
            // Arrange
            var osa = Mock.Of<IOperatingSystemAccess>();
            var fsa = Mock.Of<IFileSystemAccess>();
            Func<ITableInformationViewModel> t = Mock.Of<ITableInformationViewModel>;
            var vm = new PickTablesViewModel(osa, fsa, t);

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
            Func<ITableInformationViewModel> t = Mock.Of<ITableInformationViewModel>;
            var vm = new PickTablesViewModel(osa, fsa, t);
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
            Func<ITableInformationViewModel> t = Mock.Of<ITableInformationViewModel>;
            var vm = new PickTablesViewModel(osa, fsa, t);
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
            Func<ITableInformationViewModel> t = Mock.Of<ITableInformationViewModel>;
            var vm = new PickTablesViewModel(osa, fsa, t);
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
            Func<ITableInformationViewModel> t = Mock.Of<ITableInformationViewModel>;
            var vm = new PickTablesViewModel(osaMock.Object, fsaMock.Object, t);
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
            Func<ITableInformationViewModel> t = Mock.Of<ITableInformationViewModel>;
            var vm = new PickTablesViewModel(osaMock.Object, fsaMock.Object, t);
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
            Func<ITableInformationViewModel> t = Mock.Of<ITableInformationViewModel>;
            var vm = new PickTablesViewModel(osa, fsa, t);

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
            Func<ITableInformationViewModel> t = Mock.Of<ITableInformationViewModel>;
            var vm = new PickTablesViewModel(osa, fsa, t);
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
            Func<ITableInformationViewModel> t = Mock.Of<ITableInformationViewModel>;
            var vm = new PickTablesViewModel(osaMock.Object, fsaMock.Object, t);
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
                "foo"
            };
            var osaMock = new Mock<IOperatingSystemAccess>();
            osaMock.Setup(m => m.RequestLoadFileName(It.IsNotNull<string>(), It.Is<string>(s => s.Contains("*.txt") && s.Contains("*.*")), false, true)).Returns(filePath);
            var fsaMock = new Mock<IFileSystemAccess>();
            fsaMock.Setup(m => m.ReadAllLines(filePath)).Returns(tl);
            Func<ITableInformationViewModel> t = Mock.Of<ITableInformationViewModel>;
            var vm = new PickTablesViewModel(osaMock.Object, fsaMock.Object, t);
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
            Assert.IsTrue(tt[1].IsSelected);
            Assert.IsFalse(tt[2].IsSelected);
            Assert.IsFalse(tt[3].IsSelected);
            Assert.IsTrue(tt[4].IsSelected);
            Assert.AreEqual(string.Empty, vm.SearchText);
        }

        [Test]
        public void OkCommand_CanExecute_NoTables()
        {
            // Arrange
            var osa = Mock.Of<IOperatingSystemAccess>();
            var fsa = Mock.Of<IFileSystemAccess>();
            Func<ITableInformationViewModel> t = Mock.Of<ITableInformationViewModel>;
            var vm = new PickTablesViewModel(osa, fsa, t);

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
            Func<ITableInformationViewModel> t = Mock.Of<ITableInformationViewModel>;
            var vm = new PickTablesViewModel(osa, fsa, t);
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
            Func<ITableInformationViewModel> t = Mock.Of<ITableInformationViewModel>;
            var vm = new PickTablesViewModel(osa, fsa, t);
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
            Func<ITableInformationViewModel> t = Mock.Of<ITableInformationViewModel>;
            var vm = new PickTablesViewModel(osa, fsa, t);
            vm.CloseRequested += (sender, args) =>
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
            Func<ITableInformationViewModel> t = Mock.Of<ITableInformationViewModel>;
            var vm = new PickTablesViewModel(osa, fsa, t);

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
            Func<ITableInformationViewModel> t = Mock.Of<ITableInformationViewModel>;
            var vm = new PickTablesViewModel(osa, fsa, t);
            vm.CloseRequested += (sender, args) =>
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

        private static ITableInformationViewModel[] GetTestViewModels()
        {
            var r = new ITableInformationViewModel[5];

            r[0] = new TableInformationViewModel
            {
                IsSelected = false,
                Model = new TableInformationModel("dbo", "Atlas", true)
            };
            r[1] = new TableInformationViewModel
            {
                IsSelected = true,
                Model = new TableInformationModel("__", "RefactorLog", false)
            };
            r[2] = new TableInformationViewModel
            {
                IsSelected = true,
                Model = new TableInformationModel("dbo", "__RefactorLog", true)
            };
            r[3] = new TableInformationViewModel
            {
                IsSelected = true,
                Model = new TableInformationModel("dbo", "sysdiagrams", true)
            };
            r[4] = new TableInformationViewModel
            {
                IsSelected = true,
                Model = new TableInformationModel("unit", "test", true)
            };

            return r;
        }
    }
}