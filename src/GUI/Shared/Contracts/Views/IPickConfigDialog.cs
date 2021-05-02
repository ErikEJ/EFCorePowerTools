namespace EFCorePowerTools.Contracts.Views
{
    using Shared.Models;
    using System.Collections.Generic;

    public interface IPickConfigDialog : IDialog<ConfigModel>
    {
        void PublishConfigurations(IEnumerable<ConfigModel> configurations);
    }
}