using System.Collections.Generic;
using EFCorePowerTools.Handlers.Compare;

namespace EFCorePowerTools.Contracts.Views
{
    public interface ICompareResultDialog : IDialog<object>
    {
        void AddComparisonResult(IEnumerable<CompareLogModel> result);
    }
}