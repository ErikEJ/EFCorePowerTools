using System;
using System.Threading.Tasks;

namespace EFCorePowerTools.Contracts.Views
{
    public interface IDisposableShell : IDisposable
    {
        Task ShowMessageAsync(string message);

        IDisposableShell SetProgressInterval(int interval);
    }
}
