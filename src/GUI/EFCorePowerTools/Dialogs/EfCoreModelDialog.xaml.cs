using System.Collections.Generic;
using System.Windows;
using ErikEJ.SqlCeToolbox.Helpers;
using ReverseEngineer20;

namespace ErikEJ.SqlCeToolbox.Dialogs
{
    public partial class EfCoreModelDialog
    {
        private readonly ReverseEngineerOptions _options;
        public EfCoreModelDialog(ReverseEngineerOptions options)
        {
            _options = options;
            Telemetry.TrackPageView(nameof(EfCoreModelDialog));
            InitializeComponent();
            Background = VsThemes.GetWindowBackground();
            if (_options != null)
            {
                chkDataAnnoations.IsChecked = !options.UseFluentApiOnly;
                chkUseDatabaseNames.IsChecked = options.UseDatabaseNames;
                chkPluralize.IsChecked = options.UseInflector;
                chkHandlebars.IsChecked = options.UseHandleBars;
                chkIdReplace.IsChecked = options.IdReplace;
                chkIncludeConnectionString.IsChecked = options.IncludeConnectionString;
                ModelName = options.ContextClassName;
                NameSpace = options.ProjectRootNamespace;
                OutputPath = options.OutputPath;
                cmbLanguage.SelectedIndex = options.SelectedToBeGenerated;
                SetCheckState();
            }
        }

        public string ProjectName
        {
            set
            {
                Title = $"Generate EF Core Model in Project {value}";
            }
        }

        public  string DacpacPath
        {
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    chkIncludeConnectionString.IsEnabled = false;
                    chkIncludeConnectionString.IsChecked = false;
                }
            }
        }

        public bool UseDataAnnotations => chkDataAnnoations.IsChecked != null && chkDataAnnoations.IsChecked.Value;

        public bool UseDatabaseNames => chkUseDatabaseNames.IsChecked != null && chkUseDatabaseNames.IsChecked.Value;

        public bool UsePluralizer => chkPluralize.IsChecked != null && chkPluralize.IsChecked.Value;

        public bool UseHandelbars => chkHandlebars.IsChecked != null && chkHandlebars.IsChecked.Value;

        public bool IncludeConnectionString => chkIncludeConnectionString.IsChecked != null && chkIncludeConnectionString.IsChecked.Value;

        public bool ReplaceId => chkIdReplace.IsChecked != null && chkIdReplace.IsChecked.Value;

        public string ModelName 
        {
            get
            {
                return textBox1.Text;
            }
            set
            {
                textBox1.Text = value;
            }
        }

        public string NameSpace
        {
            get
            {
                return txtNameSpace.Text;
            }
            set
            {
                txtNameSpace.Text = value;
            }
        }

        public string OutputPath
        {
            get
            {
                return txtOutputPath.Text;
            }
            set
            {
                txtOutputPath.Text = value;
            }
        }

        public bool InstallNuGetPackage
        {
            get
            {
                return chkInstallNuGet.IsChecked.HasValue && chkInstallNuGet.IsChecked.Value;
            }
            set
            {
                chkInstallNuGet.IsChecked = value;
            }
        }

        public int SelectedTobeGenerated => cmbLanguage.SelectedIndex;

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNameSpace.Text))
            {
                EnvDteHelper.ShowMessage("Namespace is required");
                return;
            }
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                EnvDteHelper.ShowMessage("Context name is required");
                return;
            }
            DialogResult = true;
            Close();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            textBox1.Focus();
            cmbLanguage.ItemsSource = new List<string> { "EntityTypes & DbContext", "DbContext only", "EntityTypes only" };
            cmbLanguage.SelectedIndex = 0;
            if (_options != null)
            {
                cmbLanguage.SelectedIndex = _options.SelectedToBeGenerated;
            }
        }

        private void chkPluralize_Checked(object sender, RoutedEventArgs e)
        {
            chkUseDatabaseNames.IsEnabled = true;
            SetCheckState();
        }

        private void SetCheckState()
        {
            if (chkPluralize.IsChecked.Value)
            {
                chkUseDatabaseNames.IsChecked = false;
                chkUseDatabaseNames.IsEnabled = false;
            }
            if (chkUseDatabaseNames.IsChecked.Value)
            {
                chkPluralize.IsChecked = false;
                chkPluralize.IsEnabled = false;
            }
        }

        private void chkUseDatabaseNames_Checked(object sender, RoutedEventArgs e)
        {
            chkPluralize.IsEnabled = true;
            SetCheckState();
        }

        private void chkPluralize_Unchecked(object sender, RoutedEventArgs e)
        {
            chkUseDatabaseNames.IsEnabled = true;
        }

        private void chkUseDatabaseNames_Unchecked(object sender, RoutedEventArgs e)
        {
            chkPluralize.IsEnabled = true;
        }
    }
}
