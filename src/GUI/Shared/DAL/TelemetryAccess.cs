namespace EFCorePowerTools.DAL
{
    using Shared.DAL;

    public class TelemetryAccess : ITelemetryAccess
    {
        void ITelemetryAccess.TrackPageView(string key)
        {
            // Ignore
        }
    }
}