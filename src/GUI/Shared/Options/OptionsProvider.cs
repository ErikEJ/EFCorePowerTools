using System.Runtime.InteropServices;
using Community.VisualStudio.Toolkit;

namespace EFCorePowerTools.Options
{
    internal class OptionsProvider
    {
        [ComVisible(true)]
        public class AdvancedOptions : BaseOptionPage<EFCorePowerTools.AdvancedOptions>
        {
        }
    }
}