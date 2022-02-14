namespace EFCorePowerTools.ViewModels
{
    using Contracts.ViewModels;
    using EFCorePowerTools.Locales;
    using EFCorePowerTools.Messages;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;
    using GalaSoft.MvvmLight.Messaging;
    using System;
    using System.Windows.Input;

    public class ColumnInformationViewModel : ViewModelBase, IColumnInformationViewModel
    {
        private string _name;
        private string _newName;

        private bool _isPrimaryKey;
        private bool _isForeignKey;

        private bool _isTableSelected;
        private bool _isSelected;
        private bool _isEditing;

        private readonly IMessenger _messenger;

        public ColumnInformationViewModel(IMessenger messenger)
        {
            _messenger = messenger;
            StartEditCommand = new RelayCommand(StartEdit_Execute);
            ConfirmEditCommand = new RelayCommand(ConfirmEdit_Execute);
            CancelEditCommand = new RelayCommand(CancelEdit_Execute);
            SetSelectedCommand = new RelayCommand<bool>(SetSelected_Execute);
        }

        public string Name
        {
            get => _name;
            set
            {
                if (Equals(value, _name)) return;
                _name = value;
                RaisePropertyChanged();
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
            get
            {
                return _newName == _name ? _name : $"{_newName} - ({_name})";
            }
        }

        public bool IsPrimaryKey
        {
            get => _isPrimaryKey;
            set
            {
                if (Equals(value, _isPrimaryKey)) return;
                _isPrimaryKey = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IsEnabled));
            }
        }

        public bool IsForeignKey
        {
            get => _isForeignKey;
            set
            {
                if (Equals(value, _isForeignKey)) return;
                _isForeignKey = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IsEnabled));
            }
        }

        public bool IsColumn
        {
            get => !_isPrimaryKey;
        }

        public bool? IsSelected
        {
            get => _isSelected;
            private set
            {
                if (Equals(value, _isSelected)) return;
                _isSelected = value.Value;
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

        public bool IsTableSelected
        {
            get => _isTableSelected;
            set
            {
                if (Equals(value, _isTableSelected)) return;
                _isTableSelected = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IsEnabled));
            }
        }

        public bool IsEnabled { get => IsTableSelected && !IsPrimaryKey && !IsForeignKey; }

        public ICommand StartEditCommand { get; }

        public ICommand ConfirmEditCommand { get; }

        public ICommand CancelEditCommand { get; }

        public ICommand SetSelectedCommand { get; }

        //This method must be invoked only by parent view models because it skips enabled control. 
        //UI must call command
        public void SetSelected(bool value)
        {
            IsSelected = value;
        }

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

        private void SetSelected_Execute(bool value)
        {
            if (IsEnabled)
            {
                IsSelected = value;
            }
        }
    }
}