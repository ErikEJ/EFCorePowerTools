namespace EFCorePowerTools.ViewModels
{
    using Contracts.ViewModels;
    using GalaSoft.MvvmLight;

    public class ColumnInformationViewModel : ViewModelBase, IColumnInformationViewModel
    {
        private string _name;

        private bool _isSelected;

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

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (Equals(value, _isSelected)) return;
                _isSelected = value;
                RaisePropertyChanged();
            }
        }
    }
}