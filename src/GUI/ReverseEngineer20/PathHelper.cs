using System.IO;

namespace ReverseEngineer20
{
    public static class PathHelper
    {
        public static string GetAbsPath(string outputPath, string fullName)
        {
            //   ' The output folder can have these patterns:
            //   ' 1) "\\server\folder"
            //   ' 2) "drive:\folder"
            //   ' 3) "..\..\folder"
            //   ' 4) "folder"

            if (outputPath.StartsWith(string.Empty + Path.DirectorySeparatorChar + Path.DirectorySeparatorChar))
            {
                return outputPath;
            }
            else if (outputPath.Length >= 2 && outputPath.ToCharArray()[0] == Path.VolumeSeparatorChar)
            {
                return outputPath;
            }
            else if (outputPath.IndexOf("..\\") != -1)
            {
                var projectFolder = Path.GetDirectoryName(fullName);
                while (outputPath.StartsWith("..\\"))
                {
                    outputPath = outputPath.Substring(3);
                    projectFolder = Path.GetDirectoryName(projectFolder);
                }
                return Path.Combine(projectFolder, outputPath);
            }
            else
            {
                var projectFolder = Path.GetDirectoryName(fullName);
                return Path.Combine(projectFolder, outputPath);
            }
        }
    }
}
