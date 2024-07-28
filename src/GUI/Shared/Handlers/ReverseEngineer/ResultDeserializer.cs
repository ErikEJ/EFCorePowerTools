using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using RevEng.Common;

namespace EFCorePowerTools.Handlers.ReverseEngineer
{
    public class ResultDeserializer
    {
        public ReverseEngineerResult BuildResult(string output)
        {
            var resultParts = output.Split(new[] { "Result:" + Environment.NewLine }, StringSplitOptions.None);
            if (resultParts.Length == 2 && TryRead(resultParts[1], out ReverseEngineerResult deserialized))
            {
                return deserialized;
            }

            var errorParts = output.Split(new[] { "Error:" + Environment.NewLine }, StringSplitOptions.None);
            if (errorParts.Length == 2)
            {
                throw new InvalidOperationException("Reverse engineer error: " + Environment.NewLine + errorParts[1]);
            }

            throw new InvalidOperationException($"Reverse engineer error: Unable to launch external process: {Environment.NewLine + output}");
        }

        public List<TableModel> BuildTableResult(string output)
        {
            var resultParts = output.Split(new[] { "Result:" + Environment.NewLine }, StringSplitOptions.None);
            if (resultParts.Length == 2 && TryRead(resultParts[1], out List<TableModel> deserialized))
            {
                return deserialized;
            }

            var errorParts = output.Split(new[] { "Error:" + Environment.NewLine }, StringSplitOptions.None);
            if (errorParts.Length == 2)
            {
                throw new InvalidOperationException("Table list error: " + Environment.NewLine + errorParts[1]);
            }

            throw new InvalidOperationException($"Table list error: Unable to launch external process: {Environment.NewLine + output}");
        }

        public string BuildDiagramResult(string output)
        {
            var resultParts = output.Split(new[] { "Result:" + Environment.NewLine }, StringSplitOptions.None);
            if (resultParts.Length == 2)
            {
                return resultParts[1].Trim();
            }

            var errorParts = output.Split(new[] { "Error:" + Environment.NewLine }, StringSplitOptions.None);
            if (errorParts.Length == 2)
            {
                throw new InvalidOperationException("Result error: " + Environment.NewLine + errorParts[1]);
            }

            throw new InvalidOperationException($"Launch error: Unable to launch external process: {Environment.NewLine + output}");
        }

        private static bool TryRead<T>(string options, out T deserialized)
            where T : class, new()
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
    }
}
