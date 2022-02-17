using Microsoft.ApplicationInsights;
using Microsoft.VisualStudio.Shell;
using RevEng.Shared;
using System;

namespace EFCorePowerTools.Helpers
{
    /// <summary>
    /// Reports anonymous usage through ApplicationInsights
    /// </summary>
    public static class Telemetry
    {
        private static TelemetryClient _telemetry;

        /// <summary>
        /// Initializes the telemetry client.
        /// </summary>
        public static void Initialize(string version, string vsVersion, string telemetryKey)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (_telemetry != null)
                return;

            _telemetry = new TelemetryClient();
            _telemetry.Context.Session.Id = Guid.NewGuid().ToString();
            _telemetry.Context.User.Id = (Environment.UserName + Environment.MachineName).GetHashCode().ToString();
            _telemetry.Context.Device.Model = vsVersion;
            _telemetry.Context.Device.OperatingSystem = Environment.OSVersion.Version.ToString();
            _telemetry.InstrumentationKey = telemetryKey;
            _telemetry.Context.Component.Version = version;

            Enabled = true;
        }

        public static bool Enabled { get; set; }

        /// <summary>Tracks an event to ApplicationInsights.</summary>
        public static void TrackEvent(string key)
        {
            // Ignore
        }

        public static void TrackFrameworkUse(string prefix, CodeGenerationMode codeGenerationMode)
        {
#if !DEBUG
            if (Enabled && _telemetry != null)
            {
                _telemetry.TrackEvent($"{prefix}:{codeGenerationMode}");
                _telemetry.Flush();
            }
#endif
        }

        /// <summary>Tracks any exception.</summary>
        public static void TrackException(Exception ex)
        {
#if !DEBUG
            if (Enabled)
            {
                var telex = new Microsoft.ApplicationInsights.DataContracts.ExceptionTelemetry(ex);
                if (_telemetry != null)
                {
                    _telemetry.TrackException(telex);
                    _telemetry.Flush();
                }
            }
#endif
        }
    }
}
