using System;
using System.Collections.Generic;
using System.Windows;
using EFCorePowerTools.Common.Models;
using EFCorePowerTools.Contracts.ViewModels;
using EFCorePowerTools.Contracts.Views;
using EFCorePowerTools.Contracts.Wizard;
using EFCorePowerTools.Extensions;
using EFCorePowerTools.Handlers.ReverseEngineer;
using EFCorePowerTools.Helpers;
using EFCorePowerTools.Locales;
using EFCorePowerTools.Messages;
using EFCorePowerTools.ViewModels;
using Microsoft.VisualStudio.Shell;

namespace EFCorePowerTools.Wizard
{
    public partial class Wiz3_EfCoreModelDialog : WizardResultPageFunction, IModelingOptionsDialog
    {
        private readonly IWizardView wizardView;
        private readonly WizardDataViewModel wizardViewModel;
        private readonly Func<ModelingOptionsModel> getDialogResultPg3;
        private readonly Action<ModelingOptionsModel> applyPresets;
        private readonly Action<TemplateTypeItem, IList<TemplateTypeItem>> setTemplateTypes;
        private bool isRunningAgain = false;

        public Wiz3_EfCoreModelDialog(WizardDataViewModel viewModel, IWizardView wizardView)
            : base(viewModel, wizardView)
        {
#pragma warning disable S125 // Sections of code should not be commented out
            // telemetryAccess.TrackPageView(nameof(EfCoreModelDialog));
            this.wizardView = wizardView;
#pragma warning restore S125 // Sections of code should not be commented out
            this.wizardViewModel = viewModel;

            getDialogResultPg3 = () => viewModel.Model;
            applyPresets = (options) =>
            {
                if (!isRunningAgain)
                {
                    viewModel.ApplyPresets(options);
                }
            };

            setTemplateTypes = (templateType, templateTypes) =>
            {
                // ensure that subsequent navigations do not duplicate items
                if (viewModel.TemplateTypeList.Count == 0)
                {
                    foreach (var item in templateTypes)
                    {
                        viewModel.TemplateTypeList.Add(item);
                    }

                    viewModel.SelectedTemplateType = templateType.Key;
                }
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

                var isSuccessful = false;
                ThreadHelper.JoinableTaskFactory.Run(async () =>
                {
                    isSuccessful = await InvokeWithErrorHandlingAsync(async () =>
                    {
                        var neededPackages = await wea.Project.GetNeededPackagesAsync(wea.Options);
                        wea.Options.InstallNuGetPackage = neededPackages
                            .Exists(p => p.DatabaseTypes.Contains(wea.Options.DatabaseType) && !p.Installed);

                        var result = await wizardViewModel.Bll.GetModelOptionsAsync(wea.Options, wea.Project.Name, wea);

                        if (wea.NewOptions)
                        {
                            // HACK Work around for issue with web app project system on initial run
                            wea.UserOptions = null;
                        }

                        return result;
                    });

                    wizardViewModel.IsPage3Initialized = true;
                    NextButton.IsEnabled = isSuccessful;
                });

                FirstTextBox.Focus();
            }

            if (IsPageDirty)
            {
                NextButton.IsEnabled = true;
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            ShowAndAwaitUserResponse(true);

            // Go to next wizard page
            if (wizardViewModel.IsPage3Initialized && !IsPageDirty)
            {
                NavigationService.GoForward();
            }
            else
            {
                var wizardPage4 = new Wiz4_StatusDialog((WizardDataViewModel)DataContext, wizardView);
                wizardPage4.Return += WizardPage_Return;
                NavigationService?.Navigate(wizardPage4);
                Statusbar.Status.ShowStatus(ReverseEngineerLocale.StatusbarGeneratingFiles);
                GenerateFiles_Click(sender, e);
            }
        }

        private void GenerateFiles_Click(object sender, RoutedEventArgs e)
        {
            var wea = wizardViewModel.WizardEventArgs;
            var vm = wizardViewModel;

            // Update the selected handlebars language combo index - it'll be referenced on wiz page 4
            // and used if use "Customize code using templates" [UseHandlebars] checkbox is selected
            wea.Options.SelectedHandlebarsLanguage = wizardViewModel.Model.SelectedHandlebarsLanguage;
            var isHandleBarsLanguage = vm.Model.SelectedHandlebarsLanguage == 0 || vm.Model.SelectedHandlebarsLanguage == 1;
            wea.Options.UseHandleBars = vm.Model.UseHandlebars && isHandleBarsLanguage;
            // Removed redundant assignment to SelectedHandlebarsLanguage
            wea.Options.UseT4 = vm.Model.UseHandlebars && !isHandleBarsLanguage;
            wea.Options.UseT4Split = vm.Model.UseHandlebars && vm.Model.SelectedHandlebarsLanguage == 4;  // T4 (Split DbContext)
            if (wea.Options.UseT4Split)
            {
                wea.Options.UseT4 = false;
            }

            // Ensure that userOptions hint matches the options hint
            if (wea.UserOptions == null)
            {
                wea.UserOptions = new ReverseEngineerUserOptions
                {
                    UiHint = wea.Options.UiHint,
                };
            }

            if (IsPageDirty)
            {
                var isSuccessful2 = false;
                ThreadHelper.JoinableTaskFactory.Run(async () =>
                {
                    isSuccessful2 = await InvokeWithErrorHandlingAsync(async () =>
                    {
                        isRunningAgain = true;
                        var result = await wizardViewModel.Bll.GetModelOptionsAsync(wea.Options, wea.Project.Name, wea);
                        if (wea.NewOptions)
                        {
                            // HACK Work around for issue with web app project system on initial run
                            wea.UserOptions = null;
                        }

                        return result;
                    });

                    NextButton.IsEnabled = isSuccessful2;
                });
            }

            this.applyPresets(wizardViewModel.Model);

            Telemetry.TrackEvent("PowerTools.ReverseEngineer");
        }



#pragma warning disable SA1202 // Elements should be ordered by access
        public (bool ClosedByOK, ModelingOptionsModel Payload) ShowAndAwaitUserResponse(bool modal)
#pragma warning restore SA1202 // Elements should be ordered by access
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

        private void OpenBrowserHelp(object sender, RoutedEventArgs e)
        {
            OpenBrowserWithLink("https://github.com/ErikEJ/EFCorePowerTools/wiki/Reverse-Engineering");
        }

        private void OpenBrowserRate(object sender, RoutedEventArgs e)
        {
            OpenBrowserWithLink("https://marketplace.visualstudio.com/items?itemName=ErikEJ.EFCorePowerTools&ssr=false#review-details");
        }
    }
}