namespace EFCorePowerTools.DAL
{
    using System.Diagnostics;
    using System.Windows;
    using Microsoft.Win32;
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

        string IOperatingSystemAccess.RequestSaveFileName(string title,
                                                          string filter,
                                                          bool validateNames)
        {
            var sfd = new SaveFileDialog
            {
                Title = title,
                Filter = filter,
                ValidateNames = validateNames
            };
            return sfd.ShowDialog() == true
                       ? sfd.FileName
                       : null;
        }

        string IOperatingSystemAccess.RequestLoadFileName(string title,
                                                          string filter,
                                                          bool allowMultiSelection,
                                                          bool checkFileExists)
        {
            var ofd = new OpenFileDialog
            {
                Title = title,
                Filter = filter,
                Multiselect = allowMultiSelection,
                CheckFileExists = checkFileExists
            };
            return ofd.ShowDialog() == true
                       ? ofd.FileName
                       : null;
        }
    }
}