namespace EFCorePowerTools.DAL
{
    using ErikEJ.SqlCeToolbox.Helpers;
    using Shared.DAL;

    public class TelemetryAccess : ITelemetryAccess
    {
        void ITelemetryAccess.TrackPageView(string key)
        {
            Telemetry.TrackPageView(key);
        }
    }
}