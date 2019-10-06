using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;

namespace ReverseEngineer20.ReverseEngineer
{
    public class EfRevEngLauncher
    {
        private readonly ReverseEngineerCommandOptions options;

        public EfRevEngLauncher(ReverseEngineerCommandOptions options)
        {
            this.options = options;
        }

        public ReverseEngineerResult GetOutput()
        {
            var path = Path.GetTempFileName() + "json";
            File.WriteAllText(path, options.Write());

            var launchPath = DropNetCoreFiles();

            var startInfo = new ProcessStartInfo
            {
                FileName = Path.Combine(launchPath ?? throw new InvalidOperationException(), "efreveng.exe"),
                Arguments = "\"" + path + "\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            var standardOutput = new StringBuilder();
            using (var process = Process.Start(startInfo))
            {
                while (process != null && !process.HasExited)
                {
                    standardOutput.Append(process.StandardOutput.ReadToEnd());
                }
                if (process != null) standardOutput.Append(process.StandardOutput.ReadToEnd());
            }

            return BuildResult(standardOutput.ToString());
        }

        private ReverseEngineerResult BuildResult(string output)
        {
            if (output.StartsWith("Result:" + Environment.NewLine))
            {
                var result = output.Split(new[] { "Result:" + Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                TryRead(result[0], out ReverseEngineerResult deserialized);

                return deserialized;
            }

            if (output.StartsWith("Error:" + Environment.NewLine))
            {
                var result = output.Split(new[] { "Error:" + Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                throw new Exception("Reverse engineer error: " + Environment.NewLine + result[0]);
            }

            return null;
        }

        private static bool TryRead<T>(string options, out T deserialized) where T : class, new()
        {
            try
            {
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(options));
                var ser = new DataContractJsonSerializer(typeof(T));
                deserialized = ser.ReadObject(ms) as T;
                ms.Close();
                return true;
            }
            catch
            {
                deserialized = null;
                return false;
            }
        }

        private string DropNetCoreFiles()
        {
            var toDir = Path.Combine(Path.GetTempPath(), "efreveng");
            var fromDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            Debug.Assert(fromDir != null, nameof(fromDir) + " != null");
            Debug.Assert(toDir != null, nameof(toDir) + " != null");

            if (Directory.Exists(toDir))
            {
                Directory.Delete(toDir, true);
            }

            Directory.CreateDirectory(toDir);

            ZipFile.ExtractToDirectory(Path.Combine(fromDir, "efreveng.exe.zip"), toDir);
            
            return toDir;
        }
    }
}