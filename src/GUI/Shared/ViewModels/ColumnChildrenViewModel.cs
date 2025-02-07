using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using EFCorePowerTools.Contracts.ViewModels;
using EFCorePowerTools.Locales;
using EFCorePowerTools.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using RevEng.Common;

namespace EFCorePowerTools.ViewModels
{
    public class ColumnChildrenViewModel : ViewModelBase, IColumnChildrenViewModel
    {
        private readonly IMessenger messenger;

        private string name;
        private string newName;
        private string schema;
        private ObjectType objectType;

        private bool isSelected = false;
        private bool isEditing;

        private bool isVisible = true;

        public ColumnChildrenViewModel(IMessenger messenger)
        {
            this.messenger = messenger;
            StartEditCommand = new RelayCommand(StartEdit_Execute);
            ConfirmEditCommand = new RelayCommand(ConfirmEdit_Execute);
            CancelEditCommand = new RelayCommand(CancelEdit_Execute);
            SetSelectedCommand = new RelayCommand<bool>(SetSelected_Execute);
            Children.CollectionChanged += Columns_CollectionChanged;
        }

        public string Schema
        {
            get => schema;
            set
            {
                if (Equals(value, schema))
                {
                    return;
                }

                schema = value;
                RaisePropertyChanged();
            }
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
                RaisePropertyChanged(nameof(DisplayName));
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

        public string ModelDisplayName { get; set; }

        public bool HasPrimaryKey
        {
            get => Columns.Any(c => c.IsPrimaryKey);
        }

        public ObjectType ObjectType
        {
            get => objectType;
            set
            {
                if (Equals(value, objectType))
                {
                    return;
                }

                objectType = value;
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
            get => isSelected;
            private set
            {
                if (Equals(value, isSelected))
                {
                    return;
                }

                isSelected = value.Value;
                RaisePropertyChanged();
                foreach (var column in Columns)
                {
                    column.IsTableSelected = isSelected;
                    column.SetSelected(isSelected);
                }
            }
        }

        public bool IsVisible
        {
            get => isVisible;
            set
            {
                if (Equals(value, isVisible))
                {
                    return;
                }

                isVisible = value;
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

        public ICommand StartEditCommand { get; }

        public ICommand ConfirmEditCommand { get; }

        public ICommand CancelEditCommand { get; }

        public ICommand SetSelectedCommand { get; }

        public ObservableCollection<IObjectTreeEditableViewModel> Children { get; set; } = new ObservableCollection<IObjectTreeEditableViewModel>();

        private void StartEdit_Execute()
        {
            IsEditing = true;
        }

        private void ConfirmEdit_Execute()
        {
            if (string.IsNullOrWhiteSpace(NewName))
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