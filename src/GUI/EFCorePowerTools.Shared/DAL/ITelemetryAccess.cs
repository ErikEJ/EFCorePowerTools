namespace EFCorePowerTools.Shared.DAL
{
    public interface ITelemetryAccess
    {
        void TrackPageView(string key);
    }
}