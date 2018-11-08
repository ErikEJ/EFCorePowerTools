namespace EFCorePowerTools.Shared.DAL
{
    public interface IOperatingSystemAccess
    {
        void StartProcess(string exec);

        void SetClipboardText(string text);

        string RequestSaveFileName(string title,
                                   string filter,
                                   bool validateNames);

        string RequestLoadFileName(string title,
                                   string filter,
                                   bool allowMultiSelection,
                                   bool checkFileExists);
    }
}