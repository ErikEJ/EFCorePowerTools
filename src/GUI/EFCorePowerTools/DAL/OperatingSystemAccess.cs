namespace EFCorePowerTools.DAL
{
    using System.Diagnostics;
    using System.Windows;
    using Shared.DAL;

    public class OperatingSystemAccess : IOperatingSystemAccess
    {
        void IOperatingSystemAccess.SetClipboardText(string text)
        {
            Clipboard.SetText(text);
        }

        void IOperatingSystemAccess.StartProcess(string fileName)
        {
            Process.Start(fileName);
        }
    }
}