namespace EFCorePowerTools.Dialogs
{
    using Contracts.ViewModels;
    using Contracts.Views;
    using RevEng.Shared;
    using Shared.DAL;
    using System;
    using System.Collections.Generic;
    using System.Windows.Controls;
    using System.Windows.Input;

    public partial class PickTablesDialog : IPickTablesDialog
    {
        private readonly Func<SerializationTableModel[]> _getDialogResult;
        private readonly Func<Schema[]> _getReplacerResult;
        private readonly Action<IEnumerable<TableModel>, IEnumerable<Schema>> _addTables;
        private readonly Action<IEnumerable<SerializationTableModel>> _selectTables;

        public PickTablesDialog(ITelemetryAccess telemetryAccess,
                                IPickTablesViewModel viewModel)
        {
            telemetryAccess.TrackPageView(nameof(PickTablesDialog));

            DataContext = viewModel;
            viewModel.CloseRequested += (sender, args) =>
            {
                DialogResult = args.DialogResult;
                Close();
            };
            _getDialogResult = viewModel.GetSelectedObjects;
            _getReplacerResult = viewModel.GetRenamedObjects;
            _addTables = viewModel.AddObjects;
            _selectTables = viewModel.SelectObjects;

            InitializeComponent();
        }

        (bool ClosedByOK, PickTablesDialogResult Payload) IDialog<PickTablesDialogResult>.ShowAndAwaitUserResponse(bool modal)
        {
            bool closedByOkay;

            if (modal)
            {
                closedByOkay = ShowModal() == true;
            }
            else
            {
                closedByOkay = ShowDialog() == true;
            }

            return (closedByOkay, new PickTablesDialogResult { Objects = _getDialogResult(), CustomReplacers = _getReplacerResult() });
        }

        IPickTablesDialog IPickTablesDialog.AddTables(IEnumerable<TableModel> tables, IEnumerable<Schema> customReplacers)
        {
            _addTables(tables, customReplacers);
            return this;
        }

        IPickTablesDialog IPickTablesDialog.PreselectTables(IEnumerable<SerializationTableModel> tables)
        {
            _selectTables(tables);
            return this;
        }

        private void CheckBox_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            
            if (checkBox.IsChecked == false)
            {
                statusBar.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void TreeTextRenamer_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                ((IObjectTreeEditableViewModel)((TextBox)sender).DataContext).CancelEditCommand.Execute(null);
            }
            else if (e.Key == Key.Return)
            {
                ((IObjectTreeEditableViewModel)((TextBox)sender).DataContext).ConfirmEditCommand.Execute(null);
            }
            e.Handled = true;
        }

        private void DialogWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool isInEditMode = ((IPickTablesViewModel)tree.DataContext).ObjectTree.IsInEditMode;
            if (isInEditMode)
            {
                e.Cancel = true;
            }
        }

        private void tree_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.F2)
            {
                if (tree.SelectedItem is IColumnInformationViewModel cvm && cvm.IsTableSelected)
                {
                    cvm.StartEditCommand.Execute(null);
                }
                else if (tree.SelectedItem is ITableInformationViewModel tvm)
                {
                    tvm.StartEditCommand.Execute(null);
                }
            }
            else if (e.Key == Key.Space)
            {
                var vm = (IObjectTreeSelectableViewModel)tree.SelectedItem;
                vm.SetSelectedCommand.Execute(vm.IsSelected == null ? false : !vm.IsSelected);
                e.Handled = true;
            }
        }
    }
}
