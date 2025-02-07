using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using EFCorePowerTools.Contracts.ViewModels;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using RevEng.Common;

namespace EFCorePowerTools.ViewModels
{
    public class SchemaInformationViewModel : ViewModelBase, ISchemaInformationViewModel
    {
        private string name;
        private bool? isSelected = false;

        public SchemaInformationViewModel()
        {
            SetSelectedCommand = new RelayCommand<bool>(SetSelected_Execute);
            Objects.CollectionChanged += (s, e) =>
            {
                foreach (ITableInformationViewModel item in e.NewItems)
                {
                    item.PropertyChanged += ObjectPropertyChanged;
                }
            };
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

        public ObservableCollection<ITableInformationViewModel> Objects { get; } = new ObservableCollection<ITableInformationViewModel>();

        public bool? IsSelected
        {
            get => isSelected;
            private set
            {
                if (Equals(value, isSelected))
                {
                    return;
                }

                isSelected = value;
                RaisePropertyChanged();
                if (isSelected != null)
                {
                    var selected = isSelected.Value;
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
                    isSelected = true;
                    RaisePropertyChanged(nameof(IsSelected));
                }
                else if (Objects.All(m => !m.IsSelected.Value))
                {
                    isSelected = false;
                    RaisePropertyChanged(nameof(IsSelected));
                }
                else
                {
                    isSelected = null;
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