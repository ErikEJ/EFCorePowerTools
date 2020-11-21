namespace EFCorePowerTools.BLL
{
    using System;
    using Shared.Models;
    using Shared.BLL;
    using Shared.DAL;

    public class InstalledComponentsService : IInstalledComponentsService
    {
        private readonly IVisualStudioAccess _visualStudioAccess;
        private readonly IDotNetAccess _dotNetAccess;

        public InstalledComponentsService(IVisualStudioAccess visualStudioAccess,
                                          IDotNetAccess dotNetAccess)
        {
            _visualStudioAccess = visualStudioAccess ?? throw new ArgumentNullException(nameof(visualStudioAccess));
            _dotNetAccess = dotNetAccess ?? throw new ArgumentNullException(nameof(dotNetAccess));
        }

        void IInstalledComponentsService.SetMissingComponentData(AboutExtensionModel aboutExtensionModel)
        {
            if (aboutExtensionModel == null)
                throw new ArgumentNullException(nameof(aboutExtensionModel));

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