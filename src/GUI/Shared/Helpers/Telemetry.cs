using System;
using Microsoft.ApplicationInsights;
using Microsoft.VisualStudio.Shell;
using RevEng.Common;

namespace EFCorePowerTools.Helpers
{
    /// <summary>
    /// Reports anonymous usage through ApplicationInsights
    /// </summary>
    public static class Telemetry
    {
        private static TelemetryClient telemetry;

        public static bool Enabled { get; set; }

        public static void Initialize(string version, string vsVersion, string telemetryKey)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (telemetry != null)
            {
                return;
            }

            telemetry = new TelemetryClient();
            telemetry.Context.Session.Id = Guid.NewGuid().ToString();
            telemetry.Context.User.Id = (Environment.UserName + Environment.MachineName).GetHashCode().ToString();
            telemetry.Context.Device.Model = vsVersion;
            telemetry.Context.Device.OperatingSystem = Environment.OSVersion.Version.ToString();
            telemetry.InstrumentationKey = telemetryKey;
            telemetry.Context.Component.Version = version;

            Enabled = true;
        }

        public static void TrackEvent(string key)
        {
            // Ignore
        }

        public static void TrackFrameworkUse(string prefix, CodeGenerationMode codeGenerationMode)
        {
#if !DEBUG
            if (Enabled && telemetry != null)
            {
                telemetry.TrackEvent($"{prefix}:{codeGenerationMode}");
                telemetry.Flush();
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
                if (telemetry != null)
                {
                    telemetry.TrackException(telex);
                    telemetry.Flush();
                }
            }
#endif
        }
    }
}
