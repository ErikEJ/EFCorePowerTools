using BaseClasses;
using Community.VisualStudio.Toolkit;
using System.ComponentModel;
using System.Runtime.InteropServices;

// ReSharper disable once CheckNamespace
namespace EFCorePowerTools
{
    internal class OptionsProvider
    {
        [ComVisible(true)]
        public class AdvancedOptions : BaseOptionPage<EFCorePowerTools.AdvancedOptions> { }
    }

    public class AdvancedOptions : BaseOptionModel<AdvancedOptions>, IRatingConfig
    {
        [Category("Preview Features"),
        DisplayName(@"Discover multiple result sets from SQL stored procedures"),
        Description("Discover multiple result sets from SQL stored procedures, requires Dapper"),
        DefaultValue(false)]
        public bool DiscoverMultipleResultSets { get; set; }

        [Category("Reverse Engineering"),
        DisplayName(@"Open generated DbContext"),
        Description("Open the generated DbContext after reverse engineering"),
        DefaultValue(true)]
        public bool OpenGeneratedDbContext { get; set; } = true;

        [Category("Reverse Engineering"),
        DisplayName(@"Run cleanup of obsolete files"),
        Description("Remove obsolete files after reverse engineering"),
        DefaultValue(true)]
        public bool RunCleanup { get; set; } = true;

        [Category("Reverse Engineering"),
        DisplayName(@"Save connection name"),
        Description("Save connection name in efpt.config.json"),
        DefaultValue(true)]
        public bool IncludeUiHintInConfig { get; set; } = true;

        [Category("Reverse Engineering"),
        DisplayName(@"Merge .dacpac files"),
        Description("Merge dependent .dacpac files"),
        DefaultValue(false)]
        public bool MergeDacpacs { get; set; }

        [Category("Other"),
        DisplayName(@"Participate in Telemetry"),
        Description("Help improve the EF Core Power Tools by providing anynonymous usage data and crash reports"),
        DefaultValue(true)]
        public bool ParticipateInTelemetry { get; set; } = true;

        [Browsable(false)]
        public int RatingRequests { get; set; }
    }
}
