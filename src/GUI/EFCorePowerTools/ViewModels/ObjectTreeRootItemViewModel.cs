namespace EFCorePowerTools.ViewModels
{
    using Contracts.ViewModels;
    using GalaSoft.MvvmLight;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;

    public class ObjectTreeRootItemViewModel : ViewModelBase, IObjectTreeRootItemViewModel
    {
        public string _text;
        public bool? _isSelected = false;

        public ObjectTreeRootItemViewModel()
        {
            Objects.CollectionChanged += (s, e) =>
            {
                foreach (ITableInformationViewModel item in e.NewItems)
                    item.PropertyChanged += ObjectPropertyChanged;
            };
        }

        public bool IsVisible
        {
            get => Objects.Any(m => m.IsVisible);
        }

        public bool? IsSelected
        {
            get => _isSelected;
            set
            {
                if (Equals(value, _isSelected)) return;
                _isSelected = value;
                RaisePropertyChanged();
                if (_isSelected != null)
                {
                    var selected = _isSelected.Value;
                    foreach (var item in Objects)
                    {
                        item.IsSelected = selected;
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

        public bool IsTable => Text == "Tables";
        public bool IsView => Text == "Views";
        public bool IsProcedure => Text == "Stored procedures";
        public bool IsFunction => Text == "Functions";

        public ObservableCollection<ITableInformationViewModel> Objects { get; } = new ObservableCollection<ITableInformationViewModel>();

        private void ObjectPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ITableInformationViewModel.IsSelected))
            {
                if (Objects.All(m => m.IsSelected))
                {
                    _isSelected = true;
                    RaisePropertyChanged(nameof(IsSelected));
                }
                else if (Objects.All(m => !m.IsSelected))
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

            if (e.PropertyName == nameof(ITableInformationViewModel.IsVisible))
            {
                RaisePropertyChanged(nameof(IsVisible));
            }

        }

    }
}