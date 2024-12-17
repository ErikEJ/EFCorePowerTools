// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using EFCorePowerTools.Contracts.ViewModels;
using EFCorePowerTools.Contracts.Views;
using EFCorePowerTools.Contracts.Wizard;
using EFCorePowerTools.Locales;
using EFCorePowerTools.Messages;
using EFCorePowerTools.ViewModels;
using Microsoft.VisualStudio.Shell;
using RevEng.Common;

namespace EFCorePowerTools.Wizard
{
    public partial class Wiz2_PickTablesDialog : WizardResultPageFunction, IPickTablesDialog
    {
        private readonly IWizardView wizardView;
        private readonly WizardDataViewModel wizardViewModel;
        private readonly Func<SerializationTableModel[]> getDialogResultPg2;
        private readonly Func<Schema[]> getReplacerResult;
        private readonly Action<IEnumerable<TableModel>, IEnumerable<Schema>> addTables;
        private readonly Action<IEnumerable<SerializationTableModel>> selectTables;
        private bool sqliteToolboxInstalled;

        public Wiz2_PickTablesDialog(WizardDataViewModel viewModel, IWizardView wizardView)
            : base(viewModel, wizardView)
        {
            // telemetryAccess.TrackPageView(nameof(PickTablesDialog));
            DataContext = viewModel;
            this.wizardView = wizardView;
            this.wizardViewModel = viewModel;

            getDialogResultPg2 = viewModel.GetSelectedObjects;
            getReplacerResult = viewModel.GetRenamedObjects;
            addTables = viewModel.AddObjects;
            selectTables = viewModel.SelectObjects;

            InitializeComponent();
            InitializeMessengerWithStatusbar(Statusbar, ReverseEngineerLocale.LoadingDatabaseObjects);
        }

        protected override void OnPageVisible(object sender, StatusbarEventArgs e)
        {
            var viewModel = wizardViewModel;
            IsPageLoaded = viewModel.IsPage2Initialized;

            if (!IsPageLoaded && !wizardViewModel.GetSelectedObjects().Any())
            {
                var wea = wizardViewModel.WizardEventArgs;
                wea.PickTablesDialog = this;

                Messenger.Send(new ShowStatusbarMessage(ReverseEngineerLocale.LoadingDatabaseObjects));

                ThreadHelper.JoinableTaskFactory.Run(async () =>
                {
                    await wizardViewModel.Bll.LoadDataBaseObjectsAsync(wea.Options, wea.DbInfo, wea.NamingOptionsAndPath, wea);
                });
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            // Go to next wizard page
            if (wizardViewModel.IsPage2Initialized)
            {
                NavigationService.GoForward();
            }
            else
            {
                wizardViewModel.IsPage2Initialized = true;
                var wizardPage3 = new Wiz3_EfCoreModelDialog((WizardDataViewModel)DataContext, wizardView);
                wizardPage3.Return += WizardPage_Return;
                NavigationService?.Navigate(wizardPage3);
            }
        }

        public (bool ClosedByOK, PickTablesDialogResult Payload) ShowAndAwaitUserResponse(bool modal)
        {
            wizardViewModel.WizardEventArgs.PickTablesDialogComplete = true;
            return (true, new PickTablesDialogResult { Objects = getDialogResultPg2(), CustomReplacers = getReplacerResult() });
        }

        public IPickTablesDialog AddTables(IEnumerable<TableModel> tables, IEnumerable<Schema> customReplacers)
        {
            addTables(tables, customReplacers);
            return this;
        }

        public IPickTablesDialog PreselectTables(IEnumerable<SerializationTableModel> tables)
        {
            selectTables(tables);
            return this;
        }

        public IPickTablesDialog SqliteToolboxInstall(bool installed)
        {
            sqliteToolboxInstalled = installed;
            return this;
        }

        public PickTablesDialogResult GetResults()
        {
            return new PickTablesDialogResult
            {
                Objects = getDialogResultPg2(),
                CustomReplacers = getReplacerResult(),
            };
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

        private void Tree_KeyUp(object sender, KeyEventArgs e)
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

        private void SolutionTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (tree.SelectedItem is ITableInformationViewModel
                || (tree.SelectedItem is IColumnInformationViewModel cvm && cvm.IsTableSelected))
            {
                tree.ContextMenu = tree.Resources["RenamePopup"] as ContextMenu;
            }
            else
            {
                tree.ContextMenu = null;
            }
        }

        private void TreeNodeContextMenuItem_Click(object sender, RoutedEventArgs e)
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

        private void SqliteToolboxLink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo(e.Uri.AbsoluteUri),
            };
            process.Start();
        }

        private void DialogWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            WindowTitle = ReverseEngineerLocale.ChooseDatabaseObjects;

            if (!sqliteToolboxInstalled)
            {
                try
                {
                    SqliteToolboxLink.Inlines.Add(new Run("Install SQLite Toolbox"));
                }
                catch
                {
                    // Ignore
                }
            }
        }
    }
}
