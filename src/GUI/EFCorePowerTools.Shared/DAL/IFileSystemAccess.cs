namespace EFCorePowerTools.Shared.DAL
{
    using System;

    public interface IFileSystemAccess
    {
        Version GetInstalledSqlCe40Version();
    }
}