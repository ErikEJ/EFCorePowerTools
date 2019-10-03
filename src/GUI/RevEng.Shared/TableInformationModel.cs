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
    [DebuggerDisplay("{" + nameof(Name) + ",nq}")]
    public class TableInformationModel : INotifyPropertyChanged
    {
        private string _name;
        private bool _hasPrimaryKey;

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
        /// Initializes a new instance of the <see cref="TableInformationModel"/> class for a specific table.
        /// </summary>
        /// <param name="name">The table name.</param>
        /// <param name="hasPrimaryKey">Whether or not a primary key exists for the table.</param>
        /// <exception cref="ArgumentException"><paramref name="schema"/> or <paramref name="name"/> are null or only white spaces.</exception>
        public TableInformationModel(string name,
                                     bool hasPrimaryKey)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(@"Value cannot be empty or only white spaces.", nameof(name));

            Name = name;
            HasPrimaryKey = hasPrimaryKey;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}