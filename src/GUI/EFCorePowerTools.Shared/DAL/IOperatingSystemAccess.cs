namespace EFCorePowerTools.Shared.DAL
{
    public interface IOperatingSystemAccess
    {
        void StartProcess(string fileName);

        void SetClipboardText(string text);
    }
}