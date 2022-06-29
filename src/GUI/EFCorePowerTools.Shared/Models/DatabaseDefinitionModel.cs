using JetBrains.Annotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EFCorePowerTools.Common.Models
{
    /// <summary>
    /// A model holding data about a database definition, e.g. a DacPac file or database project.
    /// </summary>
    public class DatabaseDefinitionModel : INotifyPropertyChanged
    {
        private string filePath;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the file path to the database definition.
        /// </summary>
        public string FilePath
        {
            get => filePath;
            set
            {
                if (value == filePath)
                {
                    return;
                }

                filePath = value;
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
