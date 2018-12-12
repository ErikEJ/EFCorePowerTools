namespace EFCorePowerTools.Shared.Models
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization;
    using Annotations;

    /// <summary>
    /// A class holding a certain information about tables.
    /// </summary>
    [DataContract]
    [DebuggerDisplay("{" + nameof(SafeFullName) + ",nq}")]
    public class TableInformationModel : INotifyPropertyChanged
    {
        private string _schema;
        private string _name;
        private bool _hasPrimaryKey;

        /// <summary>
        /// Gets or sets the schema name of the table.
        /// </summary>
        [DataMember]
        public string Schema
        {
            get => _schema;
            set
            {
                if (value == _schema) return;
                _schema = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(UnsafeFullName));
                OnPropertyChanged(nameof(SafeFullName));
            }
        }

        /// <summary>
        /// Gets or sets the table name.
        /// </summary>
        [DataMember]
        public string Name
        {
            get => _name;
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(UnsafeFullName));
                OnPropertyChanged(nameof(SafeFullName));
            }
        }

        /// <summary>
        /// Gets or sets whether a primary key exists for the table or not.
        /// </summary>
        [DataMember]
        public bool HasPrimaryKey
        {
            get => _hasPrimaryKey;
            set
            {
                if (value == _hasPrimaryKey) return;
                _hasPrimaryKey = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the unsafe (unescaped) full name of the table.
        /// </summary>
        [IgnoreDataMember]
        public string UnsafeFullName
        {
            get
            {
                if (Schema == null)
                {
                    return Name;
                }
                return $"{Schema}.{Name}";
            }
        }

        /// <summary>
        /// Gets the safe (escaped) full name of the table.
        /// </summary>
        [IgnoreDataMember]
        public string SafeFullName
        {
            get
            {
                if (Schema == null)
                {
                    return $"[{Name}]"; ;
                }
                 return $"[{Schema}].[{Name}]";
            }
            
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="TableInformationModel"/> class for a specific table.
        /// </summary>
        /// <param name="schema">The schema name of the table.</param>
        /// <param name="name">The table name.</param>
        /// <param name="hasPrimaryKey">Whether or not a primary key exists for the table.</param>
        /// <exception cref="ArgumentException"><paramref name="schema"/> or <paramref name="name"/> are null or only white spaces.</exception>
        public TableInformationModel(string schema,
                                     string name,
                                     bool hasPrimaryKey)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(@"Value cannot be empty or only white spaces.", nameof(name));

            Schema = schema;
            Name = name;
            HasPrimaryKey = hasPrimaryKey;
        }

        /// <summary>
        /// Parses the given <paramref name="table"/> into a <see cref="TableInformationModel"/> instance.
        /// </summary>
        /// <param name="table">The table to parse.</param>
        /// <exception cref="ArgumentException"><paramref name="table"/> is null, contains only white spaces, or cannot be parsed.</exception>
        /// <returns>The created <see cref="TableInformationModel"/> instance.</returns>
        /// <remarks><paramref name="table"/> should have the format <b>schema.table</b>.</remarks>
        public static TableInformationModel Parse(string table)
        {
            if (string.IsNullOrWhiteSpace(table))
                throw new ArgumentException(@"Value cannot be empty or only white spaces.", nameof(table));

            string schema = null;
            string name = null;

            var split = table.Split('.');
            if (split.Length == 2)
            {
                schema = split[0];
                name = split[1];
            }
            else
            {
                name = table;
            }
            return new TableInformationModel(schema, name, true);
        }

        /// <summary>
        /// Parses the given <paramref name="table"/> into a <see cref="TableInformationModel"/> instance.
        /// </summary>
        /// <param name="table">The table to parse.</param>
        /// <param name="hasPrimaryKey">Whether or not a primary key exists for the table.</param>
        /// <exception cref="ArgumentException"><paramref name="table"/> is null, contains only white spaces, or cannot be parsed.</exception>
        /// <returns>The created <see cref="TableInformationModel"/> instance.</returns>
        /// <remarks><paramref name="table"/> should have the format <b>schema.table</b>.</remarks>
        public static TableInformationModel Parse(string table, bool hasPrimaryKey)
        {
            if (string.IsNullOrWhiteSpace(table))
                throw new ArgumentException(@"Value cannot be empty or only white spaces.", nameof(table));

            string schema = null;
            string name = null;

            var split = table.Split('.');
            if (split.Length == 2)
            {
                schema = split[0];
                name = split[1];
            }
            else
            {
                name = table;
            }
            
            return new TableInformationModel(schema, name, hasPrimaryKey);
        }

        /// <summary>
        /// Tries to parse the given <paramref name="table"/> into a <see cref="TableInformationModel"/> instance.
        /// </summary>
        /// <param name="table">The table to parse.</param>
        /// <param name="tableInformationModel">The parsed <see cref="TableInformationModel"/>, or <b>null</b>.</param>
        /// <returns><b>True</b>, if the parsing was successful, otherwise <b>false</b>.</returns>
        public static bool TryParse(string table,
                                    out TableInformationModel tableInformationModel)
        {
            if (string.IsNullOrWhiteSpace(table))
            {
                tableInformationModel = null;
                return false;
            }
            
            var split = table.Split('.');
            if (split.Length != 2)
            {
                tableInformationModel = null;
                return false;
            }

            var schema = split[0];
            var name = split[1];
            tableInformationModel = new TableInformationModel(schema, name, true);
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}