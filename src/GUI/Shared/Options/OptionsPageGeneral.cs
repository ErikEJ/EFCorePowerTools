using Microsoft.VisualStudio.Shell;
using System.ComponentModel;
using System.Runtime.InteropServices;

// ReSharper disable once CheckNamespace
namespace EFCorePowerTools
{
    [Guid(GuidList.guidOptionsPageGeneral)]
    public class OptionsPageGeneral : DialogPage
    {
        protected override void OnActivate(CancelEventArgs e)
        {
            ParticipateInTelemetry = Properties.Settings.Default.ParticipateInTelemetry;
            OpenGeneratedDbContext = Properties.Settings.Default.OpenGeneratedDbContext;
            RunCleanup = Properties.Settings.Default.RunCleanup;
            DiscoverMultipleResultSets = Properties.Settings.Default.DiscoverMultipleResultSets;
            IncludeUiHintInConfig = Properties.Settings.Default.IncludeUiHintInConfig;
            base.OnActivate(e);
        }

        [Category("Preview Features"),
        DisplayName(@"Discover multiple result sets from SQL stored procedures"),
        Description("Discover multiple result sets from SQL stored procedures, requires Dapper"),
        DefaultValue(false)]
        public bool DiscoverMultipleResultSets { get; set; }

        [Category("Reverse Engineering"),
        DisplayName(@"Open generated DbContext"),
        Description("Open the generated DbContext after reverse engineering"),
        DefaultValue(true)]
        public bool OpenGeneratedDbContext { get; set; }

        [Category("Reverse Engineering"),
        DisplayName(@"Run cleanup of obsolete files"),
        Description("Remove obsolete files after reverse engineering"),
        DefaultValue(true)]
        public bool RunCleanup { get; set; }

        [Category("Reverse Engineering"),
        DisplayName(@"Save connection name"),
        Description("Save connection name in efpt.config.json"),
        DefaultValue(true)]
        public bool IncludeUiHintInConfig { get; set; }

        [Category("Other"),
        DisplayName(@"Participate in Telemetry"),
        Description("Help improve the EF Core Power Tools by providing anynonymous usage data and crash reports"),
        DefaultValue(true)]
        public bool ParticipateInTelemetry { get; set; }

        protected override void OnApply(PageApplyEventArgs e)
        {
            Properties.Settings.Default.ParticipateInTelemetry = ParticipateInTelemetry;
            Properties.Settings.Default.OpenGeneratedDbContext = OpenGeneratedDbContext;
            Properties.Settings.Default.RunCleanup = RunCleanup;
            Properties.Settings.Default.DiscoverMultipleResultSets = DiscoverMultipleResultSets;
            Properties.Settings.Default.IncludeUiHintInConfig = IncludeUiHintInConfig;
            Properties.Settings.Default.Save();
            base.OnApply(e);
        }
    }
}
