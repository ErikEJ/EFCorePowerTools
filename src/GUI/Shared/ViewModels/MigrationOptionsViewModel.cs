using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Community.VisualStudio.Toolkit;
using EFCorePowerTools.Common.DAL;
using EFCorePowerTools.Contracts.EventArgs;
using EFCorePowerTools.Contracts.ViewModels;
using EFCorePowerTools.Extensions;
using EFCorePowerTools.Handlers;
using EFCorePowerTools.Locales;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.VisualStudio.Shell;

namespace EFCorePowerTools.ViewModels
{
    public class MigrationOptionsViewModel : ViewModelBase, IMigrationOptionsViewModel
    {
        private readonly IVisualStudioAccess visualStudioAccess;
        private readonly RelayCommand applyCommand;

        private string title;
        private string applyButtonContent;
        private SortedDictionary<string, string> statusList;
        private string selectedStatusKey;
        private string migrationName;
        private string statusMessage;

        private Project project;
        private ProcessLauncher processLauncher;
        private string outputPath;
        private bool applying;
        private double backgroundOpacity;
        private Visibility migrationNameVisibility;

        public MigrationOptionsViewModel(IVisualStudioAccess visualStudioAccess)
        {
            this.visualStudioAccess = visualStudioAccess;

#pragma warning disable VSTHRD101 // Avoid unsupported async delegates
            LoadedCommand = new RelayCommand(async () => await Loaded_ExecutedAsync());
            applyCommand = new RelayCommand(async () => await Apply_ExecutedAsync(), () => !applying);
#pragma warning restore VSTHRD101 // Avoid unsupported async delegates
            CancelCommand = new RelayCommand(Cancel_Executed);
        }

        public event EventHandler<CloseRequestedEventArgs> CloseRequested;

        public string Title
        {
            get => title;
            private set
            {
                if (value == title)
                {
                    return;
                }

                title = value;
                RaisePropertyChanged();
            }
        }

        public string ApplyButtonContent
        {
            get => applyButtonContent;
            private set
            {
                if (value == applyButtonContent)
                {
                    return;
                }

                applyButtonContent = value;
                RaisePropertyChanged();
            }
        }

        public SortedDictionary<string, string> StatusList
        {
            get => statusList;
            private set
            {
                if (value == statusList)
                {
                    return;
                }

                statusList = value;
                RaisePropertyChanged();
            }
        }

        public string SelectedStatusKey
        {
            get => selectedStatusKey;
            set
            {
                if (value == selectedStatusKey)
                {
                    return;
                }

                selectedStatusKey = value;
                HandleSelectedStatusKeyChange();
                RaisePropertyChanged();
            }
        }

        public string StatusMessage
        {
            get => statusMessage;
            set
            {
                if (value == statusMessage)
                {
                    return;
                }

                statusMessage = value;
                RaisePropertyChanged();
            }
        }

        public string MigrationName
        {
            get => migrationName;
            set
            {
                if (value == migrationName)
                {
                    return;
                }

                migrationName = value;
                RaisePropertyChanged();
            }
        }

        public ICommand LoadedCommand { get; }

        public ICommand ApplyCommand => applyCommand;

        public ICommand CancelCommand { get; }

        public double BackgroundOpacity
        {
            get => backgroundOpacity;
            private set
            {
                if (Math.Abs(value - backgroundOpacity) < double.Epsilon)
                {
                    return;
                }

                backgroundOpacity = value;
                RaisePropertyChanged();
            }
        }

        public Visibility MigrationNameVisibility
        {
            get => migrationNameVisibility;
            private set
            {
                if (value == migrationNameVisibility)
                {
                    return;
                }

                migrationNameVisibility = value;
                RaisePropertyChanged();
            }
        }

        void IMigrationOptionsViewModel.UseProjectForMigration(Project project)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            Title = string.Format(MigrationsLocale.ManageMigrationsInProject, project.Name);
            this.project = project;
            processLauncher = new ProcessLauncher(project);
        }

        void IMigrationOptionsViewModel.UseOutputPath(string outputPath)
        {
            this.outputPath = outputPath;
        }

        private void UpdateStatusList(SortedDictionary<string, string> statusList)
        {
            StatusList = statusList;
            if (this.statusList.Count <= 0)
            {
                return;
            }

            var f = StatusList.First();
            SelectedStatusKey = f.Key;
        }

        private void HandleSelectedStatusKeyChange()
        {
            if (SelectedStatusKey == null)
            {
                return;
            }

            if (!StatusList.TryGetValue(SelectedStatusKey, out var selectedStatusValue))
            {
                return;
            }

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

            processLauncher.BuildModelResult(modelInfo)
                            .ForEach(a =>
                            {
                                result.Add(a.Item1, a.Item2);
                            });

            return result;
        }

