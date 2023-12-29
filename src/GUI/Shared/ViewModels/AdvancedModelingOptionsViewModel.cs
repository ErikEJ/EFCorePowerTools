using System;
using System.Windows.Input;
using EFCorePowerTools.Common.Models;
using EFCorePowerTools.Contracts.EventArgs;
using EFCorePowerTools.Contracts.ViewModels;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace EFCorePowerTools.ViewModels
{
    public class AdvancedModelingOptionsViewModel : ViewModelBase, IAdvancedModelingOptionsViewModel
    {
        public AdvancedModelingOptionsViewModel()
        {
            OkCommand = new RelayCommand(Ok_Executed);
            CancelCommand = new RelayCommand(Cancel_Executed);

            Model = new ModelingOptionsModel();
        }

        public event EventHandler<CloseRequestedEventArgs> CloseRequested;

        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }

        public ModelingOptionsModel Model { get; }

        void IAdvancedModelingOptionsViewModel.ApplyPresets(ModelingOptionsModel presets)
        {
            Model.UseDbContextSplitting = presets.UseDbContextSplitting;
            Model.MapSpatialTypes = presets.MapSpatialTypes;
            Model.MapHierarchyId = presets.MapHierarchyId;
            Model.MapNodaTimeTypes = presets.MapNodaTimeTypes;
            Model.UseEf6Pluralizer = presets.UseEf6Pluralizer;
            Model.UseBoolPropertiesWithoutDefaultSql = presets.UseBoolPropertiesWithoutDefaultSql;
            Model.UseNoDefaultConstructor = presets.UseNoDefaultConstructor;
            Model.UseNoNavigations = presets.UseNoNavigations;
            Model.UseNoObjectFilter = presets.UseNoObjectFilter;
            Model.UseNullableReferences = presets.UseNullableReferences;
            Model.UseSchemaFolders = presets.UseSchemaFolders;
            Model.UseManyToManyEntity = presets.UseManyToManyEntity;
            Model.UseDateOnlyTimeOnly = presets.UseDateOnlyTimeOnly;
            Model.ContextNamespace = presets.ContextNamespace;
            Model.OutputContextPath = presets.OutputContextPath;
            Model.ModelNamespace = presets.ModelNamespace;
            Model.UseSchemaNamespaces = presets.UseSchemaNamespaces;
            Model.T4TemplatePath = presets.T4TemplatePath;
        }

        private void Ok_Executed()
        {
            CloseRequested?.Invoke(this, new CloseRequestedEventArgs(true));
        }

        private void Cancel_Executed()
        {
            CloseRequested?.Invoke(this, new CloseRequestedEventArgs(false));
        }
    }
}
