﻿namespace EFCorePowerTools.Shared.DAL
{
    using System;
    using Microsoft.VisualStudio.Data.Services;
    using Models;

    public interface IVisualStudioAccess
    {
        DatabaseConnectionModel PromptForNewDatabaseConnection();

        DatabaseDefinitionModel PromptForNewDatabaseDefinition();

        void RemoveDatabaseConnection(IVsDataConnection dataConnection);

        void ShowMessage(string message);

        bool IsDdexProviderInstalled(Guid id);
        bool IsSqLiteDbProviderInstalled();

        void StartStatusBarAnimation(ref object icon);
        void StopStatusBarAnimation(ref object icon);

        void SetStatusBarText(string text);
        void ShowError(string error);

        void OpenFile(string fileName);
    }
}