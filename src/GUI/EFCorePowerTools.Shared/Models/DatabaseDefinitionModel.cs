namespace EFCorePowerTools.Shared.Models
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Annotations;

    /// <summary>
    /// A model holding data about a database definition, e.g. a DacPac file or database project.
    /// </summary>
    public class DatabaseDefinitionModel : INotifyPropertyChanged
    {
        private string _filePath;

        /// <summary>
        /// Gets or sets the file path to the database definition.
        /// </summary>
        public string FilePath
        {
            get => _filePath;
            set
            {
                if (value == _filePath) return;
                _filePath = value;
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