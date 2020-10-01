namespace EFCorePowerTools.Contracts.Views
{
    using System.Collections.Generic;
    using Shared.Models;

    public interface IPickConfigDialog : IDialog<ConfigModel>
    {
        void PublishConfigurations(IEnumerable<ConfigModel> configurations);
    }
}