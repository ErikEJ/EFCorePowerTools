using EFCorePowerTools.Common.BLL;
using EFCorePowerTools.Common.DAL;
using EFCorePowerTools.Common.Models;
using System;

[assembly: CLSCompliant(false)]

namespace EFCorePowerTools.BLL
{
    public sealed class ExtensionVersionService : IExtensionVersionService
    {
        private readonly IDotNetAccess dotNetAccess;

        public ExtensionVersionService(IDotNetAccess dotNetAccess)
        {
            this.dotNetAccess = dotNetAccess ?? throw new ArgumentNullException(nameof(dotNetAccess));
        }

        void IExtensionVersionService.SetExtensionVersion(AboutExtensionModel aboutExtensionModel)
        {
            if (aboutExtensionModel == null)
            {
                throw new ArgumentNullException(nameof(aboutExtensionModel));
            }

            aboutExtensionModel.ExtensionVersion = dotNetAccess.GetExtensionVersion();
        }
    }
}
