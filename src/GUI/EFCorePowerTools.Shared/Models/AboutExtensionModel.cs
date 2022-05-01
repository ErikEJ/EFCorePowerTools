namespace EFCorePowerTools.Common.Models
{
    using JetBrains.Annotations;
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class AboutExtensionModel : INotifyPropertyChanged
    {
        public static string SourceCodeUrl => "https://github.com/ErikEJ/EFCorePowerTools";
        public static string MarketplaceUrl => "https://marketplace.visualstudio.com/items?itemName=ErikEJ.EFCorePowerTools&ssr=false#review-details";

        private Version _extensionVersion;

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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
