﻿using System;
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
            }
        }

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

        private bool Ok_CanExecute() => Schemas.Any();

        private void Cancel_Executed()
        {
            CloseRequested?.Invoke(this, new CloseRequestedEventArgs(false));
        }

        private void Add_Executed()
        {
            Schemas.Add(new SchemaInfo());
        }

        private void Remove_Executed()
        {
            Schemas.Remove(SelectedSchema);
            SelectedSchema = null;
        }

        private bool Remove_CanExecute() => SelectedSchema != null;
    }
}
