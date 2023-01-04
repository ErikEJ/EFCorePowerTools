using System;
using System.Collections.Generic;
using System.Windows.Input;
using EFCorePowerTools.Common.DAL;
using EFCorePowerTools.Common.Models;
using EFCorePowerTools.Contracts.EventArgs;
using EFCorePowerTools.Contracts.ViewModels;
using EFCorePowerTools.Contracts.Views;
using EFCorePowerTools.Locales;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace EFCorePowerTools.ViewModels
{
    public class ModelingOptionsViewModel : ViewModelBase, IModelingOptionsViewModel
    {
        private readonly IVisualStudioAccess visualStudioAccess;
        private readonly Func<IAdvancedModelingOptionsDialog> advancedModelingOptionsDialogFactory;

        private string title;
        private bool mayIncludeConnectionString;

        public ModelingOptionsViewModel(
            IVisualStudioAccess visualStudioAccess,
            Func<IAdvancedModelingOptionsDialog> advancedModelingOptionsDialogFactory)
        {
            this.visualStudioAccess = visualStudioAccess;
            this.advancedModelingOptionsDialogFactory = advancedModelingOptionsDialogFactory;

            Title = string.Empty;
            MayIncludeConnectionString = true;

            OkCommand = new RelayCommand(Ok_Executed);
            CancelCommand = new RelayCommand(Cancel_Executed);
            AdvancedCommand = new RelayCommand(Advanced_Executed);

            Model = new ModelingOptionsModel();
            Model.PropertyChanged += Model_PropertyChanged;
            GenerationModeList = new[]
            {
                ReverseEngineerLocale.EntityTypesAndContext,
                ReverseEngineerLocale.DbContextOnly,
                ReverseEngineerLocale.EntityTypesOnly,
            };
            HandlebarsLanguageList = new[]
            {
                "C# - Handlebars",
                "TypeScript - Handlebars",
                "C# - T4",
                "C# - T4 (POCO)",
            };
        }

        public event EventHandler<CloseRequestedEventArgs> CloseRequested;

        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand AdvancedCommand { get; }

        public ModelingOptionsModel Model { get; }
        public IReadOnlyList<string> GenerationModeList { get; }
        public IReadOnlyList<string> HandlebarsLanguageList { get; }

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

        public bool MayIncludeConnectionString
        {
            get => mayIncludeConnectionString;
            private set
            {
                if (value == mayIncludeConnectionString)
                {
                    return;
                }

                mayIncludeConnectionString = value;
                RaisePropertyChanged();
            }
        }

        void IModelingOptionsViewModel.ApplyPresets(ModelingOptionsModel presets)
        {
            Model.InstallNuGetPackage = presets.InstallNuGetPackage;
            Model.SelectedToBeGenerated = presets.SelectedToBeGenerated;
            Model.SelectedHandlebarsLanguage = presets.SelectedHandlebarsLanguage;
            Model.IncludeConnectionString = presets.IncludeConnectionString;
            Model.UseHandlebars = presets.UseHandlebars;
            Model.UsePluralizer = presets.UsePluralizer;
            Model.UseDatabaseNames = presets.UseDatabaseNames;
            Model.Namespace = presets.Namespace;
            Model.OutputPath = presets.OutputPath;
            Model.OutputContextPath = presets.OutputContextPath;
            Model.UseSchemaFolders = presets.UseSchemaFolders;
            Model.ModelNamespace = presets.ModelNamespace;
            Model.ContextNamespace = presets.ContextNamespace;
            Model.ModelName = presets.ModelName;
            Model.UseDataAnnotations = presets.UseDataAnnotations;
            Model.UseDbContextSplitting = presets.UseDbContextSplitting;
            Model.ProjectName = presets.ProjectName;
            Model.DacpacPath = presets.DacpacPath;
            Model.MapSpatialTypes = presets.MapSpatialTypes;
            Model.MapHierarchyId = presets.MapHierarchyId;
            Model.MapNodaTimeTypes = presets.MapNodaTimeTypes;
            Model.UseEf6Pluralizer = presets.UseEf6Pluralizer;
            Model.UseBoolPropertiesWithoutDefaultSql = presets.UseBoolPropertiesWithoutDefaultSql;
            Model.UseNoDefaultConstructor = presets.UseNoDefaultConstructor;
            Model.UseNullableReferences = presets.UseNullableReferences;
            Model.UseNoObjectFilter = presets.UseNoObjectFilter;
            Model.UseManyToManyEntity = presets.UseManyToManyEntity;
            Model.UseDateOnlyTimeOnly = presets.UseDateOnlyTimeOnly;

            Title = string.Format(ReverseEngineerLocale.GenerateEFCoreModelInProject, Model.ProjectName);
        }

        private void Ok_Executed()
        {
            var individualNamespacesSet = !string.IsNullOrEmpty(Model.ContextNamespace) && !string.IsNullOrEmpty(Model.ModelNamespace);

            if (string.IsNullOrWhiteSpace(Model.Namespace) && !individualNamespacesSet)
            {
                visualStudioAccess.ShowMessage(ReverseEngineerLocale.NamespaceRequired);
                return;
            }

            if (string.IsNullOrWhiteSpace(Model.ModelName))
            {
                visualStudioAccess.ShowMessage(ReverseEngineerLocale.ContextNameRequired);
                return;
            }

            CloseRequested?.Invoke(this, new CloseRequestedEventArgs(true));
        }

        private void Cancel_Executed()
        {
            CloseRequested?.Invoke(this, new CloseRequestedEventArgs(false));
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ModelingOptionsModel.DacpacPath):
                    if (!string.IsNullOrWhiteSpace(Model.DacpacPath))
                    {
                        MayIncludeConnectionString = false;
                        Model.IncludeConnectionString = false;
                    }
                    else
                    {
                        MayIncludeConnectionString = true;
                    }

                    break;

                case nameof(ModelingOptionsModel.SelectedToBeGenerated):
                    if (Model.InstallNuGetPackage && Model.SelectedToBeGenerated == 2)
                    {
                        Model.InstallNuGetPackage = false;
                    }

                    break;
            }
        }

        private void Advanced_Executed()
        {
            IAdvancedModelingOptionsDialog dialog = advancedModelingOptionsDialogFactory();
            dialog.ApplyPresets(Model);
            var advancedModelingOptionsResult = dialog.ShowAndAwaitUserResponse(true);
            if (!advancedModelingOptionsResult.ClosedByOK)
            {
                return;
            }

            Model.UseDbContextSplitting = advancedModelingOptionsResult.Payload.UseDbContextSplitting;
            Model.MapSpatialTypes = advancedModelingOptionsResult.Payload.MapSpatialTypes;
            Model.MapHierarchyId = advancedModelingOptionsResult.Payload.MapHierarchyId;
            Model.MapNodaTimeTypes = advancedModelingOptionsResult.Payload.MapNodaTimeTypes;
            Model.UseEf6Pluralizer = advancedModelingOptionsResult.Payload.UseEf6Pluralizer;
            Model.UseBoolPropertiesWithoutDefaultSql = advancedModelingOptionsResult.Payload.UseBoolPropertiesWithoutDefaultSql;
            Model.UseNoDefaultConstructor = advancedModelingOptionsResult.Payload.UseNoDefaultConstructor;
            Model.UseNullableReferences = advancedModelingOptionsResult.Payload.UseNullableReferences;
            Model.UseNoObjectFilter = advancedModelingOptionsResult.Payload.UseNoObjectFilter;
            Model.UseSchemaFolders = advancedModelingOptionsResult.Payload.UseSchemaFolders;
            Model.UseManyToManyEntity = advancedModelingOptionsResult.Payload.UseManyToManyEntity;
            Model.UseDateOnlyTimeOnly = advancedModelingOptionsResult.Payload.UseDateOnlyTimeOnly;
        }
    }
}
