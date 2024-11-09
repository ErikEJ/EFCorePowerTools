using System.Windows.Controls;

namespace EFCorePowerTools.Contracts.Views
{
    public interface IShell : IView
    {
        Grid MainGrid { get; }

        void Show();
    }
}
