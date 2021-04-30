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
            base.OnActivate(e);
        }

        [Category("Reverse Engineering"),
        DisplayName(@"Open generated DbContext"),
        Description("Open the generated DbContext after reverse engineering"),
        DefaultValue(true)]
        public bool OpenGeneratedDbContext { get; set; }

        [Category("Other"),
        DisplayName(@"Participate in Telemetry"),
        Description("Help improve the EF Core Power Tools by providing anynonymous usage data and crash reports"),
        DefaultValue(true)]
        public bool ParticipateInTelemetry { get; set; }

        protected override void OnApply(PageApplyEventArgs e)
        {
            Properties.Settings.Default.ParticipateInTelemetry = ParticipateInTelemetry;
            Properties.Settings.Default.OpenGeneratedDbContext = OpenGeneratedDbContext;
            Properties.Settings.Default.Save();
            base.OnApply(e);
        }
    }
}
