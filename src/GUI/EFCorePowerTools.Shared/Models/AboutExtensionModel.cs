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
        private Version _sqlServerCompact40GacVersion;
        private bool? _sqlServerCompact40DbProviderInstalled;
        private bool? _sqlServerCompact40DdexProviderInstalled;
        private bool? _sqlServerCompact40SimpleDdexProviderInstalled;
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

        public Version SqlServerCompact40GacVersion
        {
            get => _sqlServerCompact40GacVersion;
            set
            {
                if (Equals(value, _sqlServerCompact40GacVersion)) return;
                _sqlServerCompact40GacVersion = value;
                OnPropertyChanged();
            }
        }

        public bool? SqlServerCompact40DbProviderInstalled
        {
            get => _sqlServerCompact40DbProviderInstalled;
            set
            {
                if (Equals(value, _sqlServerCompact40DbProviderInstalled)) return;
                _sqlServerCompact40DbProviderInstalled = value;
                OnPropertyChanged();
            }
        }

        public bool? SqlServerCompact40DdexProviderInstalled
        {
            get => _sqlServerCompact40DdexProviderInstalled;
            set
            {
                if (Equals(value, _sqlServerCompact40DdexProviderInstalled)) return;
                _sqlServerCompact40DdexProviderInstalled = value;
                OnPropertyChanged();
            }
        }

        public bool? SqlServerCompact40SimpleDdexProviderInstalled
        {
            get => _sqlServerCompact40SimpleDdexProviderInstalled;
            set
            {
                if (Equals(value, _sqlServerCompact40SimpleDdexProviderInstalled)) return;
                _sqlServerCompact40SimpleDdexProviderInstalled = value;
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