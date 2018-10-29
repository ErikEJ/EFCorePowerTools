namespace EFCorePowerTools.BLL
{
    using System;
    using Shared.Models;
    using Shared.BLL;
    using Shared.DAL;

    public class InstalledComponentsService : IInstalledComponentsService
    {
        private readonly IVisualStudioAccess _visualStudioAccess;
        private readonly IFileSystemAccess _fileSystemAccess;
        private readonly IDotNetAccess _dotNetAccess;

        public InstalledComponentsService(IVisualStudioAccess visualStudioAccess,
                                          IFileSystemAccess fileSystemAccess,
                                          IDotNetAccess dotNetAccess)
        {
            _visualStudioAccess = visualStudioAccess ?? throw new ArgumentNullException(nameof(visualStudioAccess));
            _fileSystemAccess = fileSystemAccess ?? throw new ArgumentNullException(nameof(fileSystemAccess));
            _dotNetAccess = dotNetAccess ?? throw new ArgumentNullException(nameof(dotNetAccess));
        }

        void IInstalledComponentsService.SetMissingComponentData(AboutExtensionModel aboutExtensionModel)
        {
            if (aboutExtensionModel == null)
                throw new ArgumentNullException(nameof(aboutExtensionModel));

            if (aboutExtensionModel.SqlServerCompact40GacVersion == null)
                aboutExtensionModel.SqlServerCompact40GacVersion = _fileSystemAccess.GetInstalledSqlCe40Version();

            if (aboutExtensionModel.SqlServerCompact40DbProviderInstalled == null)
                aboutExtensionModel.SqlServerCompact40DbProviderInstalled = _dotNetAccess.DoesDbProviderFactoryExist(Shared.Resources.SqlCompact40InvariantName);

            if (aboutExtensionModel.SqlServerCompact40DdexProviderInstalled == null)
                aboutExtensionModel.SqlServerCompact40DdexProviderInstalled = _visualStudioAccess.IsDdexProviderInstalled(new Guid(Shared.Resources.SqlCompact40Provider));

            if (aboutExtensionModel.SqlServerCompact40SimpleDdexProviderInstalled == null)
                aboutExtensionModel.SqlServerCompact40SimpleDdexProviderInstalled = _visualStudioAccess.IsDdexProviderInstalled(new Guid(Shared.Resources.SqlCompact40PrivateProvider));

            if (aboutExtensionModel.SqLiteAdoNetProviderVersion == null)
                aboutExtensionModel.SqLiteAdoNetProviderVersion = _dotNetAccess.GetAssemblyVersion("System.Data.SQLite");

            if (aboutExtensionModel.SqLiteEf6DbProviderInstalled == null)
                aboutExtensionModel.SqLiteEf6DbProviderInstalled = _visualStudioAccess.IsSqLiteDbProviderInstalled();

            if (aboutExtensionModel.SqLiteDdexProviderInstalled == null)
                aboutExtensionModel.SqLiteDdexProviderInstalled = _visualStudioAccess.IsDdexProviderInstalled(new Guid(Shared.Resources.SQLiteProvider));

            if (aboutExtensionModel.SqlLiteSimpleDdexProviderInstalled == null)
                aboutExtensionModel.SqlLiteSimpleDdexProviderInstalled = _visualStudioAccess.IsDdexProviderInstalled(new Guid(Shared.Resources.SQLitePrivateProvider));
        }
    }
}