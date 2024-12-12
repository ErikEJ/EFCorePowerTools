// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using Community.VisualStudio.Toolkit;
using EFCorePowerTools.Common.Models;
using EFCorePowerTools.Contracts.ViewModels;
using EFCorePowerTools.Contracts.Views;
using EFCorePowerTools.Contracts.Wizard;
using EFCorePowerTools.Extensions;
using EFCorePowerTools.Handlers.Wizard;
using EFCorePowerTools.Helpers;
using EFCorePowerTools.Locales;
using EFCorePowerTools.Messages;
using EFCorePowerTools.ViewModels;
using Microsoft.VisualStudio.Shell;
using RevEng.Common;

namespace EFCorePowerTools.Wizard
{
    public partial class Wiz3_EfCoreModelDialog : WizardResultPageFunction, IModelingOptionsDialog
    {
        private readonly IWizardView wizardView;
        private readonly WizardDataViewModel wizardViewModel;
        private readonly Func<ModelingOptionsModel> getDialogResult;
        private readonly Action<ModelingOptionsModel> applyPresets;
        private readonly Action<TemplateTypeItem, IList<TemplateTypeItem>> setTemplateTypes;

        public Wiz3_EfCoreModelDialog(WizardDataViewModel wizardViewModel, IWizardView wizardView)
            : base(wizardViewModel, wizardView)
        {
            // telemetryAccess.TrackPageView(nameof(EfCoreModelDialog));
            this.wizardView = wizardView;
            this.wizardViewModel = wizardViewModel;
            getDialogResult = () => wizardViewModel.Model;
            applyPresets = wizardViewModel.ApplyPresets;
            setTemplateTypes = (templateType, templateTypes) =>
            {
                foreach (var item in templateTypes)
                {
                    wizardViewModel.TemplateTypeList.Add(item);
                }

                wizardViewModel.SelectedTemplateType = templateType.Key;
            };
            InitializeComponent();
            InitializeMessengerWithStatusbar(Statusbar, ReverseEngineerLocale.LoadingOptions);
        }

        protected override void OnPageVisible(object sender, StatusbarEventArgs e)
        {
            var viewModel = wizardViewModel;
            isPageLoaded = viewModel.IsPage3Initialized;

            if (!isPageLoaded)
            {
                messenger.Send(new ShowStatusbarMessage(ReverseEngineerLocale.LoadingOptions));

                ThreadHelper.JoinableTaskFactory.Run(async () =>
                {
                    var wea = wizardViewModel.WizardEventArgs;
                    wea.ModelingOptionsDialog = this;

                    var neededPackages = await wea.Project.GetNeededPackagesAsync(wea.Options);
                    wea.Options.InstallNuGetPackage = neededPackages
                        .Exists(p => p.DatabaseTypes.Contains(wea.Options.DatabaseType) && !p.Installed);

                    await wizardViewModel.Bll.GetModelOptionsAsync(wea.Options, wea.Project.Name, wea);

                });

                FirstTextBox.Focus();
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            // Go to next wizard page
            if (wizardViewModel.IsPage3Initialized)
            {
                NavigationService.GoForward();
            }
            else
            {
                var wizardPage4 = new Wiz4_StatusDialog((WizardDataViewModel)DataContext, wizardView);
                wizardPage4.Return += WizardPage_Return;
                NavigationService?.Navigate(wizardPage4);
                wizardViewModel.IsPage3Initialized = true;
            }
        }

        public (bool ClosedByOK, ModelingOptionsModel Payload) ShowAndAwaitUserResponse(bool modal)
        {
            return (true, getDialogResult());
        }

        public IModelingOptionsDialog ApplyPresets(ModelingOptionsModel presets)
        {
            applyPresets(presets);
            return this;
        }

        public void PublishTemplateTypes(TemplateTypeItem templateType, IList<TemplateTypeItem> allowedTemplateTypes)
        {
            setTemplateTypes(templateType, allowedTemplateTypes);
        }


        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            var hyperlink = sender as Hyperlink;
            var process = new Process
            {
                StartInfo = new ProcessStartInfo(hyperlink.NavigateUri.AbsoluteUri),
            };
            process.Start();
        }

        private void DocLink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo(e.Uri.AbsoluteUri),
            };
            process.Start();
        }

        private new void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            NextButton_Click(sender, e);
            messenger.Send(new ShowStatusbarMessage("Generating files"));

            var wea = wizardViewModel.WizardEventArgs;
            ThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                var project = wea.Project;
                var optionsPath = wea.OptionsPath;
                var options = wea.Options;
                var userOptions = wea.UserOptions;
                var namingOptionsAndPath = wea.NamingOptionsAndPath;
                var onlyGenerate = wea.OnlyGenerate;
                var forceEdit = wea.ForceEdit;

                await VS.StatusBar.ShowMessageAsync("Saving options...");

                await wizardViewModel.Bll.SaveOptionsAsync(project, optionsPath, options, userOptions, new Tuple<List<Schema>, string>(options.CustomReplacers, namingOptionsAndPath.Item2));

                await RevEngWizardHandler.InstallNuGetPackagesAsync(project, onlyGenerate, options, forceEdit);

                var neededPackages = await wea.Project.GetNeededPackagesAsync(wea.Options);
                var missingProviderPackage = neededPackages.Find(p => p.DatabaseTypes.Contains(options.DatabaseType) && p.IsMainProviderPackage && !p.Installed)?.PackageId;
                if (options.InstallNuGetPackage || options.SelectedToBeGenerated == 2)
                {
                    missingProviderPackage = null;
                }

                await VS.StatusBar.ShowMessageAsync("Generating Files...");

                await wizardViewModel.Bll.GenerateFilesAsync(project, options, missingProviderPackage, onlyGenerate, neededPackages);

                var postRunFile = Path.Combine(Path.GetDirectoryName(optionsPath), "efpt.postrun.cmd");
                if (File.Exists(postRunFile))
                {
                    Statusbar.Status.ShowStatusProgress("Invoking Post Run File...");

                    Process.Start($"\"{postRunFile}\"");
                }

                await VS.StatusBar.ClearAsync();

                Statusbar.Status.ShowStatus("Process completed");

                Telemetry.TrackEvent("PowerTools.ReverseEngineer");
            });
        }
    }
}
