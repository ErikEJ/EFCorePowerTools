namespace EFCorePowerTools.ViewModels
{
    using Contracts.ViewModels;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;
    using RevEng.Shared;
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Input;

    public class ObjectTreeRootItemViewModel : ViewModelBase, IObjectTreeRootItemViewModel
    {
        private string text1;
        private bool? isSelected1 = false;
        private ObjectType objectType1;
        public ObjectTreeRootItemViewModel()
        {
            SetSelectedCommand = new RelayCommand<bool>(SetSelected_Execute);
            Schemas.CollectionChanged += (s, e) =>
            {
                foreach (ISchemaInformationViewModel item in e.NewItems)
                    item.PropertyChanged += ObjectPropertyChanged;
            };
        }

        public bool IsVisible
        {
            get => Schemas.Any(m => m.IsVisible);
        }

        public bool? IsSelected
        {
            get => IsSelected1;
            private set
            {
                if (Equals(value, IsSelected1)) return;
                IsSelected1 = value;
                RaisePropertyChanged();
                if (IsSelected1 != null)
                {
                    var selected = IsSelected1.Value;
                    foreach (var item in Schemas)
                    {
                        item.SetSelectedCommand.Execute(selected);
                    }
                }
            }
        }

        public string Text
        {
            get => Text1;
            set
            {
                if (Equals(value, Text1)) return;
                Text1 = value;
                RaisePropertyChanged();
            }
        }

        public ObjectType ObjectType
        {
            get => ObjectType1;
            set
            {
                if (Equals(value, ObjectType1)) return;
                ObjectType1 = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(ObjectTypeIcon));
            }
        }

        public ObjectTypeIcon ObjectTypeIcon
        {
            get => (ObjectTypeIcon)Enum.Parse(typeof(ObjectTypeIcon), ObjectType.ToString());
        }

        public ObservableCollection<ISchemaInformationViewModel> Schemas { get; } = new ObservableCollection<ISchemaInformationViewModel>();

        public ICommand SetSelectedCommand { get; }
        public ObjectType ObjectType1 { get => objectType1; set => objectType1 = value; }
        public bool? IsSelected1 { get => isSelected1; set => isSelected1 = value; }
        public string Text1 { get => text1; set => text1 = value; }

        private void ObjectPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ISchemaInformationViewModel.IsSelected))
            {
                if (Schemas.All(m => m.IsSelected.GetValueOrDefault()))
                {
                    IsSelected1 = true;
                    RaisePropertyChanged(nameof(IsSelected));
                }
                else if (Schemas.All(m => m.IsSelected.HasValue && !m.IsSelected.Value))
                {
                    IsSelected1 = false;
                    RaisePropertyChanged(nameof(IsSelected));
                }
                else
                {
                    IsSelected1 = null;
                    RaisePropertyChanged(nameof(IsSelected));
                }
            }

            if (e.PropertyName == nameof(ISchemaInformationViewModel.IsVisible))
            {
                RaisePropertyChanged(nameof(IsVisible));
            }

        }

        private void SetSelected_Execute(bool value)
        {
            IsSelected = value;
        }
    }
}