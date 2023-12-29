using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace EFCorePowerTools.Common.Models
{
    /// <summary>
    /// A model holding the data of EF Core modeling options.
    /// </summary>
    public class ModelingOptionsModel : INotifyPropertyChanged
    {
        private bool installNuGetPackage;
        private int selectedToBeGenerated;
        private bool includeConnectionString;
        private bool useHandlebars;
        private int selectedHandlebarsLanguage;
        private bool usePluralizer;
        private bool useDatabaseNames;
        private string ns;
        private string outputPath;
        private string outputContextPath;
        private bool useSchemaFolders;
        private string modelNamespace;
        private string contextNamespace;
        private string modelName;
        private bool useDataAnnotations;
        private string projectName;
        private string dacpacPath;
        private bool useDbContextSplitting;
        private bool mapSpatialTypes;
        private bool mapHierarchyId;
        private bool mapNodaTimeTypes;
        private bool useEf6Pluralizer;
        private bool useBoolPropertiesWithoutDefaultSql;
        private bool useNullableReferences;
        private bool useNoDefaultConstructor;
        private bool useNoNavigations;
        private bool useNoObjectFilter;
        private bool useManyToManyEntity;
        private bool useDateOnlyTimeOnly;
        private bool useSchemaNamespaces;
        private string t4Templatepath;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool UseDataAnnotations
        {
            get => useDataAnnotations;
            set
            {
                if (value == useDataAnnotations)
                {
                    return;
                }

                useDataAnnotations = value;
                OnPropertyChanged();
            }
        }

        public string ModelName
        {
            get => modelName;
            set
            {
                if (value == modelName)
                {
                    return;
                }

                modelName = value;
                OnPropertyChanged();
            }
        }

        public string OutputPath
        {
            get => outputPath;
            set
            {
                if (value == outputPath)
                {
                    return;
                }

                outputPath = value;
                OnPropertyChanged();
            }
        }

        public string OutputContextPath
        {
            get => outputContextPath;
            set
            {
                if (value == outputContextPath)
                {
                    return;
                }

                outputContextPath = value;
                OnPropertyChanged();
            }
        }

        public bool UseSchemaFolders
        {
            get => useSchemaFolders;
            set
            {
                if (value == useSchemaFolders)
                {
                    return;
                }

                useSchemaFolders = value;
                OnPropertyChanged();
            }
        }

        public bool UseSchemaNamespaces
        {
            get => useSchemaNamespaces;
            set
            {
                if (value == useSchemaNamespaces)
                {
                    return;
                }

                useSchemaNamespaces = value;
                OnPropertyChanged();
            }
        }

        public string ModelNamespace
        {
            get => modelNamespace;
            set
            {
                if (value == modelNamespace)
                {
                    return;
                }

                modelNamespace = value;
                OnPropertyChanged();
            }
        }

        public string ContextNamespace
        {
            get => contextNamespace;
            set
            {
                if (value == contextNamespace)
                {
                    return;
                }

                contextNamespace = value;
                OnPropertyChanged();
            }
        }

        public string Namespace
        {
            get => ns;
            set
            {
                if (value == ns)
                {
                    return;
                }

                ns = value;
                OnPropertyChanged();
            }
        }

        public bool UseDatabaseNames
        {
            get => useDatabaseNames;
            set
            {
                if (value == useDatabaseNames)
                {
                    return;
                }

                useDatabaseNames = value;
                OnPropertyChanged();
            }
        }

        public bool UsePluralizer
        {
            get => usePluralizer;
            set
            {
                if (value == usePluralizer)
                {
                    return;
                }

                usePluralizer = value;
                OnPropertyChanged();
            }
        }

        public bool UseDbContextSplitting
        {
            get => useDbContextSplitting;
            set
            {
                if (value == useDbContextSplitting)
                {
                    return;
                }

                useDbContextSplitting = value;
                OnPropertyChanged();
            }
        }

        public bool UseHandlebars
        {
            get => useHandlebars;
            set
            {
                if (value == useHandlebars)
                {
                    return;
                }

                useHandlebars = value;
                OnPropertyChanged();
            }
        }

        public int SelectedHandlebarsLanguage
        {
            get => selectedHandlebarsLanguage;
            set
            {
                if (value == selectedHandlebarsLanguage)
                {
                    return;
                }

                selectedHandlebarsLanguage = value;
                OnPropertyChanged();
            }
        }

        public bool IncludeConnectionString
        {
            get => includeConnectionString;
            set
            {
                if (value == includeConnectionString)
                {
                    return;
                }

                includeConnectionString = value;
                OnPropertyChanged();
            }
        }

        public int SelectedToBeGenerated
        {
            get => selectedToBeGenerated;
            set
            {
                if (value == selectedToBeGenerated)
                {
                    return;
                }

                selectedToBeGenerated = value;
                OnPropertyChanged();
            }
        }

        public bool InstallNuGetPackage
        {
            get => installNuGetPackage;
            set
            {
                if (value == installNuGetPackage)
                {
                    return;
                }

                installNuGetPackage = value;
                OnPropertyChanged();
            }
        }

        public string ProjectName
        {
            get => projectName;
            set
            {
                if (value == projectName)
                {
                    return;
                }

                projectName = value;
                OnPropertyChanged();
            }
        }

        public string DacpacPath
        {
            get => dacpacPath;
            set
            {
                if (value == dacpacPath)
                {
                    return;
                }

                dacpacPath = value;
                OnPropertyChanged();
            }
        }

        public bool MapSpatialTypes
        {
            get => mapSpatialTypes;
            set
            {
                if (value == mapSpatialTypes)
                {
                    return;
                }

                mapSpatialTypes = value;
                OnPropertyChanged();
            }
        }

        public bool MapHierarchyId
        {
            get => mapHierarchyId;
            set
            {
                if (value == mapHierarchyId)
                {
                    return;
                }

                mapHierarchyId = value;
                OnPropertyChanged();
            }
        }

        public bool MapNodaTimeTypes
        {
            get => mapNodaTimeTypes;
            set
            {
                if (value == mapNodaTimeTypes)
                {
                    return;
                }

                mapNodaTimeTypes = value;
                OnPropertyChanged();
            }
        }

        public bool UseEf6Pluralizer
        {
            get => useEf6Pluralizer;
            set
            {
                if (value == useEf6Pluralizer)
                {
                    return;
                }

                useEf6Pluralizer = value;
                OnPropertyChanged();
            }
        }

        public bool UseBoolPropertiesWithoutDefaultSql
        {
            get => useBoolPropertiesWithoutDefaultSql;
            set
            {
                if (value == useBoolPropertiesWithoutDefaultSql)
                {
                    return;
                }

                useBoolPropertiesWithoutDefaultSql = value;
                OnPropertyChanged();
            }
        }

        public bool UseNullableReferences
        {
            get => useNullableReferences;
            set
            {
                if (value == useNullableReferences)
                {
                    return;
                }

                useNullableReferences = value;
                OnPropertyChanged();
            }
        }

        public bool UseNoObjectFilter
        {
            get => useNoObjectFilter;
            set
            {
                if (value == useNoObjectFilter)
                {
                    return;
                }

                useNoObjectFilter = value;
                OnPropertyChanged();
            }
        }

        public bool UseNoDefaultConstructor
        {
            get => useNoDefaultConstructor;
            set
            {
                if (value == useNoDefaultConstructor)
                {
                    return;
                }

                useNoDefaultConstructor = value;
                OnPropertyChanged();
            }
        }

        public bool UseNoNavigations
        {
            get => useNoNavigations;
            set
            {
                if (value == useNoNavigations)
                {
                    return;
                }

                useNoNavigations = value;
                OnPropertyChanged();
            }
        }

        public bool UseManyToManyEntity
        {
            get => useManyToManyEntity;
            set
            {
                if (value == useManyToManyEntity)
                {
                    return;
                }

                useManyToManyEntity = value;
                OnPropertyChanged();
            }
        }

        public bool UseDateOnlyTimeOnly
        {
            get => useDateOnlyTimeOnly;
            set
            {
                if (value == useDateOnlyTimeOnly)
                {
                    return;
                }

                useDateOnlyTimeOnly = value;
                OnPropertyChanged();
            }
        }

        public string T4TemplatePath
        {
            get => t4Templatepath;
            set
            {
                if (value == t4Templatepath)
                {
                    return;
                }

                t4Templatepath = value;
                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
