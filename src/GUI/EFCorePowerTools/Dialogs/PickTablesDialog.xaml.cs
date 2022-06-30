using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using EFCorePowerTools.Common.DAL;
using EFCorePowerTools.Contracts.ViewModels;
using EFCorePowerTools.Contracts.Views;
using RevEng.Common;

namespace EFCorePowerTools.Dialogs
{
    public partial class PickTablesDialog : IPickTablesDialog
    {
        private readonly Func<SerializationTableModel[]> getDialogResult;
        private readonly Func<Schema[]> getReplacerResult;
        private readonly Action<IEnumerable<TableModel>, IEnumerable<Schema>> addTables;
        private readonly Action<IEnumerable<SerializationTableModel>> selectTables;

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
            getDialogResult = viewModel.GetSelectedObjects;
            getReplacerResult = viewModel.GetRenamedObjects;
            addTables = viewModel.AddObjects;
            selectTables = viewModel.SelectObjects;

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

            return (closedByOkay, new PickTablesDialogResult { Objects = getDialogResult(), CustomReplacers = getReplacerResult() });
        }

        IPickTablesDialog IPickTablesDialog.AddTables(IEnumerable<TableModel> tables, IEnumerable<Schema> customReplacers)
        {
            addTables(tables, customReplacers);
            return this;
        }

        IPickTablesDialog IPickTablesDialog.PreselectTables(IEnumerable<SerializationTableModel> tables)
        {
            selectTables(tables);
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
            if (e.Key == Key.F2)
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
