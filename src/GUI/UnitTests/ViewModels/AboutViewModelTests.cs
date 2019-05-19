using NUnit.Framework;

namespace UnitTests.ViewModels
{
    using System;
    using EFCorePowerTools.Messages;
    using EFCorePowerTools.Shared.BLL;
    using EFCorePowerTools.Shared.DAL;
    using EFCorePowerTools.Shared.Models;
    using EFCorePowerTools.ViewModels;
    using GalaSoft.MvvmLight.Messaging;
    using Moq;

    [TestFixture]
    public class AboutViewModelTests
    {
        [Test]
        public void Constructor_ArgumentNullException_AboutExtensionModel()
        {
            // Arrange
            AboutExtensionModel aem = null;
            IExtensionVersionService evs = null;
            IInstalledComponentsService ics = null;
            IOperatingSystemAccess osa = null;
            IVisualStudioAccess vsa = null;
            IMessenger m = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new AboutViewModel(aem, evs, ics, osa, vsa, m));
        }

        [Test]
        public void Constructor_ArgumentNullException_ExtensionVersionService()
        {
            // Arrange
            var aem = new AboutExtensionModel();
            IExtensionVersionService evs = null;
            IInstalledComponentsService ics = null;
            IOperatingSystemAccess osa = null;
            IVisualStudioAccess vsa = null;
            IMessenger m = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new AboutViewModel(aem, evs, ics, osa, vsa, m));
        }

        [Test]
        public void Constructor_ArgumentNullException_InstalledComponentsService()
        {
            // Arrange
            var aem = new AboutExtensionModel();
            var evs = Mock.Of<IExtensionVersionService>();
            IInstalledComponentsService ics = null;
            IOperatingSystemAccess osa = null;
            IVisualStudioAccess vsa = null;
            IMessenger m = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new AboutViewModel(aem, evs, ics, osa, vsa, m));
        }

        [Test]
        public void Constructor_ArgumentNullException_OperatingSystemAccess()
        {
            // Arrange
            var aem = new AboutExtensionModel();
            var evs = Mock.Of<IExtensionVersionService>();
            var ics = Mock.Of<IInstalledComponentsService>();
            IOperatingSystemAccess osa = null;
            IVisualStudioAccess vsa = null;
            IMessenger m = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new AboutViewModel(aem, evs, ics, osa, vsa, m));
        }

        [Test]
        public void Constructor_ArgumentNullException_VisualStudioAccess()
        {
            // Arrange
            var aem = new AboutExtensionModel();
            var evs = Mock.Of<IExtensionVersionService>();
            var ics = Mock.Of<IInstalledComponentsService>();
            var osa = Mock.Of<IOperatingSystemAccess>();
            IVisualStudioAccess vsa = null;
            IMessenger m = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new AboutViewModel(aem, evs, ics, osa, vsa, m));
        }

        [Test]
        public void Constructor_ArgumentNullException_Messenger()
        {
            // Arrange
            var aem = new AboutExtensionModel();
            var evs = Mock.Of<IExtensionVersionService>();
            var ics = Mock.Of<IInstalledComponentsService>();
            var osa = Mock.Of<IOperatingSystemAccess>();
            var vsa = Mock.Of<IVisualStudioAccess>();
            IMessenger m = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new AboutViewModel(aem, evs, ics, osa, vsa, m));
        }

        [Test]
        public void Constructor_CommandsInitialized()
        {
            // Arrange
            var aem = new AboutExtensionModel();
            var evs = Mock.Of<IExtensionVersionService>();
            var ics = Mock.Of<IInstalledComponentsService>();
            var osa = Mock.Of<IOperatingSystemAccess>();
            var vsa = Mock.Of<IVisualStudioAccess>();
            var m = Mock.Of<IMessenger>();

            // Act
            var avm = new AboutViewModel(aem, evs, ics, osa, vsa, m);
             
            // Assert
            Assert.IsNotNull(avm.LoadedCommand);
            Assert.IsNotNull(avm.OkCommand);
            Assert.IsNotNull(avm.OpenSourcesCommand);
            Assert.IsNotNull(avm.OpenMarketplaceCommand);
            Assert.IsNotNull(avm.CopyToClipboardCommand);
        }

        [Test]
        public void LoadedCommand_CanExecute()
        {
            // Arrange
            var aem = new AboutExtensionModel();
            var evs = Mock.Of<IExtensionVersionService>();
            var ics = Mock.Of<IInstalledComponentsService>();
            var osa = Mock.Of<IOperatingSystemAccess>();
            var vsa = Mock.Of<IVisualStudioAccess>();
            var m = Mock.Of<IMessenger>();
            var avm = new AboutViewModel(aem, evs, ics, osa, vsa, m);
            
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
            var sqlServerCompact40GacVersion = new Version(1, 5, 0, 15);
            var sqLiteAdoNetProviderVersion = new Version(15, 0, 1, 2);
            var aem = new AboutExtensionModel();
            var evsMock = new Mock<IExtensionVersionService>();
            evsMock.Setup(m => m.SetExtensionVersion(aem)).Callback<AboutExtensionModel>(m => m.ExtensionVersion = extensionVersion);
            var icsMock = new Mock<IInstalledComponentsService>();
            icsMock.Setup(m => m.SetMissingComponentData(aem)).Callback<AboutExtensionModel>(m =>
            {
                m.SqlServerCompact40GacVersion = sqlServerCompact40GacVersion;
                m.SqlServerCompact40DbProviderInstalled = true;
                m.SqlServerCompact40DdexProviderInstalled = true;
                m.SqlServerCompact40SimpleDdexProviderInstalled = true;
                m.SqLiteAdoNetProviderVersion = sqLiteAdoNetProviderVersion;
                m.SqLiteEf6DbProviderInstalled = true;
                m.SqLiteDdexProviderInstalled = true;
                m.SqlLiteSimpleDdexProviderInstalled = true;
            });
            var osa = Mock.Of<IOperatingSystemAccess>();
            var vsa = Mock.Of<IVisualStudioAccess>();
            var messenger = Mock.Of<IMessenger>();
            var avm = new AboutViewModel(aem, evsMock.Object, icsMock.Object, osa, vsa, messenger);

            // Act
            avm.LoadedCommand.Execute(null);

            // Assert
            evsMock.Verify(m => m.SetExtensionVersion(aem), Times.Once);
            icsMock.Verify(m => m.SetMissingComponentData(aem), Times.Once);
            Assert.IsFalse(string.IsNullOrWhiteSpace(avm.Version));
            Assert.IsFalse(string.IsNullOrWhiteSpace(avm.StatusText));
        }

        [Test]
        public void LoadedCommand_Execute_FilledModel_FormatData()
        {
            // Arrange
            var extensionVersion = new Version(10, 14, 15, 0);
            var sqlServerCompact40GacVersion = new Version(1, 5, 0, 15);
            var sqLiteAdoNetProviderVersion = new Version(15, 0, 1, 2);
            var aem = new AboutExtensionModel
            {
                ExtensionVersion = extensionVersion,
                SqlServerCompact40GacVersion = sqlServerCompact40GacVersion,
                SqlServerCompact40DbProviderInstalled = true,
                SqlServerCompact40DdexProviderInstalled = true,
                SqlServerCompact40SimpleDdexProviderInstalled = true,
                SqLiteAdoNetProviderVersion = sqLiteAdoNetProviderVersion,
                SqLiteEf6DbProviderInstalled = true,
                SqLiteDdexProviderInstalled = true,
                SqlLiteSimpleDdexProviderInstalled = true,
            };
            var evsMock = new Mock<IExtensionVersionService>();
            var icsMock = new Mock<IInstalledComponentsService>();
            var osa = Mock.Of<IOperatingSystemAccess>();
            var vsa = Mock.Of<IVisualStudioAccess>();
            var messenger = Mock.Of<IMessenger>();
            var avm = new AboutViewModel(aem, evsMock.Object, icsMock.Object, osa, vsa, messenger);

            // Act
            avm.LoadedCommand.Execute(null);

            // Assert
            evsMock.Verify(m => m.SetExtensionVersion(aem), Times.Never);
            icsMock.Verify(m => m.SetMissingComponentData(aem), Times.Never);
            Assert.IsFalse(string.IsNullOrWhiteSpace(avm.Version));
            Assert.IsFalse(string.IsNullOrWhiteSpace(avm.StatusText));
        }

        [Test]
        public void OkCommand_CanExecute()
        {
            // Arrange
            var aem = new AboutExtensionModel();
            var evs = Mock.Of<IExtensionVersionService>();
            var ics = Mock.Of<IInstalledComponentsService>();
            var osa = Mock.Of<IOperatingSystemAccess>();
            var vsa = Mock.Of<IVisualStudioAccess>();
            var m = Mock.Of<IMessenger>();
            var avm = new AboutViewModel(aem, evs, ics, osa, vsa, m);

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
            var ics = Mock.Of<IInstalledComponentsService>();
            var osa = Mock.Of<IOperatingSystemAccess>();
            var vsa = Mock.Of<IVisualStudioAccess>();
            var m = Mock.Of<IMessenger>();
            var avm = new AboutViewModel(aem, evs, ics, osa, vsa, m);
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
            var ics = Mock.Of<IInstalledComponentsService>();
            var osa = Mock.Of<IOperatingSystemAccess>();
            var vsa = Mock.Of<IVisualStudioAccess>();
            var m = Mock.Of<IMessenger>();
            var avm = new AboutViewModel(aem, evs, ics, osa, vsa, m);

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
            var ics = Mock.Of<IInstalledComponentsService>();
            var osa = Mock.Of<IOperatingSystemAccess>();
            var vsa = Mock.Of<IVisualStudioAccess>();
            var m = Mock.Of<IMessenger>();
            var avm = new AboutViewModel(aem, evs, ics, osa, vsa, m);

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
            var ics = Mock.Of<IInstalledComponentsService>();
            var osaMock = new Mock<IOperatingSystemAccess>();
            var vsa = Mock.Of<IVisualStudioAccess>();
            var messenger = Mock.Of<IMessenger>();
            var avm = new AboutViewModel(aem, evs, ics, osaMock.Object, vsa, messenger);

            // Act
            avm.OpenMarketplaceCommand.Execute(null);

            // Assert
            osaMock.Verify(m => m.StartProcess(aem.MarketplaceUrl), Times.Once);
        }

        [Test]
        public void CopyToClipboardCommand_CanExecute_NoStatusText()
        {
            // Arrange
            var aem = new AboutExtensionModel();
            var evs = Mock.Of<IExtensionVersionService>();
            var ics = Mock.Of<IInstalledComponentsService>();
            var osa = Mock.Of<IOperatingSystemAccess>();
            var vsa = Mock.Of<IVisualStudioAccess>();
            var m = Mock.Of<IMessenger>();
            var avm = new AboutViewModel(aem, evs, ics, osa, vsa, m);

            // Act
            var canExecute = avm.CopyToClipboardCommand.CanExecute(null);

            // Assert
            Assert.IsFalse(canExecute);
        }

        [Test]
        public void CopyToClipboardCommand_CanExecute_WithStatusText()
        {
            // Arrange
            var extensionVersion = new Version(10, 14, 15, 0);
            var sqlServerCompact40GacVersion = new Version(1, 5, 0, 15);
            var sqLiteAdoNetProviderVersion = new Version(15, 0, 1, 2);
            var aem = new AboutExtensionModel
            {
                ExtensionVersion = extensionVersion,
                SqlServerCompact40GacVersion = sqlServerCompact40GacVersion,
                SqlServerCompact40DbProviderInstalled = true,
                SqlServerCompact40DdexProviderInstalled = true,
                SqlServerCompact40SimpleDdexProviderInstalled = true,
                SqLiteAdoNetProviderVersion = sqLiteAdoNetProviderVersion,
                SqLiteEf6DbProviderInstalled = true,
                SqLiteDdexProviderInstalled = true,
                SqlLiteSimpleDdexProviderInstalled = true,
            };
            var evs = Mock.Of<IExtensionVersionService>();
            var ics = Mock.Of<IInstalledComponentsService>();
            var osa = Mock.Of<IOperatingSystemAccess>();
            var vsa = Mock.Of<IVisualStudioAccess>();
            var m = Mock.Of<IMessenger>();
            var avm = new AboutViewModel(aem, evs, ics, osa, vsa, m);
            aem.SqlLiteSimpleDdexProviderInstalled = false;

            // Act
            var canExecute = avm.CopyToClipboardCommand.CanExecute(null);

            // Assert
            Assert.IsTrue(canExecute);
        }

        [Test]
        public void CopyToClipboardCommand_Executed()
        {
            // Arrange
            string clipboardText = null;
            var extensionVersion = new Version(10, 14, 15, 0);
            var sqlServerCompact40GacVersion = new Version(1, 5, 0, 15);
            var sqLiteAdoNetProviderVersion = new Version(15, 0, 1, 2);
            var aem = new AboutExtensionModel
            {
                SqlServerCompact40GacVersion = sqlServerCompact40GacVersion,
                SqlServerCompact40DbProviderInstalled = true,
                SqlServerCompact40DdexProviderInstalled = true,
                SqlServerCompact40SimpleDdexProviderInstalled = true,
                SqLiteAdoNetProviderVersion = sqLiteAdoNetProviderVersion,
                SqLiteEf6DbProviderInstalled = true,
                SqLiteDdexProviderInstalled = true,
                SqlLiteSimpleDdexProviderInstalled = true,
            };
            var evs = Mock.Of<IExtensionVersionService>();
            var ics = Mock.Of<IInstalledComponentsService>();
            var osaMock = new Mock<IOperatingSystemAccess>();
            osaMock.Setup(m => m.SetClipboardText(It.IsNotNull<string>())).Callback<string>(m => clipboardText = m);
            var vsa = Mock.Of<IVisualStudioAccess>();
            var messengerMock = new Mock<IMessenger>();
            var avm = new AboutViewModel(aem, evs, ics, osaMock.Object, vsa, messengerMock.Object);
            aem.ExtensionVersion = extensionVersion;
            aem.SqlLiteSimpleDdexProviderInstalled = false;

            // Act
            avm.CopyToClipboardCommand.Execute(null);

            // Assert
            osaMock.Verify(m => m.SetClipboardText(It.IsNotNull<string>()), Times.Once);
            Assert.IsFalse(string.IsNullOrWhiteSpace(clipboardText));
            Assert.IsTrue(clipboardText.Contains(avm.Version));
            Assert.IsTrue(clipboardText.Contains(avm.StatusText));
            messengerMock.Verify(m => m.Send(It.IsNotNull<ShowMessageBoxMessage>()), Times.Once);
        }
    }
}