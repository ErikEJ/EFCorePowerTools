using System.ComponentModel;
using System.Runtime.CompilerServices;
using Community.VisualStudio.Toolkit;
using JetBrains.Annotations;

namespace EFCorePowerTools.Contracts.Models
{
    public class ProjectModel : INotifyPropertyChanged
    {
        private Project project;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the project.
        /// </summary>
        public Project Project
        {
            get => project;
            set
            {
                if (Equals(value, project))
                {
                    return;
                }

                project = value;
                OnPropertyChanged();
            }
        }

        public string DisplayName
        {
            get
            {
                return project?.Name ?? string.Empty;
            }
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}