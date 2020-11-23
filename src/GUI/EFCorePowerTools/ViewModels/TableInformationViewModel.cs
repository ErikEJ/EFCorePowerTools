namespace EFCorePowerTools.ViewModels
{
    using Contracts.ViewModels;
    using GalaSoft.MvvmLight;
    using RevEng.Shared;
    using System.Collections.ObjectModel;

    public class TableInformationViewModel : ViewModelBase, ITableInformationViewModel
    {
        private string _name;
        private bool _hasPrimaryKey;
        private ObjectType _objectType;

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

        public bool HasPrimaryKey
        {
            get => _hasPrimaryKey;
            set
            {
                if (Equals(value, _hasPrimaryKey)) return;
                _hasPrimaryKey = value;
                RaisePropertyChanged();
            }
        }

        public bool ShowKeylessWarning
        {
            get => !HasPrimaryKey && ObjectType == ObjectType.Table;
        }

        public bool IsTable
        {
            get => HasPrimaryKey && ObjectType == ObjectType.Table;
        }

        public ObjectType ObjectType
        {
            get => _objectType;
            set
            {
                if (Equals(value, _objectType)) return;
                _objectType = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<IColumnInformationViewModel> Columns { get; } = new ObservableCollection<IColumnInformationViewModel>();

        public bool IsProcedure => ObjectType == ObjectType.Procedure;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (Equals(value, _isSelected)) return;
                _isSelected = value;
                foreach(var column in Columns)
                {
                    column.IsSelected = _isSelected;
                }
                RaisePropertyChanged();
            }
        }

    }
}