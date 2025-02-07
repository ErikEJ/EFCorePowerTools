namespace EFCorePowerTools.Common.DAL
{
    public interface ITelemetryAccess
    {
        void TrackPageView(string key);
    }
}