namespace EFCorePowerTools.Shared.DAL
{
    public interface IOperatingSystemAccess
    {
        void StartProcess(string exec);

        void SetClipboardText(string text);
    }
}