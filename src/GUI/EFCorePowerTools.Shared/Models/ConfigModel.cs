using JetBrains.Annotations;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace EFCorePowerTools.Common.Models
{
    public class ConfigModel : INotifyPropertyChanged
    {
        private string configPath;
        private string projectPath;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the file path to the eftp.*.config.json file.
        /// </summary>
        public string ConfigPath
        {
            get => configPath;
            set
            {
                if (value == configPath)
                {
                    return;
                }

                configPath = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the project path.
        /// </summary>
        public string ProjectPath
        {
            get => projectPath;
            set
            {
                if (value == projectPath)
                {
                    return;
                }

                projectPath = value;
                OnPropertyChanged();
            }
        }

        public string DisplayName
        {
            get
            {
                if (!string.IsNullOrEmpty(configPath))
                {
                    if (string.IsNullOrEmpty(projectPath))
                    {
                        return Path.GetFileName(configPath);
                    }
                    else
                    {
                        return configPath.Replace(projectPath, string.Empty).Replace(@"\\", @"\");
                    }
                }

                return null;
            }
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
