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
        public string _text;
        public bool? _isSelected = false;
        public ObjectType _objectType;
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
            get => _isSelected;
            private set
            {
                if (Equals(value, _isSelected)) return;
                _isSelected = value;
                RaisePropertyChanged();
                if (_isSelected != null)
                {
                    var selected = _isSelected.Value;
                    foreach (var item in Schemas)
                    {
                        item.SetSelectedCommand.Execute(selected);
                    }
                }
            }
        }

        public string Text
        {
            get => _text;
            set
            {
                if (Equals(value, _text)) return;
                _text = value;
                RaisePropertyChanged();
            }
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
            get => (ObjectTypeIcon)Enum.Parse(typeof(ObjectTypeIcon), ObjectType.ToString());
        }

        public ObservableCollection<ISchemaInformationViewModel> Schemas { get; } = new ObservableCollection<ISchemaInformationViewModel>();

        public ICommand SetSelectedCommand { get; }

        private void ObjectPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ISchemaInformationViewModel.IsSelected))
            {
                if (Schemas.All(m => m.IsSelected.GetValueOrDefault()))
                {
                    _isSelected = true;
                    RaisePropertyChanged(nameof(IsSelected));
                }
                else if (Schemas.All(m => m.IsSelected.HasValue && !m.IsSelected.Value))
                {
                    _isSelected = false;
                    RaisePropertyChanged(nameof(IsSelected));
                }
                else
                {
                    _isSelected = null;
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