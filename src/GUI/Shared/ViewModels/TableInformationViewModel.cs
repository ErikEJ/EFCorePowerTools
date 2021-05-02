namespace EFCorePowerTools.ViewModels
{
    using Contracts.ViewModels;
    using EFCorePowerTools.Locales;
    using EFCorePowerTools.Messages;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;
    using GalaSoft.MvvmLight.Messaging;
    using RevEng.Shared;
    using System;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Windows.Input;

    public class TableInformationViewModel : ViewModelBase, ITableInformationViewModel
    {
        private string _name;
        private string _newName;
        private string _schema;
        private ObjectType _objectType;

        private bool _isSelected = false;
        private bool _isEditing;

        private bool _isVisible = true;

        private readonly IMessenger _messenger;

        public TableInformationViewModel(IMessenger messenger)
        {
            _messenger = messenger;
            StartEditCommand = new RelayCommand(StartEdit_Execute);
            ConfirmEditCommand = new RelayCommand(ConfirmEdit_Execute);
            CancelEditCommand = new RelayCommand(CancelEdit_Execute);
            SetSelectedCommand = new RelayCommand<bool>(SetSelected_Execute);
            Columns.CollectionChanged += Columns_CollectionChanged;
        }

        public string Schema
        {
            get => _schema;
            set
            {
                if (Equals(value, _schema)) return;
                _schema = value;
                RaisePropertyChanged();
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (Equals(value, _name)) return;
                _name = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(DisplayName));
            }
        }

        public string NewName
        {
            get => _newName;
            set
            {
                if (Equals(value, _newName)) return;
                _newName = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(DisplayName));
            }
        }

        public string DisplayName
        {
            get {
                return _newName == _name ? _name : $"{_newName} - ({_name})";
            }
        }

        public string ModelDisplayName { get; set; }

        public bool HasPrimaryKey
        {
            get => Columns.Any(c => c.IsPrimaryKey);
        }

        public ObjectType ObjectType
        {
            get => _objectType;
            set
            {
                if (Equals(value, _objectType)) return;
                _objectType = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(ObjectTypeIcon));
            }
        }

        public ObjectTypeIcon ObjectTypeIcon
        {
            get
            {
                if (ObjectType == ObjectType.Table && !HasPrimaryKey)
                {
                    return ObjectTypeIcon.TableWithoutKey;
                }
                else 
                {
                    return (ObjectTypeIcon)Enum.Parse(typeof(ObjectTypeIcon), ObjectType.ToString());
                }
            }
        }

        public ObservableCollection<IColumnInformationViewModel> Columns { get; } = new ObservableCollection<IColumnInformationViewModel>();

        public bool? IsSelected
        {
            get => _isSelected;
            private set
            {
                if (Equals(value, _isSelected)) return;
                _isSelected = value.Value;
                RaisePropertyChanged();
                foreach (var column in Columns)
                {
                    column.IsTableSelected = _isSelected;
                    column.SetSelected(_isSelected);
                }
            }
        }

        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                if (Equals(value, _isVisible)) return;
                _isVisible = value;
                RaisePropertyChanged();
            }
        }

        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                if (Equals(value, _isEditing)) return;
                _isEditing = value;
                RaisePropertyChanged();
            }
        }

        public ICommand StartEditCommand { get; }

        public ICommand ConfirmEditCommand { get; }

        public ICommand CancelEditCommand { get; }

        public ICommand SetSelectedCommand { get; }

        private void StartEdit_Execute()
        {
            IsEditing = true;
        }

        private void ConfirmEdit_Execute()
        {
            if (String.IsNullOrWhiteSpace(NewName))
            {
                _messenger.Send(new ShowMessageBoxMessage
                {
                    Content = ReverseEngineerLocale.NewNameCannotBeEmpty
                });
            }
            else
            {
                IsEditing = false;
            }
        }

        private void CancelEdit_Execute()
        {
            NewName = Name;
            IsEditing = false;
        }

        private void Columns_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (var item in e.NewItems)
            {
                RaisePropertyChanged(nameof(HasPrimaryKey));
            }
        }

        private void SetSelected_Execute(bool value)
        {
            IsSelected = value;
        }
    }
}