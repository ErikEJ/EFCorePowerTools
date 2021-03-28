namespace EFCorePowerTools.Contracts.Views
{
    using EFCorePowerTools.Handlers.Compare;
    using System.Collections.Generic;

    public interface ICompareResultDialog : IDialog<object>
    {
        void AddComparisonResult(IEnumerable<CompareLogModel> result);
    }
}