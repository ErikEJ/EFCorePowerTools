using System;
using System.Diagnostics;
using Community.VisualStudio.Toolkit;
using EFCorePowerTools.Common.Models;
using EFCorePowerTools.Contracts.ViewModels;
using EFCorePowerTools.Contracts.Views;
using EFCorePowerTools.Helpers;
using RevEng.Common;

namespace EFCorePowerTools.Dialogs
{
    public partial class ConnectionDialog : IPickConnectionDialog
    {
        private readonly Func<DatabaseConnectionModel> getDialogResult;

        public ConnectionDialog(IPickConnectionViewModel viewModel)
        {
            DataContext = viewModel;
            viewModel.CloseRequested += (sender, args) =>
            {
                DialogResult = args.DialogResult;
                Close();
            };
            getDialogResult = () => viewModel.DatabaseConnection;

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

            return (closedByOkay, getDialogResult());
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S1075:URIs should not be hardcoded", Justification = "OK to hardcode here")]
#pragma warning disable VSTHRD100 // Avoid async void methods
        private async void CmdDatabaseType_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
#pragma warning restore VSTHRD100 // Avoid async void methods
        {
            txtSample.Text = string.Empty;
            DDEXLink.NavigateUri = null;
            DDEXLink.Inlines.Clear();

            var vsVersion = await VS.Shell.GetVsVersionAsync();

            var ddexInstalled = false;

            switch (cmdDatabaseType.SelectedIndex)
            {
                case 0:
                    ddexInstalled = await VsDataHelper.IsDdexProviderInstalledAsync(new Guid(EFCorePowerTools.Resources.NpgsqlProvider));
                    break;
                case 1:
                    ddexInstalled = await VsDataHelper.IsDdexProviderInstalledAsync(new Guid(Common.Resources.MicrosoftSQLiteProvider));
                    break;
                case 4:
                    ddexInstalled = await VsDataHelper.IsDdexProviderInstalledAsync(new Guid(EFCorePowerTools.Resources.MysqlVSProvider));
                    break;
                case 5:
                    ddexInstalled = await VsDataHelper.IsDdexProviderInstalledAsync(new Guid(EFCorePowerTools.Resources.OracleProvider));
                    break;
                default:
                    break;
            }

            if (cmdDatabaseType.SelectedIndex == 0 && !ddexInstalled)
            {
                DDEXLink.Inlines.Add(new System.Windows.Documents.Run("Install PostgreSQL DDEX provider"));
                DDEXLink.NavigateUri = new Uri("https://marketplace.visualstudio.com/items?itemName=RojanskyS.NpgsqlPostgreSQLIntegration");
                txtSample.Text = "Server=127.0.0.1;Port=5432;Database=myDataBase;User Id=myUsername;Password=myPassword;";
            }

            if (cmdDatabaseType.SelectedIndex == 1 && vsVersion.Major == 17 && !ddexInstalled)
            {
                DDEXLink.Inlines.Add(new System.Windows.Documents.Run("Install SQLite DDEX provider"));
                DDEXLink.NavigateUri = new Uri("https://marketplace.visualstudio.com/items?itemName=bricelam.VSDataSqlite");
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

            if (cmdDatabaseType.SelectedIndex == 4 && vsVersion.Major == 16 && !ddexInstalled)
            {
                DDEXLink.NavigateUri = new Uri("https://dev.mysql.com/downloads/windows/visualstudio/");
                txtSample.Text = "Server=myServerAddress;Database=myDataBase;Uid=myUsername;Pwd=myPassword;";
            }

            if (cmdDatabaseType.SelectedIndex == 5 && vsVersion.Major == 16 && !ddexInstalled)
            {
                DDEXLink.NavigateUri = new Uri("https://www.oracle.com/database/technologies/dotnet-odtvsix-vs2019-downloads.html");
                txtSample.Text = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=MyHost)(PORT=MyPort)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=MyOracleSID)));User Id=myUsername;Password=myPassword;";
            }
        }

        private void DDEXLink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            if (e.Uri != null)
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo(e.Uri.AbsoluteUri),
                };
                process.Start();
            }
        }
    }
}
