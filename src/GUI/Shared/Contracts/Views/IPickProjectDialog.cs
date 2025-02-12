using System.Collections.Generic;
using EFCorePowerTools.Contracts.Models;

namespace EFCorePowerTools.Contracts.Views
{
    public interface IPickProjectDialog : IDialog<ProjectModel>
    {
        void PublishProjects(IEnumerable<ProjectModel> projects);
    }
}