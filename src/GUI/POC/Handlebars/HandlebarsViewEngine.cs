using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandlebarsDotNet
{

    public abstract class ViewEngineFileSystem
    {
        public abstract string GetFileContent(string filename);

        private static string GetDir(string currentFilePath)
        {
            if (currentFilePath == "") return null;
            var parts = currentFilePath.Split(new[] {'\\', '/'});
            if (parts.Length == 1) return "";
            return string.Join("/", parts.Take(parts.Length - 1));
        }

        public string Closest(string filename, string otherFileName)
        {
            var dir = GetDir(filename);
            while (true)
            {
                if (dir == null) break;
                var fullFileName = CombinePath(dir, otherFileName);
                if (this.FileExists(fullFileName)) return fullFileName;
                dir = GetDir(dir);
            }
            return null;
        }

        protected abstract string CombinePath(string dir, string otherFileName);

        public abstract bool FileExists(string filePath);
    }
}
