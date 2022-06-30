using System;
using System.Windows.Input;
using EFCorePowerTools.Contracts.ViewModels;
using EFCorePowerTools.Locales;
using EFCorePowerTools.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;

namespace EFCorePowerTools.ViewModels
{
    public class ColumnInformationViewModel : ViewModelBase, IColumnInformationViewModel
    {
        private string name;
        private string newName;

        private bool isPrimaryKey;
        private bool isForeignKey;

        private bool isTableSelected;
        private bool isSelected;
        private bool isEditing;

        private readonly IMessenger messenger;

        public ColumnInformationViewModel(IMessenger messenger)
        {
            this.messenger = messenger;
            StartEditCommand = new RelayCommand(StartEdit_Execute);
            ConfirmEditCommand = new RelayCommand(ConfirmEdit_Execute);
            CancelEditCommand = new RelayCommand(CancelEdit_Execute);
            SetSelectedCommand = new RelayCommand<bool>(SetSelected_Execute);
        }

        public string Name
        {
            get => name;
            set
            {
                if (Equals(value, name))
                {
                    return;
                }

                name = value;
                RaisePropertyChanged();
            }
        }

        public string NewName
        {
            get => newName;
            set
            {
                if (Equals(value, newName))
                {
                    return;
                }

                newName = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(DisplayName));
            }
        }

        public string DisplayName
        {
            get
            {
                return newName == name ? name : $"{newName} - ({name})";
            }
        }

        public bool IsPrimaryKey
        {
            get => isPrimaryKey;
            set
            {
                if (Equals(value, isPrimaryKey))
                {
                    return;
                }

                isPrimaryKey = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IsEnabled));
            }
        }

        public bool IsForeignKey
        {
            get => isForeignKey;
            set
            {
                if (Equals(value, isForeignKey))
                {
                    return;
                }

                isForeignKey = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IsEnabled));
            }
        }

        public bool IsColumn
        {
            get => !isPrimaryKey;
        }

        public bool? IsSelected
        {
            get => isSelected;
            private set
            {
                if (Equals(value, isSelected))
                {
                    return;
                }

                isSelected = value.Value;
                RaisePropertyChanged();
            }
        }

        public bool IsEditing
        {
            get => isEditing;
            set
            {
                if (Equals(value, isEditing))
                {
                    return;
                }

                isEditing = value;
                RaisePropertyChanged();
            }
        }

        public bool IsTableSelected
        {
            get => isTableSelected;
            set
            {
                if (Equals(value, isTableSelected))
                {
                    return;
                }

                isTableSelected = value;
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
                messenger.Send(new ShowMessageBoxMessage
                {
                    Content = ReverseEngineerLocale.NewNameCannotBeEmpty,
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