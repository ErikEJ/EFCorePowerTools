namespace EFCorePowerTools.Contracts.Views
{
    using EnvDTE;

    public interface IMigrationOptionsDialog : IDialog<object>
    {
        IMigrationOptionsDialog UseProjectForMigration(Project project);
        IMigrationOptionsDialog UseOutputPath(string outputPath);
    }
}