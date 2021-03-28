namespace EFCorePowerTools.ViewModels
{
    using Contracts.ViewModels;
    using EFCorePowerTools.Locales;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;
    using GalaSoft.MvvmLight.Messaging;
    using Messages;
    using Shared.BLL;
    using Shared.DAL;
    using Shared.Models;
    using System;
    using System.ComponentModel;
    using System.Text;
    using System.Windows.Input;

    public class AboutViewModel : ViewModelBase, IAboutViewModel
    {
        private readonly AboutExtensionModel _aboutExtensionModel;
        private readonly IExtensionVersionService _extensionVersionService;
        private readonly IInstalledComponentsService _installedComponentsService;
        private readonly IOperatingSystemAccess _operatingSystemAccess;
        private readonly IMessenger _messenger;

        private string _version;
        private string _statusText;

        public event EventHandler CloseRequested;

        public ICommand LoadedCommand { get; }
        public ICommand OkCommand { get; }
        public ICommand OpenSourcesCommand { get; }
        public ICommand OpenMarketplaceCommand { get; }
        public ICommand CopyToClipboardCommand { get; }

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

        public string StatusText
        {
            get => _statusText;
            private set
            {
                if (Equals(value, _statusText)) return;
                _statusText = value;
                RaisePropertyChanged();
            }
        }

        public AboutViewModel(AboutExtensionModel aboutExtensionModel,
                              IExtensionVersionService extensionVersionService,
                              IInstalledComponentsService installedComponentsService,
                              IOperatingSystemAccess operatingSystemAccess,
                              IVisualStudioAccess visualStudioAccess,
                              IMessenger messenger)
        {
            _aboutExtensionModel = aboutExtensionModel ?? throw new ArgumentNullException(nameof(aboutExtensionModel));
            _extensionVersionService = extensionVersionService ?? throw new ArgumentNullException(nameof(extensionVersionService));
            _installedComponentsService = installedComponentsService ?? throw new ArgumentNullException(nameof(installedComponentsService));
            _operatingSystemAccess = operatingSystemAccess ?? throw new ArgumentNullException(nameof(operatingSystemAccess));
            _messenger = messenger ?? throw new ArgumentNullException(nameof(messenger));

            _aboutExtensionModel.PropertyChanged += AboutExtensionModelOnPropertyChanged;

            LoadedCommand = new RelayCommand(Loaded_Executed);
            OkCommand = new RelayCommand(Ok_Executed);
            OpenSourcesCommand = new RelayCommand(OpenSources_Executed);
            OpenMarketplaceCommand = new RelayCommand(OpenMarketplace_Executed);
            CopyToClipboardCommand = new RelayCommand(CopyToClipboard_Executed, CopyToClipboard_CanExecute);
        }
        
        private void Loaded_Executed()
        {
            if (_aboutExtensionModel.ExtensionVersion == null)
                _extensionVersionService.SetExtensionVersion(_aboutExtensionModel);
            else
                FormatVersion();

            if (_aboutExtensionModel.SqLiteAdoNetProviderVersion == null
                || _aboutExtensionModel.SqLiteEf6DbProviderInstalled == null
                || _aboutExtensionModel.SqLiteDdexProviderInstalled == null
                || _aboutExtensionModel.SqlLiteSimpleDdexProviderInstalled == null)
            {
                _installedComponentsService.SetMissingComponentData(_aboutExtensionModel);
            }
            else
            {
                FormatStatusText();
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

        private void CopyToClipboard_Executed()
        {
            _operatingSystemAccess.SetClipboardText(Version + Environment.NewLine + Environment.NewLine + StatusText);
            _messenger.Send(new ShowMessageBoxMessage
            {
                Content = AboutLocale.AboutCopiedToClipboard
            });
        }

        private bool CopyToClipboard_CanExecute() => !string.IsNullOrWhiteSpace(StatusText);

        private void FormatVersion()
        {
            Version = _aboutExtensionModel.ExtensionVersion == null
                          ? AboutLocale.VersionNotAvailable
                          : $"{AboutLocale.Version} {_aboutExtensionModel.ExtensionVersion.ToString(4)}";
        }

        private void FormatStatusText()
        {
            var sb = new StringBuilder();

            sb.Append($"{AboutLocale.SQLiteProviderIncluded}: ");
            sb.AppendLine(_aboutExtensionModel.SqLiteAdoNetProviderVersion == null ? AboutLocale.No : _aboutExtensionModel.SqLiteAdoNetProviderVersion.ToString());

            sb.Append($"{AboutLocale.SQLiteProviderGAC} - ");
            sb.AppendLine(_aboutExtensionModel.SqLiteEf6DbProviderInstalled == true ? AboutLocale.Yes : AboutLocale.No);

            sb.AppendLine();

            sb.Append($"{AboutLocale.SQLiteDDEXProvider} - ");
            sb.AppendLine(_aboutExtensionModel.SqLiteDdexProviderInstalled == true ? AboutLocale.Yes : AboutLocale.No);

            sb.Append($"{AboutLocale.SQLiteSimpleDDEXProvider} - ");
            sb.AppendLine(_aboutExtensionModel.SqlLiteSimpleDdexProviderInstalled == true ? AboutLocale.Yes : AboutLocale.No);

            StatusText = sb.ToString();
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
                case nameof(AboutExtensionModel.SqLiteEf6DbProviderInstalled):
                case nameof(AboutExtensionModel.SqLiteDdexProviderInstalled):
                case nameof(AboutExtensionModel.SqlLiteSimpleDdexProviderInstalled):
                    FormatStatusText();
                    break;
            }
        }
    }
}