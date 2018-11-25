namespace EFCorePowerTools.DAL
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using ErikEJ.SqlCeScripting;
    using Shared.DAL;

    public class FileSystemAccess : IFileSystemAccess
    {
        Version IFileSystemAccess.GetInstalledSqlCe40Version()
        {
            try
            {
                return new SqlCeHelper4().IsV40Installed();
            }
            catch
            {
                return null;
            }
        }

        void IFileSystemAccess.WriteAllLines(string fileName, IEnumerable<string> lines)
        {
            if (fileName == null)
                throw new ArgumentNullException(nameof(fileName));
            if (lines == null)
                throw new ArgumentNullException(nameof(lines));
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException(@"Value must be filled.", nameof(fileName));

            File.WriteAllLines(fileName, lines, Encoding.UTF8);
        }

        string[] IFileSystemAccess.ReadAllLines(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException(nameof(fileName));
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException(@"Value must be filled.", nameof(fileName));

            return File.ReadAllLines(fileName, Encoding.UTF8);
        }
    }
}