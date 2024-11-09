using System;
using System.Threading.Tasks;
using System.Windows;
using Community.VisualStudio.Toolkit;
using EFCorePowerTools.Contracts.ViewModels;

namespace EFCorePowerTools.Contracts.Views
{
    /// <summary>
    /// Helper class to manage the visibility of the shell.
    /// </summary>
    public class DisposableShell : IDisposableShell
    {
        private readonly IShell shell;
        private readonly IShellViewModel viewModel;
        private bool disposed = false;

        public DisposableShell(IShell shell)
        {
            this.shell = shell;
            this.viewModel = ((Window)shell).DataContext as IShellViewModel;

            ((Window)shell).Show();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IDisposableShell SetProgressInterval(int interval)
        {
            viewModel.SetIntervalValue(interval);
            return this;
        }

        public Task ShowMessageAsync(string message)
        {
            viewModel.Message = message;
            return VS.StatusBar.ShowMessageAsync(message);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            disposed = true;
            ((Window)shell).Hide();
        }
    }
}
