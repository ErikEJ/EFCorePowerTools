// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;
using Community.VisualStudio.Toolkit;
using EFCorePowerTools.Common.Models;
using EFCorePowerTools.Contracts.ViewModels;
using EFCorePowerTools.Contracts.Views;
using EFCorePowerTools.Contracts.Wizard;
using EFCorePowerTools.Extensions;
using EFCorePowerTools.Locales;
using GalaSoft.MvvmLight.Command;
using Microsoft.VisualStudio.Shell;

namespace EFCorePowerTools.Wizard
{
    public partial class WizardPage3 : WizardResultPageFunction, IModelingOptionsDialog
    {
        private readonly WizardDataViewModel wizardViewModel;
        private readonly Func<ModelingOptionsModel> getDialogResult;
        private readonly Action<ModelingOptionsModel> applyPresets;
        private readonly Action<TemplateTypeItem, IList<TemplateTypeItem>> setTemplateTypes;

        public WizardPage3(WizardDataViewModel wizardViewModel, IWizardView wizardView)
            : base(wizardViewModel, wizardView)
        {
            // telemetryAccess.TrackPageView(nameof(EfCoreModelDialog));
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

            wizardViewModel.Page3LoadedCommand = new RelayCommand(Page3Loaded_Executed);

            InitializeComponent();
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

        public void Page3Loaded_Executed()
        {
            var wea = wizardViewModel.WizardEventArgs;

            ThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                await VS.StatusBar.ShowMessageAsync(ReverseEngineerLocale.LoadingOptions);

                wea.ModelingOptionsDialog = this;

                var neededPackages = await wea.Project.GetNeededPackagesAsync(wea.Options);
                wea.Options.InstallNuGetPackage = neededPackages
                    .Exists(p => p.DatabaseTypes.Contains(wea.Options.DatabaseType) && !p.Installed);

                await wizardViewModel.Bll.GetModelOptionsAsync(wea.Options, wea.Project.Name, wea);

                await VS.StatusBar.ClearAsync();
            });

            FirstTextBox.Focus();
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
    }
}
