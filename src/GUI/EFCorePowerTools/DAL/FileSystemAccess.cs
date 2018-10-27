namespace EFCorePowerTools.DAL
{
    using System;
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
    }
}