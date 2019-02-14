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

        private string _title;
        private bool _mayIncludeConnectionString;

        public event EventHandler<CloseRequestedEventArgs> CloseRequested;

        public ICommand LoadedCommand { get; }
        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }

        public ModelingOptionsModel Model { get; }
        public IReadOnlyList<string> GenerationModeList { get; }

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

        public ModelingOptionsViewModel(IVisualStudioAccess visualStudioAccess)
        {
            _visualStudioAccess = visualStudioAccess;
            Title = string.Empty;

            LoadedCommand = new RelayCommand(Loaded_Executed);
            OkCommand = new RelayCommand(Ok_Executed);
            CancelCommand = new RelayCommand(Cancel_Executed);

            Model = new ModelingOptionsModel();
            Model.PropertyChanged += Model_PropertyChanged;
            GenerationModeList = new[]
            {
                "EntityTypes & DbContext",
                "DbContext only",
                "EntityTypes only"
            };
        }

        private void Loaded_Executed()
        {
            Model.SelectedTobeGenerated = 0;
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
                    break;
            }
        }

        void IModelingOptionsViewModel.ApplyPresets(ModelingOptionsModel presets)
        {
            Model.InstallNuGetPackage = presets.InstallNuGetPackage;
            Model.SelectedTobeGenerated = presets.SelectedTobeGenerated;
            Model.IncludeConnectionString = presets.IncludeConnectionString;
            Model.UseHandelbars = presets.UseHandelbars;
            Model.ReplaceId = presets.ReplaceId;
            Model.UsePluralizer = presets.UsePluralizer;
            Model.UseDatabaseNames = presets.UseDatabaseNames;
            Model.Namespace = presets.Namespace;
            Model.OutputPath = presets.OutputPath;
            Model.ModelName = presets.ModelName;
            Model.UseDataAnnotations = presets.UseDataAnnotations;
            Model.ProjectName = presets.ProjectName;
            Model.DacpacPath = presets.DacpacPath;

            Title = $"Generate EF Core Model in Project {Model.ProjectName}";
        }
    }
}