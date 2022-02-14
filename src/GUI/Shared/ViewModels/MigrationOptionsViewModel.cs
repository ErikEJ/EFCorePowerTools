namespace EFCorePowerTools.ViewModels
{
    using Community.VisualStudio.Toolkit;
    using Contracts.EventArgs;
    using Contracts.ViewModels;
    using EFCorePowerTools.Locales;
    using Extensions;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;
    using Handlers;
    using Microsoft.VisualStudio.Shell;
    using Shared.DAL;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;

    public class MigrationOptionsViewModel : ViewModelBase, IMigrationOptionsViewModel
    {
        private readonly IVisualStudioAccess _visualStudioAccess;
        private readonly RelayCommand _applyCommand;

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

            LoadedCommand = new RelayCommand(async () => await Loaded_ExecutedAsync());
            _applyCommand = new RelayCommand(async () => await Apply_ExecutedAsync(), () => !_applying);
            CancelCommand = new RelayCommand(Cancel_Executed);
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
                    throw new InvalidOperationException(selectedStatusValue);
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
            StatusMessage = MigrationsLocale.DatabaseModelSync;
            MigrationNameVisibility = Visibility.Collapsed;
            ApplyButtonContent = MigrationsLocale.ScriptMigrations;
            BackgroundOpacity = 0.4;
        }

        private void SetInformationForProjectWithNoMigrations()
        {
            StatusMessage = MigrationsLocale.NoMigrationsInProject;
            ApplyButtonContent = MigrationsLocale.AddMigration;
        }

        private void SetInformationForProjectWithChanges()
        {
            StatusMessage = MigrationsLocale.PendingModelChanges;
            ApplyButtonContent = MigrationsLocale.AddMigration;
        }

        private void SetInformationForProjectWithPending()
        {
            StatusMessage = MigrationsLocale.NotAppliedMigrations;
            MigrationNameVisibility = Visibility.Collapsed;
            ApplyButtonContent = MigrationsLocale.UpdateDatabase;
        }

        private SortedDictionary<string, string> BuildModelResult(string modelInfo)
        {
            var result = new SortedDictionary<string, string>();

            _processLauncher.BuildModelResult(modelInfo)
                            .ForEach(a =>
                            {
                                result.Add(a.Item1, a.Item2);
                            });

            return result;
        }

        private async System.Threading.Tasks.Task ReportStatusAsync(string processResult)
        {
            await _visualStudioAccess.SetStatusBarTextAsync(string.Empty);

            if (string.IsNullOrEmpty(processResult))
            {
                _visualStudioAccess.ShowError(MigrationsLocale.UnableGetMigrationStatus);
                return;
            }

            if (processResult.Contains("Error:"))
            {
                _visualStudioAccess.ShowError(processResult);
                return;
            }

            var result = BuildModelResult(processResult);
            UpdateStatusList(result);
        }

        private async System.Threading.Tasks.Task GetMigrationStatusAsync()
        {
            try
            {
                await _visualStudioAccess.StartStatusBarAnimationAsync();
                await _visualStudioAccess.SetStatusBarTextAsync(MigrationsLocale.GettingMigrationStatus);

                if (!await VS.Build.ProjectIsUpToDateAsync(_project))
                {
                    var ok = await VS.Build.BuildProjectAsync(_project);

                    if (!ok)
                    {
                        _visualStudioAccess.ShowError(MigrationsLocale.BuildFailed);
                        return;
                    }
                }

                var processResult = await _processLauncher.GetOutputAsync(_outputPath, GenerationType.MigrationStatus, null);

                await ReportStatusAsync(processResult);
            }
            catch (Exception ex)
            {
                _visualStudioAccess.ShowError(ex.ToString());
            }
            finally
            {
                await _visualStudioAccess.SetStatusBarTextAsync(string.Empty);
                await _visualStudioAccess.StopStatusBarAnimationAsync();
            }
        }

        private async Task<bool> AddMigrationAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            if (string.IsNullOrEmpty(MigrationName))
            {
                _visualStudioAccess.ShowError(MigrationsLocale.MigrationNameRequired);
                return false;
            }

            await _visualStudioAccess.SetStatusBarTextAsync(string.Format(MigrationsLocale.CreatingMigrationInDbContext, MigrationName, SelectedStatusKey));
            
            var nameSpace = await _project.GetAttributeAsync("RootNamespace");

            var processResult = await _processLauncher.GetOutputAsync(_outputPath, Path.GetDirectoryName(_project.FullPath), GenerationType.MigrationAdd, SelectedStatusKey, MigrationName, nameSpace);

            var result = BuildModelResult(processResult);

            if (processResult.Contains("Error:"))
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
                    await _project.AddExistingFilesAsync(lines);
                    await VS.Documents.OpenViaProjectAsync(lines[1]); 
                }
            }

            return true;
        }

        private async Task<bool> UpdateDatabaseAsync()
        {
            await _visualStudioAccess.SetStatusBarTextAsync(string.Format(MigrationsLocale.UpdatingDatabaseFromMigrationsInDbContext, SelectedStatusKey));
            var processResult = await _processLauncher.GetOutputAsync(_outputPath, GenerationType.MigrationApply, SelectedStatusKey);
            if (!processResult.Contains("Error:")) return true;

            _visualStudioAccess.ShowError(processResult);
            return false;

        }

        private async Task<bool> ScriptMigrationAsync()
        {
            await _visualStudioAccess.SetStatusBarTextAsync(String.Format(MigrationsLocale.ScriptingMigrationsInDbContext, SelectedStatusKey));
            var processResult = await _processLauncher.GetOutputAsync(_outputPath, GenerationType.MigrationScript, SelectedStatusKey);
            if (processResult.Contains("Error:"))
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

        private async System.Threading.Tasks.Task Loaded_ExecutedAsync()
        {
            HideInformation();

            await GetMigrationStatusAsync();
            if (StatusList != null && StatusList.Count > 0) return;

            CloseRequested?.Invoke(this, new CloseRequestedEventArgs(false));
        }

        private async System.Threading.Tasks.Task Apply_ExecutedAsync()
        {
            try
            {
                _applying = true;
                _applyCommand.RaiseCanExecuteChanged();
                await _visualStudioAccess.StartStatusBarAnimationAsync();

                if (!StatusList.TryGetValue(SelectedStatusKey, out var selectedStatusValue))
                    throw new InvalidOperationException(SelectedStatusKey);

                bool success;
                switch (selectedStatusValue)
                {
                    case "NoMigrations":
                    case "Changes":
                        success = await AddMigrationAsync();
                        break;
                    case "Pending":
                        success = await UpdateDatabaseAsync();
                        break;
                    case "InSync":
                        success = await ScriptMigrationAsync();
                        break;
                    default:
                        throw new InvalidOperationException(selectedStatusValue);
                }

                if (success)
                    await GetMigrationStatusAsync();
            }
            catch (Exception e)
            {
                _visualStudioAccess.ShowError(e.ToString());
            }
            finally
            {
                await _visualStudioAccess.StopStatusBarAnimationAsync();
                await _visualStudioAccess.SetStatusBarTextAsync(string.Empty);
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
            ThreadHelper.ThrowIfNotOnUIThread();

            Title = string.Format(MigrationsLocale.ManageMigrationsInProject, project.Name);
            _project = project;
            _processLauncher = new ProcessLauncher(project);
        }

        void IMigrationOptionsViewModel.UseOutputPath(string outputPath)
        {
            _outputPath = outputPath;
        }
    }
}