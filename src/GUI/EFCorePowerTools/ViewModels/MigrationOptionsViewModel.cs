namespace EFCorePowerTools.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using Contracts.EventArgs;
    using Contracts.ViewModels;
    using EnvDTE;
    using Extensions;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;
    using Handlers;
    using Shared.DAL;
    using Constants = Microsoft.VisualStudio.Shell.Interop.Constants;

    public class MigrationOptionsViewModel : ViewModelBase, IMigrationOptionsViewModel
    {
        private const string baseTitle = "Manage Migrations in Project";

        private readonly IVisualStudioAccess _visualStudioAccess;
        private readonly RelayCommand _applyCommand;

        private object _progressIcon;
        private string _title;
        private string _applyButtonContent;
        private SortedDictionary<string, string> _statusList;
        private string _selectedStatusKey;
        private string _migrationName;
        private string _statusMessage;

        private Project _project;
        private ProcessLauncher _processLauncher;
        private string _outputPath;
        private bool _applying;
        private double _backgroundOpacity;
        private Visibility _migrationNameVisibility;

        public event EventHandler<CloseRequestedEventArgs> CloseRequested;

        public string Title
        {
            get => _title;
            private set
            {
                if (value == _title) return;
                _title = value;
                RaisePropertyChanged();
            }
        }

        public string ApplyButtonContent
        {
            get => _applyButtonContent;
            private set
            {
                if (value == _applyButtonContent) return;
                _applyButtonContent = value;
                RaisePropertyChanged();
            }
        }

        public SortedDictionary<string, string> StatusList
        {
            get => _statusList;
            private set
            {
                if (value == _statusList) return;
                _statusList = value;
                RaisePropertyChanged();
            }
        }

        public string SelectedStatusKey
        {
            get => _selectedStatusKey;
            set
            {
                if (value == _selectedStatusKey) return;
                _selectedStatusKey = value;
                HandleSelectedStatusKeyChange();
                RaisePropertyChanged();
            }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                if (value == _statusMessage) return;
                _statusMessage = value;
                RaisePropertyChanged();
            }
        }

        public string MigrationName
        {
            get => _migrationName;
            set
            {
                if (value == _migrationName) return;
                _migrationName = value;
                RaisePropertyChanged();
            }
        }

        public ICommand LoadedCommand { get; }

        public ICommand ApplyCommand => _applyCommand;

        public ICommand CancelCommand { get; }

        public double BackgroundOpacity
        {
            get => _backgroundOpacity;
            private set
            {
                if (Math.Abs(value - _backgroundOpacity) < double.Epsilon) return;
                _backgroundOpacity = value;
                RaisePropertyChanged();
            }
        }

        public Visibility MigrationNameVisibility
        {
            get => _migrationNameVisibility;
            private set
            {
                if (value == _migrationNameVisibility) return;
                _migrationNameVisibility = value;
                RaisePropertyChanged();
            }
        }

        public MigrationOptionsViewModel(IVisualStudioAccess visualStudioAccess)
        {
            _visualStudioAccess = visualStudioAccess;

            Title = baseTitle;

            LoadedCommand = new RelayCommand(async () => await Loaded_Executed());
            _applyCommand = new RelayCommand(async () => await Apply_Executed(), () => !_applying);
            CancelCommand = new RelayCommand(Cancel_Executed);

            _progressIcon = (short)Constants.SBAI_Build;
        }

        private void UpdateStatusList(SortedDictionary<string, string> statusList)
        {
            StatusList = statusList;
            if (_statusList.Count <= 0) return;

            var f = StatusList.First();
            SelectedStatusKey = f.Key;
        }

        private void HandleSelectedStatusKeyChange()
        {
            if (SelectedStatusKey == null) return;
            if (!StatusList.TryGetValue(SelectedStatusKey, out var selectedStatusValue)) return;

            ResetInformation();

            switch (selectedStatusValue)
            {
                case "InSync":
                    SetInformationForProjectInSync();
                    break;
                case "NoMigrations":
                    SetInformationForProjectWithNoMigrations();
                    break;
                case "Changes":
                    SetInformationForProjectWithChanges();
                    break;
                case "Pending":
                    SetInformationForProjectWithPending();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(selectedStatusValue), selectedStatusValue);
            }
        }

        private void HideInformation()
        {
            BackgroundOpacity = 0;
            MigrationNameVisibility = Visibility.Collapsed;
        }

        private void ResetInformation()
        {
            BackgroundOpacity = 0;
            MigrationNameVisibility = Visibility.Visible;
            MigrationName = string.Empty;
        }

        private void SetInformationForProjectInSync()
        {
            StatusMessage = "Your database and model are in sync.";
            MigrationNameVisibility = Visibility.Collapsed;
            ApplyButtonContent = "Script Migrations";
            BackgroundOpacity = 0.4;
        }

        private void SetInformationForProjectWithNoMigrations()
        {
            StatusMessage = $"No migrations are present in your project, create your initial migration.{Environment.NewLine}Enter a name for the new migration below.";
            ApplyButtonContent = "Add Migration";
        }

        private void SetInformationForProjectWithChanges()
        {
            StatusMessage = $"There are pending model changes, add a migration with the changes.{Environment.NewLine}Enter a name for the migration below.";
            ApplyButtonContent = "Add Migration";
        }

        private void SetInformationForProjectWithPending()
        {
            StatusMessage = "There are migrations that have not been applied to the database.";
            MigrationNameVisibility = Visibility.Collapsed;
            ApplyButtonContent = "Update Database";
        }

        private SortedDictionary<string, string> BuildModelResult(string modelInfo)
        {
            var result = new SortedDictionary<string, string>();

            var contexts = modelInfo.Split(new[] { "DbContext:" + Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var context in contexts)
            {
                var parts = context.Split(new[] { "DebugView:" + Environment.NewLine }, StringSplitOptions.None);
                result.Add(parts[0].Trim(), parts.Length > 1 ? parts[1].Trim() : string.Empty);
            }

            return result;
        }

        private void ReportStatus(string processResult)
        {
            _visualStudioAccess.SetStatusBarText(string.Empty);

            if (processResult.StartsWith("Error:"))
            {
                _visualStudioAccess.ShowError(processResult);
                return;
            }

            var result = BuildModelResult(processResult);
            UpdateStatusList(result);
        }

        private async Task GetMigrationStatus()
        {
            try
            {
                _visualStudioAccess.StartStatusBarAnimation(ref _progressIcon);
                _visualStudioAccess.SetStatusBarText("Getting Migration Status");
                if (_project.TryBuild())
                {
                    var processResult = await _processLauncher.GetOutputAsync(_outputPath, GenerationType.MigrationStatus, null);

                    ReportStatus(processResult);
                }
                else
                {
                    _visualStudioAccess.ShowError("Build failed");
                }
            }
            catch (Exception ex)
            {
                _visualStudioAccess.ShowError(ex.ToString());
            }
            finally
            {
                _visualStudioAccess.SetStatusBarText(string.Empty);
                _visualStudioAccess.StopStatusBarAnimation(ref _progressIcon);
            }
        }

        private async Task<bool> AddMigration()
        {
            if (string.IsNullOrEmpty(MigrationName))
            {
                _visualStudioAccess.ShowError("Migration Name required");
                return false;
            }

            _visualStudioAccess.SetStatusBarText($"Creating Migration {MigrationName} in DbContext {SelectedStatusKey}");
            var processResult = await _processLauncher.GetOutputAsync(_outputPath, Path.GetDirectoryName(_project.FullName), GenerationType.MigrationAdd, SelectedStatusKey, MigrationName, _project.Properties.Item("DefaultNamespace").Value.ToString());

            var result = BuildModelResult(processResult);

            if (processResult.StartsWith("Error:"))
            {
                _visualStudioAccess.ShowError(processResult);
                return false;
            }

            if (result.Count == 1)
            {
                string[] lines = result.First().Value.Split(
                                                            new[] { Environment.NewLine },
                                                            StringSplitOptions.None
                                                           );
                if (lines.Length == 3)
                {
                    _project.ProjectItems.AddFromFile(lines[1]); // migrationFile
                    _visualStudioAccess.OpenFile(lines[1]); // migrationFile

                    _project.ProjectItems.AddFromFile(lines[0]); // metadataFile
                    _project.ProjectItems.AddFromFile(lines[2]); // snapshotFile
                }
            }

            return true;
        }

        private async Task<bool> UpdateDatabase()
        {
            _visualStudioAccess.SetStatusBarText($"Updating Database from migrations in DbContext {SelectedStatusKey}");
            var processResult = await _processLauncher.GetOutputAsync(_outputPath, GenerationType.MigrationApply, SelectedStatusKey);
            if (!processResult.StartsWith("Error:")) return true;

            _visualStudioAccess.ShowError(processResult);
            return false;

        }

        private async Task<bool> ScriptMigration()
        {
            _visualStudioAccess.SetStatusBarText($"Scripting migrations in DbContext {SelectedStatusKey}");
            var processResult = await _processLauncher.GetOutputAsync(_outputPath, GenerationType.MigrationScript, SelectedStatusKey);
            if (processResult.StartsWith("Error:"))
            {
                _visualStudioAccess.ShowError(processResult);
                return false;
            }

            var modelResult = _processLauncher.BuildModelResult(processResult);
            var files = _project.GenerateFiles(modelResult, ".sql");
            foreach (var file in files)
            {
                _visualStudioAccess.OpenFile(file);
            }

            return true;
        }

        private async Task Loaded_Executed()
        {
            HideInformation();

            await GetMigrationStatus();
            if (StatusList != null && StatusList.Count > 0) return;

            _visualStudioAccess.ShowMessage("No valid DbContext classes found in the current project.");
            CloseRequested?.Invoke(this, new CloseRequestedEventArgs(false));
        }

        private async Task Apply_Executed()
        {
            try
            {
                _applying = true;
                _applyCommand.RaiseCanExecuteChanged();
                _visualStudioAccess.StartStatusBarAnimation(ref _progressIcon);

                if (!StatusList.TryGetValue(SelectedStatusKey, out var selectedStatusValue))
                    throw new ArgumentOutOfRangeException(nameof(SelectedStatusKey));

                bool success;
                switch (selectedStatusValue)
                {
                    case "NoMigrations":
                    case "Changes":
                        success = await AddMigration();
                        break;
                    case "Pending":
                        success = await UpdateDatabase();
                        break;
                    case "InSync":
                        success = await ScriptMigration();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(selectedStatusValue));
                }

                if (success)
                    await GetMigrationStatus();
            }
            catch (Exception e)
            {
                _visualStudioAccess.ShowError(e.ToString());
            }
            finally
            {
                _visualStudioAccess.StopStatusBarAnimation(ref _progressIcon);
                _visualStudioAccess.SetStatusBarText(string.Empty);
                _applying = false;
                _applyCommand.RaiseCanExecuteChanged();
            }
        }

        private void Cancel_Executed()
        {
            CloseRequested?.Invoke(this, new CloseRequestedEventArgs(false));
        }

        void IMigrationOptionsViewModel.UseProjectForMigration(Project project)
        {
            Title = baseTitle + " " + project.Name;
            _project = project;
            _processLauncher = new ProcessLauncher(project);
        }

        void IMigrationOptionsViewModel.UseOutputPath(string outputPath)
        {
            _outputPath = outputPath;
        }
    }
}