using System.ComponentModel;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

// ReSharper disable once CheckNamespace
namespace EFCorePowerTools
{
    [Guid(GuidList.guidOptionsPageGeneral)]
    public class OptionsPageGeneral : DialogPage
    {
        protected override void OnActivate(CancelEventArgs e)
        {
            ModelCacheTimeToLive = Properties.Settings.Default.ModelCacheTimeToLive;
            base.OnActivate(e);
        }

        [Category("Reverse Engineer"),
        DisplayName(@"TTL for DatabaseModel cache"),
        Description("Sets the time to live value for the cache of persisted database models"),
        DefaultValue(90)]
        public int ModelCacheTimeToLive { get; set; }

        protected override void OnApply(PageApplyEventArgs e)
        {
            Properties.Settings.Default.ModelCacheTimeToLive = ModelCacheTimeToLive;
            Properties.Settings.Default.Save();
            base.OnApply(e);
        }
    }
}
