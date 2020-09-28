namespace EFCorePowerTools.Shared.Models
{
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;
    using Annotations;

    public class ConfigModel : INotifyPropertyChanged
    {
        private string _configPath;

        /// <summary>
        /// Gets or sets the file path to the eftp.*.config.json file.
        /// </summary>
        public string ConfigPath
        {
            get => _configPath;
            set
            {
                if (value == _configPath) return;
                _configPath = value;
                OnPropertyChanged();
            }
        }

        public string DisplayName
        {
            get
            {
                if (!string.IsNullOrEmpty(_configPath))
                {
                    return Path.GetFileName(_configPath);
                }

                return null;
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