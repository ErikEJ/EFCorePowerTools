namespace EFCorePowerTools.Shared.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization;
    using Annotations;
    using RevEng.Shared;

    /// <summary>
    /// A class holding information about database objects.
    /// </summary>
    [DataContract]
    [DebuggerDisplay("{" + nameof(Name) + ",nq}")]
    public class TableInformationModel : INotifyPropertyChanged
    {
        private string _name;
        private bool _hasPrimaryKey;
        private bool _showKeylessWarning;
        private ObjectType _objectType;
        private IEnumerable<string> _excludedColumns = new List<string>();

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
        /// Show if the table is keyless - always false for EF Core 2.0.
        /// </summary>
        [IgnoreDataMember]
        public bool ShowKeylessWarning
        {
            get => _showKeylessWarning;
            set
            {
                if (value == _showKeylessWarning) return;
                _showKeylessWarning = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Is this a procedure?
        /// </summary>
        [IgnoreDataMember]
        public bool IsProcedure
        {
            get => _objectType == ObjectType.Procedure;
        }

        /// <summary>
        /// Gets or sets the object type.
        /// </summary>
        [DataMember]
        public ObjectType ObjectType
        {
            get => _objectType;
            set
            {
                if (value == _objectType) return;
                _objectType = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the columns excluded from reverse engineering.
        /// </summary>
        [DataMember]
        public IEnumerable<string> ExcludedColumns
        {
            get => _excludedColumns;
            set
            {
                if (value == _excludedColumns) return;
                _excludedColumns = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableInformationModel"/> class for a specific table.
        /// </summary>
        /// <param name="name">The table name.</param>
        /// <param name="hasPrimaryKey">Whether or not a primary key exists for the table.</param>
        /// <param name="showKeylessWarning">Show warning that the table or view is keyless.</param>
        /// <exception cref="ArgumentException"><paramref name="schema"/> or <paramref name="name"/> are null or only white spaces.</exception>
        public TableInformationModel(string name,
                                     bool hasPrimaryKey,
                                     bool showKeylessWarning = false,
                                     ObjectType objectType = ObjectType.Table,
                                     IEnumerable<string> excludedColumns = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(@"Value cannot be empty or only white spaces.", nameof(name));

            Name = name;
            HasPrimaryKey = hasPrimaryKey;
            ShowKeylessWarning = showKeylessWarning;
            ObjectType = objectType;
            if (excludedColumns != null) ExcludedColumns = excludedColumns;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}