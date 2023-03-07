namespace EFCorePowerTools.Common.DAL
{
    public interface IOperatingSystemAccess
    {
        void StartProcess(string fileName);

        void SetClipboardText(string text);
    }
}
