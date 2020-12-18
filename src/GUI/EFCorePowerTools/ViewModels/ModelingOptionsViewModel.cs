using EFCorePowerTools.Contracts.Views;

namespace EFCorePowerTools.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Input;
    using Contracts.EventArgs;
    using Contracts.ViewModels;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;
    using Shared.DAL;
    using Shared.Models;

    public class ModelingOptionsViewModel : ViewModelBase, IModelingOptionsViewModel
    {
        private readonly IVisualStudioAccess _visualStudioAccess;
        private readonly Func<IAdvancedModelingOptionsDialog> _advancedModelingOptionsDialogFactory;

        private string _title;
        private bool _mayIncludeConnectionString;

        public event EventHandler<CloseRequestedEventArgs> CloseRequested;

        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand AdvancedCommand { get; }

        public ModelingOptionsModel Model { get; }
        public IReadOnlyList<string> GenerationModeList { get; }
        public IReadOnlyList<string> HandlebarsLanguageList { get; }

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

        public bool MayIncludeConnectionString
        {
            get => _mayIncludeConnectionString;
            private set
            {
                if (value == _mayIncludeConnectionString) return;
                _mayIncludeConnectionString = value;
                RaisePropertyChanged();
            }
        }

        public ModelingOptionsViewModel(IVisualStudioAccess visualStudioAccess, 
            Func<IAdvancedModelingOptionsDialog> advancedModelingOptionsDialogFactory)
        {
            _visualStudioAccess = visualStudioAccess;
            _advancedModelingOptionsDialogFactory = advancedModelingOptionsDialogFactory;

            Title = string.Empty;
            MayIncludeConnectionString = true;

            OkCommand = new RelayCommand(Ok_Executed);
            CancelCommand = new RelayCommand(Cancel_Executed);
            AdvancedCommand = new RelayCommand(Advanced_Executed);

            Model = new ModelingOptionsModel();
            Model.PropertyChanged += Model_PropertyChanged;
            GenerationModeList = new[]
            {
                "EntityTypes & DbContext",
                "DbContext only",
                "EntityTypes only"
            };
            HandlebarsLanguageList = new[]
            {
                "C#",
                "TypeScript"
            };
        }

        private void Ok_Executed()
        {
            if (string.IsNullOrWhiteSpace(Model.Namespace))
            {
                _visualStudioAccess.ShowMessage("Namespace is required");
                return;
            }

            if (string.IsNullOrWhiteSpace(Model.ModelName))
            {
                _visualStudioAccess.ShowMessage("Context name is required");
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
            }
        }

        private void Advanced_Executed()
        {
            IAdvancedModelingOptionsDialog dialog = _advancedModelingOptionsDialogFactory();
            dialog.ApplyPresets(Model);
            var advancedModelingOptionsResult = dialog.ShowAndAwaitUserResponse(true);
            if (!advancedModelingOptionsResult.ClosedByOK)
                return;

            Model.UseDbContextSplitting = advancedModelingOptionsResult.Payload.UseDbContextSplitting;
            Model.MapSpatialTypes = advancedModelingOptionsResult.Payload.MapSpatialTypes;
            Model.MapNodaTimeTypes = advancedModelingOptionsResult.Payload.MapNodaTimeTypes;
            Model.UseEf6Pluralizer = advancedModelingOptionsResult.Payload.UseEf6Pluralizer;
            Model.UseBoolPropertiesWithoutDefaultSql = advancedModelingOptionsResult.Payload.UseBoolPropertiesWithoutDefaultSql;
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
            Model.ModelNamespace = presets.ModelNamespace;
            Model.ContextNamespace = presets.ContextNamespace;
            Model.ModelName = presets.ModelName;
            Model.UseDataAnnotations = presets.UseDataAnnotations;
            Model.UseDbContextSplitting = presets.UseDbContextSplitting;
            Model.ProjectName = presets.ProjectName;
            Model.DacpacPath = presets.DacpacPath;
            Model.MapSpatialTypes = presets.MapSpatialTypes;
            Model.MapNodaTimeTypes = presets.MapNodaTimeTypes;
            Model.UseEf6Pluralizer = presets.UseEf6Pluralizer;
            Model.UseBoolPropertiesWithoutDefaultSql = presets.UseBoolPropertiesWithoutDefaultSql;

            Title = $"Generate EF Core Model in Project {Model.ProjectName}";
        }
    }
}