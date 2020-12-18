namespace EFCorePowerTools.ViewModels
{
    using Contracts.EventArgs;
    using Contracts.ViewModels;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;
    using Shared.Models;
    using System;
    using System.Windows.Input;

    public class AdvancedModelingOptionsViewModel : ViewModelBase, IAdvancedModelingOptionsViewModel
    {
        public event EventHandler<CloseRequestedEventArgs> CloseRequested;

        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }

        public ModelingOptionsModel Model { get; }

        public AdvancedModelingOptionsViewModel()
        {
            OkCommand = new RelayCommand(Ok_Executed);
            CancelCommand = new RelayCommand(Cancel_Executed);

            Model = new ModelingOptionsModel();
        }

        private void Ok_Executed()
        {
            CloseRequested?.Invoke(this, new CloseRequestedEventArgs(true));
        }

        private void Cancel_Executed()
        {
            CloseRequested?.Invoke(this, new CloseRequestedEventArgs(false));
        }

        void IAdvancedModelingOptionsViewModel.ApplyPresets(ModelingOptionsModel presets)
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
        }
    }
}