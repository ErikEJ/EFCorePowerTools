namespace EFCorePowerTools.ViewModels
{
    using Contracts.ViewModels;
    using EFCorePowerTools.Locales;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;
    using Shared.BLL;
    using Shared.DAL;
    using Shared.Models;
    using System;
    using System.ComponentModel;
    using System.Windows.Input;

    public class AboutViewModel : ViewModelBase, IAboutViewModel
    {
        private readonly AboutExtensionModel _aboutExtensionModel;
        private readonly IExtensionVersionService _extensionVersionService;
        private readonly IOperatingSystemAccess _operatingSystemAccess;

        private string _version;

        public event EventHandler CloseRequested;

        public ICommand LoadedCommand { get; }
        public ICommand OkCommand { get; }
        public ICommand OpenSourcesCommand { get; }
        public ICommand OpenMarketplaceCommand { get; }

        public string Version
        {
            get => _version;
            private set
            {
                if (Equals(value, _version)) return;
                _version = value;
                RaisePropertyChanged();
            }
        }

        public AboutViewModel(AboutExtensionModel aboutExtensionModel,
                              IExtensionVersionService extensionVersionService,
                              IOperatingSystemAccess operatingSystemAccess)
        {
            _aboutExtensionModel = aboutExtensionModel ?? throw new ArgumentNullException(nameof(aboutExtensionModel));
            _extensionVersionService = extensionVersionService ?? throw new ArgumentNullException(nameof(extensionVersionService));
            _operatingSystemAccess = operatingSystemAccess ?? throw new ArgumentNullException(nameof(operatingSystemAccess));

            _aboutExtensionModel.PropertyChanged += AboutExtensionModelOnPropertyChanged;

            LoadedCommand = new RelayCommand(Loaded_Executed);
            OkCommand = new RelayCommand(Ok_Executed);
            OpenSourcesCommand = new RelayCommand(OpenSources_Executed);
            OpenMarketplaceCommand = new RelayCommand(OpenMarketplace_Executed);
        }
        
        private void Loaded_Executed()
        {
            if (_aboutExtensionModel.ExtensionVersion == null)
                _extensionVersionService.SetExtensionVersion(_aboutExtensionModel);
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
            _operatingSystemAccess.StartProcess(_aboutExtensionModel.SourceCodeUrl);
        }

        private void OpenMarketplace_Executed()
        {
            _operatingSystemAccess.StartProcess(_aboutExtensionModel.MarketplaceUrl);
        }

        private void FormatVersion()
        {
            Version = _aboutExtensionModel.ExtensionVersion == null
                          ? AboutLocale.VersionNotAvailable
                          : $"{AboutLocale.Version} {_aboutExtensionModel.ExtensionVersion.ToString(4)}";
        }

        private void AboutExtensionModelOnPropertyChanged(object sender,
                                                          PropertyChangedEventArgs e)
        {
            if (sender != _aboutExtensionModel)
                return;

            switch (e.PropertyName)
            {
                case nameof(AboutExtensionModel.ExtensionVersion):
                    FormatVersion();
                    break;
            }
        }
    }
}