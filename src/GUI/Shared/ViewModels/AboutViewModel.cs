using System;
using System.ComponentModel;
using System.Windows.Input;
using EFCorePowerTools.Common.BLL;
using EFCorePowerTools.Common.DAL;
using EFCorePowerTools.Common.Models;
using EFCorePowerTools.Contracts.ViewModels;
using EFCorePowerTools.Locales;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace EFCorePowerTools.ViewModels
{
    public class AboutViewModel : ViewModelBase, IAboutViewModel
    {
        private readonly AboutExtensionModel aboutExtensionModel;
        private readonly IExtensionVersionService extensionVersionService;
        private readonly IOperatingSystemAccess operatingSystemAccess;

        private string version;

        public AboutViewModel(
            AboutExtensionModel aboutExtensionModel,
            IExtensionVersionService extensionVersionService,
            IOperatingSystemAccess operatingSystemAccess)
        {
            this.aboutExtensionModel = aboutExtensionModel ?? throw new ArgumentNullException(nameof(aboutExtensionModel));
            this.extensionVersionService = extensionVersionService ?? throw new ArgumentNullException(nameof(extensionVersionService));
            this.operatingSystemAccess = operatingSystemAccess ?? throw new ArgumentNullException(nameof(operatingSystemAccess));

            this.aboutExtensionModel.PropertyChanged += AboutExtensionModelOnPropertyChanged;

            LoadedCommand = new RelayCommand(Loaded_Executed);
            OkCommand = new RelayCommand(Ok_Executed);
            OpenSourcesCommand = new RelayCommand(OpenSources_Executed);
            OpenMarketplaceCommand = new RelayCommand(OpenMarketplace_Executed);
        }

        public event EventHandler CloseRequested;

        public ICommand LoadedCommand { get; }
        public ICommand OkCommand { get; }
        public ICommand OpenSourcesCommand { get; }
        public ICommand OpenMarketplaceCommand { get; }

        public string Version
        {
            get => version;
            private set
            {
                if (Equals(value, version))
                {
                    return;
                }

                version = value;
                RaisePropertyChanged();
            }
        }


        private void Loaded_Executed()
        {
            if (aboutExtensionModel.ExtensionVersion == null)
            {
                extensionVersionService.SetExtensionVersion(aboutExtensionModel);
            }
            else
            {
                FormatVersion();
            }
        }

        private void Ok_Executed()
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        private void OpenSources_Executed()
        {
            operatingSystemAccess.StartProcess(AboutExtensionModel.SourceCodeUrl);
        }

        private void OpenMarketplace_Executed()
        {
            operatingSystemAccess.StartProcess(AboutExtensionModel.MarketplaceUrl);
        }

        private void FormatVersion()
        {
            Version = aboutExtensionModel.ExtensionVersion == null
                          ? AboutLocale.VersionNotAvailable
                          : $"{AboutLocale.Version} {aboutExtensionModel.ExtensionVersion.ToString(4)}";
        }

        private void AboutExtensionModelOnPropertyChanged(
            object sender,
            PropertyChangedEventArgs e)
        {
            if (sender != aboutExtensionModel)
            {
                return;
            }

            switch (e.PropertyName)
            {
                case nameof(AboutExtensionModel.ExtensionVersion):
                    FormatVersion();
                    break;
            }
        }
    }
}
