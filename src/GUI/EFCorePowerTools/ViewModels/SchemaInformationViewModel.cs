namespace EFCorePowerTools.ViewModels
{
    using Contracts.ViewModels;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;
    using RevEng.Shared;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Input;

    public class SchemaInformationViewModel : ViewModelBase, ISchemaInformationViewModel
    {
        private string _name;
        private bool? _isSelected = false;

        public SchemaInformationViewModel()
        {
            SetSelectedCommand = new RelayCommand<bool>(SetSelected_Execute);
            Objects.CollectionChanged += (s, e) =>
            {
                foreach (ITableInformationViewModel item in e.NewItems)
                    item.PropertyChanged += ObjectPropertyChanged;
            };
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

        public ObservableCollection<ITableInformationViewModel> Objects { get; } = new ObservableCollection<ITableInformationViewModel>();

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
                    foreach (var obj in Objects)
                    {
                        obj.SetSelectedCommand.Execute(selected);
                    }

                }
            }
        }

        public bool IsVisible
        {
            get => Objects.Any(m => m.IsVisible);
        }

        public Schema ReplacingSchema { get; set; }

        public ICommand SetSelectedCommand { get; }

        private void ObjectPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ITableInformationViewModel.IsSelected))
            {
                if (Objects.All(m => m.IsSelected.Value))
                {
                    _isSelected = true;
                    RaisePropertyChanged(nameof(IsSelected));
                }
                else if (Objects.All(m => !m.IsSelected.Value))
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