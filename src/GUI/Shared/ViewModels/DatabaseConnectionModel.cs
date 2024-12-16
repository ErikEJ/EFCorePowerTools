using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Microsoft.VisualStudio.Data.Services;
using RevEng.Common;

namespace EFCorePowerTools.Common.Models
{
    /// <summary>
    /// A model holding data about a database connection.
    /// </summary>
    public class DatabaseConnectionModel : INotifyPropertyChanged
    {
        private string connectionName;
        private string connectionString;
        private string filePath;
        private DatabaseType databaseType;
        private IVsDataConnection dataConnection;

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

        public string ConnectionName
        {
            get => connectionName;
            set
            {
                if (value == connectionName)
                {
                    return;
                }

                connectionName = value;
                OnPropertyChanged();
            }
        }

        public string DisplayName
        {
            get
            {
                if (DataConnection == null)
                {
                    if (DatabaseType == DatabaseType.SQLServerDacpac)
                    {
                        if (string.IsNullOrEmpty(FilePath))
                        {
                            return "<null>";
                        }

                        if (FilePath.EndsWith(".sqlproj", StringComparison.InvariantCultureIgnoreCase)
                            || FilePath.EndsWith(".csproj", StringComparison.InvariantCultureIgnoreCase))
                        {
                            return $"{Path.GetFileNameWithoutExtension(FilePath)} (SQL Project)";
                        }

                        if (FilePath.EndsWith(".dacpac", StringComparison.InvariantCultureIgnoreCase))
                        {
                            return $"{Path.GetFileNameWithoutExtension(FilePath)} (.dacpac)";
                        }
                    }

                    return $"{ConnectionName} ({DatabaseType})";
                }

                return ConnectionName;
            }
        }

        public string ToolTip
        {
            get
            {
                if (DataConnection == null)
                {
                    if (DatabaseType == DatabaseType.SQLServerDacpac)
                    {
                        if (string.IsNullOrEmpty(FilePath))
                        {
                            return "<null>";
                        }
                        else
                        {
                            return FilePath;
                        }
                    }

                    return ConnectionString;
                }

                return ConnectionString;
            }
        }

        public string ConnectionString
        {
            get => connectionString;
            set
            {
                if (value == connectionString)
                {
                    return;
                }

                connectionString = value;
                OnPropertyChanged();
            }
        }

        public DatabaseType DatabaseType
        {
            get => databaseType;
            set
            {
                if (value == databaseType)
                {
                    return;
                }

                databaseType = value;
                OnPropertyChanged();
            }
        }

        public IVsDataConnection DataConnection
        {
            get => dataConnection;
            set
            {
                if (value == dataConnection)
                {
                    return;
                }

                dataConnection = value;
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
