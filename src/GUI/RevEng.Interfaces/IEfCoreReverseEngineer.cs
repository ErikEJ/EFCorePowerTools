using EFCorePowerTools.Shared.Models;

namespace ReverseEngineer20
{
    public interface IEfCoreReverseEngineer
    {
        string GenerateClassName(string value);
        EfCoreReverseEngineerResult GenerateFiles(ReverseEngineerOptions reverseEngineerOptions);
        System.Collections.Generic.List<TableInformationModel> GetDacpacTables(string dacpacPath);
    }
}