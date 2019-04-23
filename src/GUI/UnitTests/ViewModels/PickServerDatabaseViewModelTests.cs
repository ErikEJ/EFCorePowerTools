using NUnit.Framework;

namespace UnitTests.ViewModels
{
    using System;
    using EFCorePowerTools.Contracts.ViewModels;
    using EFCorePowerTools.Shared.DAL;
    using EFCorePowerTools.Shared.Models;
    using EFCorePowerTools.ViewModels;
    using Moq;

    [TestFixture]
    public class PickServerDatabaseViewModelTests
    {
        [Test]
        public void Constructor_ArgumentNullException()
        {
            // Arrange
            IVisualStudioAccess vsa = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new PickServerDatabaseViewModel(vsa));
        }

        [Test]
        public void Constructor_CommandsInitialized()
        {
            // Arrange
            var vsa = Mock.Of<IVisualStudioAccess>();

            // Act
            var vm = new PickServerDatabaseViewModel(vsa);

            // Assert
            Assert.IsNotNull(vm.LoadedCommand);
            Assert.IsNotNull(vm.AddDatabaseConnectionCommand);
            Assert.IsNotNull(vm.OkCommand);
            Assert.IsNotNull(vm.CancelCommand);
        }

        [Test]
        public void Constructor_CollectionsInitialized()
        {
            // Arrange
            var vsa = Mock.Of<IVisualStudioAccess>();

            // Act
            var vm = new PickServerDatabaseViewModel(vsa);

            // Assert
            Assert.IsNotNull(vm.DatabaseConnections);
            Assert.IsNotNull(vm.DatabaseDefinitions);
        }

        [Test]
        public void Constructor_NoSelection()
        {
            // Arrange
            var vsa = Mock.Of<IVisualStudioAccess>();

            // Act
            var vm = new PickServerDatabaseViewModel(vsa);

            // Assert
            Assert.IsNull(vm.SelectedDatabaseConnection);
            Assert.IsNull(vm.SelectedDatabaseDefinition);
        }

        [Test]
        public void LoadedCommand_CanExecute()
        {
            // Arrange
            var vsa = Mock.Of<IVisualStudioAccess>();
            var vm = new PickServerDatabaseViewModel(vsa);

            // Act
            var canExecute = vm.LoadedCommand.CanExecute(null);

            // Assert
            Assert.IsTrue(canExecute);
        }

        [Test]
        public void LoadedCommand_Executed_OnlyDatabaseConnections()
        {
            // Arrange
            var vsa = Mock.Of<IVisualStudioAccess>();
            var vm = new PickServerDatabaseViewModel(vsa);
            var dbConnection1 = new DatabaseConnectionModel();
            var dbConnection2 = new DatabaseConnectionModel();
            vm.DatabaseConnections.Add(dbConnection1);
            vm.DatabaseConnections.Add(dbConnection2);

            // Act
            vm.LoadedCommand.Execute(null);

            // Assert
            Assert.AreSame(dbConnection1, vm.SelectedDatabaseConnection);
            Assert.IsNull(vm.SelectedDatabaseDefinition);
        }

        [Test]
        public void LoadedCommand_Executed_DatabaseConnectionsAndDefinitions()
        {
            // Arrange
            var vsa = Mock.Of<IVisualStudioAccess>();
            var vm = new PickServerDatabaseViewModel(vsa);
            var dbConnection1 = new DatabaseConnectionModel();
            var dbConnection2 = new DatabaseConnectionModel();
            var dbDefinition1 = new DatabaseDefinitionModel();
            var dbDefinition2 = new DatabaseDefinitionModel();
            vm.DatabaseConnections.Add(dbConnection1);
            vm.DatabaseConnections.Add(dbConnection2);
            vm.DatabaseDefinitions.Add(dbDefinition1);
            vm.DatabaseDefinitions.Add(dbDefinition2);

            // Act
            vm.LoadedCommand.Execute(null);

            // Assert
            Assert.AreSame(dbConnection1, vm.SelectedDatabaseConnection);
            Assert.IsNull(vm.SelectedDatabaseDefinition);
        }

