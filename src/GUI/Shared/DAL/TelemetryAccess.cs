using EFCorePowerTools.Common.DAL;

namespace EFCorePowerTools.DAL
{
    public class TelemetryAccess : ITelemetryAccess
    {
        void ITelemetryAccess.TrackPageView(string key)
        {
            // Ignore
        }
    }
}