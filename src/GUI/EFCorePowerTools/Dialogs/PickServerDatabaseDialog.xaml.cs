namespace EFCorePowerTools.Dialogs
{
    using Contracts.ViewModels;
    using Contracts.Views;
    using RevEng.Shared;
    using Shared.DAL;
    using Shared.Models;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows;
    using System.Windows.Documents;

    public partial class PickServerDatabaseDialog : IPickServerDatabaseDialog
    {
        private readonly Func<(DatabaseConnectionModel Connection, DatabaseDefinitionModel Definition, bool IncludeViews, bool FilterSchemas, SchemaInfo[] Schemas, string UiHint)> _getDialogResult;
        private readonly Action<IEnumerable<DatabaseConnectionModel>> _addConnections;
        private readonly Action<IEnumerable<DatabaseDefinitionModel>> _addDefinitions;
        private readonly Action<IEnumerable<SchemaInfo>> _addSchemas;
        private readonly Action<bool> _useEFCore5;
        private readonly Action<string> _uiHint;

        public PickServerDatabaseDialog(ITelemetryAccess telemetryAccess,
                                        IPickServerDatabaseViewModel viewModel)
        {
            telemetryAccess.TrackPageView(nameof(PickServerDatabaseDialog));

            DataContext = viewModel;
            viewModel.CloseRequested += (sender, args) =>
            {
                DialogResult = args.DialogResult;
                Close();
            };
            _getDialogResult = () => (viewModel.SelectedDatabaseConnection, viewModel.SelectedDatabaseDefinition, viewModel.IncludeViews, viewModel.FilterSchemas, viewModel.Schemas.ToArray(), viewModel.UiHint);
            _addConnections = models =>
            {
                foreach (var model in models)
                    viewModel.DatabaseConnections.Add(model);
            };
            _addDefinitions = models =>
            {
                foreach (var model in models)
                    viewModel.DatabaseDefinitions.Add(model);
            };
            _addSchemas = models =>
            {
                viewModel.FilterSchemas = models.Any();
                foreach (var model in models)
                    viewModel.Schemas.Add(model);
            };
            _useEFCore5 = efCore5 =>
            {
                viewModel.IncludeViews = efCore5;
            };

            _uiHint = uiHint =>
            {
                viewModel.UiHint = uiHint;
            };

            InitializeComponent();
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

        public (bool ClosedByOK, (DatabaseConnectionModel Connection, DatabaseDefinitionModel Definition, bool IncludeViews, bool FilterSchemas, SchemaInfo[] Schemas, string UiHint) Payload) ShowAndAwaitUserResponse(bool modal)
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

            return (closedByOkay, _getDialogResult());
        }

        void IPickServerDatabaseDialog.PublishConnections(IEnumerable<DatabaseConnectionModel> connections)
        {
            _addConnections(connections);
        }

        void IPickServerDatabaseDialog.PublishDefinitions(IEnumerable<DatabaseDefinitionModel> definitions)
        {
            _addDefinitions(definitions);
        }

        public void PublishSchemas(IEnumerable<SchemaInfo> schemas)
        {
            _addSchemas(schemas);
        }

        public void PublishCodeGenerationMode(CodeGenerationMode codeGenerationMode)
        {
            _useEFCore5(codeGenerationMode == CodeGenerationMode.EFCore5);
        }

        public void PublishUiHint(string uiHint)
        {
            _uiHint(uiHint);
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            var hyperlink = sender as Hyperlink;
            var process = new Process
            {
                StartInfo = new ProcessStartInfo(hyperlink.NavigateUri.AbsoluteUri),
            };
            process.Start();
        }
    }
}