        [Test]
        public void LoadedCommand_Executed_OnlyDatabaseDefinitions_SimpleSortOrder()
        {
            // Arrange
            var vsa = Mock.Of<IVisualStudioAccess>();
            var vm = new PickServerDatabaseViewModel(vsa);
            var dbDefinition1 = new DatabaseDefinitionModel
            {
                FilePath = "ExampleDatabaseB.sqlproj"
            };
            var dbDefinition2 = new DatabaseDefinitionModel
            {
                FilePath = "ExampleDatabaseA.sqlproj"
            };
            vm.DatabaseDefinitions.Add(dbDefinition1);
            vm.DatabaseDefinitions.Add(dbDefinition2);

            // Act
            vm.LoadedCommand.Execute(null);

            // Assert
            Assert.IsNull(vm.SelectedDatabaseConnection);
            Assert.AreSame(dbDefinition2, vm.SelectedDatabaseDefinition);
        }

        [Test]
        public void LoadedCommand_Executed_OnlyDatabaseDefinitions_ExtendedSortOrder()
        {
            // Arrange
            var vsa = Mock.Of<IVisualStudioAccess>();
            var vm = new PickServerDatabaseViewModel(vsa);
            var dbDefinition1 = new DatabaseDefinitionModel
            {
                FilePath = "TestExampleDatabaseA.sqlproj"
            };
            var dbDefinition2 = new DatabaseDefinitionModel
            {
                FilePath = "ExampleDatabaseB.sqlproj"
            };
            var dbDefinition3 = new DatabaseDefinitionModel
            {
                FilePath = "ExampleDatabaseCtest.sqlproj"
            };
            vm.DatabaseDefinitions.Add(dbDefinition1);
            vm.DatabaseDefinitions.Add(dbDefinition2);
            vm.DatabaseDefinitions.Add(dbDefinition3);

            // Act
            vm.LoadedCommand.Execute(null);

            // Assert
            Assert.IsNull(vm.SelectedDatabaseConnection);
            Assert.AreSame(dbDefinition2, vm.SelectedDatabaseDefinition);
        }

        [Test]
        public void LoadedCommand_Executed_OnlyDatabaseDefinitions_FilePathNull()
        {
            // Arrange
            var vsa = Mock.Of<IVisualStudioAccess>();
            var vm = new PickServerDatabaseViewModel(vsa);
            var dbDefinition = new DatabaseDefinitionModel
            {
                FilePath = null
            };
            vm.DatabaseDefinitions.Add(dbDefinition);

            // Act
            vm.LoadedCommand.Execute(null);

            // Assert
            Assert.IsNull(vm.SelectedDatabaseConnection);
            Assert.IsNull(vm.SelectedDatabaseDefinition);
        }

        [Test]
        public void LoadedCommand_Executed_OnlyDatabaseDefinitions_WithOtherFiles()
        {
            // Arrange
            var vsa = Mock.Of<IVisualStudioAccess>();
            var vm = new PickServerDatabaseViewModel(vsa);
            var dbDefinition1 = new DatabaseDefinitionModel
            {
                FilePath = "ExampleDatabaseA.dacpac"
            };
            var dbDefinition2 = new DatabaseDefinitionModel
            {
                FilePath = "ExampleDatabaseB.sqlproj"
            };
            var dbDefinition3 = new DatabaseDefinitionModel
            {
                FilePath = "ExampleDatabaseC.txt"
            };
            vm.DatabaseDefinitions.Add(dbDefinition1);
            vm.DatabaseDefinitions.Add(dbDefinition2);
            vm.DatabaseDefinitions.Add(dbDefinition3);

            // Act
            vm.LoadedCommand.Execute(null);

            // Assert
            Assert.IsNull(vm.SelectedDatabaseConnection);
            Assert.AreSame(dbDefinition2, vm.SelectedDatabaseDefinition);
        }

        [Test]
        public void AddDatabaseConnectionCommand_CanExecute()
        {
            // Arrange
            var vsa = Mock.Of<IVisualStudioAccess>();
            var vm = new PickServerDatabaseViewModel(vsa);

            // Act
            var canExecute = vm.AddDatabaseConnectionCommand.CanExecute(null);

            // Assert
            Assert.IsTrue(canExecute);
        }

