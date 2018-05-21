using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using EFCorePowerTools.Handlers;
using EnvDTE;
using ErikEJ.SqlCeToolbox.Helpers;

namespace ErikEJ.SqlCeToolbox.Dialogs
{
    public partial class EfCoreMigrationsDialog
    {
        private SortedDictionary<string, string> _statusList;
        private readonly EFCorePowerTools.EFCorePowerToolsPackage _package;
        private readonly ProcessLauncher _processLauncher = new ProcessLauncher();
        private readonly string _outputPath;
        private readonly bool _isNetCore;
        private readonly Project _project;

        public EfCoreMigrationsDialog(SortedDictionary<string, string> statusList, EFCorePowerTools.EFCorePowerToolsPackage package, string outputPath, bool isNetCore, Project project)
        {
            Telemetry.TrackPageView(nameof(EfCoreModelDialog));
            InitializeComponent();
            Background = VsThemes.GetWindowBackground();
            _package = package;
            _isNetCore = isNetCore;
            _outputPath = outputPath;
            _project = project;

            UpdateStatusList(statusList);
        }

        private void UpdateStatusList(SortedDictionary<string, string> statusList)
        {
            _statusList = statusList;
            cmbDbContext.ItemsSource = _statusList.Select(s => s.Key).ToList();
            cmbDbContext.SelectionChanged += CmbDbContext_SelectionChanged;
            if (_statusList.Count > 0)
            {
                cmbDbContext.SelectedIndex = 0;
            }
        }

        private void CmbDbContext_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cmbDbContext.SelectedItem != null)
            {
                var status = _statusList[cmbDbContext.SelectedValue.ToString()];
                SetUI(status);
            };
        }

        public string ProjectName
        {
            set
            {
                Title = $"Manage Migrations in Project {value}";
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //textBox1.Focus();
        }

        private void SetUI(string status)
        {
            // InSync, NoMigrations, Changes, Pending
            if (status == "InSync")
            {
                txtMessage.Text = $"Your database and model are in sync.";
                lblMigration.Visibility = Visibility.Collapsed;
                txtMigrationName.Visibility = Visibility.Collapsed;
                btnApply.Visibility = Visibility.Collapsed;
            }

            if (status == "NoMigrations")
            {
                txtMessage.Text = $"No migrations are present in your project, create you initial migration.{Environment.NewLine}Enter a name for the new migration below.";
                lblMigration.Text = "Migration Name";
                btnApply.Content = "Add Migration";
            }

            if (status == "Changes")
            {
                txtMessage.Text = $"There are pending model changes, add a migration with the changes.{Environment.NewLine}Enter a name for the migration below.";
                lblMigration.Text = "Migration Name";
                btnApply.Content = "Add Migration";
            }

            if (status == "Pending")
            {
                txtMessage.Text = $"There are migrations that have not been applied to the database.";
                lblMigration.Visibility = Visibility.Collapsed;
                txtMigrationName.Visibility = Visibility.Collapsed;
                btnApply.Content = "Update Database";
            }
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnApply.IsEnabled = false;

                //TODO Circular progress bar?

                if (btnApply.Content.ToString() == "Add Migration")
                {
                    if (string.IsNullOrEmpty(txtMigrationName.Text))
                    {
                        EnvDteHelper.ShowError("Migration Name required");
                        return;
                    }

                    _package.Dte2.StatusBar.Text = $"Creating Migration {txtMigrationName.Text} in DbContext {cmbDbContext.SelectedValue.ToString()}";
                    var processResult = _processLauncher.GetOutput(_outputPath, _isNetCore, GenerationType.MigrationAdd, cmbDbContext.SelectedValue.ToString(), txtMigrationName.Text, _project.Properties.Item("DefaultNamespace").Value.ToString());
                    ReportStatus(processResult);
                }

                if (btnApply.Content.ToString() == "Update Database")
                {
                    _package.Dte2.StatusBar.Text = $"Updating Database from migrations in DbContext {cmbDbContext.SelectedValue.ToString()}";
                    var processResult = _processLauncher.GetOutput(_outputPath, _isNetCore, GenerationType.MigrationApply, cmbDbContext.SelectedValue.ToString(), null, null);
                    ReportStatus(processResult);
                }
            }
            catch (Exception ex)
            {
                EnvDteHelper.ShowError(ex.ToString());
            }
            finally
            {
                btnApply.IsEnabled = true;
            }
        }

        private void ReportStatus(string processResult)
        {
            if (processResult.StartsWith("Error:"))
            {
                EnvDteHelper.ShowError(processResult);
            }
            var result = MigrationsHandler.BuildModelResult(processResult);

            UpdateStatusList(result);
        }
    }
}
