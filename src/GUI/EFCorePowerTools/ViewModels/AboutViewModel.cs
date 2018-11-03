namespace EFCorePowerTools.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.Text;
    using System.Windows.Input;
    using Contracts.ViewModels;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;
    using GalaSoft.MvvmLight.Messaging;
    using Messages;
    using Shared.BLL;
    using Shared.DAL;
    using Shared.Models;

    public class AboutViewModel : ViewModelBase, IAboutViewModel
    {
        private readonly AboutExtensionModel _aboutExtensionModel;
        private readonly IExtensionVersionService _extensionVersionService;
        private readonly IInstalledComponentsService _installedComponentsService;
        private readonly IOperatingSystemAccess _operatingSystemAccess;
        private readonly IVisualStudioAccess _visualStudioAccess;
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
            _visualStudioAccess = visualStudioAccess ?? throw new ArgumentNullException(nameof(visualStudioAccess));
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

            if (_aboutExtensionModel.SqlServerCompact40GacVersion == null
                || _aboutExtensionModel.SqlServerCompact40DbProviderInstalled == null
                || _aboutExtensionModel.SqlServerCompact40DdexProviderInstalled == null
                || _aboutExtensionModel.SqlServerCompact40SimpleDdexProviderInstalled == null
                || _aboutExtensionModel.SqLiteAdoNetProviderVersion == null
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
            _visualStudioAccess.NavigateToUrl(_aboutExtensionModel.SourceCodeUrl);
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
                Content = "About info copied to clipboard"
            });
        }

        private bool CopyToClipboard_CanExecute() => !string.IsNullOrWhiteSpace(StatusText);

        private void FormatVersion()
        {
            Version = _aboutExtensionModel.ExtensionVersion == null
                          ? "Version N/A"
                          : $"Version {_aboutExtensionModel.ExtensionVersion.ToString(4)}";
        }

        private void FormatStatusText()
        {
            var sb = new StringBuilder();

            sb.Append("SQL Server Compact 4.0 in GAC - ");
            sb.AppendLine(_aboutExtensionModel.SqlServerCompact40GacVersion == null ? "No" : $"Yes - {_aboutExtensionModel.SqlServerCompact40GacVersion}");

            sb.Append("SQL Server Compact 4.0 DbProvider - ");
            sb.AppendLine(_aboutExtensionModel.SqlServerCompact40DbProviderInstalled == true ? "Yes" : "No");

            sb.AppendLine();

            sb.Append("SQL Server Compact 4.0 DDEX provider - ");
            sb.AppendLine(_aboutExtensionModel.SqlServerCompact40DdexProviderInstalled == true ? "Yes" : "No");

            sb.Append("SQL Server Compact 4.0 Simple DDEX provider - ");
            sb.AppendLine(_aboutExtensionModel.SqlServerCompact40SimpleDdexProviderInstalled == true ? "Yes" : "No");

            sb.AppendLine();
            sb.AppendLine();

            sb.Append("SQLite ADO.NET Provider included: ");
            sb.AppendLine(_aboutExtensionModel.SqLiteAdoNetProviderVersion == null ? "No" : _aboutExtensionModel.SqLiteAdoNetProviderVersion.ToString());

            sb.Append("SQLite EF6 DbProvider in GAC - ");
            sb.AppendLine(_aboutExtensionModel.SqLiteEf6DbProviderInstalled == true ? "Yes" : "No");

            sb.AppendLine();

            sb.Append("System.Data.SQLite DDEX provider - ");
            sb.AppendLine(_aboutExtensionModel.SqLiteDdexProviderInstalled == true ? "Yes" : "No");

            sb.Append("SQLite Simple DDEX provider - ");
            sb.AppendLine(_aboutExtensionModel.SqlLiteSimpleDdexProviderInstalled == true ? "Yes" : "No");

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
                case nameof(AboutExtensionModel.SqlServerCompact40GacVersion):
                case nameof(AboutExtensionModel.SqlServerCompact40DbProviderInstalled):
                case nameof(AboutExtensionModel.SqlServerCompact40DdexProviderInstalled):
                case nameof(AboutExtensionModel.SqlServerCompact40SimpleDdexProviderInstalled):
                case nameof(AboutExtensionModel.SqLiteEf6DbProviderInstalled):
                case nameof(AboutExtensionModel.SqLiteDdexProviderInstalled):
                case nameof(AboutExtensionModel.SqlLiteSimpleDdexProviderInstalled):
                    FormatStatusText();
                    break;
            }
        }
    }
}