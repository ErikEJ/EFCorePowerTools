using EFCorePowerTools.Contracts.ViewModels;
using EFCorePowerTools.Contracts.Views;
using EFCorePowerTools.Shared.Models;
using RevEng.Shared;
using System;

namespace EFCorePowerTools.Dialogs
{
    public partial class ConnectionDialog : IPickConnectionDialog
    {
        private readonly Func<DatabaseConnectionModel> _getDialogResult;

        public ConnectionDialog(IPickConnectionViewModel viewModel)
        {
            DataContext = viewModel;
            viewModel.CloseRequested += (sender, args) =>
            {
                DialogResult = args.DialogResult;
                Close();
            };
            _getDialogResult = () => viewModel.DatabaseConnection;

            InitializeComponent();
            cmdDatabaseType.SelectedIndex = 0;
        }

        (bool ClosedByOK, DatabaseConnectionModel Payload) IDialog<DatabaseConnectionModel>.ShowAndAwaitUserResponse(bool modal)
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

        private void cmdDatabaseType_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            txtSample.Text = string.Empty;

            if (cmdDatabaseType.SelectedIndex == 0)
            {
                txtSample.Text = "Server=127.0.0.1;Port=5432;Database=myDataBase;User Id=myUsername;Password=myPassword;";
            }
            if (cmdDatabaseType.SelectedIndex == 1)
            {
                txtSample.Text = "Data Source=C:\\data\\Application.db";
            }
            if (cmdDatabaseType.SelectedIndex == 2)
            {
                txtSample.Text = "Server=myserver.database.windows.net;Authentication=Active Directory Interactive;Database=mydatabase;User Id=user@domain.com";
            }
            if (cmdDatabaseType.SelectedIndex == 3)
            {
                txtSample.Text = "database=localhost:demo.fdb;user=sysdba;password=masterkey";
            }
            if (cmdDatabaseType.SelectedIndex == 4)
            {
                txtSample.Text = "Server=myServerAddress;Database=myDataBase;Uid=myUsername;Pwd=myPassword;";
            }
            if (cmdDatabaseType.SelectedIndex == 5)
            {
                txtSample.Text = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=MyHost)(PORT=MyPort)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=MyOracleSID)));User Id=myUsername;Password=myPassword;";
            }
        }
    }
}