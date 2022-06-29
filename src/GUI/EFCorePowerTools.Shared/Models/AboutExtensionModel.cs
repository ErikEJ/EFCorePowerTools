using JetBrains.Annotations;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EFCorePowerTools.Common.Models
{
    public class AboutExtensionModel : INotifyPropertyChanged
    {
        private Version extensionVersion;

        public event PropertyChangedEventHandler PropertyChanged;

        public static string SourceCodeUrl => "https://github.com/ErikEJ/EFCorePowerTools";
        public static string MarketplaceUrl => "https://marketplace.visualstudio.com/items?itemName=ErikEJ.EFCorePowerTools&ssr=false#review-details";

        public Version ExtensionVersion
        {
            get => extensionVersion;
            set
            {
                if (Equals(value, extensionVersion))
                {
                    return;
                }

                extensionVersion = value;
                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
