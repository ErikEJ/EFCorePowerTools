namespace EFCorePowerTools.Shared.BLL
{
    using System;
    using Models;

    public interface IExtensionVersionService
    {
        /// <summary>
        /// Sets the <see cref="AboutExtensionModel.ExtensionVersion"/> of the <paramref name="aboutExtensionModel"/>.
        /// </summary>
        /// <param name="aboutExtensionModel">The model to update.</param>
        /// <exception cref="ArgumentNullException"><paramref name="aboutExtensionModel"/> is <b>null</b>.</exception>
        void SetExtensionVersion(AboutExtensionModel aboutExtensionModel);
    }
}