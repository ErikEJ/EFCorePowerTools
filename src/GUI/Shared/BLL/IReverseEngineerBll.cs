using System.Collections.Generic;
using System.Threading.Tasks;
using EFCorePowerTools.Common.Models;

namespace EFCorePowerTools.BLL
{
    /// <summary>
    /// Business logic Layer Interface for Reverse Engineering.
    /// </summary>
    public interface IReverseEngineerBll
    {
        Task<List<ConfigModel>> PickConfigDialogInitializeAsync();
    }
}
