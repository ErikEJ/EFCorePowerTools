﻿// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using EFCorePowerTools.Contracts.ViewModels;
using EFCorePowerTools.Contracts.Views;
using EFCorePowerTools.Contracts.Wizard;
using GalaSoft.MvvmLight.Command;
using RevEng.Common;

namespace EFCorePowerTools.Wizard
{
    public partial class WizardPage2 : WizardResultPageFunction, IPickTablesDialog
    {
        private readonly IWizardView wizardView;
        // private readonly WizardDataViewModel wizardViewModel;
        private readonly Func<SerializationTableModel[]> getDialogResult;
        private readonly Func<Schema[]> getReplacerResult;
        private readonly Action<IEnumerable<TableModel>, IEnumerable<Schema>> addTables;
        private readonly Action<IEnumerable<SerializationTableModel>> selectTables;
        private bool sqliteToolboxInstalled;

        public WizardPage2(WizardDataViewModel viewModel, IWizardView wizardView)
            : base(viewModel, wizardView)
        {
            // telemetryAccess.TrackPageView(nameof(PickTablesDialog));

            DataContext = viewModel;
            getDialogResult = viewModel.GetSelectedObjects;
            getReplacerResult = viewModel.GetRenamedObjects;
            addTables = viewModel.AddObjects;
            selectTables = viewModel.SelectObjects;

            InitializeComponent();

            this.wizardView = wizardView;
            // this.wizardViewModel = viewModel;
            viewModel.Page2LoadedCommand = new RelayCommand(Page2Loaded_Executed);
        }

        public (bool ClosedByOK, PickTablesDialogResult Payload) ShowAndAwaitUserResponse(bool modal)
        {
            if (modal)
            {
                Debug.WriteLine("Modal");
            }
            else
            {
                Debug.WriteLine("Not modal");
            }

            // return (closedByOkay, new PickTablesDialogResult { Objects = getDialogResult(), CustomReplacers = getReplacerResult() });
            return (false, new PickTablesDialogResult());
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
                Objects = getDialogResult(),
                CustomReplacers = getReplacerResult(),
            };
        }

        private void Page2Loaded_Executed()
        {
            // var project = wizardViewModel.Project;
            // var optionsPath = wizardViewModel.OptionsPath;
            // var onlyGenerate = wizardViewModel.OnlyGenerate;
            // var fromSqlProject = wizardViewModel.FromSqlProject;
            // var uiHint = wizardViewModel.UiHint;
            // wizardViewModel.Bll.PickDatabaseTablesAsync(project, optionsPath, onlyGenerate, fromSqlProject, uiHint, this);
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            // Go to next wizard page
            var wizardPage3 = new WizardPage3((WizardDataViewModel)DataContext, wizardView);
            wizardPage3.Return += WizardPage_Return;
            NavigationService?.Navigate(wizardPage3);
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
