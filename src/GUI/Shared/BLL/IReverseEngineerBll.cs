using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Community.VisualStudio.Toolkit;
using EFCorePowerTools.Common.Models;
using EFCorePowerTools.Contracts.EventArgs;
using EFCorePowerTools.Handlers.ReverseEngineer;
using RevEng.Common;

namespace EFCorePowerTools.BLL
{
    /// <summary>
    /// Business logic Layer Interface for Reverse Engineering.
    /// </summary>
    public interface IReverseEngineerBll
    {
        Task ReverseEngineerCodeFirstAsync(string uiHint = null, WizardEventArgs wizardArgs = null);

        Task ReverseEngineerCodeFirstAsync(
            Project project,
            string optionsPath,
            bool onlyGenerate,
            bool fromSqlProj = false,
            string uiHint = null,
            WizardEventArgs wizardArgs = null);

        Task<bool> LoadDataBaseObjectsAsync(
            ReverseEngineerOptions options,
            DatabaseConnectionModel dbInfo,
            Tuple<List<Schema>, string> namingOptionsAndPath,
            WizardEventArgs wizardArgs = null);

        Task SaveOptionsAsync(
            Project project,
            string optionsPath,
            ReverseEngineerOptions options,
            ReverseEngineerUserOptions userOptions,
            Tuple<List<Schema>, string> renamingOptions);

        Task<bool> GetModelOptionsAsync(
            ReverseEngineerOptions options,
            string projectName,
            WizardEventArgs wizardArgs = null);

        bool GetModelOptionsPostDialog(
            ReverseEngineerOptions options,
            string projectName,
            WizardEventArgs wizardArgs = null,
            ModelingOptionsModel modelingOptionsResult = null);

        Task<string> GenerateFilesAsync(
            Project project,
            ReverseEngineerOptions options,
            string missingProviderPackage,
            bool onlyGenerate,
            List<NuGetPackage> packages);
    }
}
