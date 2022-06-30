using System.ComponentModel;
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
        private DatabaseType databaseType;
        private IVsDataConnection dataConnection;

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
                    return $"{ConnectionName} ({DatabaseType})";
                }

                return ConnectionName;
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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
