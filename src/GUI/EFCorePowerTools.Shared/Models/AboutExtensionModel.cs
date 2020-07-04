namespace EFCorePowerTools.Shared.Models
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Annotations;

    public class AboutExtensionModel : INotifyPropertyChanged
    {
        public string SourceCodeUrl => "https://github.com/ErikEJ/EFCorePowerTools";
        public string MarketplaceUrl => "https://marketplace.visualstudio.com/items?itemName=ErikEJ.EFCorePowerTools#review-details";

        private Version _extensionVersion;
        private Version _sqLiteAdoNetProviderVersion;
        private bool? _sqLiteEf6DbProviderInstalled;
        private bool? _sqLiteDdexProviderInstalled;
        private bool? _sqlLiteSimpleDdexProviderInstalled;

        public Version ExtensionVersion
        {
            get => _extensionVersion;
            set
            {
                if (Equals(value, _extensionVersion)) return;
                _extensionVersion = value;
                OnPropertyChanged();
            }
        }

        public Version SqLiteAdoNetProviderVersion
        {
            get => _sqLiteAdoNetProviderVersion;
            set
            {
                if (Equals(value, _sqLiteAdoNetProviderVersion)) return;
                _sqLiteAdoNetProviderVersion = value;
                OnPropertyChanged();
            }
        }

        public bool? SqLiteEf6DbProviderInstalled
        {
            get => _sqLiteEf6DbProviderInstalled;
            set
            {
                if (Equals(value, _sqLiteEf6DbProviderInstalled)) return;
                _sqLiteEf6DbProviderInstalled = value;
                OnPropertyChanged();
            }
        }

        public bool? SqLiteDdexProviderInstalled
        {
            get => _sqLiteDdexProviderInstalled;
            set
            {
                if (Equals(value, _sqLiteDdexProviderInstalled)) return;
                _sqLiteDdexProviderInstalled = value;
                OnPropertyChanged();
            }
        }

        public bool? SqlLiteSimpleDdexProviderInstalled
        {
            get => _sqlLiteSimpleDdexProviderInstalled;
            set
            {
                if (Equals(value, _sqlLiteSimpleDdexProviderInstalled)) return;
                _sqlLiteSimpleDdexProviderInstalled = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}