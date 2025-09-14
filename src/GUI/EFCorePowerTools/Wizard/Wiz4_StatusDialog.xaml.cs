using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EFCorePowerTools.Contracts.Wizard;
using EFCorePowerTools.Extensions;
using EFCorePowerTools.Handlers.ReverseEngineer;
using EFCorePowerTools.Locales;
using EFCorePowerTools.Messages;
using EFCorePowerTools.ViewModels;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading; // Added for JoinableTaskFactory (kept for SwitchToMainThreadAsync)
using RevEng.Common;

namespace EFCorePowerTools.Wizard
{
    public partial class Wiz4_StatusDialog : WizardResultPageFunction
    {
        private readonly WizardDataViewModel wizardViewModel;
        private BusyOverlay busyOverlay;

        public Wiz4_StatusDialog(WizardDataViewModel viewModel, IWizardView wizardView)
            : base(viewModel, wizardView)
        {
            this.wizardViewModel = viewModel;

            Loaded += WizardPage4_Loaded;

            InitializeComponent();

            // Add busy overlay
            busyOverlay = new BusyOverlay();
            Grid.SetColumnSpan(busyOverlay, int.MaxValue);
            Grid.SetRowSpan(busyOverlay, int.MaxValue);
            ((Grid)Content).Children.Add(busyOverlay);

            FinishButton.IsEnabled = false;
            wizardViewModel.GenerateStatus = string.Empty;
            InitializeMessengerWithStatusbar(Statusbar, ReverseEngineerLocale.StatusbarGeneratingFiles);
        }

        private void WizardPage4_Loaded(object sender, RoutedEventArgs e)
        {
            WindowTitle = "Status";
        }

#pragma warning disable SA1202 // Elements should be ordered by access
        protected override void OnPageVisible(object sender, StatusbarEventArgs e)
#pragma warning restore SA1202 // Elements should be ordered by access
        {
            var wea = wizardViewModel.WizardEventArgs;

            IsPageLoaded = wizardViewModel.IsPage4Initialized;

            if (wizardViewModel.ErrorMessage != null)
            {
                wizardViewModel.GenerateStatus = wizardViewModel.ErrorMessage;
                Statusbar.Status.ShowStatusError("Error occurred");
                PreviousButton.IsEnabled = false;
                FinishButton.IsEnabled = true;
                return;
            }

            if (!IsPageLoaded)
            {
                var project = wea.Project;
                var optionsPath = wea.OptionsPath;
                var options = wea.Options;
                var userOptions = wea.UserOptions;
                var namingOptionsAndPath = wea.NamingOptionsAndPath;
                var onlyGenerate = wea.OnlyGenerate;
                var forceEdit = wea.ForceEdit;

                wizardViewModel.GenerateStatus = string.Empty;
                PreviousButton.IsEnabled = false;
                FinishButton.IsEnabled = false;

                wizardViewModel.Bll.GetModelOptionsPostDialog(options, project.Name, wea, wizardViewModel.Model);
                var errorMessage = string.Empty;

                // Replaced JoinableTaskFactory.RunAsync(...).FileAndForget with explicit Task method and robust exception handling.
                _ = RunGenerationAsync();

                async System.Threading.Tasks.Task RunGenerationAsync()
                {
                    try
                    {
                        await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                        // Show busy overlay
                        busyOverlay.BusyMessage = "Generating files...";
                        busyOverlay.IsBusy = true;

                        try
                        {
                            await InvokeWithErrorHandlingAsync(async () =>
                            {
                                await wizardViewModel.Bll.SaveOptionsAsync(
                                    project,
                                    optionsPath,
                                    options,
                                    userOptions,
                                    new Tuple<List<Schema>, string>(options.CustomReplacers, namingOptionsAndPath.Item2));

                                await RevEngWizardHandler.InstallNuGetPackagesAsync(project, onlyGenerate, options, forceEdit, wea);

                                var neededPackages = await wea.Project.GetNeededPackagesAsync(wea.Options);
                                var missingProviderPackage = neededPackages.Find(p => p.DatabaseTypes.Contains(options.DatabaseType) && p.IsMainProviderPackage && !p.Installed)?.PackageId;
                                if (options.InstallNuGetPackage || options.SelectedToBeGenerated == 2)
                                {
                                    missingProviderPackage = null;
                                }

                                wea.ReverseEngineerStatus = await wizardViewModel.Bll.GenerateFilesAsync(
                                    project,
                                    options,
                                    missingProviderPackage,
                                    onlyGenerate,
                                    neededPackages,
                                    true);

                                var postRunFile = Path.Combine(Path.GetDirectoryName(optionsPath), "efpt.postrun.cmd");
                                if (File.Exists(postRunFile))
                                {
                                    Process.Start($"\"{postRunFile}\"");
                                }

                                return true;
                            });

                            if (string.IsNullOrEmpty(errorMessage))
                            {
                                Statusbar.Status.ShowStatus();
                                wizardViewModel.GenerateStatus = wea.ReverseEngineerStatus;
                            }
                            else
                            {
                                wizardViewModel.GenerateStatus = $"❌ {errorMessage}";
                            }

                            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                            var win = Window.GetWindow(this);
                            if (win != null)
                            {
                                win.Activate();
                                win.Focus();
                            }

                            if (FinishButton != null)
                            {
                                FinishButton.IsEnabled = true;
                                FinishButton.IsDefault = true;
                                FinishButton.Focus();
                                Keyboard.Focus(FinishButton);
                            }

                            PreviousButton.IsEnabled = true;
                        }
                        catch (Exception ex)
                        {
                            // Capture unexpected exceptions not handled by InvokeWithErrorHandlingAsync
                            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                            wizardViewModel.GenerateStatus = $"❌ {ex.Message}";
                            Statusbar.Status.ShowStatusError("Error occurred");
                            FinishButton.IsEnabled = true;
                            PreviousButton.IsEnabled = true;

                            try
                            {
                                ActivityLog.TryLogError("EFCorePowerTools", ex.ToString());
                            }
                            catch
                            {
                                // Ignore logging failures
                            }
                        }
                    }
                    finally
                    {
                        await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                        busyOverlay.IsBusy = false;
                    }
                }
            }
        }

        private void TextChangedEventHandler(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var wea = wizardViewModel.WizardEventArgs;

            var textBox = sender as TextBox;
            if (textBox != null && !string.IsNullOrEmpty(textBox.Text) && PreviousButton != null && FinishButton != null && !string.IsNullOrEmpty(wea.ReverseEngineerStatus))
            {
                // If here then we have status update - enabled buttons
                Statusbar.Status.ShowStatus(); // Will reset status bar
                PreviousButton.IsEnabled = true;
                FinishButton.IsEnabled = true;
            }
        }
    }
}