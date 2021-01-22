namespace EFCorePowerTools.Dialogs
{
    using Contracts.ViewModels;
    using Contracts.Views;
    using RevEng.Shared;
    using Shared.DAL;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public partial class PickSchemasDialog : IPickSchemasDialog
    {
        private readonly Func<SchemaInfo[]> _getDialogResult;
        private readonly Action<IEnumerable<SchemaInfo>> _addSchemas;

        public PickSchemasDialog(ITelemetryAccess telemetryAccess,
                                 IPickSchemasViewModel viewModel)
        {
            telemetryAccess.TrackPageView(nameof(PickSchemasDialog));

            DataContext = viewModel;
            viewModel.CloseRequested += (sender, args) =>
            {
                DialogResult = args.DialogResult;
                Close();
            };
            _getDialogResult = () => (viewModel.Schemas.ToArray());
            _addSchemas = models =>
            {
                foreach (var model in models)
                    viewModel.Schemas.Add(model);
            };

            InitializeComponent();
        }

        (bool ClosedByOK, SchemaInfo[] Payload) IDialog<SchemaInfo[]>.ShowAndAwaitUserResponse(bool modal)
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
        
        void IPickSchemasDialog.AddSchemas(IEnumerable<SchemaInfo> schemas)
        {
            _addSchemas(schemas);
        }
    }
}