        [Test]
        public void AddDatabaseConnectionCommand_Executed_ExceptionDuringPrompt()
        {
            // Arrange
            var vsaMock = new Mock<IVisualStudioAccess>();
            vsaMock.Setup(m => m.PromptForNewDatabaseConnection()).Throws<InvalidOperationException>();
            var vm = new PickServerDatabaseViewModel(vsaMock.Object);

            // Act
            vm.AddDatabaseConnectionCommand.Execute(null);

            // Assert
            CollectionAssert.IsEmpty(vm.DatabaseConnections);
            Assert.IsNull(vm.SelectedDatabaseConnection);
            vsaMock.Verify(m => m.PromptForNewDatabaseConnection(), Times.Once);
            vsaMock.Verify(m => m.ShowMessage(It.IsNotNull<string>()), Times.Once);
        }

        [Test]
        public void AddDatabaseConnectionCommand_Executed_NoConnectionReturned()
        {
            // Arrange
            var vsaMock = new Mock<IVisualStudioAccess>();
            vsaMock.Setup(m => m.PromptForNewDatabaseConnection()).Returns<DatabaseConnectionModel>(null);
            var vm = new PickServerDatabaseViewModel(vsaMock.Object);

            // Act
            vm.AddDatabaseConnectionCommand.Execute(null);

            // Assert
            CollectionAssert.IsEmpty(vm.DatabaseConnections);
            Assert.IsNull(vm.SelectedDatabaseConnection);
            vsaMock.Verify(m => m.PromptForNewDatabaseConnection(), Times.Once);
            vsaMock.Verify(m => m.ShowMessage(It.IsNotNull<string>()), Times.Never);
        }

        [Test]
        public void AddDatabaseConnectionCommand_Executed_NewConnectionReturned()
        {
            // Arrange
            var dbConnection = new DatabaseConnectionModel();
            var vsaMock = new Mock<IVisualStudioAccess>();
            vsaMock.Setup(m => m.PromptForNewDatabaseConnection()).Returns(dbConnection);
            var vm = new PickServerDatabaseViewModel(vsaMock.Object);

            // Act
            vm.AddDatabaseConnectionCommand.Execute(null);

            // Assert
            CollectionAssert.Contains(vm.DatabaseConnections, dbConnection);
            Assert.AreSame(dbConnection, vm.SelectedDatabaseConnection);
            vsaMock.Verify(m => m.PromptForNewDatabaseConnection(), Times.Once);
            vsaMock.Verify(m => m.ShowMessage(It.IsNotNull<string>()), Times.Never);
        }

        [Test]
        public void OkCommand_CanExecute_NothingSelected()
        {
            // Arrange
            var vsa = Mock.Of<IVisualStudioAccess>();
            var vm = new PickServerDatabaseViewModel(vsa);

            // Act
            var canExecute = vm.OkCommand.CanExecute(null);

            // Assert
            Assert.IsFalse(canExecute);
        }

        [Test]
        public void OkCommand_CanExecute_ConnectionSelected()
        {
            // Arrange
            var dbConnection = new DatabaseConnectionModel();
            var vsa = Mock.Of<IVisualStudioAccess>();
            var vm = new PickServerDatabaseViewModel(vsa)
            {
                SelectedDatabaseConnection = dbConnection
            };

            // Act
            var canExecute = vm.OkCommand.CanExecute(null);

            // Assert
            Assert.IsTrue(canExecute);
        }

        [Test]
        public void OkCommand_CanExecute_DefinitionSelected()
        {
            // Arrange
            var dbDefinition = new DatabaseDefinitionModel();
            var vsa = Mock.Of<IVisualStudioAccess>();
            var vm = new PickServerDatabaseViewModel(vsa)
            {
                SelectedDatabaseDefinition = dbDefinition
            };

            // Act
            var canExecute = vm.OkCommand.CanExecute(null);

            // Assert
            Assert.IsTrue(canExecute);
        }

        [Test]
        public void OkCommand_Executed_CloseRequestedToView_DatabaseConnection()
        {
            // Arrange
            var dbConnection = new DatabaseConnectionModel();
            var closeRequested = false;
            bool? dialogResult = null;
            var vsa = Mock.Of<IVisualStudioAccess>();
            var vm = new PickServerDatabaseViewModel(vsa)
            {
                SelectedDatabaseConnection = dbConnection
            };
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
            Assert.AreSame(dbConnection, vm.SelectedDatabaseConnection);
            Assert.IsNull(vm.SelectedDatabaseDefinition);
        }

