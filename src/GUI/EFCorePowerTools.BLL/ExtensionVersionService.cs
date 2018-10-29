namespace EFCorePowerTools.BLL
{
    using System;
    using Shared.BLL;
    using Shared.DAL;
    using Shared.Models;

    public class ExtensionVersionService : IExtensionVersionService
    {
        private readonly IDotNetAccess _dotNetAccess;

        public ExtensionVersionService(IDotNetAccess dotNetAccess)
        {
            _dotNetAccess = dotNetAccess ?? throw new ArgumentNullException(nameof(dotNetAccess));
        }

        void IExtensionVersionService.SetExtensionVersion(AboutExtensionModel aboutExtensionModel)
        {
            if (aboutExtensionModel == null)
                throw new ArgumentNullException(nameof(aboutExtensionModel));

            aboutExtensionModel.ExtensionVersion = _dotNetAccess.GetExtensionVersion();
        }
    }
}