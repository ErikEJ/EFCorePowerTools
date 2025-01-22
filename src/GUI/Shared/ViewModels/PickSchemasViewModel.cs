using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using EFCorePowerTools.Contracts.EventArgs;
using EFCorePowerTools.Contracts.ViewModels;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using RevEng.Common;

namespace EFCorePowerTools.ViewModels
{
    public class PickSchemasViewModel : ViewModelBase, IPickSchemasViewModel
    {
        private SchemaInfo selectedSchema;

        public PickSchemasViewModel()
        {
            OkCommand = new RelayCommand(Ok_Executed, Ok_CanExecute);
            CancelCommand = new RelayCommand(Cancel_Executed);
            AddCommand = new RelayCommand(Add_Executed);
            RemoveCommand = new RelayCommand(Remove_Executed, Remove_CanExecute);

            Schemas = new ObservableCollection<SchemaInfo>();
        }

        public event EventHandler<CloseRequestedEventArgs> CloseRequested;

        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand RemoveCommand { get; }

        public ObservableCollection<SchemaInfo> Schemas { get; }

        public SchemaInfo SelectedSchema
        {
            get => selectedSchema;
            set
            {
                if (value == selectedSchema)
                {
                    return;
                }

                selectedSchema = value;
                RaisePropertyChanged();
                IsSelected = selectedSchema != null;
            }
        }

        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
                RaisePropertyChanged();
            }
        }

#pragma warning disable SA1201 // Elements should appear in the correct order
        private bool isSelected;
#pragma warning restore SA1201 // Elements should appear in the correct order
        private bool isRemovedData;

        private void Ok_Executed()
        {
            // remove empty schemas
            List<SchemaInfo> schemasToRemove = Schemas.Where(s => string.IsNullOrEmpty(s.Name)).ToList();
            foreach (SchemaInfo schemaInfo in schemasToRemove)
            {
                Schemas.Remove(schemaInfo);
            }

            CloseRequested?.Invoke(this, new CloseRequestedEventArgs(true));
        }

        private bool Ok_CanExecute() => Schemas.Any() || isRemovedData;

        private void Cancel_Executed()
        {
            CloseRequested?.Invoke(this, new CloseRequestedEventArgs(false));
        }

        private void Add_Executed()
        {
            var newRecord = new SchemaInfo();
            SelectedSchema = newRecord;
            Schemas.Add(newRecord);
        }

        private void Remove_Executed()
        {
            Schemas.Remove(SelectedSchema);
            SelectedSchema = null;
            isRemovedData = true;
        }

        private bool Remove_CanExecute() => SelectedSchema != null;
    }
}