        [Test]
        public void OkCommand_Executed_CloseRequestedToView_DatabaseDefinition()
        {
            // Arrange
            var dbDefinition = new DatabaseDefinitionModel();
            var closeRequested = false;
            bool? dialogResult = null;
            var vsa = Mock.Of<IVisualStudioAccess>();
            var vm = new PickServerDatabaseViewModel(vsa)
            {
                SelectedDatabaseDefinition = dbDefinition
            };
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
            Assert.AreSame(dbDefinition, vm.SelectedDatabaseDefinition);
            Assert.IsNull(vm.SelectedDatabaseConnection);
        }

        [Test]
        public void CancelCommand_CanExecute()
        {
            // Arrange
            var vsa = Mock.Of<IVisualStudioAccess>();
            var vm = new PickServerDatabaseViewModel(vsa);

            // Act
            var canExecute = vm.CancelCommand.CanExecute(null);

            // Assert
            Assert.IsTrue(canExecute);
        }

        [Test]
        public void CancelCommand_Executed_DatabaseConnection()
        {
            // Arrange
            var dbConnection = new DatabaseConnectionModel();
            var closeRequested = false;
            bool? dialogResult = null;
            var vsa = Mock.Of<IVisualStudioAccess>();
            var vm = new PickServerDatabaseViewModel(vsa)
            {
                SelectedDatabaseConnection = dbConnection
            };
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
            Assert.IsNull(vm.SelectedDatabaseConnection);
            Assert.IsNull(vm.SelectedDatabaseDefinition);
        }

        [Test]
        public void CancelCommand_Executed_DatabaseDefinition()
        {
            // Arrange
            var dbDefinition = new DatabaseDefinitionModel();
            var closeRequested = false;
            bool? dialogResult = null;
            var vsa = Mock.Of<IVisualStudioAccess>();
            var vm = new PickServerDatabaseViewModel(vsa)
            {
                SelectedDatabaseDefinition = dbDefinition
            };
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
            Assert.IsNull(vm.SelectedDatabaseConnection);
            Assert.IsNull(vm.SelectedDatabaseDefinition);
        }

        [Test]
        public void Selection_OnlyConnectionOrDefinitionSelected_DatabaseConnection()
        {
            // Arrange
            var dbConnection = new DatabaseConnectionModel();
            var dbDefinition = new DatabaseDefinitionModel();
            var vsa = Mock.Of<IVisualStudioAccess>();
            // ReSharper disable once UseObjectOrCollectionInitializer
            var vm = new PickServerDatabaseViewModel(vsa)
            {
                SelectedDatabaseDefinition = dbDefinition
            };

            // Act
            vm.SelectedDatabaseConnection = dbConnection;

            // Assert
            Assert.IsNull(vm.SelectedDatabaseDefinition);
            Assert.AreSame(dbConnection, vm.SelectedDatabaseConnection);
        }

        [Test]
        public void Selection_OnlyConnectionOrDefinitionSelected_DatabaseDefinition()
        {
            // Arrange
            var dbConnection = new DatabaseConnectionModel();
            var dbDefinition = new DatabaseDefinitionModel();
            var vsa = Mock.Of<IVisualStudioAccess>();
            // ReSharper disable once UseObjectOrCollectionInitializer
            var vm = new PickServerDatabaseViewModel(vsa)
            {
                SelectedDatabaseConnection = dbConnection
            };

            // Act
            vm.SelectedDatabaseDefinition = dbDefinition;

            // Assert
            Assert.IsNull(vm.SelectedDatabaseConnection);
            Assert.AreSame(dbDefinition, vm.SelectedDatabaseDefinition);
        }

        [Test]
        public void PropertyChangedOnDatabaseDefinitionsChanged()
        {
            // Arrange
            string propertyChangedName = null;
            var dbDefinition = new DatabaseDefinitionModel();
            var vsa = Mock.Of<IVisualStudioAccess>();
            var vm = new PickServerDatabaseViewModel(vsa);
            vm.PropertyChanged += (sender, args) => propertyChangedName = args.PropertyName;

            // Act
            vm.DatabaseDefinitions.Add(dbDefinition);

            // Assert
            Assert.IsNotNull(propertyChangedName);
            Assert.AreEqual(nameof(IPickServerDatabaseViewModel.DatabaseDefinitions), propertyChangedName);
        }
    }
}