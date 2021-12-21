using NUnit.Framework;

namespace UnitTests.ViewModels
{
    using EFCorePowerTools.Shared.BLL;
    using EFCorePowerTools.Shared.DAL;
    using EFCorePowerTools.Shared.Models;
    using EFCorePowerTools.ViewModels;
    using Moq;
    using System;

    [TestFixture]
    public class AboutViewModelTests
    {
        [Test]
        public void Constructor_ArgumentNullException_AboutExtensionModel()
        {
            // Arrange
            AboutExtensionModel aem = null;
            IExtensionVersionService evs = null;
            IOperatingSystemAccess osa = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new AboutViewModel(aem, evs, osa));
        }

        [Test]
        public void Constructor_ArgumentNullException_ExtensionVersionService()
        {
            // Arrange
            var aem = new AboutExtensionModel();
            IExtensionVersionService evs = null;
            IOperatingSystemAccess osa = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new AboutViewModel(aem, evs, osa));
        }

        [Test]
        public void Constructor_ArgumentNullException_InstalledComponentsService()
        {
            // Arrange
            var aem = new AboutExtensionModel();
            var evs = Mock.Of<IExtensionVersionService>();
            IOperatingSystemAccess osa = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new AboutViewModel(aem, evs, osa));
        }

        [Test]
        public void Constructor_ArgumentNullException_OperatingSystemAccess()
        {
            // Arrange
            var aem = new AboutExtensionModel();
            var evs = Mock.Of<IExtensionVersionService>();
            IOperatingSystemAccess osa = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new AboutViewModel(aem, evs, osa));
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new AboutViewModel(aem, evs, osa));
        }

        [Test]
        public void Constructor_CommandsInitialized()
        {
            // Arrange
            var aem = new AboutExtensionModel();
            var evs = Mock.Of<IExtensionVersionService>();
            var osa = Mock.Of<IOperatingSystemAccess>();

            // Act
            var avm = new AboutViewModel(aem, evs, osa);
             
            // Assert
            Assert.IsNotNull(avm.LoadedCommand);
            Assert.IsNotNull(avm.OkCommand);
            Assert.IsNotNull(avm.OpenSourcesCommand);
            Assert.IsNotNull(avm.OpenMarketplaceCommand);
        }

        [Test]
        public void LoadedCommand_CanExecute()
        {
            // Arrange
            var aem = new AboutExtensionModel();
            var evs = Mock.Of<IExtensionVersionService>();
            var osa = Mock.Of<IOperatingSystemAccess>();
            var avm = new AboutViewModel(aem, evs, osa);
            
            // Act
            var canExecute = avm.LoadedCommand.CanExecute(null);

            // Assert
            Assert.IsTrue(canExecute);
        }

        [Test]
        public void LoadedCommand_Execute_EmptyModel_LoadData()
        {
            // Arrange
            var extensionVersion = new Version(10, 14, 15, 0);
            var aem = new AboutExtensionModel();
            var evsMock = new Mock<IExtensionVersionService>();
            evsMock.Setup(m => m.SetExtensionVersion(aem)).Callback<AboutExtensionModel>(m => m.ExtensionVersion = extensionVersion);
            var osa = Mock.Of<IOperatingSystemAccess>();
            var avm = new AboutViewModel(aem, evsMock.Object, osa);

            // Act
            avm.LoadedCommand.Execute(null);

            // Assert
            evsMock.Verify(m => m.SetExtensionVersion(aem), Times.Once);
            Assert.IsFalse(string.IsNullOrWhiteSpace(avm.Version));
        }

        [Test]
        public void LoadedCommand_Execute_FilledModel_FormatData()
        {
            // Arrange
            var extensionVersion = new Version(10, 14, 15, 0);
            var aem = new AboutExtensionModel
            {
                ExtensionVersion = extensionVersion,
            };
            var evsMock = new Mock<IExtensionVersionService>();
            var osa = Mock.Of<IOperatingSystemAccess>();
            var avm = new AboutViewModel(aem, evsMock.Object, osa);

            // Act
            avm.LoadedCommand.Execute(null);

            // Assert
            evsMock.Verify(m => m.SetExtensionVersion(aem), Times.Never);
            Assert.IsFalse(string.IsNullOrWhiteSpace(avm.Version));
        }

        [Test]
        public void OkCommand_CanExecute()
        {
            // Arrange
            var aem = new AboutExtensionModel();
            var evs = Mock.Of<IExtensionVersionService>();
            var osa = Mock.Of<IOperatingSystemAccess>();
            var avm = new AboutViewModel(aem, evs, osa);

            // Act
            var canExecute = avm.OkCommand.CanExecute(null);

            // Assert
            Assert.IsTrue(canExecute);
        }

        [Test]
        public void OkCommand_Executed_CloseRequestedToView()
        {
            // Arrange
            var closeRequested = false;
            var aem = new AboutExtensionModel();
            var evs = Mock.Of<IExtensionVersionService>();
            var osa = Mock.Of<IOperatingSystemAccess>();
            var avm = new AboutViewModel(aem, evs, osa);
            avm.CloseRequested += (sender, args) => closeRequested = true;

            // Act
            avm.OkCommand.Execute(null);

            // Assert
            Assert.IsTrue(closeRequested);
        }

        [Test]
        public void OpenSourcesCommand_CanExecute()
        {
            // Arrange
            var aem = new AboutExtensionModel();
            var evs = Mock.Of<IExtensionVersionService>();
            var osa = Mock.Of<IOperatingSystemAccess>();
            var avm = new AboutViewModel(aem, evs, osa);

            // Act
            var canExecute = avm.OpenSourcesCommand.CanExecute(null);

            // Assert
            Assert.IsTrue(canExecute);
        }

        [Test]
        public void OpenMarketplaceCommand_CanExecute()
        {
            // Arrange
            var aem = new AboutExtensionModel();
            var evs = Mock.Of<IExtensionVersionService>();
            var osa = Mock.Of<IOperatingSystemAccess>();
            var avm = new AboutViewModel(aem, evs, osa);

            // Act
            var canExecute = avm.OpenMarketplaceCommand.CanExecute(null);

            // Assert
            Assert.IsTrue(canExecute);
        }

        [Test]
        public void OpenMarketplace_Executed_NavigateToHttpHandler()
        {
            // Arrange
            var aem = new AboutExtensionModel();
            var evs = Mock.Of<IExtensionVersionService>();
            var osaMock = new Mock<IOperatingSystemAccess>();
            var avm = new AboutViewModel(aem, evs, osaMock.Object);

            // Act
            avm.OpenMarketplaceCommand.Execute(null);

            // Assert
            osaMock.Verify(m => m.StartProcess(aem.MarketplaceUrl), Times.Once);
        }
    }
}