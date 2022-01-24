using System.Reflection;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Settings;

namespace BaseClasses
{
    public class DefaultConfig : IRatingConfig
    {
        private const string _propertyName = "RatingIncrement";
        public int RatingRequests { get; set; }

        public async System.Threading.Tasks.Task SaveAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            ShellSettingsManager manager = new ShellSettingsManager(ServiceProvider.GlobalProvider);
            WritableSettingsStore store = manager.GetWritableSettingsStore(SettingsScope.UserSettings);
            string collection = $"rating\\{Assembly.GetCallingAssembly().GetName().Name}";

            int count = 0;
            if (!store.CollectionExists(collection))
            {
                store.CreateCollection(collection);
            }
            else if (store.PropertyExists(collection, _propertyName))
            {
                count = store.GetInt32(collection, _propertyName);
            }

            store.SetInt32(collection, _propertyName, count);
        }
    }
}
