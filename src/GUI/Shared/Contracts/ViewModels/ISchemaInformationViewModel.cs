﻿using System.Collections.ObjectModel;
using RevEng.Common;
using RevEng.Common.TableColumnRenaming;

namespace EFCorePowerTools.Contracts.ViewModels
{
    public interface ISchemaInformationViewModel : IObjectTreeSelectableViewModel, IViewModel
    {
        string Name { get; set; }

        bool IsVisible { get; }

        ObservableCollection<ITableInformationViewModel> Objects { get; }

        Schema ReplacingSchema { get; set; }
    }
}