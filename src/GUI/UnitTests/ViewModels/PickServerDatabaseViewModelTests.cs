﻿using System;
using EFCorePowerTools.Common.DAL;
using EFCorePowerTools.Common.Models;
using EFCorePowerTools.Contracts.ViewModels;
using EFCorePowerTools.Contracts.Views;
using EFCorePowerTools.DAL;
using EFCorePowerTools.ViewModels;
using Moq;
using NUnit.Framework;

namespace UnitTests.ViewModels
{
    [TestFixture]
    public class PickServerDatabaseViewModelTests
    {
        [Test]
        public void Constructor_ArgumentNullException()
        {
            // Arrange
            IVisualStudioAccess vsa = null;
            var psdFactory = Mock.Of<Func<IPickSchemasDialog>>();
            var connFactory = Mock.Of<Func<IPickConnectionDialog>>();
            var creds = Mock.Of<ICredentialStore>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new PickServerDatabaseViewModel(vsa, creds, psdFactory, connFactory));
        }

        [Test]
        public void Constructor_Argument2NullException()
        {
            // Arrange
            var vsa = Mock.Of<IVisualStudioAccess>();
            var psdFactory = Mock.Of<Func<IPickSchemasDialog>>();
            var connFactory = Mock.Of<Func<IPickConnectionDialog>>();
            ICredentialStore creds = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new PickServerDatabaseViewModel(vsa, creds, psdFactory, connFactory));
        }

        [Test]
        public void Constructor_Argument3NullException()
        {
            // Arrange
            var vsa = Mock.Of<IVisualStudioAccess>();
            Func<IPickSchemasDialog> psdFactory = null;
            var connFactory = Mock.Of<Func<IPickConnectionDialog>>();
            var creds = Mock.Of<ICredentialStore>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new PickServerDatabaseViewModel(vsa, creds, psdFactory, connFactory));
        }

        [Test]
        public void Constructor_Argument4NullException()
        {
            // Arrange
            var vsa = Mock.Of<IVisualStudioAccess>();
            var psdFactory = Mock.Of<Func<IPickSchemasDialog>>();
            var creds = Mock.Of<ICredentialStore>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new PickServerDatabaseViewModel(vsa, creds, psdFactory, null));
        }

        [Test]
        public void Constructor_CommandsInitialized()
        {
            // Arrange
            var vsa = Mock.Of<IVisualStudioAccess>();
            var psdFactory = Mock.Of<Func<IPickSchemasDialog>>();
            var connFactory = Mock.Of<Func<IPickConnectionDialog>>();
            var creds = Mock.Of<ICredentialStore>();

            // Act
            var vm = new PickServerDatabaseViewModel(vsa, creds, psdFactory, connFactory);

            // Assert
            Assert.IsNotNull(vm.LoadedCommand);
            Assert.IsNotNull(vm.AddDatabaseConnectionCommand);
            Assert.IsNotNull(vm.AddDatabaseDefinitionCommand);
            Assert.IsNotNull(vm.OkCommand);
            Assert.IsNotNull(vm.CancelCommand);
            Assert.IsNotNull(vm.FilterSchemasCommand);
        }

        [Test]
        public void Constructor_CollectionsInitialized()
        {
            // Arrange
            var vsa = Mock.Of<IVisualStudioAccess>();
            var psdFactory = Mock.Of<Func<IPickSchemasDialog>>();
            var connFactory = Mock.Of<Func<IPickConnectionDialog>>();
            var creds = Mock.Of<ICredentialStore>();

            // Act
            var vm = new PickServerDatabaseViewModel(vsa, creds, psdFactory, connFactory);

            // Assert
            Assert.IsNotNull(vm.DatabaseConnections);
        }

        [Test]
        public void Constructor_NoSelection()
        {
            // Arrange
            var vsa = Mock.Of<IVisualStudioAccess>();
            var psdFactory = Mock.Of<Func<IPickSchemasDialog>>();
            var connFactory = Mock.Of<Func<IPickConnectionDialog>>();
            var creds = Mock.Of<ICredentialStore>();

            // Act
            var vm = new PickServerDatabaseViewModel(vsa, creds, psdFactory, connFactory);

            // Assert
            Assert.IsNull(vm.SelectedDatabaseConnection);
        }

        [Test]
        public void LoadedCommand_CanExecute()
        {
            // Arrange
            var vsa = Mock.Of<IVisualStudioAccess>();
            var psdFactory = Mock.Of<Func<IPickSchemasDialog>>();
            var connFactory = Mock.Of<Func<IPickConnectionDialog>>();
            var creds = Mock.Of<ICredentialStore>();
            var vm = new PickServerDatabaseViewModel(vsa, creds, psdFactory, connFactory);

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
            var psdFactory = Mock.Of<Func<IPickSchemasDialog>>();
            var connFactory = Mock.Of<Func<IPickConnectionDialog>>();
            var creds = Mock.Of<ICredentialStore>();
            var vm = new PickServerDatabaseViewModel(vsa, creds, psdFactory, connFactory);
            var dbConnection1 = new DatabaseConnectionModel();
            var dbConnection2 = new DatabaseConnectionModel();
            vm.DatabaseConnections.Add(dbConnection1);
            vm.DatabaseConnections.Add(dbConnection2);

            // Act
            vm.LoadedCommand.Execute(null);

            // Assert
            Assert.AreSame(dbConnection1, vm.SelectedDatabaseConnection);
        }

        [Test]
        public void AddDatabaseConnectionCommand_CanExecute()
        {
            // Arrange
            var vsa = Mock.Of<IVisualStudioAccess>();
            var psdFactory = Mock.Of<Func<IPickSchemasDialog>>();
            var connFactory = Mock.Of<Func<IPickConnectionDialog>>();
            var creds = Mock.Of<ICredentialStore>();
            var vm = new PickServerDatabaseViewModel(vsa, creds, psdFactory, connFactory);

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
            var psdFactory = Mock.Of<Func<IPickSchemasDialog>>();
            var connFactory = Mock.Of<Func<IPickConnectionDialog>>();
            var creds = Mock.Of<ICredentialStore>();
            vsaMock.Setup(m => m.PromptForNewDatabaseConnection()).Throws<InvalidOperationException>();
            var vm = new PickServerDatabaseViewModel(vsaMock.Object, creds, psdFactory, connFactory);

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
            var psdFactory = Mock.Of<Func<IPickSchemasDialog>>();
            var connFactory = Mock.Of<Func<IPickConnectionDialog>>();
            var creds = Mock.Of<ICredentialStore>();
            vsaMock.Setup(m => m.PromptForNewDatabaseConnection()).Returns<DatabaseConnectionModel>(null);
            var vm = new PickServerDatabaseViewModel(vsaMock.Object, creds, psdFactory, connFactory);

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
            var psdFactory = Mock.Of<Func<IPickSchemasDialog>>();
            var connFactory = Mock.Of<Func<IPickConnectionDialog>>();
            var creds = Mock.Of<ICredentialStore>();
            vsaMock.Setup(m => m.PromptForNewDatabaseConnection()).Returns(dbConnection);
            var vm = new PickServerDatabaseViewModel(vsaMock.Object, creds, psdFactory, connFactory);

            // Act
            vm.AddDatabaseConnectionCommand.Execute(null);

            // Assert
            CollectionAssert.Contains(vm.DatabaseConnections, dbConnection);
            Assert.AreSame(dbConnection, vm.SelectedDatabaseConnection);
            vsaMock.Verify(m => m.PromptForNewDatabaseConnection(), Times.Once);
            vsaMock.Verify(m => m.ShowMessage(It.IsNotNull<string>()), Times.Never);
        }

        [Test]
        public void AddDatabaseDefinitionCommand_CanExecute()
        {
            // Arrange
            var vsa = Mock.Of<IVisualStudioAccess>();
            var psdFactory = Mock.Of<Func<IPickSchemasDialog>>();
            var connFactory = Mock.Of<Func<IPickConnectionDialog>>();
            var creds = Mock.Of<ICredentialStore>();
            var vm = new PickServerDatabaseViewModel(vsa, creds, psdFactory, connFactory);

            // Act
            var canExecute = vm.AddDatabaseDefinitionCommand.CanExecute(null);

            // Assert
            Assert.IsTrue(canExecute);
        }

        [Test]
        public void OkCommand_CanExecute_NothingSelected()
        {
            // Arrange
            var vsa = Mock.Of<IVisualStudioAccess>();
            var psdFactory = Mock.Of<Func<IPickSchemasDialog>>();
            var connFactory = Mock.Of<Func<IPickConnectionDialog>>();
            var creds = Mock.Of<ICredentialStore>();
            var vm = new PickServerDatabaseViewModel(vsa, creds, psdFactory, connFactory);

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
            var psdFactory = Mock.Of<Func<IPickSchemasDialog>>();
            var connFactory = Mock.Of<Func<IPickConnectionDialog>>();
            var creds = Mock.Of<ICredentialStore>();
            var vm = new PickServerDatabaseViewModel(vsa, creds, psdFactory, connFactory)
            {
                SelectedDatabaseConnection = dbConnection,
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
            var psdFactory = Mock.Of<Func<IPickSchemasDialog>>();
            var connFactory = Mock.Of<Func<IPickConnectionDialog>>();
            var creds = Mock.Of<ICredentialStore>();
            var vm = new PickServerDatabaseViewModel(vsa, creds, psdFactory, connFactory)
            {
                SelectedDatabaseConnection = dbConnection,
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
        }

        [Test]
        public void CancelCommand_CanExecute()
        {
            // Arrange
            var vsa = Mock.Of<IVisualStudioAccess>();
            var psdFactory = Mock.Of<Func<IPickSchemasDialog>>();
            var connFactory = Mock.Of<Func<IPickConnectionDialog>>();
            var creds = Mock.Of<ICredentialStore>();
            var vm = new PickServerDatabaseViewModel(vsa, creds, psdFactory, connFactory);

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
            var psdFactory = Mock.Of<Func<IPickSchemasDialog>>();
            var connFactory = Mock.Of<Func<IPickConnectionDialog>>();
            var creds = Mock.Of<ICredentialStore>();
            var vm = new PickServerDatabaseViewModel(vsa, creds, psdFactory, connFactory)
            {
                SelectedDatabaseConnection = dbConnection,
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
        }

        [Test]
        public void FilterSchemasCommand_CanNotExecute()
        {
            // Arrange
            var vsa = Mock.Of<IVisualStudioAccess>();
            var psdFactory = Mock.Of<Func<IPickSchemasDialog>>();
            var connFactory = Mock.Of<Func<IPickConnectionDialog>>();
            var creds = Mock.Of<ICredentialStore>();
            var vm = new PickServerDatabaseViewModel(vsa, creds, psdFactory, connFactory);

            // Act
            var canExecute = vm.FilterSchemasCommand.CanExecute(null);

            // Assert
            Assert.IsFalse(canExecute);
        }

        [Test]
        public void FilterSchemasCommand_CanExecute()
        {
            // Arrange
            var vsa = Mock.Of<IVisualStudioAccess>();
            var psdFactory = Mock.Of<Func<IPickSchemasDialog>>();
            var connFactory = Mock.Of<Func<IPickConnectionDialog>>();
            var creds = Mock.Of<ICredentialStore>();
            var vm = new PickServerDatabaseViewModel(vsa, creds, psdFactory, connFactory);

            // Act
            vm.FilterSchemas = true;
            var canExecute = vm.FilterSchemasCommand.CanExecute(null);

            // Assert
            Assert.IsTrue(canExecute);
        }
    }
}
