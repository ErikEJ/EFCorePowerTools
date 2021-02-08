using Microsoft.VisualStudio.Data.Services.SupportEntities;
using Microsoft.VisualStudio.Shell;

namespace EFCorePowerTools.Helpers
{
    public class SqliteProviderRegistration : RegistrationAttribute
    {
        const string DataSourceGuid = "4ACCEB03-8218-433D-9936-3D8E4769CC9D";
        const string ProviderGuid = "687EA40E-7B91-472A-9A58-2A3D43DE1ACE";

        public override void Register(RegistrationContext context)
        {
            Key providerKey = null;
            try
            {
                providerKey = context.CreateKey($@"DataProviders\{{{ProviderGuid}}}");
                providerKey.SetValue(null, "SQLite Provider (Simple for EF6 by ErikEJ)");
                providerKey.SetValue("AssociatedSource", $"{{{DataSourceGuid}}}");
                providerKey.SetValue("Description", "Provider_DescriptionSqlite, ErikEJ.SqlCeToolbox.DDEX4.Properties.Resources");
                providerKey.SetValue("DisplayName", "Provider_DisplayNameSqlite, ErikEJ.SqlCeToolbox.DDEX4.Properties.Resources");
                providerKey.SetValue("InvariantName", "System.Data.SQLite.EF6");
                providerKey.SetValue("PlatformVersion", "2.0");
                providerKey.SetValue("ShortDisplayName", "Provider_ShortDisplayNameSqlite, ErikEJ.SqlCeToolbox.DDEX4.Properties.Resources");
                providerKey.SetValue("Technology", "{77AB9A9D-78B9-4ba7-91AC-873F5338F1D2}");
                providerKey.SetValue("CodeBase", "$PackageFolder$\\SqlCeToolbox.DDEX4.dll");

                    var supportedObjectsKey = providerKey.CreateSubkey("SupportedObjects");
                supportedObjectsKey.CreateSubkey(nameof(IVsDataCommand));
                supportedObjectsKey.CreateSubkey(nameof(IVsDataAsyncCommand));
                supportedObjectsKey.CreateSubkey(nameof(IVsDataObjectSelector))
                    .SetValue(null, "ErikEJ.SqlCeToolbox.DDEX4.SqlCeObjectSelector");
                supportedObjectsKey.CreateSubkey(nameof(IVsDataSourceInformation))
                    .SetValue(null, "ErikEJ.SqlCeToolbox.DDEX4.SqlCeSourceInformation");

                var connectionSupportKey = supportedObjectsKey.CreateSubkey(nameof(IVsDataConnectionSupport));
                connectionSupportKey.SetValue(null, "Microsoft.VisualStudio.Data.Framework.AdoDotNet.AdoDotNetConnectionSupport");
                connectionSupportKey.SetValue("Assembly",
                    "Microsoft.VisualStudio.Data.Framework, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");

                var connectionUiPropertiesKey = supportedObjectsKey.CreateSubkey(nameof(IVsDataConnectionUIProperties));
                connectionUiPropertiesKey.SetValue(null, "Microsoft.VisualStudio.Data.Framework.AdoDotNet.AdoDotNetConnectionProperties");
                connectionUiPropertiesKey.SetValue("Assembly",
                    "Microsoft.VisualStudio.Data.Framework, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");

                var connectionPropertiesKey = supportedObjectsKey.CreateSubkey(nameof(IVsDataConnectionProperties));
                connectionPropertiesKey.SetValue(null, "Microsoft.VisualStudio.Data.Framework.AdoDotNet.AdoDotNetConnectionProperties");
                connectionPropertiesKey.SetValue("Assembly", "Microsoft.VisualStudio.Data.Framework, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");

                var dataObjectSupportKey = supportedObjectsKey.CreateSubkey(nameof(IVsDataObjectSupport));
                dataObjectSupportKey.SetValue(null, "Microsoft.VisualStudio.Data.Framework.DataObjectSupport");
                dataObjectSupportKey.SetValue("Assembly",
                     "Microsoft.VisualStudio.Data.Framework, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
                dataObjectSupportKey.SetValue("XmlResource", "ErikEJ.SqlCeToolbox.DDEX4.SqlCeObjectSupport");

                var dataViewSupportKey = supportedObjectsKey.CreateSubkey(nameof(IVsDataViewSupport));
                dataViewSupportKey.SetValue(null, "Microsoft.VisualStudio.Data.Framework.DataViewSupport");
                dataViewSupportKey.SetValue("Assembly",
                    "Microsoft.VisualStudio.Data.Framework, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
                dataViewSupportKey.SetValue("XmlResource", "ErikEJ.SqlCeToolbox.DDEX4.SqliteViewSupport");

                var dataSourceKey = context.CreateKey($@"DataSources\{{{DataSourceGuid}}}");
                dataSourceKey.SetValue(null, "SQLite Provider (Simple for EF6 by ErikEJ)");
                dataSourceKey.SetValue("DefaultProvider", $"{{{ProviderGuid}}}");
                var supportingProviderKey = dataSourceKey
                    .CreateSubkey("SupportingProviders")
                    .CreateSubkey($"{{{ProviderGuid}}}");
                supportingProviderKey.SetValue("Description", "Provider_DescriptionSqlite, ErikEJ.SqlCeToolbox.DDEX4.Properties.Resources");
                supportingProviderKey.SetValue("DisplayName", "Provider_DisplayNameSqlite, ErikEJ.SqlCeToolbox.DDEX4.Properties.Resources");
            }
            finally
            {
                providerKey?.Close();
            }
        }

        public override void Unregister(RegistrationContext context)
        {
            context.RemoveKey($@"DataProviders\{{{ProviderGuid}}}");
            context.RemoveKey($@"DataSources\{{{DataSourceGuid}}}");
        }
    }
}
