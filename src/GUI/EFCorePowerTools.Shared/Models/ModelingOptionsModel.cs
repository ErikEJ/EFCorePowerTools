namespace EFCorePowerTools.Shared.Models
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Annotations;

    /// <summary>
    /// A model holding the data of EF Core modeling options.
    /// </summary>
    public class ModelingOptionsModel : INotifyPropertyChanged
    {
        private bool _installNuGetPackage;
        private int _selectedTobeGenerated;
        private bool _includeConnectionString;
        private bool _useHandelbars;
        private bool _replaceId;
        private bool _usePluralizer;
        private bool _useDatabaseNames;
        private string _ns;
        private string _outputPath;
        private string _modelName;
        private bool _useDataAnnotations;
        private string _projectName;
        private string _dacpacPath;

        public bool UseDataAnnotations
        {
            get => _useDataAnnotations;
            set
            {
                if (value == _useDataAnnotations) return;
                _useDataAnnotations = value;
                OnPropertyChanged();
            }
        }

        public string ModelName
        {
            get => _modelName;
            set
            {
                if (value == _modelName) return;
                _modelName = value;
                OnPropertyChanged();
            }
        }

        public string OutputPath
        {
            get => _outputPath;
            set
            {
                if (value == _outputPath) return;
                _outputPath = value;
                OnPropertyChanged();
            }
        }

        public string Namespace
        {
            get => _ns;
            set
            {
                if (value == _ns) return;
                _ns = value;
                OnPropertyChanged();
            }
        }

        public bool UseDatabaseNames
        {
            get => _useDatabaseNames;
            set
            {
                if (value == _useDatabaseNames) return;
                _useDatabaseNames = value;
                OnPropertyChanged();
            }
        }

        public bool UsePluralizer
        {
            get => _usePluralizer;
            set
            {
                if (value == _usePluralizer) return;
                _usePluralizer = value;
                OnPropertyChanged();
            }
        }

        public bool ReplaceId
        {
            get => _replaceId;
            set
            {
                if (value == _replaceId) return;
                _replaceId = value;
                OnPropertyChanged();
            }
        }

        public bool UseHandelbars
        {
            get => _useHandelbars;
            set
            {
                if (value == _useHandelbars) return;
                _useHandelbars = value;
                OnPropertyChanged();
            }
        }

        public bool IncludeConnectionString
        {
            get => _includeConnectionString;
            set
            {
                if (value == _includeConnectionString) return;
                _includeConnectionString = value;
                OnPropertyChanged();
            }
        }

        public int SelectedTobeGenerated
        {
            get => _selectedTobeGenerated;
            set
            {
                if (value == _selectedTobeGenerated) return;
                _selectedTobeGenerated = value;
                OnPropertyChanged();
            }
        }

        public bool InstallNuGetPackage
        {
            get => _installNuGetPackage;
            set
            {
                if (value == _installNuGetPackage) return;
                _installNuGetPackage = value;
                OnPropertyChanged();
            }
        }

        public string ProjectName
        {
            get => _projectName;
            set
            {
                if (value == _projectName) return;
                _projectName = value;
                OnPropertyChanged();
            }
        }

        public string DacpacPath
        {
            get => _dacpacPath;
            set
            {
                if (value == _dacpacPath) return;
                _dacpacPath = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}