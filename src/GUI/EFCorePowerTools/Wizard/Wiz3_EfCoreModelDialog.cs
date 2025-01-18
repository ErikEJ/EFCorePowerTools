// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Documents;
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
        private readonly Func<ModelingOptionsModel> getDialogResultPg3;
        private readonly Action<ModelingOptionsModel> applyPresets;
        private readonly Action<TemplateTypeItem, IList<TemplateTypeItem>> setTemplateTypes;

        public Wiz3_EfCoreModelDialog(WizardDataViewModel viewModel, IWizardView wizardView)
            : base(viewModel, wizardView)
        {
            // telemetryAccess.TrackPageView(nameof(EfCoreModelDialog));
            this.wizardView = wizardView;
            this.wizardViewModel = viewModel;

            getDialogResultPg3 = () => viewModel.Model;
            applyPresets = viewModel.ApplyPresets;
            setTemplateTypes = (templateType, templateTypes) =>
            {
                foreach (var item in templateTypes)
                {
                    viewModel.TemplateTypeList.Add(item);
                }

                viewModel.SelectedTemplateType = templateType.Key;
            };

            InitializeComponent();
            InitializeMessengerWithStatusbar(Statusbar, ReverseEngineerLocale.LoadingOptions);
        }

        protected override void OnPageVisible(object sender, StatusbarEventArgs e)
        {
            var viewModel = wizardViewModel;
            IsPageLoaded = viewModel.IsPage3Initialized;

            if (!IsPageLoaded)
            {
                var wea = wizardViewModel.WizardEventArgs;
                wea.ModelingOptionsDialog = this;

                Messenger.Send(new ShowStatusbarMessage(ReverseEngineerLocale.LoadingOptions));

                ThreadHelper.JoinableTaskFactory.Run(async () =>
                {
                    var neededPackages = await wea.Project.GetNeededPackagesAsync(wea.Options);
                    wea.Options.InstallNuGetPackage = neededPackages
                        .Exists(p => p.DatabaseTypes.Contains(wea.Options.DatabaseType) && !p.Installed);

                    await wizardViewModel.Bll.GetModelOptionsAsync(wea.Options, wea.Project.Name, wea);

                    if (wea.NewOptions)
                    {
                        // HACK Work around for issue with web app project system on initial run
                        wea.UserOptions = null;
                    }
                });

                FirstTextBox.Focus();
            }
        }

        private void GenerateFiles_Click(object sender, RoutedEventArgs e)
        {
            var wea = wizardViewModel.WizardEventArgs;
            var project = wea.Project;
            var optionsPath = wea.OptionsPath;
            var options = wea.Options;
            var userOptions = wea.UserOptions;
            var namingOptionsAndPath = wea.NamingOptionsAndPath;
            var onlyGenerate = wea.OnlyGenerate;
            var forceEdit = wea.ForceEdit;

            wizardViewModel.GenerateStatus = string.Empty;
            wizardViewModel.Bll.GetModelOptionsPostDialog(options, project.Name, wea, wizardViewModel.Model);
            cancelButton.IsEnabled = false; // Once processed we can't cancel - only finish
            nextButton.IsEnabled = true;

            Messenger.Send(new ShowStatusbarMessage("Generating files"));

            this.applyPresets(wizardViewModel.Model);

            ThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                await wizardViewModel.Bll.SaveOptionsAsync(project, optionsPath, options, userOptions, new Tuple<List<Schema>, string>(options.CustomReplacers, namingOptionsAndPath.Item2));

                await RevEngWizardHandler.InstallNuGetPackagesAsync(project, onlyGenerate, options, forceEdit);

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
            });

            Statusbar.Status.ShowStatus();
            wizardViewModel.GenerateStatus = wea.ReverseEngineerStatus;

            Telemetry.TrackEvent("PowerTools.ReverseEngineer");
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            ShowAndAwaitUserResponse(true);

            // Go to next wizard page
            if (wizardViewModel.IsPage3Initialized)
            {
                NavigationService.GoForward();
            }
            else
            {
                wizardViewModel.IsPage3Initialized = true;
                var wizardPage4 = new Wiz4_StatusDialog((WizardDataViewModel)DataContext, wizardView);
                wizardPage4.Return += WizardPage_Return;
                NavigationService?.Navigate(wizardPage4);
                Statusbar.Status.ShowStatus("Generating files...");
                GenerateFiles_Click(sender, e);
            }
        }

        public (bool ClosedByOK, ModelingOptionsModel Payload) ShowAndAwaitUserResponse(bool modal)
        {
            return (true, getDialogResultPg3());
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
            e.Handled = true;
        }

        private void DocLink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo(e.Uri.AbsoluteUri),
            };
            process.Start();
        }

        private void OpenBrowserHelp(object sender, RoutedEventArgs e)
        {
            OpenBrowserWithLink("https://github.com/ErikEJ/EFCorePowerTools/wiki/Reverse-Engineering");
        }

        private void OpenBrowserRate(object sender, RoutedEventArgs e)
        {
            OpenBrowserWithLink("https://marketplace.visualstudio.com/items?itemName=ErikEJ.EFCorePowerTools&amp;ssr=false#review-details");
        }

        private void OpenBrowserWithLink(string link)
        {
            Process.Start(new ProcessStartInfo(link));
        }
    }
}