        private async System.Threading.Tasks.Task ReportStatusAsync(string processResult)
        {
            await visualStudioAccess.SetStatusBarTextAsync(string.Empty);

            if (string.IsNullOrEmpty(processResult))
            {
                visualStudioAccess.ShowError(MigrationsLocale.UnableGetMigrationStatus);
                return;
            }

            if (processResult.IndexOf("Error:", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                visualStudioAccess.ShowError(processResult);
                return;
            }

            var result = BuildModelResult(processResult);
            UpdateStatusList(result);
        }

        private async System.Threading.Tasks.Task GetMigrationStatusAsync()
        {
            try
            {
                await visualStudioAccess.StartStatusBarAnimationAsync();
                await visualStudioAccess.SetStatusBarTextAsync(MigrationsLocale.GettingMigrationStatus);

                if (!await VS.Build.ProjectIsUpToDateAsync(project))
                {
                    var ok = await VS.Build.BuildProjectAsync(project);

                    if (!ok)
                    {
                        visualStudioAccess.ShowError(MigrationsLocale.BuildFailed);
                        return;
                    }
                }

                var processResult = await processLauncher.GetOutputAsync(outputPath, GenerationType.MigrationStatus, null);

                await ReportStatusAsync(processResult);
            }
            catch (Exception ex)
            {
                visualStudioAccess.ShowError(ex.ToString());
            }
            finally
            {
                await visualStudioAccess.SetStatusBarTextAsync(string.Empty);
                await visualStudioAccess.StopStatusBarAnimationAsync();
            }
        }

        private async Task<bool> AddMigrationAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            if (string.IsNullOrEmpty(MigrationName))
            {
                visualStudioAccess.ShowError(MigrationsLocale.MigrationNameRequired);
                return false;
            }

            await visualStudioAccess.SetStatusBarTextAsync(string.Format(MigrationsLocale.CreatingMigrationInDbContext, MigrationName, SelectedStatusKey));

            var nameSpace = await project.GetAttributeAsync("RootNamespace");

            var processResult = await processLauncher.GetOutputAsync(outputPath, Path.GetDirectoryName(project.FullPath), GenerationType.MigrationAdd, SelectedStatusKey, MigrationName, nameSpace);

            var result = BuildModelResult(processResult);

            if (processResult.IndexOf("Error:", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                visualStudioAccess.ShowError(processResult);
                return false;
            }

            if (result.Count == 1)
            {
                string[] lines = result.First().Value.Split(
                                                            new[] { Environment.NewLine },
                                                            StringSplitOptions.None);
                if (lines.Length == 3)
                {
                    await project.AddExistingFilesAsync(lines);
                    await VS.Documents.OpenViaProjectAsync(lines[1]);
                }
            }

            return true;
        }

        private async Task<bool> UpdateDatabaseAsync()
        {
            await visualStudioAccess.SetStatusBarTextAsync(string.Format(MigrationsLocale.UpdatingDatabaseFromMigrationsInDbContext, SelectedStatusKey));
            var processResult = await processLauncher.GetOutputAsync(outputPath, GenerationType.MigrationApply, SelectedStatusKey);
            if (processResult.IndexOf("Error:", StringComparison.OrdinalIgnoreCase) < 0)
            {
                return true;
            }

            visualStudioAccess.ShowError(processResult);
            return false;
        }

        private async Task<bool> ScriptMigrationAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            await visualStudioAccess.SetStatusBarTextAsync(string.Format(MigrationsLocale.ScriptingMigrationsInDbContext, SelectedStatusKey));
            var processResult = await processLauncher.GetOutputAsync(outputPath, GenerationType.MigrationScript, SelectedStatusKey);
            if (processResult.IndexOf("Error:", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                visualStudioAccess.ShowError(processResult);
                return false;
            }

            var modelResult = processLauncher.BuildModelResult(processResult);
            var files = project.GenerateFiles(modelResult, ".sql");
            foreach (var file in files)
            {
                visualStudioAccess.OpenFile(file);
            }

            return true;
        }

        private async System.Threading.Tasks.Task Loaded_ExecutedAsync()
        {
            HideInformation();

            await GetMigrationStatusAsync();
            if (StatusList != null && StatusList.Count > 0)
            {
                return;
            }

            CloseRequested?.Invoke(this, new CloseRequestedEventArgs(false));
        }

        private async System.Threading.Tasks.Task Apply_ExecutedAsync()
        {
            try
            {
                applying = true;
                applyCommand.RaiseCanExecuteChanged();
                await visualStudioAccess.StartStatusBarAnimationAsync();

                if (!StatusList.TryGetValue(SelectedStatusKey, out var selectedStatusValue))
                {
                    throw new InvalidOperationException(SelectedStatusKey);
                }

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
                {
                    await GetMigrationStatusAsync();
                }
            }
            catch (Exception e)
            {
                visualStudioAccess.ShowError(e.ToString());
            }
            finally
            {
                await visualStudioAccess.StopStatusBarAnimationAsync();
                await visualStudioAccess.SetStatusBarTextAsync(string.Empty);
                applying = false;
                applyCommand.RaiseCanExecuteChanged();
            }
        }

        private void Cancel_Executed()
        {
            CloseRequested?.Invoke(this, new CloseRequestedEventArgs(false));
        }
    }
}
