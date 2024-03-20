// ReSharper disable once CheckNamespace
using System.ComponentModel;
using Community.VisualStudio.Toolkit;

namespace EFCorePowerTools
{
    public class AdvancedOptions : BaseOptionModel<AdvancedOptions>
    {
        [Category("Preview Features")]
        [DisplayName(@"Discover multiple result sets from SQL stored procedures")]
        [Description("Discover multiple result sets from SQL stored procedures, requires Dapper")]
        [DefaultValue(false)]
        public bool DiscoverMultipleResultSets { get; set; }

        [Category("Reverse Engineering")]
        [DisplayName(@"Open generated DbContext")]
        [Description("Open the generated DbContext after reverse engineering")]
        [DefaultValue(true)]
        public bool OpenGeneratedDbContext { get; set; } = true;

        [Category("Reverse Engineering")]
        [DisplayName(@"Run cleanup of obsolete files")]
        [Description("Remove obsolete files after reverse engineering")]
        [DefaultValue(true)]
        public bool RunCleanup { get; set; } = true;

        [Category("Reverse Engineering")]
        [DisplayName(@"Merge .dacpac files")]
        [Description("Merge dependent .dacpac files")]
        [DefaultValue(false)]
        public bool MergeDacpacs { get; set; }

        [Category("Reverse Engineering")]
        [DisplayName(@"Use alternate result set discovery")]
        [Description("Use sp_describe_first_result_set to retrieve stored procedure result sets")]
        [DefaultValue(false)]
        public bool UseLegacyResultSetDiscovery { get; set; }

        [Category("Reverse Engineering")]
        [DisplayName(@"Prefer async calls")]
        [Description("Prefer async/await over synchronous calls")]
        [DefaultValue(true)]
        public bool PreferAsyncCalls { get; set; } = true;

        [Category("Other")]
        [DisplayName(@"Participate in Telemetry")]
        [Description("Help improve the EF Core Power Tools by providing anynonymous usage data and crash reports")]
        [DefaultValue(true)]
        public bool ParticipateInTelemetry { get; set; } = true;
    }
}
