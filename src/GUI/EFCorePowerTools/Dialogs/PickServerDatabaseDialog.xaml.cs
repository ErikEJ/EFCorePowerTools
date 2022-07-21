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
        private readonly Func<(DatabaseConnectionModel Connection, DatabaseDefinitionModel Definition, CodeGenerationMode CodeGenerationMode, bool FilterSchemas, SchemaInfo[] Schemas, string UiHint)> getDialogResult;
        private readonly Action<IEnumerable<DatabaseConnectionModel>> addConnections;
        private readonly Action<IEnumerable<DatabaseDefinitionModel>> addDefinitions;
        private readonly Action<IEnumerable<SchemaInfo>> addSchemas;
        private readonly Action<CodeGenerationMode> codeGeneration;
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
            getDialogResult = () => (viewModel.SelectedDatabaseConnection, viewModel.SelectedDatabaseDefinition, (CodeGenerationMode)viewModel.CodeGenerationMode, viewModel.FilterSchemas, viewModel.Schemas.ToArray(), viewModel.UiHint);
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
                    viewModel.DatabaseDefinitions.Add(model);
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
            codeGeneration = codeGeneration =>
            {
                viewModel.CodeGenerationMode = (int)codeGeneration;
            };

            uiHint = uiHint =>
            {
                viewModel.UiHint = uiHint;
            };

            InitializeComponent();
        }

        public (bool ClosedByOK, (DatabaseConnectionModel Connection, DatabaseDefinitionModel Definition, CodeGenerationMode CodeGenerationMode, bool FilterSchemas, SchemaInfo[] Schemas, string UiHint) Payload) ShowAndAwaitUserResponse(bool modal)
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

        void IPickServerDatabaseDialog.PublishDefinitions(IEnumerable<DatabaseDefinitionModel> definitions)
        {
            addDefinitions(definitions);
        }

        public void PublishSchemas(IEnumerable<SchemaInfo> schemas)
        {
            addSchemas(schemas);
        }

        public void PublishCodeGenerationMode(CodeGenerationMode codeGenerationMode)
        {
            codeGeneration(codeGenerationMode);
        }

        public void PublishUiHint(string uiHint)
        {
            this.uiHint(uiHint);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ReleaseNotesLink.Inlines.Add(new Run(typeof(EFCorePowerToolsPackage).Assembly.GetName().Version.ToString(3)));
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
