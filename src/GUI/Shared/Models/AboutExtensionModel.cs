using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace EFCorePowerTools.Common.Models
{
    public class AboutExtensionModel : INotifyPropertyChanged
    {
        private string extensionVersion;

        public event PropertyChangedEventHandler PropertyChanged;

        public static string SourceCodeUrl => "https://github.com/ErikEJ/EFCorePowerTools";

        public static string MarketplaceUrl => "https://marketplace.visualstudio.com/items?itemName=ErikEJ.EFCorePowerTools&ssr=false#review-details";

        public string ExtensionVersion
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