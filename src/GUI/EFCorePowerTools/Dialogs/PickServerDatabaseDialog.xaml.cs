using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using EFCorePowerTools.Common.DAL;
using EFCorePowerTools.Common.Models;
using EFCorePowerTools.Contracts.ViewModels;
using EFCorePowerTools.Contracts.Views;
using RevEng.Common;

namespace EFCorePowerTools.Dialogs
{
    public partial class PickServerDatabaseDialog : IPickServerDatabaseDialog
    {
        private readonly Func<(DatabaseConnectionModel Connection, CodeGenerationMode CodeGenerationMode, bool FilterSchemas, SchemaInfo[] Schemas, string UiHint)> getDialogResult;
        private readonly Action<IEnumerable<DatabaseConnectionModel>> addConnections;
        private readonly Action<IEnumerable<DatabaseConnectionModel>> addDefinitions;
        private readonly Action<IEnumerable<SchemaInfo>> addSchemas;
        private readonly Action<CodeGenerationMode, IList<CodeGenerationItem>> codeGeneration;
        private readonly Action<string> uiHint;

        public PickServerDatabaseDialog(
            ITelemetryAccess telemetryAccess,
            IPickServerDatabaseViewModel viewModel)
        {
            telemetryAccess.TrackPageView(nameof(PickServerDatabaseDialog));

            DataContext = viewModel;
            viewModel.CloseRequested += (sender, args) =>
            {
                DialogResult = args.DialogResult;
                Close();
            };
            getDialogResult = () => (viewModel.SelectedDatabaseConnection, (CodeGenerationMode)viewModel.CodeGenerationMode, viewModel.FilterSchemas, viewModel.Schemas.ToArray(), viewModel.UiHint);
            addConnections = models =>
            {
                foreach (var model in models)
                {
                    viewModel.DatabaseConnections.Add(model);
                }
            };
            addDefinitions = models =>
            {
                foreach (var model in models)
                {
                    viewModel.DatabaseConnections.Add(model);
                }
            };
            addSchemas = models =>
            {
                viewModel.FilterSchemas = models.Any();
                foreach (var model in models)
                {
                    viewModel.Schemas.Add(model);
                }
            };
            codeGeneration = (codeGeneration, allowedVersions) =>
            {
                if (allowedVersions.Count == 1
                    && allowedVersions[0].Value == "DAB")
                {
                    grdRow1.Height = new GridLength(0);
                    grdRow2.Height = new GridLength(0);
                    grdRow3.Height = new GridLength(0);
                    viewModel.CodeGenerationMode = (int)codeGeneration;
                    return;
                }

                foreach (var item in allowedVersions)
                {
                    viewModel.CodeGenerationModeList.Add(item);
                }

                if (!allowedVersions.Any())
                {
                    grdRow1.Height = new GridLength(0);
                    grdRow2.Height = new GridLength(0);
                }

                viewModel.CodeGenerationMode = (int)codeGeneration;
            };

            uiHint = uiHint =>
            {
                viewModel.UiHint = uiHint;
            };

            InitializeComponent();
        }

        public (bool ClosedByOK, (DatabaseConnectionModel Connection, CodeGenerationMode CodeGenerationMode, bool FilterSchemas, SchemaInfo[] Schemas, string UiHint) Payload) ShowAndAwaitUserResponse(bool modal)
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

            return (closedByOkay, getDialogResult());
        }

        void IPickServerDatabaseDialog.PublishConnections(IEnumerable<DatabaseConnectionModel> connections)
        {
            addConnections(connections);
        }

        void IPickServerDatabaseDialog.PublishDefinitions(IEnumerable<DatabaseConnectionModel> definitions)
        {
            addDefinitions(definitions);
        }

        public void PublishSchemas(IEnumerable<SchemaInfo> schemas)
        {
            addSchemas(schemas);
        }

        public void PublishCodeGenerationMode(CodeGenerationMode codeGenerationMode, IList<CodeGenerationItem> allowedVersions)
        {
            codeGeneration(codeGenerationMode, allowedVersions);
        }

        public void PublishUiHint(string uiHint)
        {
            this.uiHint(uiHint);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ReleaseNotesLink.Inlines.Add(new Run(FileVersionInfo.GetVersionInfo(typeof(EFCorePowerToolsPackage).Assembly.Location).FileVersion));
            }
            catch
            {
                // Ignore
            }

            DatabaseConnectionCombobox.Focus();
        }

        private void ReleaseNotesLink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo(e.Uri.AbsoluteUri),
            };
            process.Start();
        }
    }
}
