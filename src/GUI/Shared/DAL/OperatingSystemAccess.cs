using System.Diagnostics;
using System.Windows;
using EFCorePowerTools.Common.DAL;

namespace EFCorePowerTools.DAL
{
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