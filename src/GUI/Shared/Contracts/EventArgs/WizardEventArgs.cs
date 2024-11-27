using Community.VisualStudio.Toolkit;

namespace EFCorePowerTools.Contracts.EventArgs
{
    public class WizardEventArgs : System.EventArgs
    {
        public Project Project { get; set; }

        public string OptionsPath { get; set; }

        public string Filename { get; set; }

        public bool OnlyGenerate { get; set; }

        public bool FromSqlProject { get; set; }

        public string UiHint { get; set; }
    }
}
