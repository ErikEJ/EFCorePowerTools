namespace EFCorePowerTools.Shared.Models
{
    using JetBrains.Annotations;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;

    public class ConfigModel : INotifyPropertyChanged
    {
        private string _configPath;
        private string _projectPath;

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

        /// <summary>
        /// Gets or sets the project path.
        /// </summary>
        public string ProjectPath
        {
            get => _projectPath;
            set
            {
                if (value == _projectPath) return;
                _projectPath = value;
                OnPropertyChanged();
            }
        }

        public string DisplayName
        {
            get
            {
                if (!string.IsNullOrEmpty(_configPath))
                {
                    if (string.IsNullOrEmpty(_projectPath))
                        return Path.GetFileName(_configPath);
                    else
                        return _configPath.Replace(_projectPath, string.Empty).Replace(@"\\", @"\");
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