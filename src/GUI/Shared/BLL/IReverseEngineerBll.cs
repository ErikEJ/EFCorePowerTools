using System.Collections.Generic;
using System.Threading.Tasks;
using Community.VisualStudio.Toolkit;
using EFCorePowerTools.Common.Models;
using EFCorePowerTools.Contracts.Views;
using EFCorePowerTools.Wizard;

namespace EFCorePowerTools.BLL
{
    /// <summary>
    /// Business logic Layer Interface for Reverse Engineering.
    /// </summary>
    public interface IReverseEngineerBll
    {
        Task<List<ConfigModel>> PickConfigDialogInitializeAsync(WizardDataViewModel viewModel = null);
        Task PickDatabaseConnectionAsync(Project project, string optionsPath, bool onlyGenerate, bool fromSqlProj = false, string uiHint = null, IPickServerDatabaseDialog databaseDialg = null);
        Task PickDatabaseTablesAsync(Project project, string optionsPath, bool onlyGenerate, bool fromSqlProj = false, string uiHint = null, IPickTablesDialog pickTablesDialog = null);


    }
}
