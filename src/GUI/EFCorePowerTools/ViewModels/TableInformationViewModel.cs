namespace EFCorePowerTools.ViewModels
{
    using Contracts.ViewModels;
    using GalaSoft.MvvmLight;
    using Shared.Models;

    public class TableInformationViewModel : ViewModelBase, ITableInformationViewModel
    {
        private TableInformationModel _model;
        private bool _isSelected;

        public TableInformationModel Model
        {
            get => _model;
            set
            {
                if (Equals(value, _model)) return;
                _model = value;
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