namespace EFCorePowerTools.DAL
{
    using Shared.DAL;
    using System.Diagnostics;
    using System.Windows;

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