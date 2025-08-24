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

                // When generating we'll initialize the page to known state
                wizardViewModel.GenerateStatus = string.Empty;
                PreviousButton.IsEnabled = false;
                FinishButton.IsEnabled = false;

                wizardViewModel.Bll.GetModelOptionsPostDialog(options, project.Name, wea, wizardViewModel.Model);
                var errorMessage = string.Empty;

                // Use async operation with proper UI feedback
                ThreadHelper.JoinableTaskFactory.Run(async () =>
                {
                    await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                    // Show busy overlay
                    busyOverlay.BusyMessage = "Generating files...";
                    busyOverlay.IsBusy = true;

                    try
                    {
                        await InvokeWithErrorHandlingAsync(async () =>
                        {
                            await wizardViewModel.Bll.SaveOptionsAsync(project, optionsPath, options, userOptions, new Tuple<List<Schema>, string>(options.CustomReplacers, namingOptionsAndPath.Item2));

                            await RevEngWizardHandler.InstallNuGetPackagesAsync(project, onlyGenerate, options, forceEdit, wea);

                            var neededPackages = await wea.Project.GetNeededPackagesAsync(wea.Options);
                            var missingProviderPackage = neededPackages.Find(p => p.DatabaseTypes.Contains(options.DatabaseType) && p.IsMainProviderPackage && !p.Installed)?.PackageId;
                            if (options.InstallNuGetPackage || options.SelectedToBeGenerated == 2)
                            {
                                missingProviderPackage = null;
                            }

                            wea.ReverseEngineerStatus = await wizardViewModel.Bll.GenerateFilesAsync(project, options, missingProviderPackage, onlyGenerate, neededPackages, true);

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

                        // Return focus to the wizard and the Finish button
                        await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                        var win = Window.GetWindow(this);
                        if (win != null)
                        {
                            win.Activate();
                            win.Focus();
                        }

                        if (FinishButton != null && FinishButton.IsEnabled)
                        {
                            // Ensure Enter triggers Finish
                            FinishButton.IsDefault = true; // already set in XAML, but safe to enforce
                            FinishButton.Focus();
                            Keyboard.Focus(FinishButton);
                        }
                    }
                    finally
                    {
                        // Hide busy overlay
                        busyOverlay.IsBusy = false;
                    }
                });
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