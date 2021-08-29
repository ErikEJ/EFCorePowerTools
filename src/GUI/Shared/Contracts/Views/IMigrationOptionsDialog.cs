using Community.VisualStudio.Toolkit;

namespace EFCorePowerTools.Contracts.Views
{
    public interface IMigrationOptionsDialog : IDialog<object>
    {
        IMigrationOptionsDialog UseProjectForMigration(Project project);
        IMigrationOptionsDialog UseOutputPath(string outputPath);
    }
}