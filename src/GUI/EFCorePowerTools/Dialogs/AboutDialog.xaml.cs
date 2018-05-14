using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System;
using ErikEJ.SqlCeToolbox.Helpers;
using ErikEJ.SqlCeScripting;


namespace EFCorePowerTools
{
    public partial class AboutDialog
    {
        private readonly EFCorePowerToolsPackage _package;

        public AboutDialog(EFCorePowerToolsPackage package)
        {
            Telemetry.TrackPageView(nameof(AboutDialog));
            _package = package;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Background = VsThemes.GetWindowBackground();
            Version.Text = "Version " + Assembly.GetExecutingAssembly().GetName().Version;

            txtStatus.Text = "SQL Server Compact 4.0 in GAC - ";
            try
            {
                var version = new SqlCeHelper4().IsV40Installed();
                if (version != null)
                {
                    txtStatus.Text += string.Format("Yes - {0}\n", version);
                }
                else
                {
                    txtStatus.Text += "No\n";
                }
            }
            catch
            {
                txtStatus.Text += "No\n";
            }

            txtStatus.Text += "SQL Server Compact 4.0 DbProvider - ";
            try
            {
                System.Data.Common.DbProviderFactories.GetFactory(EFCorePowerTools.Resources.SqlCompact40InvariantName);
                txtStatus.Text += "Yes\n";
            }
            catch 
            {
                txtStatus.Text += "No\n";
            }

            txtStatus.Text += "\nSQL Server Compact 4.0 DDEX provider - ";
            try
            {
                if (EnvDteHelper.DdexProviderIsInstalled(new Guid(EFCorePowerTools.Resources.SqlCompact40Provider)))
                {
                    txtStatus.Text += "Yes\n";
                }
                else
                {
                    txtStatus.Text += "No\n";
                }
            }
            catch
            {
                txtStatus.Text += "No\n";
            }

            txtStatus.Text += "SQL Server Compact 4.0 Simple DDEX provider - ";
            try
            {
                if (EnvDteHelper.DdexProviderIsInstalled(new Guid(EFCorePowerTools.Resources.SqlCompact40PrivateProvider)))
                {
                    txtStatus.Text += "Yes\n";
                }
                else
                {
                    txtStatus.Text += "No\n";
                }
            }
            catch
            {
                txtStatus.Text += "No\n";
            }

            txtStatus.Text += "\n\nSQLite ADO.NET Provider included: ";
            try
            {
                Assembly asm = Assembly.Load("System.Data.SQLite");
                txtStatus.Text += string.Format("{0}\n", asm.GetName().Version);
            }
            catch
            {
                txtStatus.Text += "No\n";
            }

            txtStatus.Text += "SQLite EF6 DbProvider in GAC - ";
            try
            {
                if (EnvDteHelper.IsSqLiteDbProviderInstalled())
                {
                    txtStatus.Text += "Yes\n";
                }
                else
                {
                    txtStatus.Text += "No\n";
                }
            }
            catch
            {
                txtStatus.Text += "No\n";
            }

            txtStatus.Text += "\nSystem.Data.SQLite DDEX provider - ";
            try
            {
                if (EnvDteHelper.DdexProviderIsInstalled(new Guid(EFCorePowerTools.Resources.SQLiteProvider)))
                {
                    txtStatus.Text += "Yes\n";
                }
                else
                {
                    txtStatus.Text += "No\n";
                }
            }
            catch
            {
                txtStatus.Text += "No\n";
            }

            txtStatus.Text += "SQLite Simple DDEX provider - ";
            try
            {
                if (EnvDteHelper.DdexProviderIsInstalled(new Guid(EFCorePowerTools.Resources.SqlitePrivateProvider)))
                {
                    txtStatus.Text += "Yes\n";
                }
                else
                {
                    txtStatus.Text += "No\n";
                }
            }
            catch
            {
                txtStatus.Text += "No\n";
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CodeplexLink_Click(object sender, RoutedEventArgs e)
        {
            _package.Dte2.ItemOperations.Navigate("https://github.com/ErikEJ/SqlCeToolbox");
        }

        private void GalleryLink_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://marketplace.visualstudio.com/items?itemName=ErikEJ.EFCorePowerTools#review-details");
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var text = txtStatus.Text.Replace("\n", Environment.NewLine);
            Clipboard.SetText(Version.Text + Environment.NewLine + Environment.NewLine + text);
            MessageBox.Show("About info copied to clipboard");
        }
    }
}