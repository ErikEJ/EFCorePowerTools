using System;
using System.Collections.Generic;
using System.Linq;
using EFCorePowerTools.Common.DAL;
using EFCorePowerTools.Contracts.ViewModels;
using EFCorePowerTools.Contracts.Views;
using RevEng.Common;

namespace EFCorePowerTools.Dialogs
{
    public partial class PickSchemasDialog : IPickSchemasDialog
    {
        private readonly Func<SchemaInfo[]> getDialogResult;
        private readonly Action<IEnumerable<SchemaInfo>> addSchemas;

        public PickSchemasDialog(
            ITelemetryAccess telemetryAccess,
            IPickSchemasViewModel viewModel)
        {
            telemetryAccess.TrackPageView(nameof(PickSchemasDialog));

            DataContext = viewModel;
            viewModel.CloseRequested += (sender, args) =>
            {
                DialogResult = args.DialogResult;
                Close();
            };
            getDialogResult = () => viewModel.Schemas.ToArray();
            addSchemas = models =>
            {
                foreach (var model in models)
                {
                    viewModel.Schemas.Add(model);
                }
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

            return (closedByOkay, getDialogResult());
        }

        void IPickSchemasDialog.AddSchemas(IEnumerable<SchemaInfo> schemas)
        {
            addSchemas(schemas);
        }
    }
}