namespace EFCorePowerTools.DAL
{
    using Common.DAL;

    public class TelemetryAccess : ITelemetryAccess
    {
        void ITelemetryAccess.TrackPageView(string key)
        {
            // Ignore
        }
    }
}