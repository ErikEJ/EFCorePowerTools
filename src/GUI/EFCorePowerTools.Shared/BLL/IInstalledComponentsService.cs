namespace EFCorePowerTools.Shared.BLL
{
    using System;
    using Models;

    public interface IInstalledComponentsService
    {
        /// <summary>
        /// Sets the component properties of the <paramref name="aboutExtensionModel"/>.
        /// </summary>
        /// <param name="aboutExtensionModel">The model to update.</param>
        /// <exception cref="ArgumentNullException"><paramref name="aboutExtensionModel"/> is <b>null</b>.</exception>
        void SetMissingComponentData(AboutExtensionModel aboutExtensionModel);
    }
}